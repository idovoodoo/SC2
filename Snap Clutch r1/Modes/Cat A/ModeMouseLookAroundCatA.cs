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
    public class ModeMouseLookAroundCatA : Mode
    {
        //private MOUSESTATE myMouse;
        //private Point myDragPos = new Point();

        public static Rectangle screenSize = Screen.PrimaryScreen.Bounds;
        private int x1 = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * 0.2);
        private int x2 = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * 0.8);
        private int x3 = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * 0.4);
        private int x4 = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * 0.6);

        private double xGrad1 = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * 0.3);
        private double xGrad2 = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * 0.7);


        private int y1 = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height * 0.3);
        private int y2 = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height * 0.7);

        private Point centrePoint = new Point((Screen.PrimaryScreen.WorkingArea.Width / 2), Screen.PrimaryScreen.WorkingArea.Height / 2);
        private int speedFactor = 10;
        private bool notTurning = false;

        // constructor
        public ModeMouseLookAroundCatA()
        {
            modeName = "Mouse Look Around Cat A";
        }

        // execute the mode
        public override MOUSESTATE Execute(Point argGazePos, MOUSESTATE argMouse, GazeOverlay argGazeOverlay)
        {
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
            if (argGazePos.X < xGrad1 && argGazePos.X > 0 && argGazePos.Y > y1 && argGazePos.Y < y2)
            {
                MouseEvent.RightDownPoint(centrePoint.X, centrePoint.Y + 2);
                argMouse.rightButton = true;               
                double offset = speedFactor - ((argGazePos.X / xGrad1) * speedFactor);
                Cursor.Position = new Point(centrePoint.X - Convert.ToInt16(offset), centrePoint.Y + 2);
                //Console.WriteLine("left offset = " + offset);
                notTurning = false;
            }

            // gradual turn right
            else if (argGazePos.X > xGrad2 && argGazePos.X < screenSize.Width && argGazePos.Y > y1 && argGazePos.Y < y2)
            {

                MouseEvent.RightDownPoint(centrePoint.X, centrePoint.Y + 2);
                argMouse.rightButton = true;
                double offset = (((argGazePos.X - xGrad2) / (Screen.PrimaryScreen.WorkingArea.Width - xGrad2)) * speedFactor) * 1.5;
                //Console.WriteLine("right offset = " + offset);
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
