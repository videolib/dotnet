﻿using LBFVideoLib.Common;
using LBFVideoLib.Common.Entity;
using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
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
        private List<ThumbnailInfo> _videoFilePathList = new List<ThumbnailInfo>();
        //   private TreeNode _lastSelectedNode = null;
        private bool _searchApplied = false;
        List<ThumbnailInfo> _searchList = new List<ThumbnailInfo>();

        public Form ParentFormControl { get; set; }
        public Form DashboardFormControl { get; set; }
        public ClientInfo ClientInfoObject { get; set; }
        public TreeNode SelectedNode { get; set; }


        public frmVideoLibrary()
        {
            InitializeComponent();
        }

        #region Form Events

        private void frmVideoLibrary_Load(object sender, EventArgs e)
        {
            _clientInfoFilePath = ClientHelper.GetClientInfoFilePath();
            //this.ClientInfoObject.LastAccessEndTime = DateTime.UtcNow;
            //Cryptograph.EncryptObject(this.ClientInfoObject, _clientInfoFilePath);

            CommonAppStateDataHelper.ClientInfoObject.LastAccessEndTime = this.ClientInfoObject.LastAccessEndTime = DateTime.Now;
            Cryptograph.EncryptObject(this.ClientInfoObject, _clientInfoFilePath);

            _clientRootPath = ClientHelper.GetClientRootPath();
            _clientVideoRootFilePath = ClientHelper.GetClientVideoFilePath(this.ClientInfoObject.SchoolId, this.ClientInfoObject.SchoolCity);
            lblSessionYears.Text = ClientHelper.GetSessionString(ClientInfoObject.SessionString);
            lblSchoolWelcome.Text = ClientHelper.GetWelcomeString(ClientInfoObject.SchoolName, ClientInfoObject.SchoolCity, ClientInfoObject.SchoolId);
            lblExpireDate.Text = ClientHelper.GetExpiryDateString(ClientInfoObject.ExpiryDate);

            FillTreeView();
            treeView1.ExpandAll();

            if (this.SelectedNode != null)
            {
                TreeNode[] searchedNode = this.treeView1.Nodes.Find(this.SelectedNode.Name, true);
                if (searchedNode.Length > 0)
                {
                    this.treeView1.SelectedNode = searchedNode[0];
                }
            }
            else
            {
                this.treeView1.SelectedNode = this.treeView1.Nodes[0];
            }


            FillVideoLibrary(this.treeView1.SelectedNode.Tag as TreeTag);
        }

        private void frmVideoLibrary_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Application.Exit();
            (this.DashboardFormControl as frmDashboard).SelectedNode = treeView1.SelectedNode;
            this.DashboardFormControl.Show();
        }

        #endregion

        #region Tree Methods & Events

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
                rootNode.Name = rootNode.Text;
                TreeTag treeTag = new TreeTag();
                treeTag.CurrentDirectoryPath = rootDirectoryList[i];
                rootNode.Tag = treeTag;
                treeView1.Nodes.Add(rootNode);
                AddTreeNode(rootNode, rootDirectoryList[i]);
            }
        }

        private void AddTreeNode(TreeNode parentNode, string currentDirectoryPath)
        {
            string[] fileList = Directory.GetFiles(currentDirectoryPath);
            if (fileList.Length > 0)
            {
                TreeTag treeTag = new TreeTag();
                treeTag.CurrentDirectoryPath = Directory.GetParent(fileList[0]).FullName;
                treeTag.BookVideoList = new List<string>(fileList);
                parentNode.Tag = treeTag;
            }

            else
            {
                string[] directoryList = Directory.GetDirectories(currentDirectoryPath);

                for (int i = 0; i < directoryList.Length; i++)
                {
                    TreeNode rootNode = new TreeNode(Path.GetFileName(directoryList[i]));
                    rootNode.Name = rootNode.Text;
                    TreeTag treeTag = new TreeTag();
                    treeTag.CurrentDirectoryPath = directoryList[i];
                    rootNode.Tag = treeTag;
                    parentNode.Nodes.Add(rootNode);
                    AddTreeNode(rootNode, directoryList[i]);
                }
            }
        }

        #endregion


        private void myButton1_Click(object sender, EventArgs e)
        {
            OpenUpcomingVideoForm(null);
        }

        private void CtlThumb_Click(object sender, EventArgs e)
        {
            CustomeThumbControl ctl = sender as CustomeThumbControl;
            // Find index of currently selected video in list.

            List<ThumbnailInfo> nextVideoList = new List<ThumbnailInfo>();
            List<ThumbnailInfo> previousVideoList = new List<ThumbnailInfo>();
            if (_searchApplied)
            {
                CreatePreviousAndNextPlaylist(_searchList, ctl.VideoUrl, out nextVideoList, out previousVideoList);
            }
            else
            {
                CreatePreviousAndNextPlaylist(_videoFilePathList, ctl.VideoUrl, out nextVideoList, out previousVideoList);
            }

            frmUpCommingVideo upcomingVideoForm = new frmUpCommingVideo();
            upcomingVideoForm.ParentFormControl = this;
            upcomingVideoForm.ClientInfoObject = this.ClientInfoObject;
            upcomingVideoForm.EncryptedVideo = false;
            upcomingVideoForm.NextVideoFileList = nextVideoList;
            upcomingVideoForm.PreviousVideoFileList = previousVideoList;
            upcomingVideoForm.CurrentVideo = new ThumbnailInfo() { FileName = ctl.ThumbName, ThumbnailFilePath = ctl.ThumbUrl, VideoFullUrl= ctl.VideoUrl }; 
            upcomingVideoForm.EncryptedVideo = true;
            upcomingVideoForm.SelectedNode = this.treeView1.SelectedNode;
            upcomingVideoForm.DashboardFormControl = this.DashboardFormControl;
            upcomingVideoForm.Show();
            this.Hide();
        }

        private void CreatePreviousAndNextPlaylist(List<ThumbnailInfo> thumbnailList, string videoUrl, out List<ThumbnailInfo> nextVideoList, out List<ThumbnailInfo> previousVideoList)
        {
            nextVideoList = new List<ThumbnailInfo>();
            previousVideoList = new List<ThumbnailInfo>();
            int index = -1;

            index = thumbnailList.FindIndex(k => k.VideoFullUrl.Equals(videoUrl));
            for (int i = index + 1; (i < index + 1 + 3) && (i < thumbnailList.Count); i++)
            {
                nextVideoList.Add(thumbnailList[i]);
            }

            for (int i = index - 1; (i > index - 1 - 3) && (i > -1); i--)
            {
                previousVideoList.Add(thumbnailList[i]);
            }


        }

        private void lblContact_Click(object sender, EventArgs e)
        {
            MessageBox.Show(ClientHelper.GetContactMessageString(), "Contact", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void pnlLogo_Click(object sender, EventArgs e)
        {
            this.SelectedNode = this.treeView1.SelectedNode;
            this.DashboardFormControl.Show();
            this.Close();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtSearch.Text.Trim().ToLower();
            ApplySearch(searchText);
        }

        private void ApplySearch(string searchText)
        {
            if (searchText.Length > 0)
            {
                _searchApplied = true;
                //string searchText = txtSearch.Text.Trim().ToLower();
                _searchList = _videoFilePathList.Where(item => item.FileName.ToLower().Contains(searchText)).ToList<ThumbnailInfo>();
                AddVideoThumbnailControls(_searchList);
            }
            else if (_searchApplied)
            {
                _searchApplied = false;
                _searchList.Clear();
                AddVideoThumbnailControls(_videoFilePathList);
            }

        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            FillVideoLibrary(e.Node.Tag as TreeTag);
            string searchText = txtSearch.Text.Trim();
            if (searchText.Length > 0)
            {
                ApplySearch(searchText);
            }
        }

        private void OpenUpcomingVideoForm(string[] nextVideoFileList)
        {
            frmUpCommingVideo upcomingVideoForm = new frmUpCommingVideo();
            upcomingVideoForm.ParentFormControl = this;
            upcomingVideoForm.ClientInfoObject = this.ClientInfoObject;
            upcomingVideoForm.EncryptedVideo = false;
            //upcomingVideoForm.NextVideoFileList = new string[] { Path.Combine(ClientHelper.GetClientVideoFilePath(ClientInfoObject.SchoolId, ClientInfoObject.SchoolCity), @"First\First-S1\First-S1-English\First-S1-English-Basic\VID-20150929-WA0005.mp4") };
            upcomingVideoForm.DashboardFormControl = this.DashboardFormControl;
            upcomingVideoForm.Show();
            this.Hide();
        }

        private void FillVideoLibrary(TreeTag currentNodeTag)
        {
            //TreeTag currentNodeTag = ((e.Node as TreeNode).Tag as TreeTag);
            //if (_lastSelectedNode != null && e.Node.Text.Equals(_lastSelectedNode.Text))
            //{
            //    return;
            //}
            //  _lastSelectedNode = e.Node;
            _videoFilePathList.Clear();
            if (currentNodeTag.BookVideoList.Count == 0)
            {
                string[] currentDirectoryList = Directory.GetDirectories(currentNodeTag.CurrentDirectoryPath);
                for (int i = 0; i < currentDirectoryList.Length; i++)
                {
                    GetVideoFileList(currentDirectoryList[i], _videoFilePathList);
                }
            }
            else
            {
                GetVideoFileList(currentNodeTag.CurrentDirectoryPath, _videoFilePathList);
            }

            AddVideoThumbnailControls(_videoFilePathList);
        }

        private void AddVideoThumbnailControls(List<ThumbnailInfo> thumbnailInfoList)
        {
            flowLayoutVideoPanel.Controls.Clear();
            for (int j = 0; j < thumbnailInfoList.Count; j++)
            {
                CustomeThumbControl ctlThumb = new CustomeThumbControl(CtlThumb_Click);
                ctlThumb.ThumbName = thumbnailInfoList[j].FileName;
                ctlThumb.ThumbUrl = thumbnailInfoList[j].ThumbnailFilePath; //Path.Combine(thumbnailSubjectPath, "Subjects_ENGLISH.png");
                ctlThumb.VideoUrl = thumbnailInfoList[j].VideoFullUrl;
                //ctlThumb.Click += CtlThumb_Click;
                ctlThumb.Size = new System.Drawing.Size(180, 180);
                //ctlThumb
                flowLayoutVideoPanel.Controls.Add(ctlThumb);
            }
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
                    thumbnailInfo.ThumbnailFilePath = Path.Combine(ClientHelper.GetClientThumbanailPath(), ThumbnailHelper.GetThumbnailFileName(currentPath));
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

//private void FillTreeView()
//{
//    treeView1.Nodes.Clear();

//    string[] rootDirectoryList = Directory.GetDirectories(_clientVideoRootFilePath);
//    for (int i = 0; i < rootDirectoryList.Length; i++)
//    {
//        TreeNode rootNode = new TreeNode(Path.GetFileName(rootDirectoryList[i]));
//        TreeTag treeTag = new TreeTag();
//        treeTag.CurrentDirectoryPath = rootDirectoryList[i];
//        rootNode.Tag = treeTag;
//        treeView1.Nodes.Add(rootNode);
//        AddTreeNode(rootNode, rootDirectoryList[i], "");

//    }
//}

//private void AddTreeNode(TreeNode parentNode, string currentDirectoryPath, string selectedNodeFullPath)
//{
//    string[] directoryList = Directory.GetDirectories(currentDirectoryPath);
//    string[] fileList = Directory.GetFiles(currentDirectoryPath);
//    if (fileList.Length > 0)
//    {
//        (parentNode.Tag as TreeTag).BookVideoList = new List<string>(fileList);
//    }

//    else
//    {
//        for (int i = 0; i < directoryList.Length; i++)
//        {
//            TreeNode rootNode = new TreeNode(Path.GetFileName(directoryList[i]));
//            TreeTag treeTag = new TreeTag();
//            treeTag.CurrentDirectoryPath = directoryList[i];
//            rootNode.Tag = treeTag;
//            parentNode.Nodes.Add(rootNode);
//            //if (rootNode.FullPath.Equals(selectedNodeFullPath))
//            //{
//            //    treeView1.SelectedNode = rootNode;
//            //}
//            AddTreeNode(rootNode, directoryList[i], "");
//        }
//    }
//}
