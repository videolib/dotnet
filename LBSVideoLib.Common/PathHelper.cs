using System;
using System.IO;

namespace LBFVideoLib.Common
{
    public class ClientHelper
    {

        #region Client Path Helper Methods
        public static string GetClientRootPath()
        {

            //  return Directory.GetCurrentDirectory();
            return @"D:\School\ClientPackages\c\Package";
        }



        public static string GetClientInfoFilePath()
        {
            return Path.Combine(GetClientRootPath(), ConfigHelper.ClientInfoFileName);
        }

        public static string GetClientVideoFilePath(string schoolCode, string schoolCity)
        {
            string parentPath = Directory.GetParent(GetClientRootPath()).FullName;

            return Path.Combine(parentPath, string.Format("{0}_{1}_{2}", schoolCode, schoolCity, "LBFVideos"));
        }

        public static string GetClientThumbanailPath()
        {
            return Path.Combine(GetClientRootPath(), "Thumbnails");
        }
        #endregion

        #region Admin Path Helper Methods

        public static string GetRegisteredSchoolInfoFilePath()
        {
            return Path.Combine(Directory.GetCurrentDirectory(), "ClientRegistrationData");
        }

        public static string GetClientRegistrationPackagePath(string schoolCode)
        {
            return Path.Combine(ConfigHelper.ClientDistributionTargetRootPath, Path.Combine(schoolCode, "Package"));
        }

        public static string GetClientRegistratinThumbnailPath(string schoolCode)
        {
            return Path.Combine(GetClientRegistrationPackagePath(schoolCode), "Thumbnails");
        }

        //// This path is use to create client package during registration from admin form.
        //public static string GetRegisteredClientDistributionRootPath(string schoolCode)
        //{

        //    //  return Directory.GetCurrentDirectory();
        //    Path.Combine(ConfigHelper.ClientDistributionTargetRootPath 
        //    return @"C:\LBFSetup\Registration\ClientPackages\786\Package";
        //}

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
            string registeredSchoolTitle = GetRegisteredSchoolTitleString(schoolName, schoolCity, schoolCode);
            return string.Format("Welcome, {0}", registeredSchoolTitle);
        }

        public static string GetRegisteredSchoolTitleString(string schoolName, string schoolCity, string schoolCode)
        {
            return string.Format("{0}, {1}, [{2}]", schoolName, schoolCity, schoolCode);
        }

        public static string GetExpiryDateString(DateTime expiryDate)
        {
            return string.Format("Expires on {0}", expiryDate.ToString("dd MMMM yyyy"));
        }

        public static string GetSubjectThumbnailSourcePath()
        {
            return Path.Combine(ConfigHelper.ClientDistributionPath, "Thumbnails");
        }

        public static string GetDemoVideoSourcePath()
        {
            return Path.Combine(ConfigHelper.ClientDistributionPath, "DemoVideos");
        }


        #endregion
    }
}
