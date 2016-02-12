﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using P2PPersistanceLayer.Model;


namespace P2PPersistanceLayer
{
    public class SQLPersistant : IPersistant
    {
        private SqlConnection _conn = null;

        private void GetConnection()
        {
            _conn = new SqlConnection(ConfigurationManager.ConnectionStrings["p2p"].ConnectionString);
            _conn.Open();
        }
        
        public bool RegisterDevice(PersistantDevice device)
        {

            try
            {
                GetConnection();

                SqlCommand cmd = new SqlCommand("insert into Device values(@id,@EmailId,@Name,@GcmToken,@Clubcard,@Mobile);", _conn);

                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@id",device.Id);
                cmd.Parameters.AddWithValue("@EmailId", device.EmailId);
                cmd.Parameters.AddWithValue("@Name", device.Name);
                cmd.Parameters.AddWithValue("@GcmToken", device.GcmToken);
                cmd.Parameters.AddWithValue("@Clubcard", device.Clubcard);
                cmd.Parameters.AddWithValue("@Mobile", device.Mobile);

                cmd.ExecuteNonQuery();

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                _conn.Close();
            }
                

           

            return true;
        }

        public PersistantDevice GetDeviceDetailsByDeviceId(string deviceId)
        {
            try
            {
                GetConnection();
                
                SqlCommand cmd = new SqlCommand("Select * from device where id = @id", _conn);

                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@id", deviceId);
                PersistantDevice _device = null;
          
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        _device = new PersistantDevice()
                        {
                            Id = reader["id"].ToString(),
                            Name = reader["Name"].ToString(),
                            GcmToken = reader["GcmToken"].ToString(),
                            Clubcard = reader["Clubcard"].ToString(),
                            EmailId = reader["EmailId"].ToString(),
                            Mobile = reader["Mobile"].ToString()
                        };
                    }
                }
                return _device;

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                _conn.Close();
            }
        }

        public void UpdateNotificationStatus(string deviceId, DateTime date)
        {
            try
            {
                GetConnection();

                SqlCommand cmd = new SqlCommand("insert into NotificationStatus(DeviceID,NotifiedDate) values (@deviceId,@date)", _conn);

                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@deviceId", deviceId);
                cmd.Parameters.AddWithValue("@date", date);
               
                cmd.ExecuteNonQuery();

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                _conn.Close();
            }
        }

        public bool UpdateDeviceDetails(PersistantDevice device)
        {
            try
            {
                GetConnection();

                SqlCommand cmd = new SqlCommand("update Device set EmailId = @EmailId,Name = @Name,GcmToken = @GcmToken,Clubcard = @Clubcard,Mobile = @Mobile where id = @id", _conn);

                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@id", device.Id);
                cmd.Parameters.AddWithValue("@EmailId", device.EmailId);
                cmd.Parameters.AddWithValue("@Name", device.Name);
                cmd.Parameters.AddWithValue("@GcmToken", device.GcmToken);
                cmd.Parameters.AddWithValue("@Clubcard", device.Clubcard);
                cmd.Parameters.AddWithValue("@Mobile", device.Mobile);

                cmd.ExecuteNonQuery();

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                _conn.Close();
            }
            return true;
        }

        public List<PersistantOffer> GetOffers(string clubcard, string storeId)
        {
            List<PersistantOffer> persistantOffers = new List<PersistantOffer>();
            try
            {
                GetConnection();

                // Get valid record from offer table
                string query = "select OfferCode,Name,OfferDescription,ImagePath from offer where storeid in (" + storeId + ") and startDate <='" + DateTime.Now.ToString("MM/dd/yyyy") + "' and EndDate >= '" + DateTime.Now.ToString("MM/dd/yyyy") + "'";

                // if user has a clubcard number merge coupon records also
                if (clubcard != string.Empty)
                    query += " union select CouponCode as 'OfferCode',Name,CouponDescription as 'OfferDescription',ImagePath from coupon where CouponAvailed = 0 and startDate <= '" + DateTime.Now.ToString("MM/dd/yyyy") + "' and EndDate >= '" + DateTime.Now.ToString("MM/dd/yyyy") + "';";
              

                SqlCommand cmd = new SqlCommand(query, _conn);

                cmd.CommandType = System.Data.CommandType.Text;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        PersistantOffer _PersistantOffer = new PersistantOffer()
                        {
                            OfferCode = reader["OfferCode"].ToString(),
                            Name = reader["Name"].ToString(),
                            Description = reader["OfferDescription"].ToString(),
                            ImagePath = reader["ImagePath"].ToString()
                        };
                        persistantOffers.Add(_PersistantOffer);
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                _conn.Close();
            }

            return persistantOffers;
        }

        public bool NotificationStatusForDevice(string deviceId)
        {
            try
            {
                GetConnection();

                SqlCommand cmd = new SqlCommand("Select * from NotificationStatus where DeviceID = @id and NotifiedDate = '" + DateTime.Now.ToString("MM/dd/yyyy") + "';", _conn);

                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@id", deviceId);
                
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return true;
                    }
                }
                return false;

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                _conn.Close();
            }
        }
    }
}