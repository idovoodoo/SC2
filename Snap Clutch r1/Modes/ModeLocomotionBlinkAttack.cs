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
    public class ModeLocomotionBlinkAttack : Mode
    {
        //private MOUSESTATE myMouse;
        public static Rectangle screenSize = Screen.PrimaryScreen.Bounds;
        private int x1 = Convert.ToInt32(screenSize.Width * 0.2);
        private int x2 = Convert.ToInt32(screenSize.Width * 0.8);
        private int x3 = Convert.ToInt32(screenSize.Width * 0.4);
        private int x4 = Convert.ToInt32(screenSize.Width * 0.6);
        private int y1 = Convert.ToInt32(screenSize.Height * 0.3);
        private int y2 = Convert.ToInt32(screenSize.Height * 0.7);

        // constructor
        public ModeLocomotionBlinkAttack()
        {
            modeName = "Keyboard Locomotion Blink Attack";
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

            CursorFactory.Create(Application.StartupPath + CursorSnapClutch.Locomotion());
            //CursorFactory.CreateLocomotionCursor();
            Cursor.Position = argGazePos;


            // this will all move over into overlays in the future
            
            // turn left
            if (argGazePos.X < x1 && argGazePos.Y > y1 && argGazePos.Y < y2)
            {
                //////////////if (argMouse.dKey)
                //////////////{
                //////////////    KeyEvent.DKeyUp();
                //////////////    argMouse.dKey = false;
                //////////////}

                //////////////if (argMouse.sKey)
                //////////////{
                //////////////    KeyEvent.SKeyUp();
                //////////////    argMouse.sKey = false;
                //////////////}


                //////////////KeyEvent.AKeyDown();
                //////////////KeyEvent.WKeyDown();

                //////////////argMouse.aKey = true;
                //////////////argMouse.wKey = true;

                KeyEvent.SKeyUp();
                KeyEvent.DKeyUp();
                KeyEvent.AKeyDown();

                argMouse.dKey = false;
                argMouse.aKey = true;
                argMouse.wKey = true;
                argMouse.sKey = false;
              
            }

            // turn right
            else if (argGazePos.X > x2 && argGazePos.Y > y1 && argGazePos.Y < y2)
            {
                ////////////////if (argMouse.aKey)
                ////////////////{
                ////////////////    KeyEvent.AKeyUp();
                ////////////////    argMouse.aKey = false;
                ////////////////}

                ////////////////if (argMouse.sKey)
                ////////////////{
                ////////////////    KeyEvent.SKeyUp();
                ////////////////    argMouse.sKey = false;
                ////////////////}

                ////////////////KeyEvent.DKeyDown();
                ////////////////KeyEvent.WKeyDown();

                ////////////////argMouse.dKey = true;
                ////////////////argMouse.wKey = true;



                KeyEvent.SKeyUp();
                KeyEvent.AKeyUp();
                KeyEvent.DKeyDown();

                argMouse.dKey = true;
                argMouse.aKey = false;
                argMouse.wKey = true;
                argMouse.sKey = false;

            }

            // backwards
            else if (argGazePos.Y > y2 && argGazePos.X > x3 && argGazePos.X < x4)
            {
                //////////////////if (argMouse.dKey)
                //////////////////{
                //////////////////    KeyEvent.DKeyUp();
                //////////////////    argMouse.dKey = false;
                //////////////////}

                //////////////////if (argMouse.aKey)
                //////////////////{
                //////////////////    KeyEvent.AKeyUp();
                //////////////////    argMouse.aKey = false;
                //////////////////}

                //////////////////if (argMouse.wKey)
                //////////////////{
                //////////////////    KeyEvent.WKeyUp();
                //////////////////    argMouse.wKey = false;
                //////////////////}

                //////////////////KeyEvent.SKeyDown();

                //////////////////argMouse.sKey = true;

                KeyEvent.AKeyUp();
                KeyEvent.DKeyUp();
                KeyEvent.WKeyUp();
                KeyEvent.SKeyDown();

                argMouse.dKey = false;
                argMouse.aKey = false;
                argMouse.wKey = false;
                argMouse.sKey = true;

            }

            // rotate left
            else if (argGazePos.Y > y2 && argGazePos.X < x3)
            {
                KeyEvent.AKeyDown();
                KeyEvent.DKeyUp();
                KeyEvent.WKeyUp();
                KeyEvent.SKeyUp();

                argMouse.dKey = false;
                argMouse.aKey = true;
                argMouse.wKey = false;
                argMouse.sKey = false;
            }

            //rotate right
            else if (argGazePos.Y > y2 && argGazePos.X > x4)
            {
                KeyEvent.AKeyUp();
                KeyEvent.DKeyDown();
                KeyEvent.WKeyUp();
                KeyEvent.SKeyUp();

                argMouse.dKey = true;
                argMouse.aKey = false;
                argMouse.wKey = false;
                argMouse.sKey = false;
            }

            // forward
            else// if (argGazePos.Y < y1)
            {
                ////////////////if (argMouse.dKey)
                ////////////////{
                ////////////////    KeyEvent.DKeyUp();
                ////////////////    argMouse.dKey = false;
                ////////////////}

                ////////////////if (argMouse.aKey)
                ////////////////{
                ////////////////    KeyEvent.AKeyUp();
                ////////////////    argMouse.aKey = false;
                ////////////////}

                ////////////////if (argMouse.sKey)
                ////////////////{
                ////////////////    KeyEvent.SKeyUp();
                ////////////////    argMouse.sKey = false;
                ////////////////}

                ////////////////KeyEvent.WKeyDown();
                ////////////////argMouse.wKey = true;

                KeyEvent.SKeyUp();
                KeyEvent.DKeyUp();
                KeyEvent.AKeyUp();
                KeyEvent.WKeyDown();

                argMouse.sKey = false;
                argMouse.dKey = false;
                argMouse.aKey = false;
                argMouse.wKey = true;
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
