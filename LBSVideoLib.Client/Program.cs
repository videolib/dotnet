using LBFVideoLib.Common;
using LBFVideoLib.Common.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace LBFVideoLib.Client
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // Handle the ApplicationExit event to know when the application is exiting.
            Application.ApplicationExit += new EventHandler(OnApplicationExit);
            Application.Run(new frmLogin());
        }

        private static void OnApplicationExit(object sender, EventArgs e)
        {
            try
            {
                if (CommonAppStateDataHelper.ClientInfoObject.SessionList.Count > 0)
                {
                    int count = CommonAppStateDataHelper.ClientInfoObject.SessionList.Count;
                    CommonAppStateDataHelper.ClientInfoObject.SessionList[count - 1].EndTime = DateTime.Now;
                    CommonAppStateDataHelper.ClientInfoObject.LastAccessEndTime = DateTime.Now;
                    // update it in firebase database.
                    for (int i = 0; i < count; i++)
                    {
                        SaveSessionOnFireBase(CommonAppStateDataHelper.ClientInfoObject.SchoolId,
                            CommonAppStateDataHelper.ClientInfoObject.SessionList[i].StartTime,
                            CommonAppStateDataHelper.ClientInfoObject.SessionList[i].EndTime);
                    }

                    CommonAppStateDataHelper.ClientInfoObject.SessionList.Clear();
                    //}
                }
            }
            catch { }
            finally
            {
                if (CommonAppStateDataHelper.LicenseError == false && CommonAppStateDataHelper.ClientInfoObject != null)
                {
                    CommonAppStateDataHelper.ClientInfoObject.LastAccessEndTime = DateTime.Now;
                }
                Cryptograph.EncryptObject(CommonAppStateDataHelper.ClientInfoObject, ClientHelper.GetClientInfoFilePath());
            }
        }

        private static void SaveSessionOnFireBase(string schoolCode, DateTime sessionStartTime, DateTime sessionEndTime)
        {
            SessionInfoFB info = new SessionInfoFB();
            info.MachineName = Environment.MachineName;
            info.SessionStartTime = sessionStartTime;
            info.SessionEndTime = sessionEndTime;

            string jsonString = JsonHelper.ParseObjectToJSON<SessionInfoFB>(info);
            string url = string.Format("clientanalytic-data/{0}/session", schoolCode);
            FirebaseHelper.PostData(jsonString, url);
        }


    }
}
