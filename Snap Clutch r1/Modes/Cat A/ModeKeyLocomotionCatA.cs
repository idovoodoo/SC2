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
    public class ModeKeyLocomotionCatA : Mode
    {
        //private MOUSESTATE myMouse;
        public static Rectangle screenSize = Screen.PrimaryScreen.Bounds;
        private int x1 = Convert.ToInt32(screenSize.Width * 0.2);
        private int x2 = Convert.ToInt32(screenSize.Width * 0.8);
        private int x3 = Convert.ToInt32(screenSize.Width * 0.4);
        private int x4 = Convert.ToInt32(screenSize.Width * 0.6);
        private int xc1 = Convert.ToInt32(screenSize.Width * 0.39); //based on 400x400 sq
        private int xc2 = Convert.ToInt32(screenSize.Width * 0.58); //based on 400x400 sq

        private int y1 = Convert.ToInt32(screenSize.Height * 0.3);
        private int y2 = Convert.ToInt32(screenSize.Height * 0.7);

        // constructor
        public ModeKeyLocomotionCatA()
        {
            modeName = "Keyboard Locomotion Cat A";
        }

        // execute the mode
        public override MOUSESTATE Execute(Point argGazePos, MOUSESTATE argMouse, GazeOverlay argGazeOverlay)
        {
            if (argMouse.leftButton)
            {
                MouseEvent.LeftUp();
                argMouse.leftButton = false;
            }
            if (argMouse.rightButton)
            {
                MouseEvent.RightUp();
                argMouse.rightButton = false;
            }

            CursorFactory.Create(Application.StartupPath + CursorSnapClutch.DefaultCursor());
            //CursorFactory.CreateLocomotionCursor();
            //Cursor.Position = argGazePos;


            // this will all move over into overlays in the future

            // turn left
            if (argGazePos.X < xc1 && argGazePos.Y > y1 && argGazePos.Y < y2)
            {
                if (!argMouse.wKey)
                {
                    KeyEvent.WKeyDown();
                    argMouse.wKey = true;
                }
                if (!argMouse.aKey)
                {
                    KeyEvent.AKeyDown();
                    argMouse.aKey = true;
                }
                if (argMouse.sKey)
                {
                    KeyEvent.SKeyUp();
                    argMouse.sKey = false;
                }
                if (argMouse.dKey)
                {
                    KeyEvent.DKeyUp();
                    argMouse.dKey = false;
                }
            }

            // turn right
            else if (argGazePos.X > xc2 && argGazePos.Y > y1 && argGazePos.Y < y2)
            {
                if (!argMouse.wKey)
                {
                    KeyEvent.WKeyDown();
                    argMouse.wKey = true;
                }
                if (!argMouse.dKey)
                {
                    KeyEvent.DKeyDown();
                    argMouse.dKey = true;
                }
                if (argMouse.sKey)
                {
                    KeyEvent.SKeyUp();
                    argMouse.sKey = false;
                }
                if (argMouse.aKey)
                {
                    KeyEvent.AKeyUp();
                    argMouse.aKey = false;
                }
            }

            // backwards
            else if (argGazePos.Y > y2 && argGazePos.X > x3 && argGazePos.X < x4)
            {
                if (!argMouse.sKey)
                {
                    KeyEvent.SKeyDown();
                    argMouse.sKey = true;
                }
                if (argMouse.wKey)
                {
                    KeyEvent.WKeyUp();
                    argMouse.wKey = false;
                }
                if (argMouse.dKey)
                {
                    KeyEvent.DKeyUp();
                    argMouse.dKey = false;
                }
                if (argMouse.aKey)
                {
                    KeyEvent.AKeyUp();
                    argMouse.aKey = false;
                }
            }

            // rotate left
            else if (argGazePos.Y > y2 && argGazePos.X < xc1 && argGazePos.X > 0)
            {
                if (!argMouse.aKey)
                {
                    KeyEvent.AKeyDown();
                    argMouse.aKey = true;
                }
                if (argMouse.wKey)
                {
                    KeyEvent.WKeyUp();
                    argMouse.wKey = false;
                }
                if (argMouse.dKey)
                {
                    KeyEvent.DKeyUp();
                    argMouse.dKey = false;
                }
                if (argMouse.sKey)
                {
                    KeyEvent.SKeyUp();
                    argMouse.sKey = false;
                }
            }

            //rotate right
            else if (argGazePos.Y > y2 && argGazePos.X > xc2 && argGazePos.X < screenSize.Width)
            {
                if (!argMouse.dKey)
                {
                    KeyEvent.DKeyDown();
                    argMouse.dKey = true;
                }
                if (argMouse.wKey)
                {
                    KeyEvent.WKeyUp();
                    argMouse.wKey = false;
                }
                if (argMouse.aKey)
                {
                    KeyEvent.AKeyUp();
                    argMouse.aKey = false;
                }
                if (argMouse.sKey)
                {
                    KeyEvent.SKeyUp();
                    argMouse.sKey = false;
                }
            }

            // gaze position off to the left or right of the screen
            else if (argGazePos.X < 0 || argGazePos.X > screenSize.Width)
            {
                if (argMouse.wKey)
                {
                    KeyEvent.WKeyUp();
                    argMouse.wKey = false;
                }
                if (argMouse.dKey)
                {
                    KeyEvent.DKeyUp();
                    argMouse.dKey = false;
                }
                if (argMouse.aKey)
                {
                    KeyEvent.AKeyUp();
                    argMouse.aKey = false;
                }
                if (argMouse.sKey)
                {
                    KeyEvent.SKeyUp();
                    argMouse.sKey = false;
                }
            }

            // forward
            else// if (argGazePos.Y < y1)
            {
                if (!argMouse.wKey)
                {
                    KeyEvent.WKeyDown();
                    argMouse.wKey = true;
                }
                if (argMouse.dKey)
                {
                    KeyEvent.DKeyUp();
                    argMouse.dKey = false;
                }
                if (argMouse.aKey)
                {
                    KeyEvent.AKeyUp();
                    argMouse.aKey = false;
                }
                if (argMouse.sKey)
                {
                    KeyEvent.SKeyUp();
                    argMouse.sKey = false;
                }
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
