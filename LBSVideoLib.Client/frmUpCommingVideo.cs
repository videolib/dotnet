using LBFVideoLib.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LBFVideoLib.Client
{
    public partial class frmUpCommingVideo : Form
    {
        private string _clientRootPath = "";
        private string _clientInfoFilePath = "";
        public Form ParentFormControl { get; set; }
        public ClientInfo ClientInfoObject { get; set; }
        public string[] NextVideoFileList
        {
            get; set;
        }

        public bool EncryptedVideo { get; set; }

        public frmUpCommingVideo()
        {
            InitializeComponent();
        }

        private void frmDashboard_Load(object sender, EventArgs e)
        {
            _clientRootPath = ClientHelper.GetClientRootPath();
            _clientInfoFilePath = ClientHelper.GetClientInfoFilePath();
            this.ClientInfoObject.LastAccessEndTime = DateTime.UtcNow;
            this.ClientInfoObject.LastAccessStartTime = DateTime.UtcNow;
            Cryptograph.EncryptObject(this.ClientInfoObject, _clientInfoFilePath);

            FillTreeView();
            treeView1.ExpandAll();

            lblSessionYears.Text = ClientHelper.GetSessionString(ClientInfoObject.SessionString);
            lblSchoolWelcome.Text = ClientHelper.GetWelcomeString(ClientInfoObject.SchoolName, ClientInfoObject.SchoolCity, ClientInfoObject.SchoolId);
            lblExpireDate.Text = ClientHelper.GetExpiryDateString(ClientInfoObject.ExpiryDate);


            if (this.NextVideoFileList.Length > 0)
            {
                this.lblFileName.Text = Path.GetFileNameWithoutExtension(this.NextVideoFileList[0]);
                this.axWindowsMediaPlayer1.URL = this.NextVideoFileList[0];
            }

            CustomeThumbControl ctlThumb = new CustomeThumbControl();
            ctlThumb.ThumbName = ".Net Crea Application";
            string thumbnailPath = Path.Combine(ClientHelper.GetClientRootPath(), "Thumbnails");
            string thumbnailSubjectPath = Path.Combine(thumbnailPath, "Subject");
            ctlThumb.ThumbUrl = Path.Combine (thumbnailSubjectPath, "Subjects_ENGLISH.png");
            pnlVideo.Controls.Add(ctlThumb);
        }


        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag != null)
            {
                frmVideoLibrary frmVideoLibrary = new frmVideoLibrary();
                frmVideoLibrary.ParentFormControl = this;
                frmVideoLibrary.SelectedNode = e.Node;
                frmVideoLibrary.ClientInfoObject = this.ClientInfoObject;
                this.Hide();
                frmVideoLibrary.Show();
            }
        }

        #region Private Methods

        private void FillTreeView()
        {
            treeView1.Nodes.Clear();
            // Fill Tree
            // get root
            string[] rootDirectoryList = Directory.GetDirectories(ClientHelper.GetClientVideoFilePath(ClientInfoObject.SchoolId, ClientInfoObject.SchoolCity));
            for (int i = 0; i < rootDirectoryList.Length; i++)
            {
                TreeNode rootNode = new TreeNode(Path.GetFileName(rootDirectoryList[i]));
                treeView1.Nodes.Add(rootNode);
                AddTreeNode(rootNode, rootDirectoryList[i]);
            }
        }

        private void AddTreeNode(TreeNode parentNode, string currentDirectoryPath)
        {
            string[] directoryList = Directory.GetDirectories(currentDirectoryPath);
            string[] fileList = Directory.GetFiles(currentDirectoryPath);
            if (fileList.Length > 0)
            {
                //for (int i = 0; i < fileList.Length; i++)
                //{
                //    TreeNode rootNode = new TreeNode(fileList[i]);
                //    parentNode.Nodes.Add(rootNode);                    
                //}
                parentNode.Tag = fileList;
            }

            //else { 
            for (int i = 0; i < directoryList.Length; i++)
            {
                TreeNode rootNode = new TreeNode(Path.GetFileName(directoryList[i]));
                parentNode.Nodes.Add(rootNode);
                AddTreeNode(rootNode, directoryList[i]);
            }
            //}
        }


        #endregion

        private void myButton12_Click(object sender, EventArgs e)
        {
            this.lblFileName.Text = "VID - 20160508 - WA0004";
            this.axWindowsMediaPlayer1.URL = Path.Combine(ClientHelper.GetClientVideoFilePath(ClientInfoObject.SchoolId, ClientInfoObject.SchoolCity), @"Third\Third-S2\Third-S2-Science\Third-S2-Science-Practical\VID-20160508-WA0004.mp4");
        }


        private void myButton11_Click(object sender, EventArgs e)
        {
            this.lblFileName.Text = "VID-20151009-WA0006";
            this.axWindowsMediaPlayer1.URL = Path.Combine(ClientHelper.GetClientVideoFilePath(ClientInfoObject.SchoolId, ClientInfoObject.SchoolCity), @"Third\Third-S2\Third-S2-Science\Third-S2-Science-Theory\VID-20151009-WA0006.mp4");
        }

        private void btnFullScreen_Click(object sender, EventArgs e)
        {
            if (this.axWindowsMediaPlayer1.fullScreen == false)
                this.axWindowsMediaPlayer1.fullScreen = true;
        }

        private void btnRewind_Click(object sender, EventArgs e)
        {
            this.axWindowsMediaPlayer1.Ctlcontrols.currentPosition -= 10;

        }

        private void btnFastForward_Click(object sender, EventArgs e)
        {
            this.axWindowsMediaPlayer1.Ctlcontrols.currentPosition += 30;
        }

        private void frmUpCommingVideo_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.ParentFormControl.Show();
        }

        private void lblContact_Click(object sender, EventArgs e)
        {
            MessageBox.Show(ClientHelper.GetContactMessageString(), "Contact", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
