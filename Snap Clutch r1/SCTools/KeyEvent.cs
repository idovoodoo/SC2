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
using System.Collections;
using System.Runtime.InteropServices;

namespace SnapClutch.SCTools
{
    public class KeyEvent
    {
        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

        // key event constants
        public const int KEYEVENTF_KEYUP = 0x02;
        public const int KEYEVENTF_EXTENDEDKEY = 0x01;
        public const int VK_TAB = 0x09;
        public const int VK_MENU = 0x12;
        public const int VK_LEFT = 0x25;
        public const int VK_RIGHT = 0x27;
        public const int VK_UP = 0x26;
        public const int VK_DOWN = 0x28;
        public const int VK_ESC = 0x1B;

        public const int VK_SPACE = 0x20;
        public const int VK_RETURN = 0x0D;
        public const int VK_BACK = 0x08;
        public const int VK_QMARK = 0xBF;
        public const int VK_LSHIFT = 0xA0;
        public const int VK_LMENU = 0xA4;

        public const int VK_NUMPAD0 = 0x60;
        public const int VK_NUMPAD1 = 0x61;
        public const int VK_NUMPAD2 = 0x62;
        public const int VK_NUMPAD3 = 0x63;
        public const int VK_NUMPAD4 = 0x64;
        public const int VK_NUMPAD5 = 0x65;
        public const int VK_NUMPAD6 = 0x66;
        public const int VK_NUMPAD7 = 0x67;
        public const int VK_NUMPAD8 = 0x68;
        public const int VK_NUMPAD9 = 0x69;

        public const int VK_A = 0x41;
        public const int VK_B = 0x42;
        public const int VK_C = 0x43;

        public const int VK_D = 0x44;

        public const int VK_E = 0x45;
        public const int VK_F = 0x46;
        public const int VK_G = 0x47;
        public const int VK_H = 0x48;
        public const int VK_I = 0x49;
        public const int VK_J = 0x4A;
        public const int VK_K = 0x4B;
        public const int VK_L = 0x4C;
        public const int VK_M = 0x4D;
        public const int VK_N = 0x4E;
        public const int VK_O = 0x4F;
        public const int VK_P = 0x50;
        public const int VK_Q = 0x51;
        public const int VK_R = 0x52;

        public const int VK_S = 0x53;

        public const int VK_T = 0x54;
        public const int VK_U = 0x55;
        public const int VK_V = 0x56;

        public const int VK_W = 0x57;

        public const int VK_X = 0x58;
        public const int VK_Y = 0x59;
        public const int VK_Z = 0x5A;



        private static string str;

        public static void SendString(string argString)
        {
            str = argString;
            IEnumerator StringEnum = str.GetEnumerator();

            while (StringEnum.MoveNext())
            {
                switch (StringEnum.Current.ToString())
                {

                    case "a":
                        AKeyDown();
                        AKeyUp();
                        break;
                    case "b":
                        BKeyDown();
                        BKeyUp();
                        break;
                    case "c":
                        CKeyDown();
                        CKeyUp();
                        break;
                    case "d":
                        DKeyDown();
                        DKeyUp();
                        break;
                    case "e":
                        EKeyDown();
                        EKeyUp();
                        break;
                    case "f":
                        FKeyDown();
                        FKeyUp();
                        break;
                    case "g":
                        GKeyDown();
                        GKeyUp();
                        break;
                    case "h":
                        HKeyDown();
                        HKeyUp();
                        break;
                    case "i":
                        IKeyDown();
                        IKeyUp();
                        break;
                    case "j":
                        JKeyDown();
                        JKeyUp();
                        break;
                    case "k":
                        KKeyDown();
                        KKeyUp();
                        break;
                    case "l":
                        LKeyDown();
                        LKeyUp();
                        break;
                    case "m":
                        MKeyDown();
                        MKeyUp();
                        break;
                    case "n":
                        NKeyDown();
                        NKeyUp();
                        break;
                    case "o":
                        OKeyDown();
                        OKeyUp();
                        break;
                    case "p":
                        PKeyDown();
                        PKeyUp();
                        break;
                    case "q":
                        QKeyDown();
                        QKeyUp();
                        break;
                    case "r":
                        RKeyDown();
                        RKeyUp();
                        break;
                    case "s":
                        SKeyDown();
                        SKeyUp();
                        break;
                    case "t":
                        TKeyDown();
                        TKeyUp();
                        break;
                    case "u":
                        UKeyDown();
                        UKeyUp();
                        break;
                    case "v":
                        VKeyDown();
                        VKeyUp();
                        break;
                    case "w":
                        WKeyDown();
                        WKeyUp();
                        break;
                    case "x":
                        XKeyDown();
                        XKeyUp();
                        break;
                    case "y":
                        YKeyDown();
                        YKeyUp();
                        break;
                    case "z":
                        ZKeyDown();
                        ZKeyUp();
                        break;
                    case " ":
                        SpaceKeyDown();
                        SpaceKeyUp();
                        break;
                    case "?":
                        LeftShiftKeyDown();
                        QuestionKeyDown();
                        QuestionKeyUp();
                        LeftShiftKeyUp();
                        break;
                }
            }
        }

