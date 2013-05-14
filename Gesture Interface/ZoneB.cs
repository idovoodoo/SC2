using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Gesture_Interface
{
    public partial class ZoneB : Form
    {
        private Rectangle rect;

        public ZoneB()
        {
            InitializeComponent();
        }

        public void SetZone(Rectangle argRect)
        {
            rect = argRect;

            this.Height = rect.Height;
            this.Width = rect.Width;
            this.Left = rect.Left;
            this.Top = rect.Top;
            //Console.WriteLine("RECT left {0} top {1} right {2} bottom {3} width {4} height {5}", rect.Left, rect.Top, rect.Right, rect.Bottom, rect.Width, rect.Height);
            //Console.WriteLine("THIS left {0} top {1} right {2} bottom {3} width {4} height {5}", this.Left, this.Top, this.Right, this.Bottom, this.Width, this.Height);

            GraphicsPath path = new GraphicsPath();

            // for circular segment
            //path.AddArc(0, 0, this.Width, this.Height, 270, 90);
            //path.AddLine(this.Width, this.Height / 2, this.Width, 0);

            //use this for trianglular segment
            path.AddLine(this.Width / 2, 0, this.Width, 0);
            path.AddLine(this.Width, this.Height / 2, this.Width / 2, 0);
            //path.AddLine(this.Width, 0, 0);


            this.Region = new Region(path);
            //this.Opacity = 0.1;
            this.BackColor = Color.Red;

        }

        // events handled in mainform!!!
        //private void ZoneB_MouseEnter(object sender, EventArgs e)
        //{
        //    this.Opacity = 0.3;
        //}

        //private void ZoneB_MouseLeave(object sender, EventArgs e)
        //{
        //    this.Opacity = 0.1;
        //}
    }
}
