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
using SnapClutch.SCTools;

namespace SnapClutch.Modes
{
    public class Mode
    {
        public string modeName = "default";
        public MOUSESTATE myMouse;

        public Mode()
        {
        }

        public virtual MOUSESTATE Execute(Point argGazePos, MOUSESTATE argMouse, GazeOverlay argGazeOverlay)
        {
            return argMouse;
        }

        public virtual MOUSESTATE Execute(Point argGazePos, MOUSESTATE argMouse)
        {
            return argMouse;
        }

        public virtual string GetModeName()
        {
            return modeName;
        }

        public virtual void Update(Point argGazePos)
        {
        }

        public virtual MOUSESTATE SomeEvent(Point argGazePos, bool argSwitch, bool argDown)
        {
            return myMouse;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="argGazePos"></param>
        /// <param name="argEye">0 = left, 1 = right</param>
        /// <returns></returns>
        public virtual MOUSESTATE BlinkEvent(Point argGazePos, int argEye)
        {
            return myMouse;
        }

    }
}
