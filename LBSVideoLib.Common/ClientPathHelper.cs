using System;
using System.IO;

namespace LBFVideoLib.Common
{
    public class ClientHelper
    {

        #region Path Helper Methods
        public static string GetClientRootPath()
        {
            //return Directory.GetCurrentDirectory();
            return @"D:\School\ClientPackages\1111";
        }

        public static string GetClientInfoFilePath()
        {
            return Path.Combine(GetClientRootPath(), Path.Combine("Package", ConfigHelper.ClientInfoFileName));
        }

        public static string GetClientVideoFilePath(string schoolCode, string schoolCity)
        {
            return Path.Combine(GetClientRootPath(), string.Format("{0}_{1}_{2}", schoolCode, schoolCity, "LBFVideos"));
        }

        public static string GetClientThumbanailPath()
        {
            return Path.Combine(GetClientRootPath(), "Thumbnails");
        }
        #endregion

        #region Message String Helper

        public static string GetContactMessageString()
        {
            return "In case of any query, feedback or if you have forgot your password\nContact: info@lbf or call on +91 0 9109138808";
        }

        public static string GetSessionString(string sessionString)
        {
            return string.Format("Session : {0}", sessionString);
        }

        public static string GetWelcomeString(string schoolName, string schoolCity, string schoolCode)
        {
            return string.Format("Welcome, {0} {1} {2}", schoolName, schoolCity, schoolCode);
        }

        public static string GetExpiryDateString(DateTime expiryDate)
        {
            return string.Format("Expires on {0}", expiryDate.ToString("dd MMMM yyyy"));
        }

        public static string GetSubjectThumbnailSourcePath()
        {
            return Path.Combine(ConfigHelper.ClientDistributionPath, "Thumbnails");
        }

    

        #endregion
    }
}
