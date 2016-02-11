using P2P.Model;
using System;
using System.Collections.Generic;
using P2PBusinessLayer.Storage.Model;

namespace P2P.Contracts.Storage
{
    public interface IPersistant
    {
        bool RegisterDevice(Device device);
        bool UpdateDeviceDetails(Device device);
        List<Offer> GetOffers(string clubcard, string storeId);
        Device getDeviceDetailsByDeviceId(string deviceId);
        void updateNotificationStatus(string deviceId, DateTime date);

    }
}
