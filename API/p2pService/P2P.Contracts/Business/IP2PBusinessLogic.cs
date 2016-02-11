using P2P.Model;
using System;
using System.Collections.Generic;

namespace P2P.Contracts.Business
{
    public interface IP2PBusinessLogic
    {
        bool RegisterDevice(Device device);
        List<Offer> GetOffers(string deviceId);
        List<Offer> GetOffers(string deviceId, string storeId, bool refreshFlag);
        void SendNotification(string deviceId, List<Offer> offers);
        void updateNotificationStatus(string deviceId, DateTime date);
    }
}
