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
            sessionList.Add("2021-22", DateTime.ParseExact("04-30-2022 11:59:59 PM", format, provider).ToString());
            sessionList.Add("2022-23", DateTime.ParseExact("04-30-2023 11:59:59 PM", format, provider).ToString());
            sessionList.Add("2023-24", DateTime.ParseExact("04-30-2024 11:59:59 PM", format, provider).ToString());
            sessionList.Add("2024-25", DateTime.ParseExact("04-30-2025 11:59:59 PM", format, provider).ToString());
            sessionList.Add("2025-26", DateTime.ParseExact("04-30-2026 11:59:59 PM", format, provider).ToString());
            sessionList.Add("2026-27", DateTime.ParseExact("04-30-2027 11:59:59 PM", format, provider).ToString());
            sessionList.Add("2027-28", DateTime.ParseExact("04-30-2028 11:59:59 PM", format, provider).ToString());

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

        public static bool CheckLicenseValidity(ClientInfo clientInfo, out string message, out bool deleteVideo)
        {
            bool valid = true;
            deleteVideo = false;
            message = "";

            //Caes1 => RegDate < CurrentDate & CurrentDate < Exp Date & LastAccessTime > CurrentDate = Show Message
            if (clientInfo.RegistrationDate.CompareTo(DateTime.Now) < 0 && clientInfo.ExpiryDate.CompareTo(DateTime.Now) > 0 && clientInfo.LastAccessEndTime.CompareTo(DateTime.Now) > 0)
            {
                valid = false;
                message = "Invalid clock";
            }
            // Clock time is behind
            //Caes2 => RegDate > CurrentDate & CurrentDate < Exp Date = Show Message
            else if (clientInfo.RegistrationDate.CompareTo(DateTime.Now) > 0)
            {
                valid = false;
                message = "Invalid clock";
            }
            //Caes3 => 	RegDate < CurrentDate & CurrentDate > Exp Date = Del Video Folder
            else if (clientInfo.ExpiryDate.CompareTo(DateTime.Now) < 0)
            {
                valid = false;
                message = "Your subscription has expired. To renew please\nContact:info@lbf.in or Call on +91 0 9109138808";
                deleteVideo = true;
            }
            //if (DateTime.Now < clientInfo.LastAccessStartTime || DateTime.Now < clientInfo.LastAccessEndTime)
            //{
            //    valid = false;
            //    message = "Your subscription has expired. To renew please\nContact:info@lbf.in or Call on +91 0 9109138808";
            //}
            //else if (DateTime.Now > clientInfo.ExpiryDate)
            //{
            //    valid = false;
            //    message = "Your subscription has expired. To renew please\nContact:info@lbf.in or Call on +91 0 9109138808";
            //}
            return valid;
        }
    }
}
