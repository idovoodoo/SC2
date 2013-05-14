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
    public class ModeKeyLookAroundCatA : Mode
    {
        //private MOUSESTATE myMouse;
        public static Rectangle screenSize = Screen.PrimaryScreen.Bounds;
        private int x1 = Convert.ToInt32(screenSize.Width * 0.2);
        private int x2 = Convert.ToInt32(screenSize.Width * 0.8);
        private int xc1 = Convert.ToInt32(screenSize.Width * 0.39); //based on 400x400 sq
        private int xc2 = Convert.ToInt32(screenSize.Width * 0.58); //based on 400x400 sq
        private int y1 = Convert.ToInt32(screenSize.Height * 0.3);
        private int y2 = Convert.ToInt32(screenSize.Height * 0.7);

        // constructor
        public ModeKeyLookAroundCatA()
        {
            modeName = "Keyboard Look Around Cat A";
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

            //CursorFactory.Create(Application.StartupPath + CursorSnapClutch.EyeControlOffSmall());
            //CursorFactory.CreateEyeControlOffSmallCursor();
            // cursor follows gaze position
            //Cursor.Position = argGazePos;

            // turn left
            if (argGazePos.X < xc1 && argGazePos.Y > y1 && argGazePos.Y < y2)
            {
                if (!argMouse.aKey)
                {
                    KeyEvent.AKeyDown();
                    argMouse.aKey = true;
                }
                if (argMouse.dKey)
                {
                    KeyEvent.DKeyUp();
                    argMouse.dKey = false;
                }
            }
            //turn right
            else if (argGazePos.X > xc2 && argGazePos.Y > y1 && argGazePos.Y < y2)
            {
                if (!argMouse.dKey)
                {
                    KeyEvent.DKeyDown();
                    argMouse.dKey = true;
                }
                if (argMouse.aKey)
                {
                    KeyEvent.AKeyUp();
                    argMouse.aKey = false;
                }
            }
            else
            {
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
