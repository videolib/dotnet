﻿using LBFVideoLib.Common;
using System;
using System.IO;
using System.Windows.Forms;

namespace LBFVideoLib.Client
{
    public partial class frmLogin : Form
    {
        private string _clientInfoFilePath = "";
        // Check license duraion
        private ClientInfo _clientInfo = null;

        public frmLogin()
        {
            InitializeComponent();
        }

        #region Form Events

        private void frmLogin_Load(object sender, EventArgs e)
        {
            //ClientHelper.GetClientThumbanailPath();

            _clientInfoFilePath = ClientHelper.GetClientInfoFilePath();
            if (!File.Exists(_clientInfoFilePath))
            {
                MessageBox.Show("Invalid Configuration", "Configuration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            CommonAppStateDataHelper.ClientInfoObject = Cryptograph.DecryptObject<ClientInfo>(_clientInfoFilePath);
            _clientInfo = CommonAppStateDataHelper.ClientInfoObject;

            if (_clientInfo != null)
            {
                lblSessionYears.Text = ClientHelper.GetSessionString(_clientInfo.SessionString);
                lblSchoolWelcome.Text = ClientHelper.GetWelcomeString(_clientInfo.SchoolName, _clientInfo.SchoolCity, _clientInfo.SchoolId);
                lblExpireDate.Text = ClientHelper.GetExpiryDateString(_clientInfo.ExpiryDate);
            }

            // Check license duraion
            string message = ""; bool deleteVideos = false;
            bool valid = LicenseHelper.CheckLicenseValidity(_clientInfo, out message, out deleteVideos);

            if (deleteVideos && valid == false)
            {
                Directory.Delete(ClientHelper.GetClientVideoFilePath(_clientInfo.SchoolId, _clientInfo.SchoolCity), true);
            }
            if (valid == false)
            {
                MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();
            }
        }

        #endregion

        #region Control Events
        private void btnLogin_Click(object sender, EventArgs e)
        {
            bool authenticated = Authentication.AuthenticateClient(txtEmailId.Text.Trim(), txtPwd.Text.Trim());

            if (authenticated)
            {
                SessionInfo sessionInfo = new SessionInfo();
                sessionInfo.StartTime = DateTime.Now;
                //_clientInfo.LastAccessStartTime = DateTime.UtcNow;
                //_clientInfo.LastAccessEndTime = DateTime.UtcNow;
                _clientInfo.SessionList.Add(sessionInfo);
                Cryptograph.EncryptObject(_clientInfo, ClientHelper.GetClientInfoFilePath());

                frmDashboard frm = new frmDashboard();
                // frm.MdiParent = this.MdiParent;
                frm.ParentFormControl = this;
                frm.ClientInfoObject = CommonAppStateDataHelper.ClientInfoObject;//_clientInfo
                CommonAppStateDataHelper.PushForm(this);
                frm.Show();
                this.Hide();
            }
            else
            {
                lblStatus.Text = "Invalid Email Id or Password!!";
            }
        }

        private void lblShowPwd_Click(object sender, EventArgs e)
        {
            if (txtPwd.PasswordChar == '*')
            {
                txtPwd.PasswordChar = '\0';
                lblShowPwd.Text = "Hide";
            }
            else
            {
                lblShowPwd.Text = "Show";
                txtPwd.PasswordChar = '*';
            }
        }

        private void lblContact_Click(object sender, EventArgs e)
        {
            MessageBox.Show(ClientHelper.GetContactMessageString(), "Contact", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void lblForgotPwd_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(string.Format("mailto:info@lbf.in?subject={0}-{1}", _clientInfo.SchoolName, _clientInfo.SchoolId));
        }
        #endregion

        private void lblPrivacyPolicy_Click(object sender, EventArgs e)
        {
            frmPri
        }
    }
}
