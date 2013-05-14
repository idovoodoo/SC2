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
    public partial class GestureInfo : Form
    {

        public GestureInfo()
        {
            InitializeComponent();
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

        public void UpdateLabels(string[] argLabels)
        {
            label1A.Text = argLabels[0];
            label1B.Text = argLabels[1];
            label1C.Text = argLabels[2];
            label2A.Text = argLabels[3];
            label2B.Text = argLabels[4];
            label2C.Text = argLabels[5];
            label3A.Text = argLabels[6];
            label3B.Text = argLabels[7];
            label3C.Text = argLabels[8];
            label4A.Text = argLabels[9];
            label4B.Text = argLabels[10];
            label4C.Text = argLabels[11];
        }

        private void trackBarOpacityEnter_Scroll(object sender, EventArgs e)
        {
            TrackBar tb = (TrackBar)sender;
            this.Opacity = ((double)tb.Value / tb.Maximum);
        }
     
    }
}
