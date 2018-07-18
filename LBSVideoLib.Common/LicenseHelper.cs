using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace LBFVideoLib.Common
{
    public class LicenseHelper
    {
        private static Dictionary<string, string> sessionList = new Dictionary<string, string>();

        static LicenseHelper()
        {
            var provider = CultureInfo.InvariantCulture;
            var format = "MM-dd-yyyy hh:mm:ss tt";


            sessionList.Add("2018-19", DateTime.ParseExact("04-30-2019 11:59:59 PM", format, provider).ToString());
            sessionList.Add("2019-20", DateTime.ParseExact("04-30-2020 11:59:59 PM", format, provider).ToString());
            sessionList.Add("2020-21", DateTime.ParseExact("04-30-2021 11:59:59 PM", format, provider).ToString());
            //sessionList.Add("2018-19", "30-04-2019 11:59:59 PM");
            //sessionList.Add("2019-20", "30-04-2020 11:59:59 PM");
            //sessionList.Add("2020-21", "30-04-2021 11:59:59 PM");
        }

        public static List<string> GetSessionList()
        {
            return sessionList.Keys.ToList<string>();
        }

        public static DateTime GetExpiryDateBySessionString(string session)
        {
            return Convert.ToDateTime(sessionList[session]);
        }

        public static bool CheckLicenseValidity(ClientInfo clientInfo, out string message)
        {
            bool valid = true;
            message = "";
            if (DateTime.Now < clientInfo.LastAccessStartTime || DateTime.Now < clientInfo.LastAccessEndTime)
            {
                valid = false;
                message = "Invalid clock";
            }
            else if (DateTime.Now > clientInfo.ExpiryDate)
            {
                valid = false;
                message = "Your subscription has expired. To renew please\nContact:info@lbf.in or Call on +91 0 9109138808";
            }
            return valid;
        }
    }
}