        #region other keys

        public static void AltTab()
        {
            keybd_event(VK_LMENU, 0x45, 0, (UIntPtr)0);
            keybd_event(VK_TAB, 0x45, 0, (UIntPtr)0);

            keybd_event(VK_TAB, 0x45, KEYEVENTF_KEYUP, (UIntPtr)0);
            keybd_event(VK_LMENU, 0x45, KEYEVENTF_KEYUP, (UIntPtr)0);
        }

        public static void NumKey0Up()
        {
            keybd_event(VK_NUMPAD0, 0x45, KEYEVENTF_KEYUP, (UIntPtr)0);
        }

        public static void NumKey0Down()
        {
            keybd_event(VK_NUMPAD0, 0x45, 0, (UIntPtr)0);

        }

        public static void NumKey1Up()
        {
            keybd_event(VK_NUMPAD1, 0x45, KEYEVENTF_KEYUP, (UIntPtr)0);
        }
        public static void NumKey1Down()
        {
            keybd_event(VK_NUMPAD1, 0x45, 0, (UIntPtr)0);

        }

        public static void NumKey2Up()
        {
            keybd_event(VK_NUMPAD2, 0x45, KEYEVENTF_KEYUP, (UIntPtr)0);
        }
        public static void NumKey2Down()
        {
            keybd_event(VK_NUMPAD2, 0x45, 0, (UIntPtr)0);

        }

        public static void NumKey3Up()
        {
            keybd_event(VK_NUMPAD3, 0x45, KEYEVENTF_KEYUP, (UIntPtr)0);
        }
        public static void NumKey3Down()
        {
            keybd_event(VK_NUMPAD3, 0x45, 0, (UIntPtr)0);

        }

        public static void NumKey4Up()
        {
            keybd_event(VK_NUMPAD4, 0x45, KEYEVENTF_KEYUP, (UIntPtr)0);
        }
        public static void NumKey4Down()
        {
            keybd_event(VK_NUMPAD4, 0x45, 0, (UIntPtr)0);

        }

        public static void NumKey5Up()
        {
            keybd_event(VK_NUMPAD5, 0x45, KEYEVENTF_KEYUP, (UIntPtr)0);
        }
        public static void NumKey5Down()
        {
            keybd_event(VK_NUMPAD5, 0x45, 0, (UIntPtr)0);

        }

        public static void NumKey6Up()
        {
            keybd_event(VK_NUMPAD6, 0x45, KEYEVENTF_KEYUP, (UIntPtr)0);
        }
        public static void NumKey6Down()
        {
            keybd_event(VK_NUMPAD6, 0x45, 0, (UIntPtr)0);

        }

