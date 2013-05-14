using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using SnapClutch.SCTools;

// A class to create objects for each of the guitar string selection buttons
namespace SnapClutch.Modes.EyeGuitar
{
    public class StringButton
    {
        public static Rectangle screenSize = Screen.PrimaryScreen.Bounds;

        private int xStart;
        private int xEnd;
        private int yStart;
        private int yEnd;
        private char keyBoardMapping;

        public StringButton(int xStart, int xEnd, int yStart, int yEnd)
        {
            this.xStart = xStart;
            this.xEnd = xEnd;
            this.yStart = yStart;
            this.yEnd = yEnd;
        }

        public int XStart
        {
            get { return xStart; }
            set { xStart = value; }
        }
     
        public int XEnd
        {
            get { return xEnd; }
            set { xEnd = value; }
        }
      
        public int YStart
        {
            get { return yStart; }
            set { yStart = value; }
        }
        

        public int YEnd
        {
            get { return yEnd; }
            set { yEnd = value; }
        }

        public char KeyBoardMapping
        {
            get { return keyBoardMapping; }
            set { keyBoardMapping = value; }
        }
       

    }
}
