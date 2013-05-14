using System;
using System.Collections.Generic;
using System.Text;

namespace SnapClutch.Modes.EyeGuitar
{
   public class StringPoint
    {
        private int Xcoord;
        private int Ycoord;
        private int defaultColor;

        public int DefaultColor
        {
            get { return defaultColor; }
            set { defaultColor = value; }
        }

        public int Ycoord1
        {
            get { return Ycoord; }
            set { Ycoord = value; }
        }

        public int Xcoord1
        {
            get { return Xcoord; }
            set { Xcoord = value; }
        }
       
           public StringPoint(int x, int y)
        {
            this.Xcoord = x;
            this.Ycoord = y;
         
        }

    }
}
