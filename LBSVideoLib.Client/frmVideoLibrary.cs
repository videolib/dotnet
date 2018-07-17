using LBFVideoLib.Common;
using System;
using System.IO;
using System.Windows.Forms;

namespace LBFVideoLib.Client
{
    public partial class frmVideoLibrary : Form
    {
        private string _clientRootPath = "";
        private string _clientInfoFilePath = "";
        public Form ParentFormControl { get; set; }
        public ClientInfo ClientInfoObject { get; set; }
        public TreeNode SelectedNode { get; set; }

        public frmVideoLibrary()
        {
            InitializeComponent();
        }

        private void frmVideoLibrary_Load(object sender, EventArgs e)
        {
            _clientRootPath = ClientInfo.GetClientRootPath();
            _clientInfoFilePath = ClientInfo.GetClientInfoFilePath();

            this.ClientInfoObject.LastAccessEndTime = DateTime.UtcNow;
            this.ClientInfoObject.LastAccessStartTime = DateTime.UtcNow;
            Cryptograph.EncryptObject(this.ClientInfoObject, _clientInfoFilePath);

            lblSchoolDetail.Text = this.ClientInfoObject.GetClientDetail();

            FillTreeView();
            treeView1.ExpandAll();
            
        }

        #region Private Methods

        private void FillTreeView( )
        {
            treeView1.Nodes.Clear();

            // Fill Tree
            // get root
            string[] rootDirectoryList = Directory.GetDirectories(_clientRootPath);
            for (int i = 0; i < rootDirectoryList.Length; i++)
            {
                TreeNode rootNode = new TreeNode(Path.GetFileName(rootDirectoryList[i]));
                treeView1.Nodes.Add(rootNode);
                //if (rootNode.FullPath.Equals(selectedNodeFullPath))
                //{
                //    treeView1.SelectedNode = rootNode;
                //}
                AddTreeNode(rootNode, rootDirectoryList[i],   "");

            }
        }

        private void AddTreeNode(TreeNode parentNode, string currentDirectoryPath, string selectedNodeFullPath)
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
                //if (rootNode.FullPath.Equals(selectedNodeFullPath))
                //{
                //    treeView1.SelectedNode = rootNode;
                //}
                AddTreeNode(rootNode, directoryList[i],"");
            }
            //}
        }


        #endregion

        private void frmVideoLibrary_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void myButton1_Click(object sender, EventArgs e)
        {
            frmUpCommingVideo upcomingVideoForm = new frmUpCommingVideo();
            upcomingVideoForm.ParentFormControl = this;
            upcomingVideoForm.ClientInfoObject = this.ClientInfoObject;
            upcomingVideoForm.EncryptedVideo = false;
            upcomingVideoForm.NextVideoFileList = new string[] { Path.Combine(ClientInfo.GetClientVideoFilePath(), @"First\First-S1\First-S1-English\First-S1-English-Basic\VID-20150929-WA0005.mp4") };
            upcomingVideoForm.Show();
            this.Hide();
        }

        
    }
}
