using LBFVideoLib.Common;
using LBFVideoLib.Common.Entity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace LBFVideoLib.Client
{
    public partial class frmVideoLibrary : Form
    {
        private string _clientRootPath = "";
        private string _clientInfoFilePath = "";
        private string _clientVideoRootFilePath = "";
        public Form ParentFormControl { get; set; }
        public Form DashboardFormControl { get; set; }
        public ClientInfo ClientInfoObject { get; set; }
        public TreeNode SelectedNode { get; set; }
        List<ThumbnailInfo> videFilePathList = new List<ThumbnailInfo>();
        private TreeNode _lastSelectedNode = null;
        public frmVideoLibrary()
        {
            InitializeComponent();
        }

        private void frmVideoLibrary_Load(object sender, EventArgs e)
        {
            _clientInfoFilePath = ClientHelper.GetClientInfoFilePath();
            this.ClientInfoObject.LastAccessEndTime = DateTime.UtcNow;
            this.ClientInfoObject.LastAccessStartTime = DateTime.UtcNow;
            Cryptograph.EncryptObject(this.ClientInfoObject, _clientInfoFilePath);

            _clientRootPath = ClientHelper.GetClientRootPath();
            _clientVideoRootFilePath = ClientHelper.GetClientVideoFilePath(this.ClientInfoObject.SchoolId, this.ClientInfoObject.SchoolCity);
            lblSessionYears.Text = ClientHelper.GetSessionString(ClientInfoObject.SessionString);
            lblSchoolWelcome.Text = ClientHelper.GetWelcomeString(ClientInfoObject.SchoolName, ClientInfoObject.SchoolCity, ClientInfoObject.SchoolId);
            lblExpireDate.Text = ClientHelper.GetExpiryDateString(ClientInfoObject.ExpiryDate);

            FillTreeView();
            treeView1.CollapseAll();
        }

        #region Private Methods

        private void FillTreeView()
        {
            treeView1.Nodes.Clear();

            string[] rootDirectoryList = Directory.GetDirectories(_clientVideoRootFilePath);
            for (int i = 0; i < rootDirectoryList.Length; i++)
            {
                TreeNode rootNode = new TreeNode(Path.GetFileName(rootDirectoryList[i]));
                TreeTag treeTag = new TreeTag();
                treeTag.CurrentDirectoryPath = rootDirectoryList[i];
                rootNode.Tag = treeTag;
                treeView1.Nodes.Add(rootNode);
                AddTreeNode(rootNode, rootDirectoryList[i], "");

            }
        }

        private void AddTreeNode(TreeNode parentNode, string currentDirectoryPath, string selectedNodeFullPath)
        {
            string[] directoryList = Directory.GetDirectories(currentDirectoryPath);
            string[] fileList = Directory.GetFiles(currentDirectoryPath);
            if (fileList.Length > 0)
            {
                (parentNode.Tag as TreeTag).BookVideoList = new List<string>(fileList);
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
                    //if (rootNode.FullPath.Equals(selectedNodeFullPath))
                    //{
                    //    treeView1.SelectedNode = rootNode;
                    //}
                    AddTreeNode(rootNode, directoryList[i], "");
                }
            }
        }


        #endregion

        private void frmVideoLibrary_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void myButton1_Click(object sender, EventArgs e)
        {
            OpenUpcomingVideoForm(null);
        }

        private void OpenUpcomingVideoForm(string[] nextVideoFileList)
        {
            frmUpCommingVideo upcomingVideoForm = new frmUpCommingVideo();
            upcomingVideoForm.ParentFormControl = this;
            upcomingVideoForm.ClientInfoObject = this.ClientInfoObject;
            upcomingVideoForm.EncryptedVideo = false;
            upcomingVideoForm.NextVideoFileList = new string[] { Path.Combine(ClientHelper.GetClientVideoFilePath(ClientInfoObject.SchoolId, ClientInfoObject.SchoolCity), @"First\First-S1\First-S1-English\First-S1-English-Basic\VID-20150929-WA0005.mp4") };
            upcomingVideoForm.Show();
            this.Hide();
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {

            TreeTag currentNodeTag = ((e.Node as TreeNode).Tag as TreeTag);
            if (_lastSelectedNode != null && e.Node.Equals(_lastSelectedNode))
            {
                return;
            }
            _lastSelectedNode = e.Node;
            videFilePathList.Clear();
            if (currentNodeTag.BookVideoList.Count == 0)
            {
                string[] currentDirectoryList = Directory.GetDirectories(currentNodeTag.CurrentDirectoryPath);
                for (int i = 0; i < currentDirectoryList.Length; i++)
                {
                    GetVideoFileList(currentDirectoryList[i], videFilePathList);
                }
            }
            else
            {
                GetVideoFileList(currentNodeTag.CurrentDirectoryPath, videFilePathList);
            }

            flowLayoutVideoListPnl.Controls.Clear();
            for (int j = 0; j < videFilePathList.Count; j++)
            {
                //CustomeThumbControl ctlThumb = new CustomeThumbControl();
                //ctlThumb.ThumbName = videFilePathList[j].FileName;
                //string thumbnailPath = videFilePathList[j].ThumbnailFilePath; ;// Path.Combine(ClientHelper.GetClientRootPath(), "Thumbnails");
                ////string thumbnailSubjectPath = Path.Combine(thumbnailPath, "Subject");
                //ctlThumb.ThumbUrl = videFilePathList[j].ThumbnailFilePath; //Path.Combine(thumbnailSubjectPath, "Subjects_ENGLISH.png");
                //ctlThumb.Click += CtlThumb_Click;
                ////ctlThumb
                //flowLayoutVideoListPnl.Controls.Add(ctlThumb);
            }


        }

        private void CtlThumb_Click(object sender, EventArgs e)
        {
           
        }

        private void GetVideoFileList(string currentPath, List<ThumbnailInfo> videFilePathList)
        {
            if (Directory.GetFiles(currentPath).Length > 0)
            {
                string[] fileList = Directory.GetFiles(currentPath);
                for (int i = 0; i < fileList.Length; i++)
                {
                    ThumbnailInfo thumbnailInfo = new ThumbnailInfo();
                    thumbnailInfo.VideoFullUrl = fileList[i];
                    thumbnailInfo.FileName = Path.GetFileName(fileList[i]);
                    thumbnailInfo.ThumbnailFilePath = "";
                    videFilePathList.Add(thumbnailInfo);
                }
            }
            else
            {
                string[] currentDirectoryList = Directory.GetDirectories(currentPath);
                for (int i = 0; i < currentDirectoryList.Length; i++)
                {
                    GetVideoFileList(currentDirectoryList[i], videFilePathList);
                }
            }
        }
    }
}
