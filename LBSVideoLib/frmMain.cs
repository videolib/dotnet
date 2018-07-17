using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LBSVideoLib
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnRegistration_Click(object sender, EventArgs e)
        {
            frmRegistration frmReg = new frmRegistration();
            frmReg.Show();
            this.Hide();
        }

        private void btnVideoLib_Click(object sender, EventArgs e)
        {
            frmVideoLib frmLib = new frmVideoLib();
            frmLib.Show();
            this.Hide();
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            frmLogin frmLogin = new frmLogin();
            frmLogin.Show();
        }
    }
}
