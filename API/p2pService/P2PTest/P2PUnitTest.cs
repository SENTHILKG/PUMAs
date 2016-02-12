using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using P2P.Model;
using P2PBusinessLayer;
using P2PPersistanceLayer;
using P2PPersistanceLayer.Model;
using P2PService.Controllers;

namespace P2PTest
{
    [TestClass]
    public class P2PUnitTest
    {
        private Mock<IPersistant> _persist;

        [TestInitialize]
        public void Setup()
        {
            _persist = new Mock<IPersistant>();

            
        }

        [TestMethod]
        public void RegisterDevice_Test()
        {
            Device _device = new Device()
            {
                Name = "Test Name",
                Id = "Device ID",
                GcmToken = "Test GCM Token",
                Mobile = "9999999999",
                EmailId = "test@tesco.com",
                Clubcard = "2345677765"
            };

            PersistantDevice persistantDevice = new PersistantDevice()
            {
                Clubcard = "222222222",
                EmailId = "Test@test.com",
                GcmToken = "TR000001",
                Id = "1234555",
                Mobile = "9999999999",
                Name = "Test"
            };

            _persist.Setup(r => r.GetDeviceDetailsByDeviceId(It.IsAny<string>()));
            _persist.Setup(r => r.RegisterDevice(It.IsAny<PersistantDevice>())).Returns(true);
            IP2PBusinessLogic business = new P2PBusinessLogic(_persist.Object);
            var result = business.RegisterDevice(_device);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void UpdateDevice_Test()
        {
            Device _device = new Device()
            {
                Name = "Test Name",
                Id = "Device ID",
                GcmToken = "Test GCM Token",
                Mobile = "9999999999",
                EmailId = "test@tesco.com",
                Clubcard = "2345677765"
            };

            PersistantDevice persistantDevice = new PersistantDevice()
            {
                Clubcard = "222222222",
                EmailId = "Test@test.com",
                GcmToken = "TR000001",
                Id = "1234555",
                Mobile = "9999999999",
                Name = "Test"
            };

            _persist.Setup(r => r.GetDeviceDetailsByDeviceId(It.IsAny<string>())).Returns(persistantDevice);
            _persist.Setup(r => r.RegisterDevice(It.IsAny<PersistantDevice>())).Returns(true);
            IP2PBusinessLogic business = new P2PBusinessLogic(_persist.Object);
            var result = business.RegisterDevice(_device);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void GetOfferForDevice_Test()
        {
            PersistantDevice persistantDevice = new PersistantDevice()
            {
                Clubcard = "222222222",
                EmailId = "Test@test.com",
                GcmToken = "TR000001",
                Id = "1234555",
                Mobile = "9999999999",
                Name = "Test"
            };

            PersistantOffer offer = new PersistantOffer()
            {
                Name = "test Offer",
                Description = "Test Offer Description",
                ImagePath = "Image test path",
                OfferCode = "Offer Code"
            };
            List<PersistantOffer> lstPersistantOffer = new List<PersistantOffer>();
            lstPersistantOffer.Add(offer);

            _persist.Setup(r => r.GetDeviceDetailsByDeviceId(It.IsAny<string>())).Returns(persistantDevice);
            _persist.Setup(r => r.GetOffers(It.IsAny<string>(), It.IsAny<string>())).Returns(lstPersistantOffer);
            IP2PBusinessLogic business = new P2PBusinessLogic(_persist.Object);
            var result = business.GetOffers("test device id","test store id",true);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetOfferForFirstTimeNotification_Test()
        {
            PersistantDevice persistantDevice = new PersistantDevice()
            {
                Clubcard = "222222222",
                EmailId = "Test@test.com",
                GcmToken = "TR000001",
                Id = "1234555",
                Mobile = "9999999999",
                Name = "Test"
            };

            PersistantOffer offer = new PersistantOffer()
            {
                Name = "test Offer",
                Description = "Test Offer Description",
                ImagePath = "Image test path",
                OfferCode = "Offer Code"
            };
            List<PersistantOffer> lstPersistantOffer = new List<PersistantOffer>();
            lstPersistantOffer.Add(offer);

            _persist.Setup(r => r.GetDeviceDetailsByDeviceId(It.IsAny<string>())).Returns(persistantDevice);
            _persist.Setup(r => r.GetOffers(It.IsAny<string>(), It.IsAny<string>())).Returns(lstPersistantOffer);
            _persist.Setup(r => r.NotificationStatusForDevice(It.IsAny<string>())).Returns(false);
            _persist.Setup(r => r.UpdateNotificationStatus(It.IsAny<string>(), It.IsAny<DateTime>()));
            IP2PBusinessLogic business = new P2PBusinessLogic(_persist.Object);
            var result = business.GetOffers("test device id", "test store id", false);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetOfferAfterFirstTimeNotification_Test()
        {
            PersistantDevice persistantDevice = new PersistantDevice()
            {
                Clubcard = "222222222",
                EmailId = "Test@test.com",
                GcmToken = "TR000001",
                Id = "1234555",
                Mobile = "9999999999",
                Name = "Test"
            };

            PersistantOffer offer = new PersistantOffer()
            {
                Name = "test Offer",
                Description = "Test Offer Description",
                ImagePath = "Image test path",
                OfferCode = "Offer Code"
            };
            List<PersistantOffer> lstPersistantOffer = new List<PersistantOffer>();
            lstPersistantOffer.Add(offer);

            _persist.Setup(r => r.GetDeviceDetailsByDeviceId(It.IsAny<string>())).Returns(persistantDevice);
            _persist.Setup(r => r.GetOffers(It.IsAny<string>(), It.IsAny<string>())).Returns(lstPersistantOffer);
            _persist.Setup(r => r.NotificationStatusForDevice(It.IsAny<string>())).Returns(true);
            _persist.Setup(r => r.UpdateNotificationStatus(It.IsAny<string>(), It.IsAny<DateTime>()));
            IP2PBusinessLogic business = new P2PBusinessLogic(_persist.Object);
            var result = business.GetOffers("test device id", "test store id", false);
            Assert.IsNotNull(result);
        }
    }
}
