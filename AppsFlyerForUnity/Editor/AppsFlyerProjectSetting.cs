using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace AppsFlyerForUnity {
    public class AppsFlyerProjectSetting : SettingsProvider {
        public AppsFlyerProjectSetting(string path, SettingsScope scopes, IEnumerable<string> keywords = null) : base(path, scopes, keywords) { }

        public static AppsFlyerSettings Settings { get; private set; }

        [InitializeOnLoadMethod]
        private static void Initialize() {
            if (Settings != null) return;

            var directoryPath = System.IO.Path.GetDirectoryName(AppsFlyerSettings.SettingsPath);
            if (!AssetDatabase.IsValidFolder(directoryPath)) {
                AssetDatabase.CreateFolder(System.IO.Path.GetDirectoryName(directoryPath), System.IO.Path.GetFileName(directoryPath));
            }

            Settings = AssetDatabase.LoadAssetAtPath<AppsFlyerSettings>(AppsFlyerSettings.SettingsPath);

            if (Settings != null) return;
            Settings = ScriptableObject.CreateInstance<AppsFlyerSettings>();
            AssetDatabase.CreateAsset(Settings, AppsFlyerSettings.SettingsPath);
            AssetDatabase.SaveAssets();
        }

        [SettingsProvider]
        public static SettingsProvider CreateProtobufBuilderSettingsProvider() {
            var provider = new AppsFlyerProjectSetting("Project/AppsFlyerForUnity", SettingsScope.Project);
            return provider;
        }

        public override void OnActivate(string searchContext, VisualElement rootElement) {
            if (Settings == null) {
                Initialize();
            }

            var _settingsObject = new SerializedObject(Settings);
            var label = new Label("AppsFlyer Settings") {
                style = {
                    fontSize       = 20,
                    unityTextAlign = TextAnchor.MiddleCenter,
                    marginTop = 5,
                    marginBottom = 20,
                }
            };
            var devKeyField   = new PropertyField(_settingsObject.FindProperty(nameof(AppsFlyerSettings.devKey)));
            var testModeField = new PropertyField(_settingsObject.FindProperty(nameof(AppsFlyerSettings.testMode)));
            rootElement.Add(label);
            rootElement.Add(devKeyField);
            rootElement.Add(new Label("IKeyProvider를 통한 키 설정이 있다면 이 필드는 무시됩니다.") {
                style = {
                    fontSize = 10,
                }
            });
            rootElement.Add(testModeField);
            rootElement.Bind(_settingsObject);
        }
    }
}