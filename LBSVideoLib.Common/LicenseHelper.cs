using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LBFVideoLib.Common
{
  public  class LicenseHelper
    {
        private static Dictionary<string, DateTime> sessionList = new Dictionary<string, DateTime>();

        static LicenseHelper()
        {
            sessionList.Add("2018-19", DateTime.Parse("30-04-2019 11:59:59 PM").ToUniversalTime());
            sessionList.Add("2019-20", DateTime.Parse("30-04-2020 11:59:59 PM").ToUniversalTime());
            sessionList.Add("2020-21", DateTime.Parse("30-04-2021 11:59:59 PM").ToUniversalTime());
        }

        public static List<string> GetSessionList()
        {
            return sessionList.Keys.ToList<string>();
        }

        public static DateTime GetExpiryDateBySessionString(string session)
        {
            return sessionList[session];
        }
    }
}
