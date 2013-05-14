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
using System.IO;
using System.Runtime.InteropServices;



namespace SnapClutch.Modes.EyeGuitar
{
    // dwell click on left mouse button
    public class ModeStrings : Mode
    {
        //private MOUSESTATE myMouse;
        public static Rectangle screenSize = Screen.PrimaryScreen.Bounds;
        private int x1 = Convert.ToInt32(screenSize.Width);// * 0.2);
        private int x2 = Convert.ToInt32(screenSize.Width * 0.8);
        private int x3 = Convert.ToInt32(screenSize.Width * 0.4);
        private int x4 = Convert.ToInt32(screenSize.Width * 0.6);
        private int y1 = Convert.ToInt32(screenSize.Height * 0.3);
        private int y2 = Convert.ToInt32(screenSize.Height * 0.7);
        private int string1Select = Convert.ToInt32(screenSize.Height * 0.7);
        //StringButton stringButton1 = new StringButton(200, 800, 400, 800); old
        //StringButton stringButton2 = new StringButton(410, 610, 200, 400); old
        //StringButton stringButton3 = new StringButton(620, 820, 200, 400); old
        StringButton stringButton1 = new StringButton(534, 700, 394, 917); //376, 881
        StringButton stringButton2 = new StringButton(683, 773, 394, 917);
        StringButton stringButton3 = new StringButton(819, 936, 394, 917); //443, 904
        int currentY = 0;
        int previousY = 0;
        int currentX = 0;
        public Bitmap myBitmap; // to get colour
        int previousX = 0;
        guitarOSD noteOnscreenTop;
        guitarOSD noteOnscreenBottom;
        guitarOSD noteTest;

        int verticalCounter = 0;
       
        int strumcounter = 0;
        Boolean countdown = false;
        Boolean firstStrum = false;
        Boolean createDisplay = true;
        int qa = 0;
        int noNoteCounter = 0;
        int stringSelected = 1;
        
        int eyeCalibrator = 0;
        int eyecalib2 = 0;
        int eyeYcalibrator = 0;
        Boolean eyeCalibratorOn = true;
        int leftOrRight = 0;
        double trig = 0;
        double trigInterim = 0;
        
       // private StringPoint stringPointA = new StringPoint(439, 890); // these should be worked out from window position not offsetin case screen moves - actual
        //private StringPoint stringPointA = new StringPoint(419, 930); // experimental old
        private StringPoint stringPointA = new StringPoint(480, 887); // steve
        private StringPoint stringPointB = new StringPoint(644, 887); // steve
        private StringPoint stringPointC = new StringPoint(836, 887); // steve
        private StringPoint stringPointD = new StringPoint(1013, 887); // steve
      //  private StringPoint stringPointB = new StringPoint(623, 890); // actual
        //private StringPoint stringPointB = new StringPoint(610, 930); // experimental old
      //  private StringPoint stringPointC = new StringPoint(801, 890); // actual
        //private StringPoint stringPointC = new StringPoint(801, 930); // experimental old
        //private StringPoint stringPointD = new StringPoint(991, 930); // experimental old
        private StringPoint stringPointE = new StringPoint(1184, 930); // experimental old
        //private StringPoint stringPointA = new StringPoint(470, 834); // these should be worked out from window position not offsetin case screen moves
        //private StringPoint stringPointB = new StringPoint(635, 834);
        //private StringPoint stringPointC = new StringPoint(801, 834);




        // constructor
        public ModeStrings()
        {
            modeName = "Strings on Speed";              
        }

        [DllImport("user32.dll")]
        static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("user32.dll")]
        static extern Int32 ReleaseDC(IntPtr hwnd, IntPtr hdc);

