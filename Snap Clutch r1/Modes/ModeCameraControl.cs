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
    public class ModeCameraControl : Mode
    {
        //private MOUSESTATE myMouse;
        public static Rectangle screenSize = Screen.PrimaryScreen.Bounds;
        private int x2 = Convert.ToInt32(screenSize.Width * 0.125);
        private int x3 = Convert.ToInt32(screenSize.Width * 0.25);
        private int x4 = Convert.ToInt32(screenSize.Width * 0.75);
        private int x5 = Convert.ToInt32(screenSize.Width * 0.875);
        private int x6 = screenSize.Width;
        private int y2 = Convert.ToInt32(screenSize.Height * 0.125);
        private int y3 = Convert.ToInt32(screenSize.Height * 0.25);
        private int y4 = Convert.ToInt32(screenSize.Height * 0.75);
        private int y5 = Convert.ToInt32(screenSize.Height * 0.875);
        private int y6 = screenSize.Height;

        // constructor
        public ModeCameraControl()
        {
            modeName = "Keyboard Camera Control";
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

            if (argMouse.numKey2)
            {
                KeyEvent.NumKey2Up();
                argMouse.numKey2 = false;
            }
            if (argMouse.numKey4)
            {
                KeyEvent.NumKey4Up();
                argMouse.numKey4 = false;
            }
            if (argMouse.numKey6)
            {
                KeyEvent.NumKey6Up();
                argMouse.numKey6 = false;
            }
            if (argMouse.numKey8)
            {
                KeyEvent.NumKey8Up();
                argMouse.numKey8 = false;
            }


            CursorFactory.Create(Application.StartupPath + CursorSnapClutch.EyeControlOffSmall());
            // cursor follows gaze position
            Cursor.Position = argGazePos;

            // camera left
            if (argGazePos.X < x3 && argGazePos.Y > y2 && argGazePos.Y < y5)
            {

                KeyEvent.NumKey4Up();
                KeyEvent.NumKey2Up();
                KeyEvent.NumKey6Down();
                KeyEvent.NumKey8Up();

                argMouse.numKey2 = false;
                argMouse.numKey4 = false;
                argMouse.numKey6 = true;
                argMouse.numKey8 = false;

            }
            // camera right
            else if (argGazePos.X > x4 && argGazePos.Y > y2 && argGazePos.Y < y5)
            {
                KeyEvent.NumKey4Down();
                KeyEvent.NumKey2Up();
                KeyEvent.NumKey6Up();
                KeyEvent.NumKey8Up();

                argMouse.numKey2 = false;
                argMouse.numKey4 = true;
                argMouse.numKey6 = false;
                argMouse.numKey8 = false;
            }
            // camera tilt up
            else if (argGazePos.X > x2 && argGazePos.X < x5 && argGazePos.Y < y3)
            {
                KeyEvent.NumKey4Up();
                KeyEvent.NumKey2Down();
                KeyEvent.NumKey6Up();
                KeyEvent.NumKey8Up();

                argMouse.numKey2 = true;
                argMouse.numKey4 = false;
                argMouse.numKey6 = false;
                argMouse.numKey8 = false;
            }
            // camera tilt down
            else if (argGazePos.X > x2 && argGazePos.X < x5 && argGazePos.Y > y4)
            {
                KeyEvent.NumKey4Up();
                KeyEvent.NumKey2Up();
                KeyEvent.NumKey6Up();
                KeyEvent.NumKey8Down();

                argMouse.numKey2 = false;
                argMouse.numKey4 = false;
                argMouse.numKey6 = false;
                argMouse.numKey8 = true;
            }
            else
            {
                // do nothing!!!!!! :P
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
