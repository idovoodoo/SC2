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
using System.Threading;
using SnapClutch.SCTools;

namespace SnapClutch.Modes
{
    // dwell click on left mouse button
    public class ModeMouseLocomotionCatA : Mode
    {
        //private MOUSESTATE myMouse;
        public static Rectangle screenSize = Screen.PrimaryScreen.Bounds;
        private int x1 = Convert.ToInt32(screenSize.Width * 0.2);
        private int x2 = Convert.ToInt32(screenSize.Width * 0.8);
        private int x3 = Convert.ToInt32(screenSize.Width * 0.4);
        private int x4 = Convert.ToInt32(screenSize.Width * 0.6);
        private int y1 = Convert.ToInt32(screenSize.Height * 0.3);
        private int y2 = Convert.ToInt32(screenSize.Height * 0.7);

        private double xGrad1 = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * 0.3);
        private double xGrad2 = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * 0.7);
        private double xGrad3 = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * 0.3);
        private double xGrad4 = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * 0.7);

        private Point centrePoint = new Point((Screen.PrimaryScreen.WorkingArea.Width / 2), Screen.PrimaryScreen.WorkingArea.Height / 2);
        private bool goingForward = false;
        private int speedFactor = 10;

        // constructor
        public ModeMouseLocomotionCatA()
        {
            modeName = "Mouse Locomotion Cat A";
        }

        // execute the mode
        public override MOUSESTATE Execute(Point argGazePos, MOUSESTATE argMouse, GazeOverlay argGazeOverlay)
        {
            CursorFactory.Create(Application.StartupPath + CursorSnapClutch.DefaultCursor());
            // turn left
            if (argGazePos.X < xGrad1 && argGazePos.X > 0 && argGazePos.Y > y1 && argGazePos.Y < y2)
            {
                double offset = speedFactor - ((argGazePos.X / xGrad1) * speedFactor);

                if (!argMouse.wKey)
                {
                    KeyEvent.WKeyDown();
                    argMouse.wKey = true;
                }
                if (argMouse.sKey)
                {
                    KeyEvent.SKeyUp();
                    argMouse.sKey = false;
                }

                MouseEvent.RightDownPoint(centrePoint.X, centrePoint.Y + 2);
                argMouse.rightButton = true;
                Cursor.Position = new Point(centrePoint.X - Convert.ToInt16(offset), centrePoint.Y + 2); //was +2
            }

            // turn right
            else if (argGazePos.X > xGrad2 && argGazePos.X < screenSize.Width && argGazePos.Y > y1 && argGazePos.Y < y2)
            {
                double offset = (((argGazePos.X - xGrad4) / (Screen.PrimaryScreen.WorkingArea.Width - xGrad4)) * speedFactor) * 1.5;

                if (!argMouse.wKey)
                {
                    KeyEvent.WKeyDown();
                    argMouse.wKey = true;
                }
                if (argMouse.sKey)
                {
                    KeyEvent.SKeyUp();
                    argMouse.sKey = false;
                }

                MouseEvent.RightDownPoint(centrePoint.X, centrePoint.Y + 2); //was +2
                argMouse.rightButton = true;
                Cursor.Position = new Point(centrePoint.X + Convert.ToInt16(offset), centrePoint.Y + 2); //was +2
            }

            // backwards
            else if (argGazePos.Y > y2 && argGazePos.X > xGrad3 && argGazePos.X < xGrad4)
            {
                if (argMouse.wKey)
                {
                    KeyEvent.WKeyUp();
                    argMouse.wKey = false;
                }
                if (!argMouse.sKey)
                {
                    KeyEvent.SKeyDown();
                    argMouse.sKey = true;
                }
            }

            // rotate left
            else if (argGazePos.Y > y2 && argGazePos.X < xGrad3 && argGazePos.X > 0)
            {
                double offset = speedFactor - ((argGazePos.X / xGrad3) * speedFactor);

                if (argMouse.wKey)
                {
                    KeyEvent.WKeyUp();
                    argMouse.wKey = false;
                }
                if (argMouse.sKey)
                {
                    KeyEvent.SKeyUp();
                    argMouse.sKey = false;
                }

                MouseEvent.RightDownPoint(centrePoint.X, centrePoint.Y + 2); //was +2
                argMouse.rightButton = true;
                Cursor.Position = new Point(centrePoint.X - Convert.ToInt16(offset), centrePoint.Y + 2); // was +2
            }

            //rotate right
            else if (argGazePos.Y > y2 && argGazePos.X > xGrad4 && argGazePos.X < screenSize.Width)
            {

                double offset = (((argGazePos.X - xGrad4) / (Screen.PrimaryScreen.WorkingArea.Width - xGrad4)) * speedFactor) * 1.5;

                if (argMouse.wKey)
                {
                    KeyEvent.WKeyUp();
                    argMouse.wKey = false;
                }
                if (argMouse.sKey)
                {
                    KeyEvent.SKeyUp();
                    argMouse.sKey = false;
                }

                MouseEvent.RightDownPoint(centrePoint.X, centrePoint.Y + 2); //was +2
                argMouse.rightButton = true;
                Cursor.Position = new Point(centrePoint.X + Convert.ToInt16(offset), centrePoint.Y + 2); //was +2
            }

            // gaze position off to the left or right of the screen
            else if (argGazePos.X < 0 || argGazePos.X > screenSize.Width)
            {
                if (argMouse.wKey)
                {
                    KeyEvent.WKeyUp();
                    argMouse.wKey = false;
                }
                if (argMouse.sKey)
                {
                    KeyEvent.SKeyUp();
                    argMouse.sKey = false;
                }
                if (argMouse.rightButton)
                {
                    MouseEvent.RightUpPoint(centrePoint.X, centrePoint.Y + 2);
                    argMouse.rightButton = false;
                }

            }

            // forward
            else //if (argGazePos.Y < y1)
            {
                if (!argMouse.wKey)
                {
                    KeyEvent.WKeyDown();
                    argMouse.wKey = true;
                }
                if (argMouse.sKey)
                {
                    KeyEvent.SKeyUp();
                    argMouse.sKey = false;
                }

                if (argMouse.rightButton)
                {
                    MouseEvent.RightUpPoint(centrePoint.X, centrePoint.Y + 2);
                    argMouse.rightButton = false;
                }

            }

            return argMouse;
        }

        public override MOUSESTATE SomeEvent(Point argGazePos, bool argSwitch, bool argDown)
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
