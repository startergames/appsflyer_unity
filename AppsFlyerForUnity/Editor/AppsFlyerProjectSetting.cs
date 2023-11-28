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
            var devKeyAndroidField = new PropertyField(_settingsObject.FindProperty("devKeyForAndroid"));
            var devKeyIosField     = new PropertyField(_settingsObject.FindProperty("devKeyForIOS"));
            var devKeyStandaloneField = new PropertyField(_settingsObject.FindProperty("devKeyForStandalone"));
            var appIDAndroidField  = new PropertyField(_settingsObject.FindProperty("appIDForAndroid"));
            var appIDIosField      = new PropertyField(_settingsObject.FindProperty("appIDForIOS"));
            var testModeField      = new PropertyField(_settingsObject.FindProperty("IsDebug"));
            
            devKeyAndroidField.RegisterValueChangeCallback(evt => {
                _settingsObject.ApplyModifiedProperties();
            });
            devKeyIosField.RegisterValueChangeCallback(evt => {
                _settingsObject.ApplyModifiedProperties();
            });
            devKeyStandaloneField.RegisterValueChangeCallback(evt => {
                _settingsObject.ApplyModifiedProperties();
            });
            appIDAndroidField.RegisterValueChangeCallback(evt => {
                _settingsObject.ApplyModifiedProperties();
            });
            appIDIosField.RegisterValueChangeCallback(evt => {
                _settingsObject.ApplyModifiedProperties();
            });
            testModeField.RegisterValueChangeCallback(evt => {
                _settingsObject.ApplyModifiedProperties();
            });
            
            rootElement.Add(label);
            rootElement.Add(devKeyAndroidField);
            rootElement.Add(devKeyIosField);
            rootElement.Add(devKeyStandaloneField);
            rootElement.Add(new Label("IKeyProvider를 통한 키 설정이 있다면 여기서 설정한 DevKey는 무시됩니다.") {
                style = {
                    fontSize = 10,
                }
            });
            
            rootElement.Add(appIDAndroidField);
            rootElement.Add(appIDIosField);
            rootElement.Add(testModeField);
            rootElement.Bind(_settingsObject);
        }
    }
}