        public static void NumKey7Up()
        {
            keybd_event(VK_NUMPAD7, 0x45, KEYEVENTF_KEYUP, (UIntPtr)0);
        }
        public static void NumKey7Down()
        {
            keybd_event(VK_NUMPAD7, 0x45, 0, (UIntPtr)0);

        }

        public static void NumKey8Up()
        {
            keybd_event(VK_NUMPAD8, 0x45, KEYEVENTF_KEYUP, (UIntPtr)0);
        }
        public static void NumKey8Down()
        {
            keybd_event(VK_NUMPAD8, 0x45, 0, (UIntPtr)0);

        }

        public static void NumKey9Up()
        {
            keybd_event(VK_NUMPAD9, 0x45, KEYEVENTF_KEYUP, (UIntPtr)0);
        }

        public static void NumKey9Down()
        {
            keybd_event(VK_NUMPAD9, 0x45, 0, (UIntPtr)0);

        }



















        public static void LeftShiftKeyDown()
        {
            keybd_event(VK_LSHIFT, 0x45, 0, (UIntPtr)0);

        }

        public static void LeftShiftKeyUp()
        {
            keybd_event(VK_LSHIFT, 0x45, KEYEVENTF_KEYUP, (UIntPtr)0);

        }

        public static void CursorDownKeyDown()
        {
            keybd_event(VK_DOWN, 0x45, 0, (UIntPtr)0);

        }

        public static void CursorDownKeyUp()
        {
            keybd_event(VK_DOWN, 0x45, KEYEVENTF_KEYUP, (UIntPtr)0);

        }

        public static void CursorUpKeyDown()
        {
            keybd_event(VK_UP, 0x45, 0, (UIntPtr)0);

        }

        public static void CursorUpKeyUp()
        {
            keybd_event(VK_UP, 0x45, KEYEVENTF_KEYUP, (UIntPtr)0);

        }

        public static void CursorLeftKeyDown()
        {
            keybd_event(VK_LEFT, 0x45, 0, (UIntPtr)0);

        }

        public static void CursorLeftKeyUp()
        {
            keybd_event(VK_LEFT, 0x45, KEYEVENTF_KEYUP, (UIntPtr)0);

        }

        public static void CursorRightKeyDown()
        {
            keybd_event(VK_RIGHT, 0x45, 0, (UIntPtr)0);

        }

        public static void CursorRightKeyUp()
        {
            keybd_event(VK_RIGHT, 0x45, KEYEVENTF_KEYUP, (UIntPtr)0);

        }


        public static void SpaceKeyDown()
        {
            keybd_event(VK_SPACE, 0x45, 0, (UIntPtr)0);

        }

        public static void SpaceKeyUp()
        {
            keybd_event(VK_SPACE, 0x45, KEYEVENTF_KEYUP, (UIntPtr)0);

        }

        public static void QuestionKeyDown()
        {
            keybd_event(VK_QMARK, 0x45, 0, (UIntPtr)0);

        }

        public static void QuestionKeyUp()
        {
            keybd_event(VK_QMARK, 0x45, KEYEVENTF_KEYUP, (UIntPtr)0);

        }

        public static void ReturnKeyDown()
        {
            keybd_event(VK_RETURN, 0x45, 0, (UIntPtr)0);

        }

        public static void ReturnKeyUp()
        {
            keybd_event(VK_RETURN, 0x45, KEYEVENTF_KEYUP, (UIntPtr)0);

        }

        public static void BackspaceKeyDown()
        {
            keybd_event(VK_BACK, 0x45, 0, (UIntPtr)0);

        }

        public static void BackspaceKeyUp()
        {
            keybd_event(VK_BACK, 0x45, KEYEVENTF_KEYUP, (UIntPtr)0);

        }

        public static void TabKeyDown()
        {
            keybd_event(VK_TAB, 0x45, 0, (UIntPtr)0);

        }

        public static void TabKeyUp()
        {
            keybd_event(VK_TAB, 0x45, KEYEVENTF_KEYUP, (UIntPtr)0);
        }
        #endregion

