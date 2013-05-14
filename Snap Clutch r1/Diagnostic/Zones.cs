using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace SnapClutch.Diagnostic
{
    public class Zones
    {
        // these are the default template rectangles
        public Rectangle centre, top, bottom, left, right, 
            topleft, topright, bottomleft, bottomright, 
            offtop, offbottom, offleft, offright;
        // these are the actual smaller rectangles
        public Rectangle centre2, top2, bottom2, left2, right2, 
            topleft2, topright2, bottomleft2, bottomright2, 
            offtop2, offbottom2, offleft2, offright2;
        // a 3rd set of rectangles now!!
        public Rectangle centre3, top3, bottom3, left3, right3,
            topleft3, topright3, bottomleft3, bottomright3,
            offtop3, offbottom3, offleft3, offright3;

        public int screenWidth = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
        public int screenHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
        public int overlayCentreWidth, overlayCentreHeight, distance;
        public double scaleFactor;

        public Zones(int centreWidth, int centreHeight, double argScale)
        {
            overlayCentreWidth = centreWidth;
            overlayCentreHeight = centreHeight;
            scaleFactor = argScale;
            SetUpZones();
        }

        /// <summary>
        /// This constructor sets up the rectangles based on distance from centre. Also, all rectangles are the same size
        /// </summary>
        /// <param name="centreWidth"></param>
        /// <param name="centreHeight"></param>
        /// <param name="argScale"></param>
        /// <param name="argDistance"></param>
        public Zones(int centreWidth, int centreHeight, double argScale, int argDistance)
        {
            overlayCentreWidth = centreWidth;
            overlayCentreHeight = centreHeight;
            scaleFactor = argScale;
            distance = argDistance;
            SetUpZones();
            SetUp3rdZones();
        }

        /// <summary>
        /// Checks if fixation is with a FULL zone
        /// </summary>
        /// <param name="argFixation"></param>
        /// <returns></returns>
        public string CheckZones(FixationDataPoint argFixation)
        {
            if(topleft.Contains(argFixation.GetFixationPoint()))
                return "1";

            if(top.Contains(argFixation.GetFixationPoint()))
                return "2";

            if(topright.Contains(argFixation.GetFixationPoint()))
                return "3";
            
            if(left.Contains(argFixation.GetFixationPoint()))
                return "4";
            
            if(centre.Contains(argFixation.GetFixationPoint()))
                return "5";
            
            if(right.Contains(argFixation.GetFixationPoint()))
                return "6";
            
            if(bottomleft.Contains(argFixation.GetFixationPoint()))
                return "7";
            
            if(bottom.Contains(argFixation.GetFixationPoint()))
                return "8";
            
            if(bottomright.Contains(argFixation.GetFixationPoint()))
                return "9";
            
            if(offtop.Contains(argFixation.GetFixationPoint()))
                return "A";
            
            if(offright.Contains(argFixation.GetFixationPoint()))
                return "B";
            
            if(offbottom.Contains(argFixation.GetFixationPoint()))
                return "C";

            if(offleft.Contains(argFixation.GetFixationPoint()))
                return "D";

            return "0";
        }

        /// <summary>
        /// Checks if fixation is within a SCALED zone
        /// </summary>
        /// <param name="argFixation"></param>
        /// <returns></returns>
        public string CheckScaledZones(FixationDataPoint argFixation)
        {
            if (topleft2.Contains(argFixation.GetFixationPoint()))
                return "1";

            if (top2.Contains(argFixation.GetFixationPoint()))
                return "2";

            if (topright2.Contains(argFixation.GetFixationPoint()))
                return "3";

            if (left2.Contains(argFixation.GetFixationPoint()))
                return "4";

            if (centre2.Contains(argFixation.GetFixationPoint()))
                return "5";

            if (right2.Contains(argFixation.GetFixationPoint()))
                return "6";

            if (bottomleft2.Contains(argFixation.GetFixationPoint()))
                return "7";

            if (bottom2.Contains(argFixation.GetFixationPoint()))
                return "8";

            if (bottomright2.Contains(argFixation.GetFixationPoint()))
                return "9";

            if (offtop.Contains(argFixation.GetFixationPoint()))
                return "A";

            if (offright.Contains(argFixation.GetFixationPoint()))
                return "B";

            if (offbottom.Contains(argFixation.GetFixationPoint()))
                return "C";

            if (offleft.Contains(argFixation.GetFixationPoint()))
                return "D";

            return "0";
        }

        /// <summary>
        /// Checks if fixation is within a zone that has been set using DISTANCE
        /// </summary>
        /// <param name="argFixation"></param>
        /// <returns></returns>
        public string CheckDistanceZones(FixationDataPoint argFixation)
        {
            if (topleft3.Contains(argFixation.GetFixationPoint()))
                return "1";

            if (top3.Contains(argFixation.GetFixationPoint()))
                return "2";

            if (topright3.Contains(argFixation.GetFixationPoint()))
                return "3";

            if (left3.Contains(argFixation.GetFixationPoint()))
                return "4";

            if (centre3.Contains(argFixation.GetFixationPoint()))
                return "5";

            if (right3.Contains(argFixation.GetFixationPoint()))
                return "6";

            if (bottomleft3.Contains(argFixation.GetFixationPoint()))
                return "7";

            if (bottom3.Contains(argFixation.GetFixationPoint()))
                return "8";

            if (bottomright3.Contains(argFixation.GetFixationPoint()))
                return "9";

            if (offtop.Contains(argFixation.GetFixationPoint()))
                return "A";

            if (offright.Contains(argFixation.GetFixationPoint()))
                return "B";

            if (offbottom.Contains(argFixation.GetFixationPoint()))
                return "C";

            if (offleft.Contains(argFixation.GetFixationPoint()))
                return "D";

            return "0";
        }

        /// <summary>
        /// Sets up the rectangular zones
        /// </summary>
        private void SetUpZones()
        {           
            topleft = new Rectangle(0, 0, (screenWidth - overlayCentreWidth) / 2, (screenHeight - overlayCentreHeight) / 2);
            top = new Rectangle(topleft.Right, 0, overlayCentreWidth, (screenHeight - overlayCentreHeight) / 2);
            topright = new Rectangle(top.Right, 0, topleft.Width, topleft.Height);
            left = new Rectangle(0, topleft.Bottom, topleft.Width, overlayCentreHeight);
            centre = new Rectangle((screenWidth / 2) - (overlayCentreWidth / 2), (screenHeight / 2) - (overlayCentreHeight / 2), overlayCentreWidth, overlayCentreHeight);
            right = new Rectangle(centre.Right, centre.Top, topleft.Width, centre.Height);
            bottom = new Rectangle(centre.Left, centre.Bottom, centre.Width, top.Height);
            bottomleft = new Rectangle(0, left.Bottom, left.Width, bottom.Height);
            bottomright = new Rectangle(bottom.Right, centre.Bottom, right.Width, bottom.Height);
            offtop = new Rectangle(0, -500, screenWidth, 500);
            offright = new Rectangle(screenWidth, 0, 500, screenHeight);
            offbottom = new Rectangle(0, screenHeight, screenWidth, 500);
            offleft = new Rectangle(-500, 0, 500, screenHeight);


            topleft2 = new Rectangle((topleft.Width - Converter(topleft.Width)) / 2, (topleft.Height - Converter(topleft.Height)) / 2,
                Converter(topleft.Width), Converter(topleft.Height));
            top2 = new Rectangle(top.Left + (top.Width - Converter(top.Width)) / 2, (top.Height - Converter(top.Height)) / 2,
                Converter(top.Width), Converter(top.Height));
            topright2 = new Rectangle(topright.Left + (topright.Width - Converter(topright.Width)) / 2, (topright.Height - Converter(topright.Height)) / 2,
                Converter(topright.Width), Converter(topright.Height));
            left2 = new Rectangle((left.Width - Converter(left.Width)) / 2, left.Top + (left.Height - Converter(left.Height)) / 2,
                Converter(left.Width), Converter(left.Height));
            centre2 = new Rectangle(centre.Left + (centre.Width - Converter(centre.Width)) / 2, centre.Top + (centre.Height - Converter(centre.Height)) / 2,
                Converter(centre.Width), Converter(centre.Height));
            right2 = new Rectangle(right.Left + (right.Width - Converter(right.Width)) / 2, right.Top + (right.Height - Converter(right.Height)) / 2,
                Converter(right.Width), Converter(right.Height));
            bottomleft2 = new Rectangle((bottomleft.Width - Converter(bottomleft.Width)) / 2, bottomleft.Top + (bottomleft.Height - Converter(bottomleft.Height)) / 2,
                Converter(bottomleft.Width), Converter(bottomleft.Height));
            bottom2 = new Rectangle(bottom.Left + (bottom.Width - Converter(bottom.Width)) / 2, bottom.Top + (bottom.Height - Converter(bottom.Height)) / 2,
                Converter(bottom.Width), Converter(bottom.Height));           
            bottomright2 = new Rectangle(bottomright.Left + (bottomright.Width - Converter(bottomright.Width)) / 2, bottomright.Top + (bottomright.Height - Converter(bottom.Height)) / 2,
                Converter(bottomright.Width), Converter(bottomright.Height));
           

            //Console.WriteLine(bottomright.ToString());
            //Console.WriteLine(bottomright2.ToString());
            //Console.WriteLine(topleft.Width * Convert.ToInt32(scaleFactor));
        }

        private void SetUp3rdZones()
        {
            centre3 = new Rectangle((screenWidth / 2) - (overlayCentreWidth / 2), (screenHeight / 2) - (overlayCentreHeight / 2), overlayCentreWidth, overlayCentreHeight);
            top3 = new Rectangle(new Point(centre3.Left, centre3.Top - distance), centre3.Size);
            bottom3 = new Rectangle(new Point(centre3.Left, centre3.Bottom + distance - centre3.Height), centre3.Size);
            left3 = new Rectangle(new Point(centre3.Left - distance, centre3.Top), centre3.Size);
            right3 = new Rectangle(new Point(centre3.Right + distance - centre3.Width, centre3.Top), centre3.Size);
            topleft3 = new Rectangle(new Point(left3.Left, top3.Top), centre3.Size);
            topright3 = new Rectangle(new Point(right3.Left, top3.Top), centre3.Size);
            bottomleft3 = new Rectangle(new Point(left3.Left, bottom3.Top), centre3.Size);
            bottomright3 = new Rectangle(new Point(right3.Left, bottom3.Top), centre3.Size);
        }

        private int Converter(int argDim)
        {
            return (int) (Convert.ToDouble(argDim) * scaleFactor);
        }
    }
}