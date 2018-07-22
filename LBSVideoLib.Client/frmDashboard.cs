using LBFVideoLib.Common;
using LBFVideoLib.Common.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace LBFVideoLib.Client
{
    public partial class frmDashboard : Form
    {
        //private string _clientRootPath = "";
        //private string _clientInfoFilePath = "";
        public Form ParentFormControl { get; set; }
        public ClientInfo ClientInfoObject { get; set; }
        private Dictionary<string, string> _bookVideoList = new Dictionary<string, string>();

        public frmDashboard()
        {
            InitializeComponent();
        }


        private void frmDashboard_Load(object sender, EventArgs e)
        {
            // FillVideoList();
            //FillRandomVideoList();

            lblSessionYears.Text = ClientHelper.GetSessionString(ClientInfoObject.SessionString);
            lblSchoolWelcome.Text = ClientHelper.GetWelcomeString(ClientInfoObject.SchoolName, ClientInfoObject.SchoolCity, ClientInfoObject.SchoolId);
            lblExpireDate.Text = ClientHelper.GetExpiryDateString(ClientInfoObject.ExpiryDate);

            FillTreeView();
            treeView1.ExpandAll();
            TreeViewHelper.TreeViewObject = treeView1;

            AddRecomVideos();
            AddMostWatchesVideos();
        }

        private Dictionary<string, string> FillRandomVideoList()
        {
            //List<int> randomNumberList = new List<int>();
            //Random random = new Random();
            //randomNumberList.Add(random.Next(0, _bookVideoList.Count - 1));
            return null;
        }

        private void FillVideoList()
        {
            // Fill video list
            string videoRelativePath = "";
            for (int i = 0; i < ClientInfoObject.SelectedVideoDetails.Count; i++)
            {
                ClassFB classFB = ClientInfoObject.SelectedVideoDetails[i];
                videoRelativePath = Path.Combine(ClientHelper.GetClientVideoFilePath(ClientInfoObject.SchoolId, ClientInfoObject.SchoolCity), classFB.Name);
                for (int k = 0; k < classFB.Series.Count; k++)
                {
                    string seriesVideoPath = Path.Combine(videoRelativePath, classFB.Series[k].Name);
                    for (int j = 0; j < classFB.Series[k].Subjects.Count; j++)
                    {
                        string subjectVideoPath = Path.Combine(seriesVideoPath, classFB.Series[k].Subjects[j].Name);
                        for (int l = 0; l < classFB.Series[k].Subjects[j].Books.Count; l++)
                        {
                            string bookVideoPath = Path.Combine(subjectVideoPath, classFB.Series[k].Subjects[j].Books[l].Name);
                            string[] videoList = Directory.GetFiles(bookVideoPath);
                            for (int m = 0; m < videoList.Length; m++)
                            {
                                _bookVideoList.Add(Path.Combine(bookVideoPath, videoList[m]), videoList[m]);
                            }
                        }
                    }
                }
            }
        }

        #region Private Methods

        private void FillTreeView()
        {
            treeView1.Nodes.Clear();

            // Fill Tree
            string[] rootDirectoryList = Directory.GetDirectories(ClientHelper.GetClientVideoFilePath(ClientInfoObject.SchoolId, ClientInfoObject.SchoolCity));
            for (int i = 0; i < rootDirectoryList.Length; i++)
            {
                //TreeNode rootNode = new TreeNode(ClientInfoObject.SchoolName);
                //treeView1.Nodes.Add(rootNode);
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

        private void OpenVideoLibrary()
        {
            frmVideoLibrary frmVideoLibrary = new frmVideoLibrary();
            frmVideoLibrary.ParentFormControl = this;
            frmVideoLibrary.DashboardFormControl = this;
            frmVideoLibrary.ClientInfoObject = this.ClientInfoObject;
            this.Hide();
            frmVideoLibrary.Show();
        }

        #endregion

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag != null)
            {
                OpenVideoLibrary();
            }
        }



        private void myButton1_Click(object sender, EventArgs e)
        {
            PlayVideo();
            // OpenVideoLibrary();
        }

        private void PlayVideo()
        {
            // frmPlayVideo playVideo = new frmPlayVideo();
            frmUpCommingVideo upcomingVideo = new frmUpCommingVideo();
            upcomingVideo.ParentFormControl = this;
            upcomingVideo.ClientInfoObject = this.ClientInfoObject;
            upcomingVideo.EncryptedVideo = false;
            upcomingVideo.NextVideoFileList = new string[] { Path.Combine(ClientHelper.GetClientVideoFilePath(ClientInfoObject.SchoolId, ClientInfoObject.SchoolCity), @"First\First-S1\First-S1-English\First-S1-English-Basic\VID-20150929-WA0005.mp4") };
            upcomingVideo.Show();
            this.Hide();
        }

        private void myButton8_Click(object sender, EventArgs e)
        {
            PlayVideo();
        }

        private void frmDashboard_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void lblContact_Click(object sender, EventArgs e)
        {
            MessageBox.Show(ClientHelper.GetContactMessageString(), "Contact", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void AddRecomVideos()
        {
            string thumbnailPath = Path.Combine(ClientHelper.GetClientRootPath(), "Thumbnails");
            string demoVideoPath = Path.Combine(ClientHelper.GetClientRootPath(), "DemoVideos");


            CustomeThumbControl ctlThumb = new CustomeThumbControl(this.CtlThumb_Click);
            ctlThumb.ThumbName = "EGBC01F002L01P002";
            ctlThumb.VideoUrl = Path.Combine(demoVideoPath, "EGBC01F002L01P002.mp4");
            ctlThumb.ThumbUrl = Path.Combine(thumbnailPath, "Subjects_ENGLISH.png");
            ctlThumb.Click += CtlThumb_Click;
            ctlThumb.Height = 180;
            ctlThumb.Width = 180;
            pnlRecomVideo.Controls.Add(ctlThumb);

            ctlThumb = new CustomeThumbControl(this.CtlThumb_Click);
            ctlThumb.ThumbName = "HVKC01F002L001P002";
            ctlThumb.VideoUrl = Path.Combine(demoVideoPath, "HVKC01F002L001P002.mp4");
            ctlThumb.ThumbUrl = Path.Combine(thumbnailPath, "Subjects_HINDI.png");
            ctlThumb.Click += CtlThumb_Click;
            ctlThumb.Height = 180;
            ctlThumb.Width = 180;
            pnlRecomVideo.Controls.Add(ctlThumb);

            ctlThumb = new CustomeThumbControl(this.CtlThumb_Click);
            ctlThumb.ThumbName = "EGBC02F026L07P048";
            ctlThumb.VideoUrl = Path.Combine(demoVideoPath, "EGBC02F026L07P048.mp4");
            ctlThumb.ThumbUrl = Path.Combine(thumbnailPath, "Subjects_ENGLISH.png");
            ctlThumb.Click += CtlThumb_Click;
            ctlThumb.Height = 180;
            ctlThumb.Width = 180;
            pnlRecomVideo.Controls.Add(ctlThumb);

            ctlThumb = new CustomeThumbControl(this.CtlThumb_Click);
            ctlThumb.ThumbName = "HVKC01F006L003P017";
            ctlThumb.VideoUrl = Path.Combine(demoVideoPath, "HVKC01F006L003P017.mp4");
            ctlThumb.ThumbUrl = Path.Combine(thumbnailPath, "Subjects_HINDI.png");
            ctlThumb.Click += CtlThumb_Click;
            ctlThumb.Height = 180;
            ctlThumb.Width = 180;
            pnlRecomVideo.Controls.Add(ctlThumb);

            ctlThumb = new CustomeThumbControl(this.CtlThumb_Click);
            ctlThumb.ThumbName = "HVKC01F038L016P105";
            ctlThumb.VideoUrl = Path.Combine(demoVideoPath, "HVKC01F038L016P105.mp4");
            ctlThumb.ThumbUrl = Path.Combine(thumbnailPath, "Subjects_HINDI.png");
            ctlThumb.Click += CtlThumb_Click;
            ctlThumb.Height = 180;
            ctlThumb.Width = 180;
            pnlRecomVideo.Controls.Add(ctlThumb);

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
            ctlThumb.Height = 180;
            ctlThumb.Width = 180;
            pnlMostWatchVideo.Controls.Add(ctlThumb);

            ctlThumb = new CustomeThumbControl(this.CtlThumb_Click);
            ctlThumb.ThumbName = "HVKC01F002L001P002";
            ctlThumb.VideoUrl = Path.Combine(demoVideoPath, "HVKC01F002L001P002.mp4");
            ctlThumb.ThumbUrl = Path.Combine(thumbnailPath, "Subjects_HINDI.png");
            ctlThumb.Click += CtlThumb_Click;
            ctlThumb.Height = 180;
            ctlThumb.Width = 180;
            pnlMostWatchVideo.Controls.Add(ctlThumb);

            ctlThumb = new CustomeThumbControl(this.CtlThumb_Click);
            ctlThumb.ThumbName = "EGBC02F026L07P048";
            ctlThumb.VideoUrl = Path.Combine(demoVideoPath, "EGBC02F026L07P048.mp4");
            ctlThumb.ThumbUrl = Path.Combine(thumbnailPath, "Subjects_ENGLISH.png");
            ctlThumb.Click += CtlThumb_Click;
            ctlThumb.Height = 180;
            ctlThumb.Width = 180;
            pnlMostWatchVideo.Controls.Add(ctlThumb);

            ctlThumb = new CustomeThumbControl(this.CtlThumb_Click);
            ctlThumb.ThumbName = "HVKC01F006L003P017";
            ctlThumb.VideoUrl = Path.Combine(demoVideoPath, "HVKC01F006L003P017.mp4");
            ctlThumb.ThumbUrl = Path.Combine(thumbnailPath, "Subjects_HINDI.png");
            ctlThumb.Click += CtlThumb_Click;
            ctlThumb.Height = 180;
            ctlThumb.Width = 180;
            pnlMostWatchVideo.Controls.Add(ctlThumb);

            ctlThumb = new CustomeThumbControl(this.CtlThumb_Click);
            ctlThumb.ThumbName = "HVKC01F038L016P105";
            ctlThumb.VideoUrl = Path.Combine(demoVideoPath, "HVKC01F038L016P105.mp4");
            ctlThumb.ThumbUrl = Path.Combine(thumbnailPath, "Subjects_HINDI.png");
            ctlThumb.Click += CtlThumb_Click;
            ctlThumb.Height = 180;
            ctlThumb.Width = 180;
            pnlMostWatchVideo.Controls.Add(ctlThumb);
        }

        private void CtlThumb_Click(object sender, EventArgs e)
        {
            CustomeThumbControl ctl = sender as CustomeThumbControl;

            frmUpCommingVideo upcomingVideoForm = new frmUpCommingVideo();
            upcomingVideoForm.ParentFormControl = this;
            upcomingVideoForm.ClientInfoObject = this.ClientInfoObject;
            upcomingVideoForm.EncryptedVideo = false;
            upcomingVideoForm.NextVideoFileList = new string[] { ctl.VideoUrl };
            upcomingVideoForm.EncryptedVideo = false;
            upcomingVideoForm.DashboardFormControl = this;
            upcomingVideoForm.Show();
            this.Hide();
        }

        private void pnlRecomVideo_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
