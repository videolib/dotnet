using LBFVideoLib.Common;
using LBFVideoLib.Common.Entity;
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
        private bool _validLicense = false;
        private bool _showLoginForm = true;
        private string _currentMacAddress = "";

        public frmLogin()
        {
            InitializeComponent();
        }

        #region Form Events

        private void frmLogin_Load(object sender, EventArgs e)
        {
            this.progressBar1.Visible = true;
            this.progressBar1.Enabled = true;
            this.progressBar1.Value = 10;
            //ClientHelper.GetClientThumbanailPath();

            _clientInfoFilePath = ClientHelper.GetClientInfoFilePath();
            if (!File.Exists(_clientInfoFilePath))
            {
                MessageBox.Show("Invalid Configuration", "Configuration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            this.progressBar1.Value = 30;

            CommonAppStateDataHelper.ClientInfoObject = Cryptograph.DecryptObject<ClientInfo>(_clientInfoFilePath);
            _clientInfo = CommonAppStateDataHelper.ClientInfoObject;
            this.progressBar1.Value = 70;
            if (_clientInfo != null)
            {
                lblSessionYears.Text = ClientHelper.GetSessionString(_clientInfo.SessionString);
                lblSchoolWelcome.Text = ClientHelper.GetWelcomeString(_clientInfo.SchoolName, _clientInfo.SchoolCity, _clientInfo.SchoolId);
                lblExpireDate.Text = ClientHelper.GetExpiryDateString(_clientInfo.SessionEndDate);

                _currentMacAddress = MacAddressHelper.GetMacAddress();

                _validLicense = ValidateNoOfLicense(_currentMacAddress);

                this.progressBar1.Value = 75;

                // update mac address in local client info file
                if (_validLicense)
                {
                    this.progressBar1.Value = 90;
                    if (string.IsNullOrEmpty(CommonAppStateDataHelper.ClientInfoObject.MacAddress))
                    {
                        _showLoginForm = true;
                    }
                    else
                    {
                        _showLoginForm = false;
                    }
                }
                else
                {
                    this.progressBar1.Value = 100;
                    Application.Exit();
                }
            }
            //if (_clientInfo.LastAccessEndTime.Equals(DateTime.MinValue))
            //{
            //    _clientInfo.LastAccessEndTime = _clientInfo.RegistrationDate;
            //}


            // Check license duraion
            ValidateLicense();
        }


        private bool ValidateNoOfLicense(string currentMacAddress)
        {

            //RegInfoFB regInfo = GetRegInfoFromFirebase(_clientInfo.SchoolId, _clientInfo.SessionString);

            //if (!regInfo.MacAddresses.Contains(currentMacAddress)){

            //    if (regInfo.MacAddresses.Count >= regInfo.NoOfPcs)
            //    {

            //        MessageBox.Show("Number of licenses exceeded", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //        return;
            //    }
            //    else
            //    {
            //        regInfo.MacAddresses.Add(currentMacAddress);
            //        UpdateRegInfo(regInfo);
            //    }
            //}

            bool validLicense = false;
            RegInfoFB regInfo = GetRegInfoFromFirebase(_clientInfo.SchoolId, _clientInfo.SessionString);

            if (!regInfo.MacAddresses.Contains(currentMacAddress))
            {

                if (regInfo.MacAddresses.Count >= regInfo.NoOfPcs)
                {
                    validLicense = false;
                    MessageBox.Show("Number of licenses exceeded", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    regInfo.MacAddresses.Add(currentMacAddress);
                    UpdateRegInfo(regInfo);
                    validLicense = true;
                }
            }
            else
            {
                if (regInfo.MacAddresses.Count >= regInfo.NoOfPcs)
                {
                    validLicense = false;
                    MessageBox.Show("Number of licenses exceeded", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    validLicense = true;
                }

            }
            return validLicense;
        }

        private void ValidateLicense()
        {
            string message = ""; bool deleteVideos = false;
            LicenseValidationState licenseState = LicenseValidationState.None;
            if (CommonAppStateDataHelper.ClientInfoObject.Expired)
            {
                deleteVideos = true;
                message = LicenseHelper.licenseExpiredMessage;
                licenseState = LicenseValidationState.Expired;
            }
            else
            {
                licenseState = LicenseHelper.CheckLicenseState(_clientInfo, out message, out deleteVideos);
            }

            if (deleteVideos && licenseState == LicenseValidationState.Expired)
            {
                CommonAppStateDataHelper.ClientInfoObject.Expired = true;
                CommonAppStateDataHelper.LicenseError = true;

                if (Directory.Exists(ClientHelper.GetClientVideoFilePath(_clientInfo.SchoolId, _clientInfo.SchoolCity)))
                {
                    Directory.Delete(ClientHelper.GetClientVideoFilePath(_clientInfo.SchoolId, _clientInfo.SchoolCity), true);
                }

                MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                this.Close();
            }
            else if (licenseState != LicenseValidationState.Valid)
            {
                CommonAppStateDataHelper.LicenseError = true;
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
                OnAfterAuthentication();
            }
            else
            {
                lblStatus.Text = "Invalid Email Id or Password!!";
            }
        }

        private void OnAfterAuthentication()
        {
            SessionInfo sessionInfo = new SessionInfo();
            sessionInfo.StartTime = DateTime.Now;
            _clientInfo.LastAccessStartTime = DateTime.Now;
            _clientInfo.LastAccessEndTime = DateTime.Now;
            _clientInfo.SessionList.Add(sessionInfo);
            if (string.IsNullOrEmpty(_clientInfo.MacAddress))
            {
                _clientInfo.MacAddress = _currentMacAddress;
            }

            FileInfo clientInfoFileInfo = new FileInfo(ClientHelper.GetClientInfoFilePath());
            try
            {
                clientInfoFileInfo.Attributes &= ~FileAttributes.Hidden;
                // this.ClientInfoObject.LastAccessStartTime = DateTime.UtcNow;
                Cryptograph.EncryptObject(_clientInfo, ClientHelper.GetClientInfoFilePath());
                clientInfoFileInfo.Attributes |= FileAttributes.Hidden;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                clientInfoFileInfo.Attributes |= FileAttributes.Hidden;
            }

            frmDashboard frm = new frmDashboard();
            // frm.MdiParent = this.MdiParent;
            frm.ParentFormControl = this;
            frm.ClientInfoObject = CommonAppStateDataHelper.ClientInfoObject;
            CommonAppStateDataHelper.AddForm(this);
            frm.Show();
            this.Hide();
            CommonAppStateDataHelper.LoggedIn = true;
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
            frmPrivacyPolicy frm = new frmPrivacyPolicy();
            frm.Show();
        }


        private static RegInfoFB GetRegInfoFromFirebase(string schoolCode, string sessionYear)
        {
            string url = string.Format("registrations-data/{0}-{1}", schoolCode, sessionYear);
            string jsonString = FirebaseHelper.GetData(url);
            return JsonHelper.ParseJsonToObject<RegInfoFB>(jsonString) as RegInfoFB;
        }


        private void UpdateRegInfo(RegInfoFB info)
        {
            string jsonString1 = JsonHelper.ParseObjectToJSON<RegInfoFB>(info);
            string url = string.Format("registrations-data/{0}-{1}", _clientInfo.SchoolId, _clientInfo.SessionString);
            FirebaseHelper.PatchData(jsonString1, url);
        }

        private void frmLogin_Shown(object sender, EventArgs e)
        {
            this.progressBar1.Value = 100;

            if (_showLoginForm == false)
            {
                OnAfterAuthentication();
            }
            else
            {
                btnLogin.Visible = true;
                lblEmail.Visible = true;
                txtEmailId.Visible = true;
                lblPassword.Visible = true;
                lblShowPwd.Visible = true;
                txtPwd.Visible = true;
                linklblForgotPwd.Visible = true;
                lblShowContact.Visible = true;
                progressBar1.Visible = false;
            }


        }
    }
}
