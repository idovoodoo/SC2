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
using System.Runtime.InteropServices;

namespace SnapClutch.Modes
{
    // dwell click on left mouse button
    public class ModeMouseLocomotionFPS : Mode
    {
        //private MOUSESTATE myMouse;
        public static Rectangle screenSize = Screen.PrimaryScreen.Bounds;
        private int x1 = Convert.ToInt32(screenSize.Width * 0.2);
        private int x2 = Convert.ToInt32(screenSize.Width * 0.8);
        private int x3 = Convert.ToInt32(screenSize.Width * 0.4);
        private int x4 = Convert.ToInt32(screenSize.Width * 0.6);
        private int y1 = Convert.ToInt32(screenSize.Height * 0.3);
        private int y2 = Convert.ToInt32(screenSize.Height * 0.7);

        private double xGrad1 = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * 0.4);
        private double xGrad2 = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * 0.6);
        private double xGrad3 = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * 0.4);
        private double xGrad4 = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * 0.6);

        private Point centrePoint = new Point((Screen.PrimaryScreen.WorkingArea.Width / 2), Screen.PrimaryScreen.WorkingArea.Height / 2);
        private bool goingForward = false;
        private int speedFactor = 10;
        private InputControl inputControl = new InputControl();


        // constructor
        public ModeMouseLocomotionFPS()
        {
            modeName = "Mode Mouse Locomotion FPS";
        }

        // execute the mode
        public override MOUSESTATE Execute(Point argGazePos, MOUSESTATE argMouse, GazeOverlay argGazeOverlay)
        {
            if (!argMouse.rightButton)
            {
                MouseEvent.RightDownPoint(centrePoint.X, centrePoint.Y);
                Console.WriteLine("mouse down");
                argMouse.rightButton = true;
                //Cursor.Position = new Point(centrePoint.X, centrePoint.Y);
            }

            // turn left
            if (argGazePos.X < xGrad1 && argGazePos.Y > y1 && argGazePos.Y < y2)
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


                goingForward = false;

                inputControl.RMoveMouse((int) -offset, 0);
                //MouseEvent.HorizontalMove((int) offset);
                //Cursor.Position = new Point(centrePoint.X - Convert.ToInt16(offset), centrePoint.Y);
            }

            // turn right
            else if (argGazePos.X > xGrad2 && argGazePos.Y > y1 && argGazePos.Y < y2)
            {
                double offset = ((argGazePos.X - xGrad2) / (Screen.PrimaryScreen.WorkingArea.Width - xGrad2)) * speedFactor;

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
                goingForward = false;

                inputControl.RMoveMouse((int)offset, 0);
                //MouseEvent.HorizontalMove((int) offset);
            }

            // backwards
            else if (argGazePos.Y > y2 && argGazePos.X > xGrad3 && argGazePos.X < xGrad4)
            {
                if (argMouse.wKey)
                {
                    KeyEvent.WKeyUp();
                    argMouse.wKey = false;
                }
                if (argMouse.sKey)
                {
                    KeyEvent.SKeyDown();
                    argMouse.sKey = true;
                }

                goingForward = false;
            }

            // rotate left
            else if (argGazePos.Y > y2 && argGazePos.X < xGrad3)
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

                goingForward = false;

                inputControl.RMoveMouse((int)-offset, 0);
                //MouseEvent.HorizontalMove((int)-offset);
            }

            //rotate right
            else if (argGazePos.Y > y2 && argGazePos.X > xGrad4)
            {

                double offset = ((argGazePos.X - xGrad4) / (Screen.PrimaryScreen.WorkingArea.Width - xGrad4)) * speedFactor;

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

                goingForward = false;

                inputControl.RMoveMouse((int)offset, 0);
                //MouseEvent.HorizontalMove((int)offset);
            }

            // forward
            else
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

                goingForward = true;
            }

            return argMouse;
        }

        public override MOUSESTATE SomeEvent(Point argGazePos, bool argSwitch, bool argDown)
        {
            return myMouse;
        }

        public override MOUSESTATE BlinkEvent(Point argGazePos, int argEye)
        {
            SnapClutchSounds.Click2();
            KeyEvent.SpaceKeyDown();
            KeyEvent.SpaceKeyUp();
            return myMouse;
        }

        // return the name of the mode
        public override string GetModeName()
        {
            return modeName;
        }

    }
}
