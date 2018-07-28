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
        public TreeNode SelectedNode { get; set; }

        public List<ThumbnailInfo> PreviousVideoFileList { get; set; }

        public List<ThumbnailInfo> NextVideoFileList { get; set; }

        public ThumbnailInfo CurrentVideo { get; set; }

        public bool EncryptedVideo { get; set; }

        public Form DashboardFormControl { get; set; }

        public string SelectedVideo { get; set; }

        private List<string> tempFileList = new List<string>();
        private string _lastPlayedVideoFullUrl = "";
        private bool _skipNodeSelection = true;
        private Control _hiddenSourceControl = null;

        public frmUpCommingVideo()
        {
            InitializeComponent();
        }

        private void frmUpCommingVideo_Load(object sender, EventArgs e)
        {
            InitializeForm();
        }

        private void InitializeForm()
        {
            _clientRootPath = ClientHelper.GetClientRootPath();
            _clientInfoFilePath = ClientHelper.GetClientInfoFilePath();
            this.ClientInfoObject.LastAccessEndTime = DateTime.UtcNow;
            // this.ClientInfoObject.LastAccessStartTime = DateTime.UtcNow;
            Cryptograph.EncryptObject(this.ClientInfoObject, _clientInfoFilePath);

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

            lblSessionYears.Text = ClientHelper.GetSessionString(ClientInfoObject.SessionString);
            lblWelcome.Text = ClientHelper.GetWelcomeString(ClientInfoObject.SchoolName, ClientInfoObject.SchoolCity, ClientInfoObject.SchoolId);
            lblExpireDate.Text = ClientHelper.GetExpiryDateString(ClientInfoObject.ExpiryDate);
            lblAppTitle.Text = ClientHelper.GetRegisteredSchoolTitleString(ClientInfoObject.SchoolName, ClientInfoObject.SchoolCity, ClientInfoObject.SchoolId);

            AddPreviousVideoList();
            AddNextVideoList();

            if (EncryptedVideo)
            {
                PlayEncryptedVideo(CurrentVideo.VideoFullUrl);
            }
            else
            {
                this.axWindowsMediaPlayer1.URL = CurrentVideo.VideoFullUrl;
            }

            this.lblFileName.Text = Path.GetFileNameWithoutExtension(CurrentVideo.FileName);


        }

        private void PlayEncryptedVideo(string videoUrl)
        {
           if(ValidateLicense()==false)
            {
                return;
            }

            VideoInfo currentVideoInfo = this.ClientInfoObject.VideoInfoList.First(i => i.VideoFullUrl.ToLower().Equals(videoUrl.ToLower()));
            currentVideoInfo.WatchCount++;
            //string tempDirectory = Path.Combine(Path.GetDirectoryName(this.NextVideoFileList[0]), "Temp");
            string tempDirectory = Path.Combine(Path.GetTempPath(), "Temp");
            Directory.CreateDirectory(tempDirectory);
            string tempFilePath = Path.Combine(tempDirectory, Path.GetFileName(videoUrl));
            tempFileList.Add(tempFilePath);
            Cryptograph.DecryptFile(videoUrl, tempFilePath);
            this.axWindowsMediaPlayer1.URL = tempFilePath;
            lblWelcome.Text = string.Format("{0}, {1}, {2}", currentVideoInfo.ClassName, currentVideoInfo.SeriesName, currentVideoInfo.Subject);
            lblWatchCount.Text = string.Format("Watch Count: {0} Times", currentVideoInfo.WatchCount);

            SaveWatchedVideoCountOnFireBase(currentVideoInfo.VideoName, currentVideoInfo.WatchCount);
        }

        private void AddPreviousVideoList()
        {
            for (int i = 0; i < PreviousVideoFileList.Count; i++)
            {
                CustomeThumbControl ctlThumb = new CustomeThumbControl(CtlThumb_Click);
                ctlThumb.ThumbName = PreviousVideoFileList[i].FileName;
                ctlThumb.ThumbUrl = PreviousVideoFileList[i].ThumbnailFilePath; //Path.Combine(thumbnailSubjectPath, "Subjects_ENGLISH.png");
                ctlThumb.VideoUrl = PreviousVideoFileList[i].VideoFullUrl;
                //ctlThumb.Click += CtlThumb_Click;
                ctlThumb.Size = new System.Drawing.Size(150, 180);
                //ctlThumb
                flowLayoutPanelPrevious.Controls.Add(ctlThumb);
            }

            if (PreviousVideoFileList.Count <= 0)
            {
                pnlPreviousVideo.Visible = false;
                pnlSep.Visible = false;
            }
        }

        private void AddNextVideoList()
        {
            for (int i = 0; i < NextVideoFileList.Count; i++)
            {
                CustomeThumbControl ctlThumb = new CustomeThumbControl(CtlThumb_Click);
                ctlThumb.ThumbName = NextVideoFileList[i].FileName;
                ctlThumb.ThumbUrl = NextVideoFileList[i].ThumbnailFilePath; //Path.Combine(thumbnailSubjectPath, "Subjects_ENGLISH.png");
                ctlThumb.VideoUrl = NextVideoFileList[i].VideoFullUrl;
                //ctlThumb.Click += CtlThumb_Click;
                ctlThumb.Size = new System.Drawing.Size(150, 180);
                //ctlThumb
                flowLayoutPanelUpcoming.Controls.Add(ctlThumb);
            }

            if (NextVideoFileList.Count <= 0)
            {
                pnlUpcomingVideo.Visible = false;
                pnlSep.Visible = false;
            }
            else if (PreviousVideoFileList.Count <= 0)
            {
                flowLayoutPanelUpcoming.FlowDirection = FlowDirection.LeftToRight;
            }
        }



        private void CtlThumb_Click(object sender, EventArgs e)
        {
            CustomeThumbControl ctl = sender as CustomeThumbControl;
            if (_lastPlayedVideoFullUrl.Equals(ctl.VideoUrl) == true)
            {
                return;
            }

            this.axWindowsMediaPlayer1.URL = "";
            _lastPlayedVideoFullUrl = ctl.VideoUrl;
            this.lblFileName.Text = Path.GetFileNameWithoutExtension(ctl.ThumbName);
            this.EncryptedVideo = true;
            PlayEncryptedVideo(ctl.VideoUrl);
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

        #endregion
        private void frmUpCommingVideo_FormClosed(object sender, FormClosedEventArgs e)
        {
            OnFormVisiblityChangeAndClose();
        }

        private void lblContact_Click(object sender, EventArgs e)
        {
            MessageBox.Show(ClientHelper.GetContactMessageString(), "Contact", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        private void pnlLogo_Click(object sender, EventArgs e)
        {
            this._hiddenSourceControl = pnlLogo;
            this.Hide();
            this.DashboardFormControl.Show();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (_skipNodeSelection == false)
            {
                //frmVideoLibrary frmVideoLibrary = new frmVideoLibrary();
                //frmVideoLibrary.ParentFormControl = this.DashboardFormControl;
                //frmVideoLibrary.SelectedNode = e.Node;
                //frmVideoLibrary.ClientInfoObject = this.ClientInfoObject;
                //frmVideoLibrary.Show();
                _hiddenSourceControl = treeView1;
                this.Hide();
            }
            else
            {
                _skipNodeSelection = false;
            }
        }

        private void lblPrivacyPolicy_Click(object sender, EventArgs e)
        {
            frmPrivacyPolicy frm = new frmPrivacyPolicy();
            frm.Show();
        }



        private void frmUpCommingVideo_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible == false)
            {

                OnFormVisiblityChangeAndClose();
            }
        }

        private void OnFormVisiblityChangeAndClose()
        {
            this.axWindowsMediaPlayer1.URL = "";

            for (int i = 0; i < tempFileList.Count; i++)
            {
                File.Delete(tempFileList[i]);
            }
            if (_hiddenSourceControl == null)
            {
                this.ParentFormControl.Show();
                return;
            }
            switch (_hiddenSourceControl.Name.ToLower())
            {
                case "treeview1":
                    if (this.ParentFormControl.Name == "frmVideoLibrary")
                    {
                        (this.ParentFormControl as frmVideoLibrary).SelectedNode = this.treeView1.SelectedNode;
                        this.ParentFormControl.Show();
                    }
                    else
                    {
                        frmVideoLibrary frmVideoLibrary = new frmVideoLibrary();
                        frmVideoLibrary.ParentFormControl = this.DashboardFormControl;
                        frmVideoLibrary.DashboardFormControl = this.DashboardFormControl;
                        frmVideoLibrary.ClientInfoObject = this.ClientInfoObject;
                        frmVideoLibrary.SelectedNode = this.treeView1.SelectedNode;
                        frmVideoLibrary.Show();
                    }
                    break;
                case "pnllogo":
                    this.DashboardFormControl.Show();
                    break;
                case "frmUpCommingVideo":
                    this.ParentFormControl.Show();
                    break;
                default:
                    this.Visible = true;
                    break;
            }

        }

        private void btnFullScreen_Click_1(object sender, EventArgs e)
        {
            if (this.axWindowsMediaPlayer1.fullScreen == false)
            {
                this.axWindowsMediaPlayer1.fullScreen = true;
            }
        }

        private void btnRewind_Click(object sender, EventArgs e)
        {
            this.axWindowsMediaPlayer1.Ctlcontrols.currentPosition -= 10;
        }



        private void btnFastForward_Click(object sender, EventArgs e)
        {
            this.axWindowsMediaPlayer1.Ctlcontrols.currentPosition += 10;
        }

        private void SaveWatchedVideoCountOnFireBase(string videoName, int watchCount)
        {
            WatchCountInfoFB info = new WatchCountInfoFB();
            info.MachineName = Environment.MachineName;
            info.VideoName = videoName;
            info.VideoWatchCount = watchCount;

            string jsonString = JsonHelper.ParseObjectToJSON<WatchCountInfoFB>(info);
            string url = string.Format("clientanalytic-data/{0}/videowatchcount", ClientInfoObject.SchoolId);
            FirebaseHelper.PostData(jsonString, url);
        }

        private void axWindowsMediaPlayer1_KeyDownEvent(object sender, AxWMPLib._WMPOCXEvents_KeyDownEvent e)
        {
            if (e.nKeyCode == (short)Keys.F11)
            {
                this.axWindowsMediaPlayer1.fullScreen = !this.axWindowsMediaPlayer1.fullScreen;
            }
        }

        private bool ValidateLicense()
        {
            string message = ""; bool deleteVideos = false;
            bool valid = LicenseHelper.CheckLicenseValidity(this.ClientInfoObject, out message, out deleteVideos);

            if (deleteVideos && valid == false)
            {
                CommonAppStateDataHelper.LicenseError = true;
                Directory.Delete(ClientHelper.GetClientVideoFilePath(this.ClientInfoObject.SchoolId, this.ClientInfoObject.SchoolCity), true);
                this.Close();
                Application.Exit();
            }
            if (valid == false)
            {
                CommonAppStateDataHelper.LicenseError = true;
                MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Application.Exit();
            }
            return valid;
        }
    }



}
//private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
//{