        public static void WKeyDown()
        {
            keybd_event(VK_W, 0x45, 0, (UIntPtr)0);

        }

        public static void WKeyUp()
        {
            keybd_event(VK_W, 0x45, KEYEVENTF_KEYUP, (UIntPtr)0);
        }

        public static void AKeyDown()
        {
            keybd_event(VK_A, 0x45, 0, (UIntPtr)0);

        }

        public static void AKeyUp()
        {
            keybd_event(VK_A, 0x45, KEYEVENTF_KEYUP, (UIntPtr)0);
        }

        public static void BKeyDown()
        {
            keybd_event(VK_B, 0x45, 0, (UIntPtr)0);

        }

        public static void BKeyUp()
        {
            keybd_event(VK_B, 0x45, KEYEVENTF_KEYUP, (UIntPtr)0);
        }

        public static void CKeyDown()
        {
            keybd_event(VK_C, 0x45, 0, (UIntPtr)0);

        }

        public static void CKeyUp()
        {
            keybd_event(VK_C, 0x45, KEYEVENTF_KEYUP, (UIntPtr)0);
        }

        public static void DKeyDown()
        {
            keybd_event(VK_D, 0x45, 0, (UIntPtr)0);

        }

        public static void DKeyUp()
        {
            keybd_event(VK_D, 0x45, KEYEVENTF_KEYUP, (UIntPtr)0);
        }




        public static void EKeyDown()
        {
            keybd_event(VK_E, 0x45, 0, (UIntPtr)0);

        }

        public static void EKeyUp()
        {
            keybd_event(VK_E, 0x45, KEYEVENTF_KEYUP, (UIntPtr)0);
        }

        public static void FKeyDown()
        {
            keybd_event(VK_F, 0x45, 0, (UIntPtr)0);

        }

        public static void FKeyUp()
        {
            keybd_event(VK_F, 0x45, KEYEVENTF_KEYUP, (UIntPtr)0);
        }





        public static void GKeyDown()
        {
            keybd_event(VK_G, 0x45, 0, (UIntPtr)0);

        }

        public static void GKeyUp()
        {
            keybd_event(VK_G, 0x45, KEYEVENTF_KEYUP, (UIntPtr)0);
        }



        public static void HKeyDown()
        {
            keybd_event(VK_H, 0x45, 0, (UIntPtr)0);

        }

        public static void HKeyUp()
        {
            keybd_event(VK_H, 0x45, KEYEVENTF_KEYUP, (UIntPtr)0);
        }


        public static void IKeyDown()
        {
            keybd_event(VK_I, 0x45, 0, (UIntPtr)0);

        }

        public static void IKeyUp()
        {
            keybd_event(VK_I, 0x45, KEYEVENTF_KEYUP, (UIntPtr)0);
        }



        public static void JKeyDown()
        {
            keybd_event(VK_J, 0x45, 0, (UIntPtr)0);

        }

        public static void JKeyUp()
        {
            keybd_event(VK_J, 0x45, KEYEVENTF_KEYUP, (UIntPtr)0);
        }



        public static void KKeyDown()
        {
            keybd_event(VK_K, 0x45, 0, (UIntPtr)0);

        }

        public static void KKeyUp()
        {
            keybd_event(VK_K, 0x45, KEYEVENTF_KEYUP, (UIntPtr)0);
        }



        public static void LKeyDown()
        {
            keybd_event(VK_L, 0x45, 0, (UIntPtr)0);

        }

        public static void LKeyUp()
        {
            keybd_event(VK_L, 0x45, KEYEVENTF_KEYUP, (UIntPtr)0);
        }




        public static void MKeyDown()
        {
            keybd_event(VK_M, 0x45, 0, (UIntPtr)0);

        }

        public static void MKeyUp()
        {
            keybd_event(VK_M, 0x45, KEYEVENTF_KEYUP, (UIntPtr)0);
        }




