using System;
using System.Drawing;
using System.Collections;
using System.Runtime.InteropServices;

namespace Gesture_Interface
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

        public const int VK_0 = 0x30;
        public const int VK_1 = 0x31;
        public const int VK_2 = 0x32;
        public const int VK_3 = 0x33;
        public const int VK_4 = 0x34;
        public const int VK_5 = 0x35;
        public const int VK_6 = 0x36;
        public const int VK_7 = 0x37;
        public const int VK_8 = 0x38;
        public const int VK_9 = 0x39;


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

        public static void ExecuteKeyCode(string argKeyCode)
        {
            if(argKeyCode.Equals("VK_0"))
                KeyPress0();
            else if (argKeyCode.Equals("VK_1"))
                KeyPress1();
            else if (argKeyCode.Equals("VK_2"))
                KeyPress2();
            else if (argKeyCode.Equals("VK_3"))
                KeyPress3();
            else if (argKeyCode.Equals("VK_4"))
                KeyPress4();
            else if (argKeyCode.Equals("VK_5"))
                KeyPress5();
            else if (argKeyCode.Equals("VK_6"))
                KeyPress6();
            else if (argKeyCode.Equals("VK_7"))
                KeyPress7();
            else if (argKeyCode.Equals("VK_8"))
                KeyPress8();
            else if (argKeyCode.Equals("VK_9"))
                KeyPress9();
            else if (argKeyCode.Equals("VK_A")){
                AKeyDown(); AKeyUp();}
            else if (argKeyCode.Equals("VK_B")){
                BKeyDown(); BKeyUp();}
            else if (argKeyCode.Equals("VK_C")){
                CKeyDown(); CKeyUp();}
            else if (argKeyCode.Equals("VK_D")){
                DKeyDown(); DKeyUp();}
            else if (argKeyCode.Equals("VK_E")){
                EKeyDown(); EKeyUp();}

            else if (argKeyCode.Equals("VK_F"))
            {
                FKeyDown(); FKeyUp();
            }
            else if (argKeyCode.Equals("VK_G"))
            {
                GKeyDown(); GKeyUp();
            }
            else if (argKeyCode.Equals("VK_H"))
            {
                HKeyDown(); HKeyUp();
            }
            else if (argKeyCode.Equals("VK_I"))
            {
                IKeyDown(); IKeyUp();
            }
            else if (argKeyCode.Equals("VK_J"))
            {
                JKeyDown(); JKeyUp();
            }
            else if (argKeyCode.Equals("VK_K"))
            {
                KKeyDown(); KKeyUp();
            }
            else if (argKeyCode.Equals("VK_L"))
            {
                LKeyDown(); LKeyUp();
            }
            else if (argKeyCode.Equals("VK_M"))
            {
                MKeyDown(); MKeyUp();
            }
            else if (argKeyCode.Equals("VK_N"))
            {
                NKeyDown(); NKeyUp();
            }
            else if (argKeyCode.Equals("VK_O"))
            {
                OKeyDown(); OKeyUp();
            }
            else if (argKeyCode.Equals("VK_P"))
            {
                PKeyDown(); PKeyUp();
            }
            else if (argKeyCode.Equals("VK_Q"))
            {
                QKeyDown(); QKeyUp();
            }
            else if (argKeyCode.Equals("VK_R"))
            {
                RKeyDown(); RKeyUp();
            }
            else if (argKeyCode.Equals("VK_S"))
            {
                SKeyDown(); SKeyUp();
            }

            else if (argKeyCode.Equals("VK_T"))
            {
                TKeyDown(); TKeyUp();
            }
            else if (argKeyCode.Equals("VK_U"))
            {
                UKeyDown(); UKeyUp();
            }
            else if (argKeyCode.Equals("VK_V"))
            {
                VKeyDown(); VKeyUp();
            }
            else if (argKeyCode.Equals("VK_W"))
            {
                WKeyDown(); WKeyUp();
            }
            else if (argKeyCode.Equals("VK_X"))
            {
                XKeyDown(); XKeyUp();
            }
            else if (argKeyCode.Equals("VK_Y"))
            {
                YKeyDown(); YKeyUp();
            }
            else if (argKeyCode.Equals("VK_Z"))
            {
                ZKeyDown(); ZKeyUp();
            }
            else if (argKeyCode.Equals("VK_TAB"))
            {
                TabKeyDown(); TabKeyUp();
            }
        }

        public static void KeyPress0()
        {
            keybd_event(VK_0, 0x45, 0, (UIntPtr)0);
            keybd_event(VK_0, 0x45, KEYEVENTF_KEYUP, (UIntPtr)0);
        }

        public static void KeyPress1()
        {
            keybd_event(VK_1, 0x45, 0, (UIntPtr)0);
            keybd_event(VK_1, 0x45, KEYEVENTF_KEYUP, (UIntPtr)0);
        }

        public static void KeyPress2()
        {
            keybd_event(VK_2, 0x45, 0, (UIntPtr)0);
            keybd_event(VK_2, 0x45, KEYEVENTF_KEYUP, (UIntPtr)0);
        }

        public static void KeyPress3()
        {
            keybd_event(VK_3, 0x45, 0, (UIntPtr)0);
            keybd_event(VK_3, 0x45, KEYEVENTF_KEYUP, (UIntPtr)0);
        }

        public static void KeyPress4()
        {
            keybd_event(VK_4, 0x45, 0, (UIntPtr)0);
            keybd_event(VK_4, 0x45, KEYEVENTF_KEYUP, (UIntPtr)0);
        }

        public static void KeyPress5()
        {
            keybd_event(VK_5, 0x45, 0, (UIntPtr)0);
            keybd_event(VK_5, 0x45, KEYEVENTF_KEYUP, (UIntPtr)0);
        }

        public static void KeyPress6()
        {
            keybd_event(VK_6, 0x45, 0, (UIntPtr)0);
            keybd_event(VK_6, 0x45, KEYEVENTF_KEYUP, (UIntPtr)0);
        }

        public static void KeyPress7()
        {
            keybd_event(VK_7, 0x45, 0, (UIntPtr)0);
            keybd_event(VK_7, 0x45, KEYEVENTF_KEYUP, (UIntPtr)0);
        }

        public static void KeyPress8()
        {
            keybd_event(VK_8, 0x45, 0, (UIntPtr)0);
            keybd_event(VK_8, 0x45, KEYEVENTF_KEYUP, (UIntPtr)0);
        }

        public static void KeyPress9()
        {
            keybd_event(VK_9, 0x45, 0, (UIntPtr)0);
            keybd_event(VK_9, 0x45, KEYEVENTF_KEYUP, (UIntPtr)0);
        }

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