//    if (e.Node.Tag != null)
//    {
//        frmVideoLibrary frmVideoLibrary = new frmVideoLibrary();
//        frmVideoLibrary.ParentFormControl = this;
//        frmVideoLibrary.SelectedNode = e.Node;
//        frmVideoLibrary.ClientInfoObject = this.ClientInfoObject;
//        this.Hide();
//        frmVideoLibrary.Show();
//    }
//}

//private void AddMostWatchesVideos()
//{
//    string thumbnailPath = Path.Combine(ClientHelper.GetClientRootPath(), "Thumbnails");
//    string demoVideoPath = Path.Combine(ClientHelper.GetClientRootPath(), "DemoVideos");

//    CustomeThumbControl ctlThumb = new CustomeThumbControl(this.CtlThumb_Click);
//    ctlThumb.ThumbName = "EGBC01F002L01P002";
//    ctlThumb.VideoUrl = Path.Combine(demoVideoPath, "EGBC01F002L01P002.mp4");
//    ctlThumb.ThumbUrl = Path.Combine(thumbnailPath, "Subjects_ENGLISH.png");
//    ctlThumb.Click += CtlThumb_Click;
//    ctlThumb.Height = 180;
//    ctlThumb.Width = 150;
//    flowLayoutPanelUpcoming.Controls.Add(ctlThumb);

