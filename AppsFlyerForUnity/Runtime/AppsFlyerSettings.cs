using UnityEngine;
using UnityEngine.Serialization;

namespace AppsFlyerForUnity {
    public class AppsFlyerSettings : ScriptableObject {
        [FormerlySerializedAs("apiKey")]
        public const string SettingsPath = "Assets/Resources/AppsFlyerSettings.asset";

        public string devKey;
        public bool   testMode;
    }
}