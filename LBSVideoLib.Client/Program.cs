using LBFVideoLib.Common;
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
                    //if (online)
                    //{
                    //    // update it in firebase database.
                    CommonAppStateDataHelper.ClientInfoObject.SessionList.Clear();
                    //}
                    CommonAppStateDataHelper.ClientInfoObject.LastAccessEndTime = DateTime.Now;
                    Cryptograph.EncryptObject(CommonAppStateDataHelper.ClientInfoObject, ClientHelper.GetClientInfoFilePath());
                }
            }
            catch { }
        }


    }
}
