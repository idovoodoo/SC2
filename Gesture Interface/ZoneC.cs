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
    public partial class ZoneC : Form
    {
        private Rectangle rect;

        public ZoneC()
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
            //path.AddArc(0, 0, this.Width, this.Height, 0, 90);
            //path.AddLine(this.Width / 2, this.Height, this.Width, this.Height);


            //use this for trianglular segment
            path.AddLine(this.Width, this.Height / 2, this.Width, this.Height);
            path.AddLine(this.Width, this.Height, this.Width / 2, this.Height);


            this.Region = new Region(path);
            //this.Opacity = 0.1;
            this.BackColor = Color.Red;

        }

        // events handled in mainform!!!
        //private void ZoneC_MouseEnter(object sender, EventArgs e)
        //{
        //    this.Opacity = 0.3;
        //}

        //private void ZoneC_MouseLeave(object sender, EventArgs e)
        //{
        //    this.Opacity = 0.1;
        //}
    }
}
