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

namespace SnapClutch.Modes
{
    public class ModeCollection
    {
        Mode modeTop;
        Mode modeBottom;
        Mode modeLeft;
        Mode modeRight;
        Mode modeTop2;
        Mode modeBottom2;
        Mode modeLeft2;
        Mode modeRight2;

        int activeMode;
        //private enum Glance { Off, Top, Bottom, Right, Left };

        // constructor
        public ModeCollection()
        {
            activeMode = 0;
        }

        public void SetActiveMode(int argGlance)
        {
            activeMode = argGlance;
        }

        public void SetModeTop(Mode argMode)
        {
            modeTop = argMode;
        }

        public void SetModeRight(Mode argMode)
        {
            modeRight = argMode;
        }

        public void SetModeBottom(Mode argMode)
        {
            modeBottom = argMode;
        }

        public void SetModeLeft(Mode argMode)
        {
            modeLeft = argMode;
        }


        public void SetModeTop2(Mode argMode)
        {
            modeTop2 = argMode;
        }

        public void SetModeRight2(Mode argMode)
        {
            modeRight2 = argMode;
        }

        public void SetModeBottom2(Mode argMode)
        {
            modeBottom2 = argMode;
        }

        public void SetModeLeft2(Mode argMode)
        {
            modeLeft2 = argMode;
        }


        public int GetActiveMode()
        {
            return activeMode;
        }

        public Mode GetModeTop()
        {
            return modeTop;
        }

        public Mode GetModeRight()
        {
            return modeRight;
        }

        public Mode GetModeBottom()
        {
            return modeBottom;
        }

        public Mode GetModeLeft()
        {
            return modeLeft;
        }

        public Mode GetModeTop2()
        {
            return modeTop2;
        }

        public Mode GetModeRight2()
        {
            return modeRight2;
        }

        public Mode GetModeBottom2()
        {
            return modeBottom2;
        }

        public Mode GetModeLeft2()
        {
            return modeLeft2;
        }
    }
}