        public static void NKeyDown()
        {
            keybd_event(VK_N, 0x45, 0, (UIntPtr)0);

        }

        public static void NKeyUp()
        {
            keybd_event(VK_N, 0x45, KEYEVENTF_KEYUP, (UIntPtr)0);
        }



        public static void OKeyDown()
        {
            keybd_event(VK_O, 0x45, 0, (UIntPtr)0);

        }

        public static void OKeyUp()
        {
            keybd_event(VK_O, 0x45, KEYEVENTF_KEYUP, (UIntPtr)0);
        }




        public static void PKeyDown()
        {
            keybd_event(VK_P, 0x45, 0, (UIntPtr)0);

        }

        public static void PKeyUp()
        {
            keybd_event(VK_P, 0x45, KEYEVENTF_KEYUP, (UIntPtr)0);
        }



        public static void QKeyDown()
        {
            keybd_event(VK_Q, 0x45, 0, (UIntPtr)0);

        }

        public static void QKeyUp()
        {
            keybd_event(VK_Q, 0x45, KEYEVENTF_KEYUP, (UIntPtr)0);
        }



        public static void RKeyDown()
        {
            keybd_event(VK_R, 0x45, 0, (UIntPtr)0);

        }

        public static void RKeyUp()
        {
            keybd_event(VK_R, 0x45, KEYEVENTF_KEYUP, (UIntPtr)0);
        }










        public static void SKeyDown()
        {
            keybd_event(VK_S, 0x45, 0, (UIntPtr)0);

        }

        public static void SKeyUp()
        {
            keybd_event(VK_S, 0x45, KEYEVENTF_KEYUP, (UIntPtr)0);
        }







        public static void TKeyDown()
        {
            keybd_event(VK_T, 0x45, 0, (UIntPtr)0);

        }

        public static void TKeyUp()
        {
            keybd_event(VK_T, 0x45, KEYEVENTF_KEYUP, (UIntPtr)0);
        }


        public static void UKeyDown()
        {
            keybd_event(VK_U, 0x45, 0, (UIntPtr)0);

        }

        public static void UKeyUp()
        {
            keybd_event(VK_U, 0x45, KEYEVENTF_KEYUP, (UIntPtr)0);
        }



        public static void VKeyDown()
        {
            keybd_event(VK_V, 0x45, 0, (UIntPtr)0);

        }

        public static void VKeyUp()
        {
            keybd_event(VK_V, 0x45, KEYEVENTF_KEYUP, (UIntPtr)0);
        }



        public static void XKeyDown()
        {
            keybd_event(VK_X, 0x45, 0, (UIntPtr)0);

        }

        public static void XKeyUp()
        {
            keybd_event(VK_X, 0x45, KEYEVENTF_KEYUP, (UIntPtr)0);
        }


        public static void YKeyDown()
        {
            keybd_event(VK_Y, 0x45, 0, (UIntPtr)0);

        }

        public static void YKeyUp()
        {
            keybd_event(VK_Y, 0x45, KEYEVENTF_KEYUP, (UIntPtr)0);
        }


        public static void ZKeyDown()
        {
            keybd_event(VK_Z, 0x45, 0, (UIntPtr)0);

        }

        public static void ZKeyUp()
        {
            keybd_event(VK_Z, 0x45, KEYEVENTF_KEYUP, (UIntPtr)0);
        }



        public static void EscKeyDown()
        {
            keybd_event(VK_ESC, 0x45, 0, (UIntPtr)0);

        }

        public static void EscKeyUp()
        {
            keybd_event(VK_ESC, 0x45, KEYEVENTF_KEYUP, (UIntPtr)0);
        }

        public static void KeysUp()
        {

            WKeyUp();
            AKeyUp();
            DKeyUp();
            EscKeyUp();
            SKeyUp();
            CursorLeftKeyUp();
            CursorRightKeyUp();
            CursorUpKeyUp();
            CursorDownKeyUp();
        }
    }
}
