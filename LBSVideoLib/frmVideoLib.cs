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
    public partial class frmVideoLib : Form
    {
        public frmVideoLib()
        {
            InitializeComponent();
        }

        private void frmVideoLib_FormClosed(object sender, FormClosedEventArgs e)
        {
            frmMain frmMain= new frmMain();
            frmMain.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmPlayVideo frmVideo = new frmPlayVideo();
            frmVideo.Show();
        }
    }
}
