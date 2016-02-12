using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Locations;
using P2P.Models;
using System.Device.Location;
using System.Net;
using System.IO;

namespace P2P
{
    [Activity(Label = "HomeActivity")]
    public class HomeActivity : Activity, ILocationListener
    {
        #region Private Varibales
        LocationManager _locmngr;
        TextView _lat;
        TextView _lon;
        TextView _distanceTo;
        Dictionary<string, Loc> _storeLocations = new Dictionary<string, Loc>();
        //List<TescoPromotion> offers;
        readonly string urlGetOffers = "http://pumas.cloudapp.net/p2pservice/api/p2p/GetOffers?";
        string _deviceId = string.Empty;
        #endregion

        /// <summary>
        /// OnStart
        /// </summary>
        protected override void OnStart()
        {
            base.OnStart();
        }

        /// <summary>
        /// OnResume
        /// </summary>
        protected override void OnResume()
        {
            base.OnResume();

            _locmngr = GetSystemService(Context.LocationService) as LocationManager;

            if (_locmngr.AllProviders.Contains(LocationManager.NetworkProvider) &&
                    _locmngr.IsProviderEnabled(LocationManager.NetworkProvider))
            {
                _locmngr.RequestLocationUpdates(LocationManager.NetworkProvider, 2000, 1, this);
            }
            else
            {
                Toast.MakeText(this, "The network provider does not exist or enabled", ToastLength.Long).Show();
            }
        }

        /// <summary>
        /// OnPause
        /// </summary>
        protected override void OnPause()
        {
            base.OnPause();
            _locmngr.RemoveUpdates(this);
        }

        /// <summary>
        /// OnStop
        /// </summary>
        protected override void OnStop()
        {
            base.OnStop();
        }

        /// <summary>
        /// OnLocationChanged
        /// </summary>
        /// <param name="location"></param>
        public void OnLocationChanged(Location location)
        {
            try
            {
                double lat = location.Latitude;
                double lon = location.Longitude;

                _lat.Text = lat.ToString();
                _lon.Text = lon.ToString();

                GeoCoordinate currentloc = new GeoCoordinate(lat, lon);

                foreach (KeyValuePair<string, Loc> v in _storeLocations)
                {
                    GeoCoordinate storelocation = new GeoCoordinate(v.Value.Latitude, v.Value.Longitude);
                    double distance = currentloc.GetDistanceTo(storelocation);
                    _distanceTo.Text = distance.ToString();

                    Context context = this;
                    Android.Content.Res.Resources res = context.Resources;
                    string minRadius = res.GetString(Resource.String.minRadiusinMeters);

                    if (distance < Convert.ToInt16(minRadius))
                    {
                        var tescoPromo = GetOffers(urlGetOffers, _deviceId, v.Key, false);

                        List<string> soffers = tescoPromo.Select(e => e.Promotion).ToList();

                        if (soffers.Count != 0)
                        {
                            new AlertDialog.Builder(this)
                           .SetMessage("You have " + soffers.Count + " in hand!...")
                           .SetNeutralButton("Ok", delegate { }).Show();
                        }

                        new AlertDialog.Builder(this)
                         .SetMessage("Hey you are near to store, happy shopping!..")
                         .SetNeutralButton("Ok", delegate { }).Show();
                    }
                    return;
                }
            }
            catch (Exception ex)
            {
                new AlertDialog.Builder(this)
                         .SetMessage("Location Changed **culprit** " + ex.Message).Show();
            }
        }

        /// <summary>
        /// OnCreate
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Home);

            _storeLocations.Add("2907", new Loc() { Latitude = 12.9673067, Longitude = 77.7228424 });

            _lat = FindViewById<TextView>(Resource.Id.txtLat);
            _lon = FindViewById<TextView>(Resource.Id.txtlong);
            _distanceTo = FindViewById<TextView>(Resource.Id.txtdistanceto);

            string name = Intent.GetStringExtra("Name");
            TextView tv = FindViewById<TextView>(Resource.Id.txtmessage);
            tv.Text = "Welcome, " + name;
            _deviceId = Intent.GetStringExtra("DeviceId");
        }

        /// <summary>
        /// OnProviderDisabled
        /// </summary>
        /// <param name="provider"></param>
        public void OnProviderDisabled(string provider)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// OnProviderEnabled
        /// </summary>
        /// <param name="provider"></param>
        public void OnProviderEnabled(string provider)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// OnStatusChanged
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="status"></param>
        /// <param name="extras"></param>
        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// GetOffers
        /// </summary>
        /// <param name="baseURL"></param>
        /// <param name="deviceID"></param>
        /// <param name="storeid"></param>
        /// <param name="refreshFlag"></param>
        /// <returns></returns>
        public List<TescoPromotion> GetOffers(string baseURL, string deviceID, string storeid, bool refreshFlag)
        {
            string temp = string.Empty;
            List<TescoPromotion> offers = new List<TescoPromotion>();
            string url = baseURL + "deviceID=" + deviceID + "&StoreId=" + storeid + "&refreshflag=" + refreshFlag;
            try
            {

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
                request.ContentType = "application/json";
                request.Method = "GET";
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        Stream respStream = response.GetResponseStream();
                        using (var reader = new StreamReader(respStream))
                        {
                            temp = reader.ReadToEnd();
                        }
                        offers = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TescoPromotion>>(temp);
                    }
                }
            }
            catch (Exception ex)
            {
                new AlertDialog.Builder(this).SetMessage("Get Offer **culprit** " + ex.Message).Show();
            }
            return offers;
        }
    }
}