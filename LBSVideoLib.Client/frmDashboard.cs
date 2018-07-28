using LBFVideoLib.Common;
using LBFVideoLib.Common.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Linq;
using System.Linq.Expressions;
using System.Collections;

namespace LBFVideoLib.Client
{
    public partial class frmDashboard : Form
    {
        //private string _clientRootPath = "";
        //private string _clientInfoFilePath = "";
        public Form ParentFormControl { get; set; }
        public ClientInfo ClientInfoObject { get; set; }
        public TreeNode SelectedNode { get; set; }
        private Dictionary<string, string> _bookVideoList = new Dictionary<string, string>();
        private bool _skipNodeSelection = true;
        private List<VideoInfo> _mostWatchedVideos = new List<VideoInfo>();
        private List<VideoInfo> _mostRecommandedVideos = new List<VideoInfo>();

        private List<ThumbnailInfo> _mostWatchedVideosThumbList = new List<ThumbnailInfo>();
        private List<ThumbnailInfo> _mostRecommandedVideosThumbList = new List<ThumbnailInfo>();


        public frmDashboard()
        {
            InitializeComponent();
        }


        private void frmDashboard_Load(object sender, EventArgs e)
        {
            // FillVideoList();
            // FillRandomVideoList();

            lblSessionYears.Text = ClientHelper.GetSessionString(ClientInfoObject.SessionString);
            lblSchoolWelcome.Text = ClientHelper.GetWelcomeString(ClientInfoObject.SchoolName, ClientInfoObject.SchoolCity, ClientInfoObject.SchoolId);
            lblExpireDate.Text = ClientHelper.GetExpiryDateString(ClientInfoObject.ExpiryDate);

            FillTreeView();
            treeView1.ExpandAll();

            //if (this.treeView1.Nodes != null && this.treeView1.Nodes.Count > 0)
            //{
            //    this.treeView1.SelectedNode = this.treeView1.Nodes[0];

            AddRecomandatedVideos();
            AddMostWatchedVideos();
            //}
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
                    rootNode.Name = rootNode.Text;

                    TreeTag treeTag = new TreeTag();
                    treeTag.CurrentDirectoryPath = directoryList[i];
                    rootNode.Tag = treeTag;
                    parentNode.Nodes.Add(rootNode);
                    AddTreeNode(rootNode, directoryList[i]);
                }
            }
        }

        private void OpenVideoLibrary(TreeNode selectedNode)
        {
            frmVideoLibrary frmVideoLibrary = new frmVideoLibrary();
            frmVideoLibrary.ParentFormControl = this;
            frmVideoLibrary.DashboardFormControl = this;
            frmVideoLibrary.ClientInfoObject = this.ClientInfoObject;
            frmVideoLibrary.SelectedNode = selectedNode;
            frmVideoLibrary.Show();
            CommonAppStateDataHelper.AddForm(this);
            this.Hide();
        }

        #endregion



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
            //upcomingVideo.NextVideoFileList = new string[] { Path.Combine(ClientHelper.GetClientVideoFilePath(ClientInfoObject.SchoolId, ClientInfoObject.SchoolCity), @"First\First-S1\First-S1-English\First-S1-English-Basic\VID-20150929-WA0005.mp4") };
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

        private void AddRecomandatedVideos()
        {
            Random random = new Random();
            int[] randomVideoIndexList = new int[CommonAppStateDataHelper.ClientInfoObject.VideoInfoList.Count];
            int noOfIterations = 0;
            do
            {
                noOfIterations++;
                int newRandomNumber = random.Next(0, CommonAppStateDataHelper.ClientInfoObject.VideoInfoList.Count - 1);
                if (randomVideoIndexList.Contains(newRandomNumber) == false)
                {
                    _mostRecommandedVideos.Add(CommonAppStateDataHelper.ClientInfoObject.VideoInfoList[newRandomNumber]);
                    randomVideoIndexList[newRandomNumber] = newRandomNumber;
                }
            }
            while (_mostRecommandedVideos.Count < 6);

            
            for (int i = 0; _mostRecommandedVideos != null && i < _mostRecommandedVideos.Count - 1; i++)
            {
                ThumbnailInfo thumbInfo = new ThumbnailInfo();
                thumbInfo.FileName = _mostRecommandedVideos[i].VideoName;
                thumbInfo.ThumbnailFilePath = Path.Combine(ClientHelper.GetClientThumbanailPath(), ThumbnailHelper.GetThumbnailFileName(_mostRecommandedVideos[i].Book));
                thumbInfo.VideoFullUrl = _mostRecommandedVideos[i].VideoFullUrl;
                _mostRecommandedVideosThumbList.Add(thumbInfo);
            }
            if (_mostRecommandedVideosThumbList != null && _mostRecommandedVideosThumbList.Count > 0)
            {
                AddVideoThumbnailControls(pnlRecomVideo, _mostRecommandedVideosThumbList, CtlMostWatchedVideo_Click);
            }
        }

        private void AddMostWatchedVideos()
        {
            _mostWatchedVideos = CommonAppStateDataHelper.ClientInfoObject.VideoInfoList.OrderByDescending(k => k.WatchCount).Take(6).ToList<VideoInfo>();

            for (int i = 0; _mostWatchedVideos != null && i < _mostWatchedVideos.Count - 1; i++)
            {
                ThumbnailInfo thumbInfo = new ThumbnailInfo();
                thumbInfo.FileName = _mostWatchedVideos[i].VideoName;
                thumbInfo.ThumbnailFilePath = Path.Combine(ClientHelper.GetClientThumbanailPath(), ThumbnailHelper.GetThumbnailFileName(_mostWatchedVideos[i].Book));
                thumbInfo.VideoFullUrl = _mostWatchedVideos[i].VideoFullUrl;
                _mostWatchedVideosThumbList.Add(thumbInfo);
            }
            if (_mostWatchedVideosThumbList != null && _mostWatchedVideosThumbList.Count > 0)
            {
                AddVideoThumbnailControls(pnlMostWatchVideo, _mostWatchedVideosThumbList, CtlMostWatchedVideo_Click);
            }
        }

        private void AddVideoThumbnailControls(FlowLayoutPanel flowLayoutPanel, List<ThumbnailInfo> thumbnailInfoList, Action<object, EventArgs> clickDeligate)
        {
            pnlMostWatchVideo.Controls.Clear();
            for (int j = 0; j < thumbnailInfoList.Count; j++)
            {
                CustomeThumbControl ctlThumb = new CustomeThumbControl(clickDeligate);
                ctlThumb.ThumbnailInformation = thumbnailInfoList[j];
                ctlThumb.ThumbName = thumbnailInfoList[j].FileName;
                ctlThumb.ThumbUrl = thumbnailInfoList[j].ThumbnailFilePath;
                ctlThumb.VideoUrl = thumbnailInfoList[j].VideoFullUrl;
                ctlThumb.Size = new System.Drawing.Size(180, 180);
                flowLayoutPanel.Controls.Add(ctlThumb);
            }
        }


        private void CtlRecommanded_Click(object sender, EventArgs e)
        {
            CustomeThumbControl ctl = sender as CustomeThumbControl;
            // Find index of currently selected video in list.

            List<ThumbnailInfo> nextVideoList = new List<ThumbnailInfo>();
            List<ThumbnailInfo> previousVideoList = new List<ThumbnailInfo>();

            CreatePreviousAndNextPlaylist(_mostRecommandedVideosThumbList, ctl.VideoUrl, out nextVideoList, out previousVideoList);

            frmUpCommingVideo upcomingVideoForm = new frmUpCommingVideo();
            upcomingVideoForm.ParentFormControl = this;
            upcomingVideoForm.ClientInfoObject = this.ClientInfoObject;
            upcomingVideoForm.EncryptedVideo = false;
            upcomingVideoForm.NextVideoFileList = nextVideoList;
            upcomingVideoForm.PreviousVideoFileList = previousVideoList;
            upcomingVideoForm.CurrentVideo = new ThumbnailInfo() { FileName = ctl.ThumbName, ThumbnailFilePath = ctl.ThumbUrl, VideoFullUrl = ctl.VideoUrl };
            upcomingVideoForm.EncryptedVideo = true;
            upcomingVideoForm.SelectedNode = this.treeView1.SelectedNode;
            upcomingVideoForm.DashboardFormControl = this;
            upcomingVideoForm.Show();
            this.Hide();
        }

        private void CtlMostWatchedVideo_Click(object sender, EventArgs e)
        {
            CustomeThumbControl ctl = sender as CustomeThumbControl;
            // Find index of currently selected video in list.

            List<ThumbnailInfo> nextVideoList = new List<ThumbnailInfo>();
            List<ThumbnailInfo> previousVideoList = new List<ThumbnailInfo>();

            CreatePreviousAndNextPlaylist(_mostWatchedVideosThumbList, ctl.VideoUrl, out nextVideoList, out previousVideoList);

            frmUpCommingVideo upcomingVideoForm = (CommonAppStateDataHelper.FindFormByFormType("frmUpCommingVideo") as frmUpCommingVideo);
            if (upcomingVideoForm == null)
            {
                upcomingVideoForm = new frmUpCommingVideo();
            }
            upcomingVideoForm.ParentFormControl = this;
            upcomingVideoForm.ClientInfoObject = this.ClientInfoObject;
            upcomingVideoForm.EncryptedVideo = false;
            upcomingVideoForm.NextVideoFileList = nextVideoList;
            upcomingVideoForm.PreviousVideoFileList = previousVideoList;
            upcomingVideoForm.CurrentVideo = new ThumbnailInfo() { FileName = ctl.ThumbName, ThumbnailFilePath = ctl.ThumbUrl, VideoFullUrl = ctl.VideoUrl };
            upcomingVideoForm.EncryptedVideo = true;
            upcomingVideoForm.SelectedNode = this.treeView1.SelectedNode;
            upcomingVideoForm.DashboardFormControl = this;
            upcomingVideoForm.Show();
            this.Hide();
        }

        private void CreatePreviousAndNextPlaylist(List<ThumbnailInfo> thumbnailList, string videoUrl, out List<ThumbnailInfo> nextVideoList, out List<ThumbnailInfo> previousVideoList)
        {
            nextVideoList = new List<ThumbnailInfo>();
            previousVideoList = new List<ThumbnailInfo>();
            int index = thumbnailList.FindIndex(k => k.VideoFullUrl.Equals(videoUrl));
            for (int i = index + 1; (i < index + 1 + 3) && (i < thumbnailList.Count); i++)
            {
                nextVideoList.Add(thumbnailList[i]);
            }

            for (int i = index - 1; (i > index - 1 - 3) && (i > -1); i--)
            {
                previousVideoList.Add(thumbnailList[i]);
            }
        }

        private void frmDashboard_VisibleChanged(object sender, EventArgs e)
        {

            if (this.SelectedNode != null && this.Visible)
            {
                TreeNode[] searchedNode = this.treeView1.Nodes.Find(this.SelectedNode.Name, true);
                if (searchedNode.Length > 0)
                {
                    _skipNodeSelection = true;
                    this.treeView1.SelectedNode = searchedNode[0];
                    _skipNodeSelection = false;
                }
            }

        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                if (_skipNodeSelection == false)
                {
                    if (e.Node.Tag != null)
                    {
                        OpenVideoLibrary(e.Node);
                    }
                }
                else
                {
                    _skipNodeSelection = false;
                }
            }
            finally
            {
                _skipNodeSelection = false;
            }
        }

        private void lblPrivacyPolicy_Click(object sender, EventArgs e)
        {
            frmPrivacyPolicy frm = new frmPrivacyPolicy();
            frm.Show();
        }
    }
}
