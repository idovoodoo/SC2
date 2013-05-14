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
    public class ModeLeftDrag : Mode
    {
        private Point myDragPos = new Point();
        //private MOUSESTATE myMouse;

        // constructor
        public ModeLeftDrag()
        {
            modeName = "Left Drag";
        }

        // execute the mode
        public override MOUSESTATE Execute(Point argGazePos, MOUSESTATE argMouse, GazeOverlay argGazeOverlay)
        {
            myMouse = argMouse;

            if (argMouse.rightButton)
            {
                MouseEvent.RightUp();
                argMouse.rightButton = false;
            }
            if (argMouse.leftButton)
            {
            }
            else
            {
                CursorFactory.Create(Application.StartupPath + CursorSnapClutch.DragInactive());
                //CursorFactory.CreateDragInActiveCursor();
            }

            Cursor.Position = argGazePos;

            return argMouse;
        }

        public override MOUSESTATE SomeEvent(Point argGazePos, bool argSwitch, bool argDown)
        {
            if (!myMouse.leftButton)
            {
                SnapClutchSounds.Click2();
                CursorFactory.Create(Application.StartupPath + CursorSnapClutch.DragActive());
                //CursorFactory.CreateDragActiveCursor();
                myDragPos = Cursor.Position;
                MouseEvent.LeftClickDownPoint(argGazePos.X, argGazePos.Y);
                myMouse.leftButton = true;
            }
            else
            {
                SnapClutchSounds.Click2();
                CursorFactory.Create(Application.StartupPath + CursorSnapClutch.DragInactive());
                //CursorFactory.CreateDragInActiveCursor();
                MouseEvent.LeftClickUpPoint(argGazePos.X, argGazePos.Y);
                myMouse.leftButton = false;
            }

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
