﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LBSVideoLib.Admin
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            frmAdminLogin adminLogin = new frmAdminLogin();
            adminLogin.MdiParent = this;
            adminLogin.Parent = this.panel4;
            adminLogin.Show();
        }
    }
}
