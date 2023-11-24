using System.Collections.Generic;
using System.Threading.Tasks;
using AppsFlyerForUnity.Interfaces;
using UnityEngine;
using AppsFlyerSDK;
using VContainer;

namespace AppsFlyerForUnity {
    public class AppsFlyerSetup : MonoBehaviour, IAppsFlyerConversionData, IAppTracker {
        [Inject]
        private IDevKeyProvider keyProvider;

        private void Start() {
            DontDestroyOnLoad(this);
            Initialize();
        }

        private async Task Initialize() {
            var projectSettings = Resources.Load<AppsFlyerSettings>("AppsFlyerSettings");

            var devKey = await keyProvider?.GetKey() ?? projectSettings?.DevKey;
            if(string.IsNullOrWhiteSpace(devKey)) {
                Debug.LogError("AppsFlyer dev key is not set");
                return;
            }
            
#if UNITY_IOS && !UNITY_EDITOR
            AppsFlyer.waitForATTUserAuthorizationWithTimeoutInterval(60);
#endif
            if(keyProvider?.IsDevMode ?? projectSettings?.IsDebug ?? false) {
                AppsFlyer.AFLog("Initialize", "AppsFlyer is in debug mode");
                Debug.LogWarning("AppsFlyer is in debug mode");
                AppsFlyerSDK.AppsFlyer.setIsDebug(true);
            }
            
            AppsFlyerSDK.AppsFlyer.OnRequestResponse += (sender, args) => {
                var af_args = args as AppsFlyerRequestEventArgs;
                AppsFlyer.AFLog("AppsFlyerOnRequestResponse", $"status: {af_args.statusCode}, desc: {af_args.errorDescription}");
                if (af_args.statusCode != 200) {
                    Debug.LogError($"AppsFlyerOnRequestResponse: status: {af_args.statusCode}, desc: {af_args.errorDescription}");
                }
            };
            
            AppsFlyerSDK.AppsFlyer.initSDK(devKey, Application.identifier, this);
            AppsFlyerSDK.AppsFlyer.startSDK();
        }

        public void onConversionDataSuccess(string conversionData) {
            AppsFlyerSDK.AppsFlyer.AFLog("onConversionDataSuccess", conversionData);
            var conversionDataDictionary = AppsFlyerSDK.AppsFlyer.CallbackStringToDictionary(conversionData);
        }

        public void onConversionDataFail(string error) {
            AppsFlyerSDK.AppsFlyer.AFLog("onConversionDataFail", error);
        }

        public void onAppOpenAttribution(string attributionData) {
            AppsFlyerSDK.AppsFlyer.AFLog("onAppOpenAttribution", attributionData);
            var attributionDataDictionary = AppsFlyerSDK.AppsFlyer.CallbackStringToDictionary(attributionData);
            // add direct deeplink logic here
        }

        public void onAppOpenAttributionFailure(string error) {
            AppsFlyerSDK.AppsFlyer.AFLog("onAppOpenAttributionFailure", error);
        }

        public void SetUserId(string userId) {
            AppsFlyerSDK.AppsFlyer.setCustomerUserId(userId);
        }

        public void TrackEvent(string eventName) {
            AppsFlyerSDK.AppsFlyer.sendEvent(eventName, new());
        }

        public void TrackAuthEvent(string registrationMethod) {
            AppsFlyerSDK.AppsFlyer.sendEvent(AFInAppEvents.COMPLETE_REGISTRATION, new() {
                { AFInAppEvents.REGSITRATION_METHOD, registrationMethod }
            });
        }

        public void TrackPurchase(double revenue, string currency, int quantity, string receiptid, string content, string productid) {
            var param = new Dictionary<string, string>();
            param.Add(AFInAppEvents.REVENUE, revenue.ToString());
            param.Add(AFInAppEvents.CURRENCY, currency);
            param.Add(AFInAppEvents.QUANTITY, quantity.ToString());
            param.Add(AFInAppEvents.RECEIPT_ID, receiptid);
            param.Add(AFInAppEvents.CONTENT_TYPE, content);
            param.Add(AFInAppEvents.CONTENT_ID, productid);
            AppsFlyerSDK.AppsFlyer.sendEvent(AFInAppEvents.PURCHASE, param);
        }
    }
}