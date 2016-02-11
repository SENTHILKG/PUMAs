using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using P2P.Model;
using P2PPersistanceLayer;
using P2PPersistanceLayer.Model;


namespace P2PBusinessLayer
{
    public class P2PBusinessLogic : IP2PBusinessLogic
    {
        IPersistant persist = new SQLPersistant();

        /// <summary>
        /// Save or update device to database
        /// </summary>
        /// <param name="device">Device object that need to update or save</param>
        /// <returns></returns>
        public bool RegisterDevice(P2P.Model.Device device)
        {
            //check if device already exist
            PersistantDevice _device = persist.GetDeviceDetailsByDeviceId(device.Id);

            if (_device == null)
            {
                //if exist then update else insert new record
                persist.RegisterDevice(ConvertModelToPersistant(device));
            }
            else
            {
                //if exist then update else insert new record
                persist.UpdateDeviceDetails(ConvertModelToPersistant(device));
            }
            return true;
        }

        public PersistantDevice ConvertModelToPersistant(Device device)
        {
            return new PersistantDevice()
            {
                Name = device.Name,
                Id = device.Id,
                Mobile = device.Mobile,
                GcmToken = device.GcmToken,
                EmailId = device.EmailId,
                Clubcard = device.Clubcard
            };
        } 

        /// <summary>
        /// Get list of general offers for home screen of app
        /// </summary>
        /// <param name="deviceId">Device ID to get clubcard number for CC specific coupons</param>
        /// <returns></returns>
        //public List<Offer> GetOffers(string deviceId)
        //{
        //    Device _device = persist.GetDeviceDetailsByDeviceId(deviceId);
        //    return persist.GetOffers(_device != null ? _device.Clubcard : null, "0");
        //}

       /// <summary>
       /// Get list of offers and coupons store specific
       /// </summary>
        /// <param name="deviceId">Device ID to get clubcard number for CC specific coupons</param>
       /// <param name="storeId">Store ID to get store specific offer</param>
       /// <returns></returns>
        public List<Promotions> GetOffers(string deviceId, string storeId,bool refresh)
        {
           if (string.IsNullOrEmpty(storeId))
               storeId = "0";
           else
               storeId += ",0";

            //get device details by deviceid
            PersistantDevice device = persist.GetDeviceDetailsByDeviceId(deviceId);

          
            List<PersistantOffer> offers = persist.GetOffers(device != null ? device.Clubcard : string.Empty, storeId);

           if (Convert.ToBoolean(ConfigurationManager.AppSettings["sendNotification"]) && !refresh)
           {
               SendNotification(device.Id, offers);
               persist.UpdateNotificationStatus(deviceId, DateTime.Now);
           }

            return ConvertToPromotion(offers);
        }

        private List<Promotions> ConvertToPromotion(List<PersistantOffer> offers)
        {
            List<Promotions> promotion = new List<Promotions>();
            foreach (var offer in offers)
            {
                promotion.Add(new Promotions()
                {
                    Promotion = offer.Description
                });
            }
            return promotion;
        }
        

        /// <summary>
        /// Sending Notification and updating Notification status in DB
        /// </summary>
        /// <param name="deviceId"> Device ID</param>
        /// <param name="offers">List Of Offers</param>
        private void SendNotification(string deviceId, List<PersistantOffer> offers)
        {
           //to to check notification is send or not
            PersistantDevice deviceDetails = persist.GetDeviceDetailsByDeviceId(deviceId);

            foreach (var _offer in offers)
            {
                AndroidPush(deviceDetails.GcmToken,deviceDetails.Id,_offer.Description);
            }
            
        }

        /// <summary>
        /// GCM Push Notification
        /// </summary>
        /// <param name="regisrationId">GCM Registration ID</param>
        /// <param name="senderId">Sender ID</param>
        /// <param name="message">Message</param>
        private void AndroidPush(string regisrationId,string senderId,string message)
        {
            // your RegistrationID paste here which is received from GCM server.                                                               
            string regId = regisrationId;
            // applicationID means google Api key                                                                                                     
            var applicationID = ConfigurationManager.AppSettings["applicationId"].ToString();   
            // SENDER_ID is nothing but your ProjectID (from API Console- google code)//                                          
            var SENDER_ID = senderId;

            var value = message; //message text box                                                                               

            WebRequest tRequest;

            tRequest = WebRequest.Create("https://android.googleapis.com/gcm/send");

            tRequest.Method = "post";

            tRequest.ContentType = " application/x-www-form-urlencoded;charset=UTF-8";

            tRequest.Headers.Add(string.Format("Authorization: key={0}", applicationID));

            tRequest.Headers.Add(string.Format("Sender: id={0}", SENDER_ID));

            //Data post to server                                                                                                                                         
            string postData =
                 "collapse_key=score_update&time_to_live=108&delay_while_idle=1&data.message="
                  + value + "&data.time=" + System.DateTime.Now.ToString() + "&registration_id=" +
                     regId + "";
            
            Byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            tRequest.ContentLength = byteArray.Length;

            Stream dataStream = tRequest.GetRequestStream();

            dataStream.Write(byteArray, 0, byteArray.Length);

            dataStream.Close();

            WebResponse tResponse = tRequest.GetResponse();

            dataStream = tResponse.GetResponseStream();

            StreamReader tReader = new StreamReader(dataStream);

            String sResponseFromServer = tReader.ReadToEnd();   //Get response from GCM server.

            var res = sResponseFromServer;      //Assigning GCM response to Label text 

            tReader.Close();

            dataStream.Close();
            tResponse.Close();
        }
        
    }
}