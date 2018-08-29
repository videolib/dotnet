﻿using LBFVideoLib.Common;
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

        public List<VideoInfo> PreviousVideoFileList { get; set; }

        public List<VideoInfo> NextVideoFileList { get; set; }

        // public VideoInfo CurrentVideo { get; set; }
        public VideoInfo CurrentVideoInfo { get; set; }

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
            try
            {
                InitializeForm();
            }

            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }

        private void InitializeForm()
        {
            _clientRootPath = ClientHelper.GetClientRootPath();
            _clientInfoFilePath = ClientHelper.GetClientInfoFilePath();
            this.ClientInfoObject.LastAccessEndTime = DateTime.Now;
            FileInfo clientInfoFileInfo = new FileInfo(ClientHelper.GetClientInfoFilePath());
            clientInfoFileInfo.Attributes &= ~FileAttributes.Hidden;
            // this.ClientInfoObject.LastAccessStartTime = DateTime.UtcNow;
            Cryptograph.EncryptObject(this.ClientInfoObject, _clientInfoFilePath);
            clientInfoFileInfo.Attributes |= FileAttributes.Hidden;

            FillTreeView();
            treeView1.CollapseAll();

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
            //lblWelcome.Text = ClientHelper.GetWelcomeString(ClientInfoObject.SchoolName, ClientInfoObject.SchoolCity, ClientInfoObject.SchoolId);

            lblExpireDate.Text = ClientHelper.GetExpiryDateString(ClientInfoObject.SessionEndDate);
            lblAppTitle.Text = ClientHelper.GetRegisteredSchoolTitleString(ClientInfoObject.SchoolName, ClientInfoObject.SchoolCity, ClientInfoObject.SchoolId);

            AddPreviousVideoList();
            AddNextVideoList();

            if (EncryptedVideo)
            {
                PlayEncryptedVideo(CurrentVideoInfo.VideoFullUrl);
            }
            else
            {
                this.axWindowsMediaPlayer1.URL = CurrentVideoInfo.VideoFullUrl;
            }

        }

        private void PlayEncryptedVideo(string videoUrl)
        {

            try
            {
                if (ValidateLicense() == false || backgroundWorker1.IsBusy)
                {
                    return;
                }
                this.axWindowsMediaPlayer1.URL = "";

                progressBar1.Visible = true;

                VideoInfo currentVideoInfo = CurrentVideoInfo = this.ClientInfoObject.VideoInfoList.First(i => i.VideoFullUrl.ToLower().Equals(videoUrl.ToLower()));

                BackgroundProcessData backgroundProcessData = new BackgroundProcessData();
                backgroundProcessData.CurrentVideoInfo = currentVideoInfo;
                backgroundProcessData.OrignalVideoPath = videoUrl;
                backgroundProcessData.State = BackgroundAppState.DecryptingVideoToPlay;

                if (backgroundWorker1.IsBusy == true)
                {
                    backgroundWorker1.CancelAsync();
                }

                while (backgroundWorker1.IsBusy == true)
                {
                    // backgroundWorker1.CancelAsync();
                }
                backgroundWorker1.RunWorkerAsync(backgroundProcessData);

                //VideoInfo currentVideoInfo = this.ClientInfoObject.VideoInfoList.First(i => i.VideoFullUrl.ToLower().Equals(videoUrl.ToLower()));
                //currentVideoInfo.WatchCount++;
                ////string tempDirectory = Path.Combine(Path.GetDirectoryName(this.NextVideoFileList[0]), "Temp");
                //string tempDirectory = Path.Combine(Path.GetTempPath(), "Temp");
                //Directory.CreateDirectory(tempDirectory);
                //string tempFilePath = Path.Combine(tempDirectory, Path.GetFileName(videoUrl));
                //tempFileList.Add(tempFilePath);
                //Cryptograph.DecryptFile(videoUrl, tempFilePath);
                //this.axWindowsMediaPlayer1.URL = tempFilePath;
                //lblWelcome.Text = string.Format("{0}", currentVideoInfo.Subject);// string.Format("{0}, {1}, {2}", currentVideoInfo.ClassName, currentVideoInfo.SeriesName, currentVideoInfo.Subject);
                //lblWatchCount.Text = string.Format("Watch Count: {0} Times", currentVideoInfo.WatchCount);

                //   backgroundWorker1.RunWorkerAsync(videoUrl);
            }
            catch (Exception ex)
            {
                progressBar1.Visible = false;
                ExceptionHandler.HandleException(ex,"", false);
            }
        }

        private void AddPreviousVideoList()
        {
            for (int i = 0; i < PreviousVideoFileList.Count; i++)
            {
                CustomeThumbControl ctlThumb = new CustomeThumbControl(CtlThumb_Click);
                ctlThumb.ThumbName = PreviousVideoFileList[i].VideoName;
                ctlThumb.ThumbUrl = PreviousVideoFileList[i].ThumbnailFilePath; //Path.Combine(thumbnailSubjectPath, "Subjects_ENGLISH.png");
                ctlThumb.VideoUrl = PreviousVideoFileList[i].VideoFullUrl;
                ctlThumb.ThumbnailInformation = PreviousVideoFileList[i];
                ctlThumb.Size = new System.Drawing.Size(80, 80);
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
                ctlThumb.ThumbName = NextVideoFileList[i].VideoName;
                ctlThumb.ThumbUrl = NextVideoFileList[i].ThumbnailFilePath; //Path.Combine(thumbnailSubjectPath, "Subjects_ENGLISH.png");
                ctlThumb.VideoUrl = NextVideoFileList[i].VideoFullUrl;
                ctlThumb.Size = new System.Drawing.Size(80, 80);
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
                lblUpcomming.TextAlign = ContentAlignment.MiddleLeft;
            }
        }



        private void CtlThumb_Click(object sender, EventArgs e)
        {
            CustomeThumbControl ctl = sender as CustomeThumbControl;
            if (_lastPlayedVideoFullUrl.Equals(ctl.VideoUrl) == true)
            {
                return;
            }

            //this.axWindowsMediaPlayer1.URL = "";
            //_lastPlayedVideoFullUrl = ctl.VideoUrl;
            //this.lblFileName.Text = Path.GetFileNameWithoutExtension(ctl.ThumbName);
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
            //if (e.CloseReason != CloseReason.ApplicationExitCall)
            //{
            //    OnFormVisiblityChangeAndClose();
            //}
            Application.Exit();
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
            if (backgroundWorker1.IsBusy == true)
            {
                backgroundWorker1.CancelAsync();
            }
            this.axWindowsMediaPlayer1.URL = "";

            try
            {
                for (int i = 0; i < tempFileList.Count; i++)
                {
                    File.Delete(tempFileList[i]);
                }
            }
            // if file is already decrypting and we are trying to delete it.
            catch (Exception)
            {

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
                        (this.ParentFormControl as frmVideoLibrary).UpdateTreeSelectedNode = true;
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
            try
            {
                if (this.axWindowsMediaPlayer1.fullScreen == false)
                {
                    this.axWindowsMediaPlayer1.fullScreen = true;
                }
            }
            catch (Exception)
            {
            }
        }

        private void btnRewind_Click(object sender, EventArgs e)
        {
            try
            {
                this.axWindowsMediaPlayer1.Ctlcontrols.currentPosition -= 10;
            }
            catch (Exception)
            {
            }
        }

        private void btnFastForward_Click(object sender, EventArgs e)
        {
            try
            {
                this.axWindowsMediaPlayer1.Ctlcontrols.currentPosition += 10;
            }
            catch (Exception)
            {
            }
        }

        private void SaveWatchedVideoCountOnFireBase(string videoName, int watchCount)
        {
            try
            {
                videoName = videoName.Remove(videoName.LastIndexOf("."));
                string machineName = MacAddressHelper.GetMacAddress(); // Environment.MachineName
                //List<WatchCountInfoFB> watchCountInfoList = new List<WatchCountInfoFB>();
                //WatchCountInfoFB info = new WatchCountInfoFB();
                //info.machinename = Environment.MachineName;
                //info.videoname = videoName;
                //info.videowatchcount = watchCount;
                //watchCountInfoList.Add(info);
                //string jsonString = JsonHelper.ParseObjectToJSON<List<WatchCountInfoFB>>(watchCountInfoList);

                string jsonString = watchCount.ToString();
                string url = string.Format("clientanalytic-data/{0}/{1}/videowatchcount/{2}/Count", ClientInfoObject.SchoolId, machineName, videoName);
                FirebaseHelper.PostData(jsonString, url);
            }
            catch (Exception e)
            {
                if (e.Message.Contains("A connection attempt failed because the connected party did not properly respond after a period of time, or established connection failed because connected host has failed to respond"))
                {
                    Console.Out.WriteLine("-----------------");
                    Console.Out.WriteLine(e.Message);
                }
            }

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

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.SelectedNode = null;
            this._hiddenSourceControl = pnlLogo;
            this.Hide();
            this.DashboardFormControl.Show();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundProcessData currentData = e.Argument as BackgroundProcessData;
            if (currentData.CurrentVideoInfo != null)
            {
                VideoInfo currentVideoInfo = currentData.CurrentVideoInfo;
                currentVideoInfo.WatchCount++;
                backgroundWorker1.ReportProgress(50, currentData);

                string tempDirectory = Path.Combine(Path.GetTempPath(), "Temp");
                Directory.CreateDirectory(tempDirectory);
                backgroundWorker1.ReportProgress(97, currentData);

                string tempFilePath = Path.Combine(tempDirectory, Path.GetFileName(currentData.OrignalVideoPath));
                tempFileList.Add(tempFilePath);
                Cryptograph.DecryptFile(currentData.OrignalVideoPath, tempFilePath);
                currentData.DecryptedVideoPath = tempFilePath;
                backgroundWorker1.ReportProgress(99, currentData);


                SaveWatchedVideoCountOnFireBase(currentVideoInfo.VideoName, currentVideoInfo.WatchCount);
                currentData.DecryptedVideoPath = "";
                backgroundWorker1.ReportProgress(100, currentData);
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBar1.Visible = false;
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            BackgroundProcessData currentData = e.UserState as BackgroundProcessData;
            // Change the value of the ProgressBar  
            progressBar1.Value = e.ProgressPercentage;
            if (currentData != null && currentData.State == BackgroundAppState.DecryptingVideoToPlay && currentData.DecryptedVideoPath != "")
            {
                progressBar1.Visible = false;
                this.CurrentVideoInfo = currentData.CurrentVideoInfo;
                this.axWindowsMediaPlayer1.URL = currentData.DecryptedVideoPath;
                this.axWindowsMediaPlayer1.stretchToFit = true;
                //lblWelcome.Text = string.Format("{0}", this.CurrentVideoInfo.Subject);
                lblWelcome.Text = string.Format("{0}", this.CurrentVideoInfo.Book);                

                lblWatchCount.Text = string.Format("Watch Count: {0} Times", currentData.CurrentVideoInfo.WatchCount);
                this.lblFileName.Text = Path.GetFileNameWithoutExtension(currentData.CurrentVideoInfo.VideoFullUrl);
                _lastPlayedVideoFullUrl = currentData.CurrentVideoInfo.VideoFullUrl;


            }
        }

        private void frmUpCommingVideo_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (progressBar1.Value != 100)
            {
                e.Cancel = true;
            }
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