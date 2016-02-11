using System;
using System.Collections.Generic;
using P2PPersistanceLayer.Model;

namespace P2PPersistanceLayer
{
    public interface IPersistant
    {
        bool RegisterDevice(PersistantDevice device);
        bool UpdateDeviceDetails(PersistantDevice device);
        List<PersistantOffer> GetOffers(string clubcard, string storeId);
        PersistantDevice GetDeviceDetailsByDeviceId(string deviceId);
        void UpdateNotificationStatus(string deviceId, DateTime date);

    }
}
