using LBFVideoLib.Common;
using LBFVideoLib.Common.Entity;
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

        public Form DashboardFormControl { get; set; }

        public string SelectedVideo { get; set; }

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
            lblWelcome.Text = ClientHelper.GetWelcomeString(ClientInfoObject.SchoolName, ClientInfoObject.SchoolCity, ClientInfoObject.SchoolId);
            lblExpireDate.Text = ClientHelper.GetExpiryDateString(ClientInfoObject.ExpiryDate);


            if (this.NextVideoFileList.Length > 0)
            {

                if (EncryptedVideo)
                {
                    //string tempDirectory = Path.Combine(Path.GetDirectoryName(this.NextVideoFileList[0]), "Temp");
                    string tempDirectory = Path.Combine(Path.GetTempPath(), "Temp");
                    System.IO.Directory.CreateDirectory(tempDirectory);
                    string tempFilePath = Path.Combine(tempDirectory, Path.GetFileName(this.NextVideoFileList[0]));
                    Cryptograph.DecryptFile(this.NextVideoFileList[0], tempFilePath);
                    this.axWindowsMediaPlayer1.URL = tempFilePath;
                }
                else
                {
                    this.axWindowsMediaPlayer1.URL = this.NextVideoFileList[0];
                }

                this.lblFileName.Text = Path.GetFileNameWithoutExtension(this.NextVideoFileList[0]);
                //   this.axWindowsMediaPlayer1.URL = this.NextVideoFileList[0];
            }

            AddMostWatchesVideos();


        }


        private void AddMostWatchesVideos()
        {
            string thumbnailPath = Path.Combine(ClientHelper.GetClientRootPath(), "Thumbnails");
            string demoVideoPath = Path.Combine(ClientHelper.GetClientRootPath(), "DemoVideos");

            CustomeThumbControl ctlThumb = new CustomeThumbControl(this.CtlThumb_Click);
            ctlThumb.ThumbName = "EGBC01F002L01P002";
            ctlThumb.VideoUrl = Path.Combine(demoVideoPath, "EGBC01F002L01P002.mp4");
            ctlThumb.ThumbUrl = Path.Combine(thumbnailPath, "Subjects_ENGLISH.png");
            ctlThumb.Click += CtlThumb_Click;
            ctlThumb.Height = 150;
            ctlThumb.Width = 150;
            pnlVideo.Controls.Add(ctlThumb);

            ctlThumb = new CustomeThumbControl(this.CtlThumb_Click);
            ctlThumb.ThumbName = "HVKC01F002L001P002";
            ctlThumb.VideoUrl = Path.Combine(demoVideoPath, "HVKC01F002L001P002.mp4");
            ctlThumb.ThumbUrl = Path.Combine(thumbnailPath, "Subjects_HINDI.png");
            ctlThumb.Click += CtlThumb_Click;
            ctlThumb.Height = 150;
            ctlThumb.Width = 150;
            pnlVideo.Controls.Add(ctlThumb);

            ctlThumb = new CustomeThumbControl(this.CtlThumb_Click);
            ctlThumb.ThumbName = "EGBC02F026L07P048";
            ctlThumb.VideoUrl = Path.Combine(demoVideoPath, "EGBC02F026L07P048.mp4");
            ctlThumb.ThumbUrl = Path.Combine(thumbnailPath, "Subjects_ENGLISH.png");
            ctlThumb.Click += CtlThumb_Click;
            ctlThumb.Height = 150;
            ctlThumb.Width = 150;
            pnlVideo.Controls.Add(ctlThumb);

            ctlThumb = new CustomeThumbControl(this.CtlThumb_Click);
            ctlThumb.ThumbName = "HVKC01F006L003P017";
            ctlThumb.VideoUrl = Path.Combine(demoVideoPath, "HVKC01F006L003P017.mp4");
            ctlThumb.ThumbUrl = Path.Combine(thumbnailPath, "Subjects_HINDI.png");
            ctlThumb.Click += CtlThumb_Click;
            ctlThumb.Height = 150;
            ctlThumb.Width = 150;
            pnlVideo.Controls.Add(ctlThumb);

            ctlThumb = new CustomeThumbControl(this.CtlThumb_Click);
            ctlThumb.ThumbName = "HVKC01F038L016P105";
            ctlThumb.VideoUrl = Path.Combine(demoVideoPath, "HVKC01F038L016P105.mp4");
            ctlThumb.ThumbUrl = Path.Combine(thumbnailPath, "Subjects_HINDI.png");
            ctlThumb.Click += CtlThumb_Click;
            ctlThumb.Height = 150;
            ctlThumb.Width = 150;
            pnlVideo.Controls.Add(ctlThumb);
        }

        private void CtlThumb_Click(object sender, EventArgs e)
        {
            CustomeThumbControl ctl = sender as CustomeThumbControl;
            this.lblFileName.Text = Path.GetFileNameWithoutExtension(ctl.VideoUrl);
            this.axWindowsMediaPlayer1.URL = ctl.VideoUrl;
            this.EncryptedVideo = false;
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
                TreeTag treeTag = new TreeTag();
                treeTag.CurrentDirectoryPath = rootDirectoryList[i];
                rootNode.Tag = treeTag;
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

            else
            {
                for (int i = 0; i < directoryList.Length; i++)
                {
                    TreeNode rootNode = new TreeNode(Path.GetFileName(directoryList[i]));
                    TreeTag treeTag = new TreeTag();
                    treeTag.CurrentDirectoryPath = directoryList[i];
                    rootNode.Tag = treeTag;
                    parentNode.Nodes.Add(rootNode);
                    AddTreeNode(rootNode, directoryList[i]);
                }
            }
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

        }

        private void btnRewind_Click(object sender, EventArgs e)
        {

        }

        private void btnFastForward_Click(object sender, EventArgs e)
        {
        }

        private void frmUpCommingVideo_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.ParentFormControl.Name == "frmVideoLibrary")
            {
                this.ParentFormControl.Show();
            }
            else
            {
                frmVideoLibrary frmVideoLibrary = new frmVideoLibrary();
                frmVideoLibrary.ParentFormControl = this.DashboardFormControl;
                frmVideoLibrary.DashboardFormControl = this.DashboardFormControl;
                frmVideoLibrary.ClientInfoObject = this.ClientInfoObject;
                frmVideoLibrary.Show();
            }
        }

        private void lblContact_Click(object sender, EventArgs e)
        {
            MessageBox.Show(ClientHelper.GetContactMessageString(), "Contact", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnRewind_Click_1(object sender, EventArgs e)
        {
            this.axWindowsMediaPlayer1.Ctlcontrols.currentPosition -= 10;

        }

        private void btnFullScreen_Click_1(object sender, EventArgs e)
        {
            if (this.axWindowsMediaPlayer1.fullScreen == false)
                this.axWindowsMediaPlayer1.fullScreen = true;
        }

        private void btnFastForward_Click_1(object sender, EventArgs e)
        {
            this.axWindowsMediaPlayer1.Ctlcontrols.currentPosition += 10;

        }

        private void pnlLogo_Click(object sender, EventArgs e)
        {
            this.Close();
            this.DashboardFormControl.Show();
        }

        private void treeView1_NodeMouseClick_1(object sender, TreeNodeMouseClickEventArgs e)
        {
            this.Close();

        }
    }
}