        [DllImport("gdi32.dll")]
        static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);

        // Print out the RGB value of the pixel which is under the mouse cursor.
        // NB: BLUE and RED components will be swapped because GetPixel returns ABGR
       

        // execute the mode
        public override MOUSESTATE Execute(Point argGazePos, MOUSESTATE argMouse, GazeOverlay argGazeOverlay)
        {

            
            
            if (createDisplay == true)
            {
                noteOnscreenTop = new guitarOSD();
                noteOnscreenTop.Show();
                noteOnscreenTop.Left = 400;
                noteOnscreenTop.Top = 200;
                noteOnscreenTop.BackColor = Color.Black;

                noteOnscreenBottom = new guitarOSD();
                noteOnscreenBottom.Show();
                noteOnscreenTop.BackColor = Color.Black;
                noteOnscreenBottom.Width = 184;
                noteOnscreenBottom.Height = 20;
                noteOnscreenBottom.Top = 1106;
                createDisplay = false;

                noteTest = new guitarOSD();
                noteTest.Width = 160;
                noteTest.Height = 20;
                noteTest.BackColor = Color.White;
                noteTest.Top = 200;
                noteTest.Show();
            }

         

            IntPtr hdc = GetDC(IntPtr.Zero);
            uint pixel = GetPixel(hdc, Cursor.Position.X, Cursor.Position.Y);
            ReleaseDC(IntPtr.Zero, hdc);
            Color color = Color.FromArgb((int)pixel);
            //Console.WriteLine("" + stringPointC.Xcoord1 + " " + stringPointC.Ycoord1 + " " + color.G);
            //Console.WriteLine(" " + argGazePos.X + " " + argGazePos.Y + " " + color.G);
            
            paddleMethod(argGazePos);
            //followNote(argGazePos);
            //CursorFactory.Create(Application.StartupPath + CursorSnapClutch.Locomotion());
            //CursorFactorya.CreateLocomotionCursor();
        //    Cursor.Position = argGazePos;

            argMouse.gazeY2 = argMouse.gazeY1; 
            argMouse.gazeY1 = argGazePos.Y;
            return argMouse;
        }

        public void HideDisplay(bool argHide)
        {
            if (argHide)
            {
                noteOnscreenTop.Hide();
                noteOnscreenBottom.Hide();
                noteTest.Hide();
            }
            else
            {
                noteOnscreenTop.Show();
                noteOnscreenBottom.Show();
                noteTest.Show();
            }

        }

        public override MOUSESTATE SomeEvent(Point argGazePos, bool argSwitch, bool argDown)
        {
            //////string 1
            ////if (argGazePos.Y > stringButton1.YStart && argGazePos.Y < stringButton1.YEnd && argGazePos.X > stringButton1.XStart && argGazePos.X < stringButton1.XEnd)
            ////{
            ////    //KeyEvent.AKeyDown();
            ////    //KeyEvent.DKeyUp();
            ////    //KeyEvent.AKeyDown();
            ////    //KeyEvent.BKeyUp();
            ////    //KeyEvent.CKeyUp();
            ////    myMouse.stringSelected = 1;
            ////}

            //////string 2
            ////if (argGazePos.Y > stringButton2.YStart && argGazePos.Y < stringButton2.YEnd && argGazePos.X > stringButton2.XStart && argGazePos.X < stringButton2.XEnd)
            ////{
            ////    //KeyEvent.AKeyDown();
            ////    //KeyEvent.DKeyUp();
            ////    myMouse.stringSelected = 2;
            ////    //KeyEvent.BKeyDown();
            ////    //KeyEvent.AKeyUp();
            ////    //KeyEvent.CKeyUp();
            ////}

            ////if (argGazePos.Y > stringButton3.YStart && argGazePos.Y < stringButton3.YEnd && argGazePos.X > stringButton3.XStart && argGazePos.X < stringButton3.XEnd)
            ////{
            ////    //KeyEvent.AKeyDown();
            ////    //KeyEvent.DKeyUp();
            ////    myMouse.stringSelected = 3;
            ////    //KeyEvent.CKeyDown();
            ////    //KeyEvent.BKeyUp();
            ////    //KeyEvent.AKeyUp();
            ////}

            return myMouse;
        }

        public void PlayString() // original strum attempt. not very good.
        {
            {
                {
                    {
                        qa++;
                        //  for (int qa = 0; qa < 50; qa++)
                        if (qa == 0 || qa > 35)
                        {
                            //      if (strumcounter < 4)
                            //    {
                            KeyEvent.SpaceKeyUp(); // end the space bar press. after X amount of time
                            //     KeyEvent.SpaceKeyDown();
                            strumcounter++;
                            //     Console.WriteLine(""+strumcounter);
                            //      }
                            qa = 1;
                        }
                        KeyEvent.SpaceKeyDown();

                        if (strumcounter >= 4)
                        {
                            strumcounter = 0;
                            KeyEvent.AKeyUp();
                            KeyEvent.BKeyUp();
                            countdown = false;
                        }


                    }

                    //          countdown = false;
                    //  KeyEvent.AKeyUp();
                }
                // strumcounter++;
            }
        }

        public void DetectButtons() // legacy of the original design investigation
        {
            //IntPtr hdc = GetDC(IntPtr.Zero);
            //uint pixel = GetPixel(hdc, Cursor.Position.X, Cursor.Position.Y);
            //ReleaseDC(IntPtr.Zero, hdc);
            //Color color = Color.FromArgb((int)pixel);
            //Console.WriteLine("" + stringPointB.Xcoord1 + " " + stringPointB.Ycoord1 + " " + color.G);
            //Console.WriteLine(" " + Cursor.Position.X + " " + Cursor.Position.Y + " " + color.G);


        }

        public void PlayString(StringPoint stringPoint, Point argGazePos)
        {
           // Console.WriteLine("aasasa");
            // grab pixel color information   
            IntPtr hdc = GetDC(IntPtr.Zero);
            uint pixel = GetPixel(hdc, stringPoint.Xcoord1, stringPoint.Ycoord1);
            ReleaseDC(IntPtr.Zero, hdc);
            Color color = Color.FromArgb((int)pixel);
            //Console.WriteLine("color is " + color.G + " string is " + stringSelected);
            //KeyEvent.SpaceKeyUp();
            //if (color.G > 45 && color.G < 55)
            if (color.G == 50 || color.G == 219 || color.G == 212)
            {
                //Console.WriteLine("its clear" + color.G);
                KeyEvent.SpaceKeyUp();

                noNoteCounter++;
                if (noNoteCounter >= 100)
                {
                    //       Console.WriteLine("no note for a while, turn off the string down event");
                    KeyEvent.AKeyUp();
                    KeyEvent.BKeyUp();
                    KeyEvent.CKeyUp();
                    KeyEvent.DKeyUp();
                    KeyEvent.EKeyUp();
                    countdown = false;
                    noNoteCounter = 0;
                    noteOnscreenTop.BackColor = Color.Black;
                }
            }
            else
            {
                if (stringSelected == 1) // if want to revert to old method, remove all this
                {
                    //Console.WriteLine("1");
                    KeyEvent.BKeyUp();
                    KeyEvent.CKeyUp();
                    KeyEvent.DKeyUp();
                    KeyEvent.EKeyUp();
                    KeyEvent.AKeyDown();
                }
                if (stringSelected == 2)
                {
                    //Console.WriteLine("2");
                    KeyEvent.AKeyUp();
                    KeyEvent.CKeyUp();
                    KeyEvent.DKeyUp();
                    KeyEvent.EKeyUp();
                    KeyEvent.BKeyDown();
                }
                if (stringSelected == 3)
                {
                    //Console.WriteLine("3");
                    KeyEvent.BKeyUp();
                    KeyEvent.AKeyUp();
                    KeyEvent.DKeyUp();
                    KeyEvent.EKeyUp();
                    KeyEvent.CKeyDown();
                }
                if (stringSelected == 4)
                {
                    Console.WriteLine("4");
                    KeyEvent.BKeyUp();
                    KeyEvent.AKeyUp();
                    KeyEvent.CKeyUp();
                    KeyEvent.EKeyUp();
                    KeyEvent.DKeyDown();
                    
                }
                if (stringSelected == 5)
                {
                    //Console.WriteLine("5");
                    KeyEvent.BKeyUp();
                    KeyEvent.AKeyUp();
                    KeyEvent.CKeyUp();
                    KeyEvent.DKeyUp();
                    KeyEvent.EKeyDown();
                }
                noNoteCounter = 0;
                KeyEvent.SpaceKeyDown();
                if (firstStrum == true)
                {
                  //  SnapClutchSounds.Click2(); // strum the first time for each note.
                    firstStrum = false;
                }
             
            }

        }

        public class Win32APICall
        {
            [DllImport("gdi32.dll", EntryPoint = "DeleteDC")]
            public static extern IntPtr DeleteDC(IntPtr hdc);

            [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
            public static extern IntPtr DeleteObject(IntPtr hObject);

            [DllImport("gdi32.dll", EntryPoint = "BitBlt")]
            public static extern bool BitBlt(IntPtr hdcDest, int nXDest,
                int nYDest, int nWidth, int nHeight, IntPtr hdcSrc,
                int nXSrc, int nYSrc, int dwRop);

            [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleBitmap")]
            public static extern IntPtr CreateCompatibleBitmap(IntPtr hdc,
                int nWidth, int nHeight);

            [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleDC")]
            public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

            [DllImport("gdi32.dll", EntryPoint = "SelectObject")]
            public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobjBmp);

            [DllImport("user32.dll", EntryPoint = "GetDesktopWindow")]
            public static extern IntPtr GetDesktopWindow();

            [DllImport("user32.dll", EntryPoint = "GetDC")]
            public static extern IntPtr GetDC(IntPtr hWnd);

            [DllImport("user32.dll", EntryPoint = "GetSystemMetrics")]
            public static extern int GetSystemMetrics(int nIndex);

            [DllImport("user32.dll", EntryPoint = "ReleaseDC")]
            public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);

            public static Bitmap GetDesktop()
            {
                int screenX;
                int screenY;
                IntPtr hBmp;
                IntPtr hdcScreen = GetDC(GetDesktopWindow());
                IntPtr hdcCompatible = CreateCompatibleDC(hdcScreen);

                screenX = GetSystemMetrics(0);
                screenY = GetSystemMetrics(1);
                hBmp = CreateCompatibleBitmap(hdcScreen, screenX, screenY);

                if (hBmp != IntPtr.Zero)
                {
                    IntPtr hOldBmp = (IntPtr)SelectObject(hdcCompatible, hBmp);
                    BitBlt(hdcCompatible, 0, 0, screenX, screenY, hdcScreen, 0, 0, 13369376);

                    SelectObject(hdcCompatible, hOldBmp);
                    DeleteDC(hdcCompatible);
                    ReleaseDC(GetDesktopWindow(), hdcScreen);

                    Bitmap bmp = System.Drawing.Image.FromHbitmap(hBmp);

                    DeleteObject(hBmp);
                    GC.Collect();

                    return bmp;
                }
            //   Thread.Sleep(0);
                return null;
            }
        }

        // return the name of the mode
        public override string GetModeName()
        {
            return modeName;
        }

        public void followNote(Point argGazePos)
        {
            previousX = currentX;
            currentX = argGazePos.X;
            previousY = currentY;
            currentY = argGazePos.Y;
            Console.WriteLine(currentX);
            //// method 1 below:
            //   Console.WriteLine("" + (argGazePos.Y));
            if (currentY - previousY >= 0)
            {
                if (currentY - previousY < 15)
                {
                    if (currentX - previousX < 5)
                    {
                        if (previousX - currentX < 5)
                        {
                            //     Console.WriteLine("" + (currentX-previousX));

                            verticalCounter++;

                            //    Console.WriteLine("" + (verticalCounter));


                            if (verticalCounter > 15) // was 100 but need to find optimum balancce between tolerance and accuracy... notes: 40 or 60
                            {
                                if (argGazePos.Y > 525) // 510 was 530
                                {
                                    countdown = true;
                                    strumcounter = 0;
                                    verticalCounter = 0;
                                    //  Console.WriteLine("" + currentX);
                                    if (currentX >= 274 && currentX <= 638) // was 659 before... // change to use stringButton object for each collision // was 683
                                    {
                                        //KeyEvent.BKeyUp(); // if want to revert to old method, ADD THESE BACK IN
                                        //KeyEvent.CKeyUp();
                                        //  KeyEvent.AKeyDown();
                                        firstStrum = true; // to allow the click sound to be played the first time that the note is selected
                                        stringSelected = 1;
                                        noNoteCounter = 0; // reset as a new note has been selected, so any existing no note value should be wiped.
                                        noteOnscreenTop.BackColor = Color.Green;
                                        noteOnscreenBottom.Left = 240;
                                        noteOnscreenBottom.BackColor = Color.Green;
                                    }
                                    else if (currentX >= 639 && currentX <= 764)//1083 was 757
                                    {
                                        //KeyEvent.AKeyUp();
                                        //KeyEvent.CKeyUp();
                                        //   KeyEvent.BKeyDown();
                                        firstStrum = true; // to allow the click sound to be played the first time that the note is selected
                                        stringSelected = 2;
                                        noNoteCounter = 0; // reset as a new note has been selected, so any existing no note value should be wiped.
                                        noteOnscreenTop.BackColor = Color.Red;
                                        noteOnscreenBottom.Left = 480;
                                        noteOnscreenBottom.BackColor = Color.Red;
                                    }
                                    else if (currentX > 764 && currentX <= 882)//1083) // was 757
                                    {
                                        //KeyEvent.AKeyUp();
                                        //KeyEvent.BKeyUp();
                                        //  KeyEvent.CKeyDown();
                                        firstStrum = true; // to allow the click sound to be played the first time that the note is selected
                                        stringSelected = 3;
                                        noNoteCounter = 0; // reset as a new note has been selected, so any existing no note value should be wiped.
                                        noteOnscreenTop.BackColor = Color.Yellow;
                                        noteOnscreenBottom.Left = 709;
                                        noteOnscreenBottom.BackColor = Color.Yellow;


                                    }
                                    else if (currentX > 882 && currentX <= 1128)//1083) // was 757
                                    {
                                        //KeyEvent.AKeyUp();
                                        //KeyEvent.BKeyUp();
                                        //  KeyEvent.CKeyDown();
                                        firstStrum = true; // to allow the click sound to be played the first time that the note is selected
                                        stringSelected = 4;
                                        noNoteCounter = 0; // reset as a new note has been selected, so any existing no note value should be wiped.
                                        noteOnscreenTop.BackColor = Color.Blue;
                                        noteOnscreenBottom.Left = 925;
                                        noteOnscreenBottom.BackColor = Color.Blue;


                                    }
                                    else if (currentX > 1128 )//1083) // was 757
                                    {
                                        //KeyEvent.AKeyUp();
                                        //KeyEvent.BKeyUp();
                                        //  KeyEvent.CKeyDown();
                                        firstStrum = true; // to allow the click sound to be played the first time that the note is selected
                                        stringSelected = 5;
                                        noNoteCounter = 0; // reset as a new note has been selected, so any existing no note value should be wiped.
                                        noteOnscreenTop.BackColor = Color.Pink;
                                        noteOnscreenBottom.Left = 1184;
                                        noteOnscreenBottom.BackColor = Color.Pink;


                                    }
                                }
                            }
                            else
                            {
                                //  test1.Width = verticalCounter*10;
                            }
                           
                        }
                    }
                }
            }
            else
            {
                verticalCounter = 0;

            }

            // end of method 1


            if (countdown == true)
            {
                //  PlayString(); // space bar every half a second method

                //Send string number to play. Used to detect  pixel colour changes at screen location, which triggers and ends strums.
                if (stringSelected == 1) PlayString(stringPointA, argGazePos);
                if (stringSelected == 2) PlayString(stringPointB, argGazePos);
                if (stringSelected == 3) PlayString(stringPointC, argGazePos);
                if (stringSelected == 4) PlayString(stringPointD, argGazePos);
                // image recognition method

            }
        }

        public void paddleMethod(Point argGazePos)
        {
            noteTest.Top = 1000;
            noteTest.Left = (argGazePos.X - (noteTest.Width / 2) + eyeCalibrator);
            //if (eyeCalibratorOn == true)
            //{
            //    if ((noteTest.Left + eyeCalibrator + (noteTest.Width / 2) < (screenSize.Width / 2)))
            //    {
            //        eyeCalibrator++;
            //    }
            //    else if ((noteTest.Left + eyeCalibrator + (noteTest.Width / 2) > (screenSize.Width / 2)))
            //    {
            //        eyeCalibrator--;
            //    }
            //    else
            //    {
            //        eyeCalibratorOn = false;
            //        guitarOSD doneosd = new guitarOSD();
            //        doneosd.Show();
            //        doneosd.Width = 50;
            //        doneosd.Height = 10;
            //        doneosd.Left = 300;
            //        doneosd.Top = 30;
            //    }

            //}
            eyeCalibratorOn=false;
           // else
            {
                if (leftOrRight == 0)
                {
                    eyeCalibrator = eyeCalibrator + eyeYcalibrator;
                    eyeCalibrator = eyeCalibrator + eyecalib2;
                }
                if (leftOrRight == 1)
                {
                    eyeCalibrator = eyeCalibrator + (eyeYcalibrator);
                    eyeCalibrator = eyeCalibrator + (eyecalib2*-1);
                }
               
                eyecalib2 = (((800 - noteTest.Left) / 20));
                if (noteTest.Left + (noteTest.Width / 2) < (screenSize.Width / 2))
                {
                    trig = Convert.ToDouble(argGazePos.Y / Convert.ToDouble(screenSize.Height));
                    trig = (double)1 - trig;
                    eyeYcalibrator = ((606 - argGazePos.Y) / 3); // was 606
                    trigInterim = (Convert.ToDouble(eyeYcalibrator)) + (((Convert.ToDouble(screenSize.Width / 2)) - Convert.ToDouble(argGazePos.X)) * (trig * 5.5)); // was 6.5
                    eyeYcalibrator = Convert.ToInt32(trigInterim / 2);
                    leftOrRight = 0;
                }
                if (noteTest.Left + (noteTest.Width / 2) > (screenSize.Width / 2))
                {

                    trig = Convert.ToDouble(argGazePos.Y / Convert.ToDouble(screenSize.Height));
                    trig = (double)1 - trig;
                    eyeYcalibrator = (((606 - argGazePos.Y) / 3));
                    trigInterim = (Convert.ToDouble(eyeYcalibrator)) + (((Convert.ToDouble(screenSize.Width / 2)) - Convert.ToDouble(argGazePos.X)) * (trig * 5.5));
                    eyeYcalibrator = Convert.ToInt32((trigInterim / 2));
                    leftOrRight = 1;
                 //   Console.WriteLine(((trigInterim / 2) * -1));
                }
                if (leftOrRight == 0)
                {
                    eyeCalibrator = eyeCalibrator - eyecalib2;
                    eyeCalibrator = eyeCalibrator - eyeYcalibrator;
                }
                if (leftOrRight == 1)
                {
                    eyeCalibrator = eyeCalibrator - (eyecalib2*-1);
                    eyeCalibrator = eyeCalibrator - eyeYcalibrator;
                }
           
            }

            if ((noteTest.Left + (noteTest.Width / 2) > 240) && (noteTest.Left + (noteTest.Width / 2) < 600)) // 424
            {
                firstStrum = true; // to allow the click sound to be played the first time that the note is selected
                stringSelected = 1;
                noNoteCounter = 0; // reset as a new note has been selected, so any existing no note value should be wiped.
                noteOnscreenTop.BackColor = Color.Green;
                noteOnscreenBottom.Left = 240;
                noteOnscreenBottom.BackColor = Color.Green;
                countdown = true;
            }
            if ((noteTest.Left + (noteTest.Width / 2) >= 480) && (noteTest.Left + (noteTest.Width / 2) < 800)) // 664
            {
                ////KeyEvent.AKeyUp();
                ////KeyEvent.CKeyUp();
                ////   KeyEvent.BKeyDown();
                firstStrum = true; // to allow the click sound to be played the first time that the note is selected
                stringSelected = 2;
                noNoteCounter = 0; // reset as a new note has been selected, so any existing no note value should be wiped.
                noteOnscreenTop.BackColor = Color.Red;
                noteOnscreenBottom.Left = 480;
                noteOnscreenBottom.BackColor = Color.Red;
                countdown = true;
            }
            if ((noteTest.Left + (noteTest.Width / 2) >= 709) && (noteTest.Left + (noteTest.Width / 2) < 1000)) // 887 1057
            {
                firstStrum = true; // to allow the click sound to be played the first time that the note is selected
                stringSelected = 3;
                noNoteCounter = 0; // reset as a new note has been selected, so any existing no note value should be wiped.
                noteOnscreenTop.BackColor = Color.Yellow;
                noteOnscreenBottom.Left = 709;
                noteOnscreenBottom.BackColor = Color.Yellow;
                countdown = true;
            }
            if ((noteTest.Left + (noteTest.Width / 2) >= 925) && (noteTest.Left + (noteTest.Width / 2) < 1109)) // 274 638 764 1057
            {
                firstStrum = true; // to allow the click sound to be played the first time that the note is selected
                stringSelected = 4;
                noNoteCounter = 0; // reset as a new note has been selected, so any existing no note value should be wiped.
                noteOnscreenTop.BackColor = Color.Blue;
                noteOnscreenBottom.Left = 925;
                noteOnscreenBottom.BackColor = Color.Blue;
                countdown = true;
            }
            if ((noteTest.Left + (noteTest.Width / 2) >= 1109) ) // 274 638 764 1057
            {
                firstStrum = true; // to allow the click sound to be played the first time that the note is selected
                stringSelected = 5;
                noNoteCounter = 0; // reset as a new note has been selected, so any existing no note value should be wiped.
                noteOnscreenTop.BackColor = Color.Pink;
                noteOnscreenBottom.Left = 1184;
                noteOnscreenBottom.BackColor = Color.Pink;
                countdown = true;
            }

          

            if (countdown == true)
            {
                //PlayString(); // space bar every half a second method
                if (stringSelected == 1) PlayString(stringPointA, argGazePos);
                if (stringSelected == 2) PlayString(stringPointB, argGazePos);
                if (stringSelected == 3) PlayString(stringPointC, argGazePos);
                if (stringSelected == 4) PlayString(stringPointD, argGazePos);
                if (stringSelected == 5) PlayString(stringPointE, argGazePos);
                // image recognition method

            }
        }

        public void beginCountdown()
        {

        }


    }
}
