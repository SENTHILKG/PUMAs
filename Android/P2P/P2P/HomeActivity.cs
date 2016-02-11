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
            double lat = location.Latitude;
            double lon = location.Longitude;

            _lat.Text = lat.ToString();
            _lon.Text = lon.ToString();

            var currentloc = new GeoCoordinate(lat, lon);

            foreach (var v in _storeLocations)
            {
                var storelocation = new GeoCoordinate(v.Value.Latitude, v.Value.Longitude);
                var distance = currentloc.GetDistanceTo(storelocation);
                _distanceTo.Text = distance.ToString();

                Context context = this;
                Android.Content.Res.Resources res = context.Resources;
                string minRadius = res.GetString(Resource.String.minRadiusinMeters);

                if (distance < Convert.ToInt16(minRadius))
                {
                    new AlertDialog.Builder(this)
                     .SetMessage("Hey you are near to store, happy shopping!..")
                     .SetNeutralButton("Ok", delegate { }).Show();
                }
                return;
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

            _storeLocations.Add("101DTPLPILLAR", new Loc() { Latitude = 12.9673067, Longitude = 77.7228424 });

            _lat = FindViewById<TextView>(Resource.Id.txtLat);
            _lon = FindViewById<TextView>(Resource.Id.txtlong);
            _distanceTo = FindViewById<TextView>(Resource.Id.txtdistanceto);

            string name = Intent.GetStringExtra("Name");
            TextView tv = FindViewById<TextView>(Resource.Id.txtmessage);
            tv.Text = "Welcome, " + name;
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

        
    }
}