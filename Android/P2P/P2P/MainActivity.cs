using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Text;
using Android.Telephony;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using P2P.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace P2P
{
    [Activity(Label = "P2P", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        #region Private Variables
        TextView _ccno;
        TextView _name;
        TextView _mobileno;
        TextView _email;
        Button _login;
        TelephonyManager _teleMngr;
        StringBuilder _resp;
        readonly string URL = "http://pumas.cloudapp.net/p2pservice/api/p2p/registerdevice";
        RegistrationRequest _req;
        #endregion

        /// <summary>
        /// OnCreate
        /// </summary>
        /// <param name="bundle"></param>
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            _name = FindViewById<TextView>(Resource.Id.txtname);
            _mobileno = FindViewById<TextView>(Resource.Id.txtphoneno);
            _email = FindViewById<TextView>(Resource.Id.txtemailid);
            _ccno = FindViewById<TextView>(Resource.Id.txtccno);
            _login = FindViewById<Button>(Resource.Id.btnsubmit);
            _resp = new StringBuilder();
            _req = new RegistrationRequest();

            _teleMngr = (TelephonyManager)GetSystemService(TelephonyService);
            _mobileno.Text = _teleMngr.Line1Number;

            _login.Click += delegate
            {
                if (!DoRegistrationValidation())
                {
                    new AlertDialog.Builder(this)
                    .SetMessage(_resp.ToString())
                    .SetNeutralButton("Ok", delegate { }).Show();
                    return;
                }

                try
                {
                    ConstructRequest();

                    HttpStatusCode respCode = PostRegistrationDetails(URL, _req);

                    if (respCode != HttpStatusCode.OK)
                    {
                        new AlertDialog.Builder(this)
                       .SetMessage(respCode.ToString())
                       .SetNeutralButton("Ok", delegate { }).Show();
                        return;
                    }

                }
                catch (Exception ex)
                {
                    new AlertDialog.Builder(this).SetMessage(ex.Message).Show();
                }

                var intent = new Intent(this, typeof(HomeActivity));
                intent.PutExtra("Name", _name.Text);
                StartActivity(intent);
            };
        }

        /// <summary>
        /// DoRegistrationValidation
        /// </summary>
        /// <returns></returns>
        protected bool DoRegistrationValidation()
        {
            _resp.Clear();
            if (_mobileno.Text == String.Empty || _email.Text == string.Empty || _name.Text == string.Empty)
            {
                _resp.Append("Name, Mobile no & Email can not be left empty");
                return false;
            }

            if (!IsValidEmail(_email.Text))
            {
                _resp.Append("Email id is invalid");
                return false;
            }

            if (!IsValidMobileNumber(_mobileno.Text))
            {
                _resp.Append("Mobile no is invalid");
                return false;
            }

            return true;
        }

        /// <summary>
        /// IsValidEmail
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        protected bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// IsValidMobileNumber
        /// </summary>
        /// <param name="MobileNum"></param>
        /// <returns></returns>
        protected bool IsValidMobileNumber(String MobileNum)
        {
            bool RtnVal = true;

            if ((MobileNum.Length < 10) || (MobileNum.Length > 12))
            {
                RtnVal = false;
            }
            else
            {
                int Pos;
                int NumChars;

                NumChars = MobileNum.Length;

                for (Pos = 0; Pos < NumChars; Pos++)
                {

                    if (!Char.IsDigit(MobileNum[Pos]))
                    {
                        if (!char.IsWhiteSpace(MobileNum[Pos]))
                        {
                            if ((MobileNum[Pos] != '(') && (MobileNum[Pos] != ')'))
                            {
                                RtnVal = false;
                            }
                        }
                    }
                }

                if (MobileNum.Contains("("))
                {
                    if (!MobileNum.Contains(")"))
                    {
                        RtnVal = false;
                    }

                }
            }

            return RtnVal;
        }

        /// <summary>
        /// PostRegistrationDetails
        /// </summary>
        /// <param name="url"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        protected HttpStatusCode PostRegistrationDetails(string url, RegistrationRequest req)
        {
            string serializedObj = JsonConvert.SerializeObject(req);
            byte[] buf = Encoding.UTF8.GetBytes(serializedObj);

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "POST";
            request.GetRequestStream().Write(buf, 0, buf.Length);

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                return response.StatusCode;
            }
        }

        /// <summary>
        /// ConstructRequest
        /// </summary>
        protected void ConstructRequest()
        {
            if (_req != null)
            {
                _req.Clubcard = _ccno.Text;
                _req.EmailId = _email.Text;
                _req.GcmToken = "74382075";
                _req.id = _teleMngr.DeviceId;
                _req.Mobile = _mobileno.Text;
                _req.Name = _name.Text;
            }
        }
    }
}

