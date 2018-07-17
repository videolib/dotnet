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
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtEmailId.Text=="sanjeev"  && txtPwd.Text == "sanjeev")
            {
                frmMain frm = new frmMain();
                frm.Show();
                this.Hide();     
            }
            else
            {
                lblStatus.Text = "Invalid Email Id or Password!!";
            } 

        }

        private void frmLogin_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void sanjeev(object sender, EventArgs e)
        {

        }
    }
}
