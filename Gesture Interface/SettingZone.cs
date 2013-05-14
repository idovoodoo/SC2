using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Gesture_Interface
{
    public partial class SettingZone : Form
    {
        public SettingZone()
        {
            InitializeComponent();
            this.Opacity = 0.5;
            
            //this.Show();
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            int WM_NCHITTEST = 0x84;
            if (m.Msg == WM_NCHITTEST)
            {
                int HTCLIENT = 1;
                int HTCAPTION = 2;
                if (m.Result.ToInt32() == HTCLIENT)
                    m.Result = (IntPtr)HTCAPTION;
            }
        }

        public Rectangle GetPosition()
        {
            //Console.WriteLine("ZONE left {0} top {1} right {2} bottom {3} width {4} height {5}", this.Left, this.Top, this.Right, this.Bottom, this.Width, this.Height);
            return new Rectangle(this.Left, this.Top, this.Width, this.Height);   
      
        }

        private void buttonSmall_Click(object sender, EventArgs e)
        {
            this.Width = 200;
            this.Height = 200;
        }

        private void buttonMax_Click(object sender, EventArgs e)
        {
            this.Left = 0;
            this.Top = 0;
            this.Width = Screen.PrimaryScreen.Bounds.Width;
            this.Height = Screen.PrimaryScreen.Bounds.Height;
        }

        private void buttonSquare_Click(object sender, EventArgs e)
        {
            this.Width = this.Height;
        }

        //private void SettingZone_DoubleClick(object sender, EventArgs e)
        //{

        //}       
    }
}
