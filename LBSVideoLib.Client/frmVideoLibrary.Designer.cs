namespace LBFVideoLib.Client
{
    partial class frmVideoLibrary
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Urdu");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("First-S1", new System.Windows.Forms.TreeNode[] {
            treeNode1});
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("First", new System.Windows.Forms.TreeNode[] {
            treeNode2});
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Maths");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Second-S1", new System.Windows.Forms.TreeNode[] {
            treeNode4});
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Hindi");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Second-S2", new System.Windows.Forms.TreeNode[] {
            treeNode6});
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Second", new System.Windows.Forms.TreeNode[] {
            treeNode5,
            treeNode7});
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("English");
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("Third-S1", new System.Windows.Forms.TreeNode[] {
            treeNode9});
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("Third", new System.Windows.Forms.TreeNode[] {
            treeNode10});
            System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("Classes", new System.Windows.Forms.TreeNode[] {
            treeNode3,
            treeNode8,
            treeNode11});
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmVideoLibrary));
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.txtEmailId = new System.Windows.Forms.TextBox();
            this.lblEmail = new System.Windows.Forms.Label();
            this.pnlLogo = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lblSchoolWelcome = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label11 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.lblExpireDate = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblContact = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblSessionYears = new System.Windows.Forms.Label();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.flowLayoutVideoPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel2.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.BackColor = System.Drawing.Color.White;
            this.treeView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeView1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(50)))), ((int)(((byte)(55)))));
            this.treeView1.Location = new System.Drawing.Point(0, 167);
            this.treeView1.Name = "treeView1";
            treeNode1.Name = "Node12";
            treeNode1.Text = "Urdu";
            treeNode2.Name = "Node2";
            treeNode2.Text = "First-S1";
            treeNode3.Name = "Node1";
            treeNode3.Text = "First";
            treeNode4.Name = "Node11";
            treeNode4.Text = "Maths";
            treeNode5.Name = "Node4";
            treeNode5.Text = "Second-S1";
            treeNode6.Name = "Node10";
            treeNode6.Text = "Hindi";
            treeNode7.Name = "Node5";
            treeNode7.Text = "Second-S2";
            treeNode8.Name = "Node3";
            treeNode8.Text = "Second";
            treeNode9.Name = "Node9";
            treeNode9.Text = "English";
            treeNode10.Name = "Node8";
            treeNode10.Text = "Third-S1";
            treeNode11.Name = "Node7";
            treeNode11.Text = "Third";
            treeNode12.Name = "Node0";
            treeNode12.Text = "Classes";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode12});
            this.treeView1.Size = new System.Drawing.Size(271, 484);
            this.treeView1.TabIndex = 4;
            this.treeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);
            // 
            // txtEmailId
            // 
            this.txtEmailId.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEmailId.Location = new System.Drawing.Point(819, 10);
            this.txtEmailId.Multiline = true;
            this.txtEmailId.Name = "txtEmailId";
            this.txtEmailId.Size = new System.Drawing.Size(231, 36);
            this.txtEmailId.TabIndex = 28;
            // 
            // lblEmail
            // 
            this.lblEmail.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(50)))), ((int)(((byte)(55)))));
            this.lblEmail.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmail.ForeColor = System.Drawing.Color.White;
            this.lblEmail.Location = new System.Drawing.Point(736, 10);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(84, 36);
            this.lblEmail.TabIndex = 27;
            this.lblEmail.Text = "Search";
            this.lblEmail.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlLogo
            // 
            this.pnlLogo.BackColor = System.Drawing.Color.White;
            this.pnlLogo.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlLogo.BackgroundImage")));
            this.pnlLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pnlLogo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLogo.Location = new System.Drawing.Point(0, 0);
            this.pnlLogo.Name = "pnlLogo";
            this.pnlLogo.Size = new System.Drawing.Size(271, 167);
            this.pnlLogo.TabIndex = 1;
            this.pnlLogo.Click += new System.EventHandler(this.pnlLogo_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.lblSchoolWelcome);
            this.panel3.Controls.Add(this.txtEmailId);
            this.panel3.Controls.Add(this.lblEmail);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(273, 53);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1097, 57);
            this.panel3.TabIndex = 26;
            // 
            // lblSchoolWelcome
            // 
            this.lblSchoolWelcome.AutoSize = true;
            this.lblSchoolWelcome.BackColor = System.Drawing.Color.White;
            this.lblSchoolWelcome.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSchoolWelcome.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(50)))), ((int)(((byte)(55)))));
            this.lblSchoolWelcome.Location = new System.Drawing.Point(28, 18);
            this.lblSchoolWelcome.Name = "lblSchoolWelcome";
            this.lblSchoolWelcome.Size = new System.Drawing.Size(282, 29);
            this.lblSchoolWelcome.TabIndex = 30;
            this.lblSchoolWelcome.Text = "Welcome, {0}, {1}, [{2}]";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.treeView1);
            this.panel1.Controls.Add(this.pnlLogo);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 53);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(273, 653);
            this.panel1.TabIndex = 25;
            // 
            // label11
            // 
            this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.White;
            this.label11.Location = new System.Drawing.Point(594, 11);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(337, 16);
            this.label11.TabIndex = 0;
            this.label11.Text = "LBF Publications Pvt. Ltd. |  All Rights Reserved";
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(50)))), ((int)(((byte)(55)))));
            this.panel4.Controls.Add(this.lblExpireDate);
            this.panel4.Controls.Add(this.label11);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 706);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1370, 43);
            this.panel4.TabIndex = 24;
            // 
            // lblExpireDate
            // 
            this.lblExpireDate.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblExpireDate.AutoSize = true;
            this.lblExpireDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExpireDate.ForeColor = System.Drawing.Color.White;
            this.lblExpireDate.Location = new System.Drawing.Point(63, 11);
            this.lblExpireDate.Name = "lblExpireDate";
            this.lblExpireDate.Size = new System.Drawing.Size(105, 16);
            this.lblExpireDate.TabIndex = 2;
            this.lblExpireDate.Text = "Expires on {0}";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(559, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(167, 24);
            this.label2.TabIndex = 5;
            this.label2.Text = "LBF Video Portal";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblContact
            // 
            this.lblContact.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblContact.AutoSize = true;
            this.lblContact.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblContact.ForeColor = System.Drawing.Color.White;
            this.lblContact.Location = new System.Drawing.Point(1265, 15);
            this.lblContact.Name = "lblContact";
            this.lblContact.Size = new System.Drawing.Size(80, 24);
            this.lblContact.TabIndex = 4;
            this.lblContact.Text = "Contact";
            this.lblContact.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblContact.Click += new System.EventHandler(this.lblContact_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(50)))), ((int)(((byte)(55)))));
            this.panel2.Controls.Add(this.lblSessionYears);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.lblContact);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1370, 53);
            this.panel2.TabIndex = 12;
            // 
            // lblSessionYears
            // 
            this.lblSessionYears.AutoSize = true;
            this.lblSessionYears.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSessionYears.ForeColor = System.Drawing.Color.White;
            this.lblSessionYears.Location = new System.Drawing.Point(35, 15);
            this.lblSessionYears.Name = "lblSessionYears";
            this.lblSessionYears.Size = new System.Drawing.Size(115, 24);
            this.lblSessionYears.TabIndex = 7;
            this.lblSessionYears.Text = "Session {0}";
            this.lblSessionYears.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlMain
            // 
            this.pnlMain.BackColor = System.Drawing.Color.White;
            this.pnlMain.Controls.Add(this.flowLayoutVideoPanel);
            this.pnlMain.Controls.Add(this.panel3);
            this.pnlMain.Controls.Add(this.panel1);
            this.pnlMain.Controls.Add(this.panel4);
            this.pnlMain.Controls.Add(this.panel2);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(1370, 749);
            this.pnlMain.TabIndex = 1;
            // 
            // flowLayoutVideoPanel
            // 
            this.flowLayoutVideoPanel.AutoScroll = true;
            this.flowLayoutVideoPanel.AutoSize = true;
            this.flowLayoutVideoPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutVideoPanel.Location = new System.Drawing.Point(273, 110);
            this.flowLayoutVideoPanel.Name = "flowLayoutVideoPanel";
            this.flowLayoutVideoPanel.Size = new System.Drawing.Size(1097, 596);
            this.flowLayoutVideoPanel.TabIndex = 27;
            // 
            // frmVideoLibrary
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1370, 749);
            this.Controls.Add(this.pnlMain);
            this.Name = "frmVideoLibrary";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmVideoLibrary_FormClosed);
            this.Load += new System.EventHandler(this.frmVideoLibrary_Load);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.TextBox txtEmailId;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.Panel pnlLogo;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblContact;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Label lblExpireDate;
        private System.Windows.Forms.Label lblSchoolWelcome;
        private System.Windows.Forms.Label lblSessionYears;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutVideoPanel;
    }
}