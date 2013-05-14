using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SnapClutch.Diagnostic
{
    public partial class SemiTranspZone : Form
    {
        string key = "X";
        bool hide = false;

        public SemiTranspZone()
        {
            InitializeComponent();
        }

        //public void Hide(bool argHide)
        //{
        //    hide = argHide;
        //}

        private void SetImage()
        {
            if (key.Equals("W"))
            {
                pictureBox1.Image = Properties.Resources.forward;
                pictureBox1.Width = Properties.Resources.forward.Width;
                pictureBox1.Height = Properties.Resources.forward.Height;
            }
            if (key.Equals("A"))
            {
                pictureBox1.Image = Properties.Resources.left;
                pictureBox1.Width = Properties.Resources.left.Width;
                pictureBox1.Height = Properties.Resources.left.Height;
            }
            if (key.Equals("S"))
            {
                pictureBox1.Image = Properties.Resources.backwards;
                pictureBox1.Width = Properties.Resources.backwards.Width;
                pictureBox1.Height = Properties.Resources.backwards.Height;
            }
            if (key.Equals("D"))
            {
                pictureBox1.Image = Properties.Resources.right;
                pictureBox1.Width = Properties.Resources.right.Width;
                pictureBox1.Height = Properties.Resources.right.Height;
            }
            if (key.Equals("WA"))
            {
                pictureBox1.Image = Properties.Resources.forwardleft;
                pictureBox1.Width = Properties.Resources.forwardleft.Width;
                pictureBox1.Height = Properties.Resources.forwardleft.Height;
            }
            if (key.Equals("WD"))
            {
                pictureBox1.Image = Properties.Resources.forwardright;
                pictureBox1.Width = Properties.Resources.forwardright.Width;
                pictureBox1.Height = Properties.Resources.forwardright.Height;
            }
            
            pictureBox1.Left = (this.Width / 2) - (pictureBox1.Width / 2);
            pictureBox1.Top = (this.Height / 2) - (pictureBox1.Height / 2);
        }

        public bool IsHidden()
        {
            if (key.Equals("X"))
                return true;
            else
                return false;
        }

        public void SetKey(string argKey)
        {
            key = argKey;
            SetImage();
        }

        public string GetKey()
        {
            return key;
        }
    }
}
