/*
Copyright (c) 2008 - 2009, De Montfort University
All rights reserved.

Redistribution and use in source and binary forms, with or without modification,
is not permitted.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR
ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

*/
using System;
using System.Drawing;
using System.Windows.Forms;
using SnapClutch.SCTools;

namespace SnapClutch.Modes
{
    // dwell click on left mouse button
    public class ModeMouseLookAroundCatB : Mode
    {
        //private MOUSESTATE myMouse;
        //private Point myDragPos = new Point();

        public static Rectangle screenSize = Screen.PrimaryScreen.Bounds;
        private int x1 = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * 0.1);
        private int x2 = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * 0.9);
        private int x3 = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * 0.4);
        private int x4 = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * 0.6);

        private double xGrad1 = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * 0.1);
        private double xGrad2 = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * 0.9);

        private int y1 = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height * 0.3);
        private int y2 = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height * 0.7);

        private int xL1 = -Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * 0.2);
        private int rectWidth = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * 0.3);
        private int rectHeight = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height * 0.4);

        private Point centrePoint = new Point((Screen.PrimaryScreen.WorkingArea.Width / 2), Screen.PrimaryScreen.WorkingArea.Height / 2);
        private int speedFactor = 7;
        private bool notTurning = false;

        //private Rectangle leftRect = new Rectangle();
        //private Rectangle rightRect = new Rectangle();

        // constructor
        public ModeMouseLookAroundCatB()
        {
            modeName = "Mouse Look Around Cat B";

            //leftRect = new Rectangle(xL1, y1, rectWidth, rectHeight);
        }

        // execute the mode
        public override MOUSESTATE Execute(Point argGazePos, MOUSESTATE argMouse, GazeOverlay argGazeOverlay)
        {
            CursorFactory.Create(Application.StartupPath + CursorSnapClutch.DefaultCursor());
            if (argMouse.sKey)
            {
                KeyEvent.SKeyUp();
                argMouse.sKey = false;
            }
            if (argMouse.wKey)
            {
                KeyEvent.WKeyUp();
                argMouse.wKey = false;
            }
           
            // gradual turn left
            if (argGazePos.X < xGrad1 && argGazePos.Y > y1 && argGazePos.Y < y2)
            {
                double offset = 0.0;

                MouseEvent.RightDownPoint(centrePoint.X, centrePoint.Y + 2);
                argMouse.rightButton = true;

                if (argGazePos.X > 0)
                    offset = (speedFactor / 2) * 0.8;
                else
                    offset = speedFactor * 0.8;

                Cursor.Position = new Point(centrePoint.X - Convert.ToInt16(offset), centrePoint.Y + 2);
                notTurning = false;
            }

            // gradual turn right
            else if (argGazePos.X > xGrad2 && argGazePos.Y > y1 && argGazePos.Y < y2)
            {
                double offset = 0.0;

                MouseEvent.RightDownPoint(centrePoint.X, centrePoint.Y + 2);
                argMouse.rightButton = true;

                if (argGazePos.X < Screen.PrimaryScreen.WorkingArea.Width)
                    offset = (speedFactor / 2) * 2;
                else
                    offset = speedFactor * 1.5;

                Cursor.Position = new Point(centrePoint.X + Convert.ToInt16(offset), centrePoint.Y + 2);
                notTurning = false;
            }

            else
            {
                if (argMouse.rightButton && !notTurning)
                {
                    MouseEvent.RightUpPoint(centrePoint.X, centrePoint.Y + 2);
                    argMouse.rightButton = false;
                }

                notTurning = true;
            }

            return argMouse;
        }

        public override MOUSESTATE SomeEvent(Point argGazePos, bool argSwitch, bool argDown)
        {
            return myMouse;
        }

        public override MOUSESTATE BlinkEvent(Point argGazePos, int argEye)
        {
            return myMouse;
        }

        // return the name of the mode
        public override string GetModeName()
        {
            return modeName;
        }
    }
}
