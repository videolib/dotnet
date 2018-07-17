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
        private ClientInfo clientInfo = null;
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
            //if (clientInfo==null || ( clientInfo.LastAccessEndTime.CompareTo(DateTime.UtcNow) <= 0 && clientInfo.LastAccessStartTime.CompareTo(DateTime.UtcNow) <= 0 && clientInfo.ExpiryDate.CompareTo(DateTime.UtcNow) > 0))
            //{
            //    MessageBox.Show("License is expired.");
            //}

            //bool authenticated = LBSVideoLib.Common.Authentication.AuthenticateClient(txtEmailId.Text.Trim(), txtPwd.Text.Trim());

            //if (authenticated)
            //{
                frmDashboard frm = new frmDashboard();
                // frm.MdiParent = this.MdiParent;
                frm.ParentFormControl = this;
                frm.ClientInfoObject = clientInfo;
                frm.Show();
                this.Hide();
            //}
            //else
            //{
            //    lblStatus.Text = "Invalid Email Id or Password!!";
            //}
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            _clientInfoFilePath = ClientInfo.GetClientInfoFilePath();
            if (!File.Exists(_clientInfoFilePath))
            {
                MessageBox.Show("Invalid Configuration","Configuration Error", MessageBoxButtons.OK);
            }
            // Check license duraion
            clientInfo = Cryptograph.DecryptObject<ClientInfo>(_clientInfoFilePath);

        }

       
    }
}
