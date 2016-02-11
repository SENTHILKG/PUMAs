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

namespace P2P.Models
{
    public class RegistrationRequest
    {
        public string id { get; set; }
        public string EmailId { get; set; }
        public string Name { get; set; }
        public string GcmToken { get; set; }
        public string Clubcard { get; set; }
        public string Mobile { get; set; }
    }
}