//    ctlThumb = new CustomeThumbControl(this.CtlThumb_Click);
//    ctlThumb.ThumbName = "HVKC01F002L001P002";
//    ctlThumb.VideoUrl = Path.Combine(demoVideoPath, "HVKC01F002L001P002.mp4");
//    ctlThumb.ThumbUrl = Path.Combine(thumbnailPath, "Subjects_HINDI.png");
//    ctlThumb.Click += CtlThumb_Click;
//    ctlThumb.Height = 180;
//    ctlThumb.Width = 150;
//    flowLayoutPanelUpcoming.Controls.Add(ctlThumb);

//    ctlThumb = new CustomeThumbControl(this.CtlThumb_Click);
//    ctlThumb.ThumbName = "EGBC02F026L07P048";
//    ctlThumb.VideoUrl = Path.Combine(demoVideoPath, "EGBC02F026L07P048.mp4");
//    ctlThumb.ThumbUrl = Path.Combine(thumbnailPath, "Subjects_ENGLISH.png");
//    ctlThumb.Click += CtlThumb_Click;
//    ctlThumb.Height = 180;
//    ctlThumb.Width = 150;
//    flowLayoutPanelUpcoming.Controls.Add(ctlThumb);

//    ctlThumb = new CustomeThumbControl(this.CtlThumb_Click);
//    ctlThumb.ThumbName = "HVKC01F006L003P017";
//    ctlThumb.VideoUrl = Path.Combine(demoVideoPath, "HVKC01F006L003P017.mp4");
//    ctlThumb.ThumbUrl = Path.Combine(thumbnailPath, "Subjects_HINDI.png");
//    ctlThumb.Click += CtlThumb_Click;
//    ctlThumb.Height = 180;
//    ctlThumb.Width = 150;
//    flowLayoutPanelPrevious.Controls.Add(ctlThumb);

//    ctlThumb = new CustomeThumbControl(this.CtlThumb_Click);
//    ctlThumb.ThumbName = "HVKC01F038L016P105";
//    ctlThumb.VideoUrl = Path.Combine(demoVideoPath, "HVKC01F038L016P105.mp4");
//    ctlThumb.ThumbUrl = Path.Combine(thumbnailPath, "Subjects_HINDI.png");
//    ctlThumb.Click += CtlThumb_Click;
//    ctlThumb.Height = 180;
//    ctlThumb.Width = 150;
//    flowLayoutPanelPrevious.Controls.Add(ctlThumb);

//    ctlThumb = new CustomeThumbControl(this.CtlThumb_Click);
//    ctlThumb.ThumbName = "HVKC01F038L016P105";
//    ctlThumb.VideoUrl = Path.Combine(demoVideoPath, "HVKC01F038L016P105.mp4");
//    ctlThumb.ThumbUrl = Path.Combine(thumbnailPath, "Subjects_HINDI.png");
//    ctlThumb.Click += CtlThumb_Click;
//    ctlThumb.Height = 180;
//    ctlThumb.Width = 150;
//    flowLayoutPanelPrevious.Controls.Add(ctlThumb);
//}