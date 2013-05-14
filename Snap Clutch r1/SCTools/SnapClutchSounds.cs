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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Media;

namespace SnapClutch.SCTools
{
    public class SnapClutchSounds
    {
        public static void Click()
        {
            SoundPlayer myClick = new SoundPlayer(Properties.Resources.switch4);
            myClick.PlaySync();
        }

        public static void Click2()
        {
            SoundPlayer myClick = new SoundPlayer(Properties.Resources.CLICK14A);
            myClick.PlaySync();
        }

        public static void ModeChange()
        {
            SoundPlayer myClick = new SoundPlayer(Properties.Resources.zap1);
            myClick.PlaySync();
        }

        public static void OffScreenDwell()
        {
            SoundPlayer myClick = new SoundPlayer(Properties.Resources.CLICK16A);
            myClick.PlaySync();
        }
    }
}
