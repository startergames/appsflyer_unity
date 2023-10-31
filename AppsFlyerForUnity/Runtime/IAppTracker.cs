using System.Threading.Tasks;

namespace AppsFlyerForUnity.Interfaces {
    public interface IAppTracker {
        void SetUserId(string      userId);
        void TrackEvent(string     eventName);
        void TrackAuthEvent(string registrationMethod);
        void TrackPurchase(double  revenue, string currency, int quantity, string receiptid, string content, string productid);
    }

    public interface IDevKeyProvider {
        Task<string> GetKey();
        bool         IsDevMode { get; }
    }
}