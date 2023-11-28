using UnityEngine;
using UnityEngine.Serialization;

namespace AppsFlyerForUnity {
    public class AppsFlyerSettings : ScriptableObject {
        [FormerlySerializedAs("apiKey")]
        public const string SettingsPath = "Assets/Resources/AppsFlyerSettings.asset";

        [SerializeField]
        private string devKeyForAndroid;

        [SerializeField]
        private string devKeyForIOS;

        [SerializeField]
        private string devKeyForStandalone;

        [SerializeField]
        private string appIDForIOS;

        [SerializeField]
        private string appIDForAndroid;


        public bool IsDebug;

        public string DevKey {
            get {
#if UNITY_IOS
                return devKeyForIOS;
#elif UNITY_ANDROID
                return devKeyForAndroid;
#else
                return devKeyForStandalone;
#endif
            }
        }

        public string AppID {
            get {
#if UNITY_IOS
                return appIDForIOS;
#elif UNITY_ANDROID
                return appIDForAndroid;
#else
                return UnityEngine.Application.identifier;
#endif
            }
        }
    }
}