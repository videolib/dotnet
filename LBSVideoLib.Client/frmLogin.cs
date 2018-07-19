using LBFVideoLib.Common;
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

        private void frmLogin_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            bool authenticated = Authentication.AuthenticateClient(txtEmailId.Text.Trim(), txtPwd.Text.Trim());

            if (authenticated)
            {
                _clientInfo.LastAccessStartTime = DateTime.UtcNow;
                _clientInfo.LastAccessEndTime = DateTime.UtcNow;
                Cryptograph.EncryptObject(_clientInfo, ClientHelper.GetClientInfoFilePath());

                frmDashboard frm = new frmDashboard();
                // frm.MdiParent = this.MdiParent;
                frm.ParentFormControl = this;
                frm.ClientInfoObject = _clientInfo;
                frm.Show();
                this.Hide();
            }
            else
            {
                lblStatus.Text = "Invalid Email Id or Password!!";
            }
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            _clientInfoFilePath = ClientHelper.GetClientInfoFilePath();
            if (!File.Exists(_clientInfoFilePath))
            {
                MessageBox.Show("Invalid Configuration", "Configuration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            _clientInfo = Cryptograph.DecryptObject<ClientInfo>(_clientInfoFilePath);
            if (_clientInfo != null)
            {
                lblSessionYears.Text = ClientHelper.GetSessionString(_clientInfo.SessionString);
                lblSchoolWelcome.Text = ClientHelper.GetWelcomeString(_clientInfo.SchoolName, _clientInfo.SchoolCity, _clientInfo.SchoolId);
                lblExpireDate.Text = ClientHelper.GetExpiryDateString(_clientInfo.ExpiryDate);
            }

            // Check license duraion
            string message = "";
            bool valid = LicenseHelper.CheckLicenseValidity(_clientInfo, out message);
            if (valid == false)
            {
                MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
    }
}
