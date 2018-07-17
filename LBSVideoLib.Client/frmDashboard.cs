using LBFVideoLib.Common;
using System;
using System.IO;
using System.Windows.Forms;

namespace LBFVideoLib.Client
{
    public partial class frmDashboard : Form
    {
        private string _clientRootPath = "";
        private string _clientInfoFilePath = "";
        public Form ParentFormControl { get; set; }
        public ClientInfo ClientInfoObject { get; set; }



        public frmDashboard()
        {
            InitializeComponent();
        }

      
        private void frmDashboard_Load(object sender, EventArgs e)
        {
            _clientRootPath = ClientInfo.GetClientRootPath();
            _clientInfoFilePath= ClientInfo.GetClientInfoFilePath();
            this.ClientInfoObject.LastAccessEndTime = DateTime.UtcNow;
            this.ClientInfoObject.LastAccessStartTime = DateTime.UtcNow;
            Cryptograph.EncryptObject(this.ClientInfoObject, _clientInfoFilePath);

            FillTreeView();
            treeView1.ExpandAll();
            this.lblSchoolDetail.Text = this.ClientInfoObject.GetClientDetail();

        }

        #region Private Methods

        private void FillTreeView()
        {
            treeView1.Nodes.Clear();
            //_clientRootPath = ConfigHelper.TargetFolderPath;
            //_clientInfoFilePath = Path.Combine(_clientRootPath, ConfigHelper.ClientInfoFileName);
       
            //this.lblSchoolName.Text = this.ClientInfoObject.SchoolName;

            // Fill Tree
            // get root
            string[] rootDirectoryList = Directory.GetDirectories(_clientRootPath);
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

        private void OpenVideoLibrary()
        {
            frmVideoLibrary frmVideoLibrary = new frmVideoLibrary();
            frmVideoLibrary.ParentFormControl = this;
            // frmVideoLibrary.SelectedNode = e.Node;
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
            upcomingVideo.NextVideoFileList = new string[] { Path.Combine(ClientInfo.GetClientVideoFilePath(), @"First\First-S1\First-S1-English\First-S1-English-Basic\VID-20150929-WA0005.mp4") };
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
    }
}
