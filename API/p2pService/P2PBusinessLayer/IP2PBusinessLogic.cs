using System;
using System.Collections.Generic;
using P2PPersistanceLayer.Model;
using P2P.Model;

namespace P2PBusinessLayer
{
    public interface IP2PBusinessLogic
    {
        bool RegisterDevice(Device device);
        //List<Offer> GetOffers(string deviceId);
        List<Promotions> GetOffers(string deviceId, string storeId, bool refresh);
        //void SendNotification(string deviceId, List<PersistantOffer> offers);
        //void UpdateNotificationStatus(string deviceId, DateTime date);
    }
}
