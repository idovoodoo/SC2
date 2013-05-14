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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;
using TetComp;
using GazeTrackerUDPClient;
using SnapClutch.Config;
using SnapClutch.Modes;
using SnapClutch.SCTools;
using Kennedy.ManagedHooks;

namespace SnapClutch
{
    public partial class MainForm : Form
    {
        // import api dll for getting cursor pos
        [DllImport("user32.dll")]
        static extern bool GetCursorPos(ref Point pt);

        private GazeEngine gazeEngine;
        private Configuration configuration = new Configuration();
        private SCModule scModule;

        //private string configFileName = "configData.xml";

        private bool snapClutchOn = false;
        private bool useG9Text = true;

        private Point mouseEmulationPos;
        private Point dwellPos, centreScreen;
        
        private Rectangle rectAbout, rectStartStop, rectWindows, rectMMORPG, rectExit, rectDwellMinus, rectDwellPlus,
            rectG9Text, rectMMORPG2, rectRecal;

        private Rectangle rectProfileA, rectProfileB, rectProfileGo, rectProfileUp, rectProfileDown;

        private double gapSize;

        private ModeSet modeSet;
        private ModeCollection modeCollection;

        private GazeOverlay gazeOverlay;
        private int dwellIncrement = 50;

        private TetClient myTetClient;
        private TetCalibProc tetCalibProc;
        private Client ituClient;

        private float x, y;
        private bool gazeOn = true;
        private Point mousePosition = new Point(0, 0);
        private Point ituPosition = new Point(0,0);

        //snap clutch interface dwell stuff
        private DateTime start;
        private TimeSpan dwellTimer;
        private bool dwellStarted = false;
        private Thread dwellThread;

        private enum EyeTracker { Mouse = 0, ITU = 1, Tobii = 2, SMI = 3, Dev = 4 };
        private int eyeTrackerType;

        private KeyboardHook keyboardHook;
        private bool switchDown = false;
        private bool useSwitch = false;
        private bool useOverlay = true;

        private int currentGame = 0;

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool BringWindowToTop(IntPtr hWnd);

        private const int SW_SHOWNORMAL = 1;
        private const int SW_SHOWMINIMIZED = 2;
        private const int SW_SHOWMAXIMIZED = 3;


        // From winuser.h
        const UInt32 SWP_NOSIZE = 0x0001;
        const UInt32 SWP_NOMOVE = 0x0002;
        const UInt32 SWP_NOZORDER = 0x0004;
        const UInt32 SWP_NOREDRAW = 0x0008;
        const UInt32 SWP_NOACTIVATE = 0x0010;
        const UInt32 SWP_FRAMECHANGED = 0x0020;  /* The frame changed: send WM_NCCALCSIZE */
        const UInt32 SWP_SHOWWINDOW = 0x0040;
        const UInt32 SWP_HIDEWINDOW = 0x0080;
        const UInt32 SWP_NOCOPYBITS = 0x0100;
        const UInt32 SWP_NOOWNERZORDER = 0x0200;  /* Don't do owner Z ordering */
        const UInt32 SWP_NOSENDCHANGING = 0x0400;  /* Don't send WM_WINDOWPOSCHANGING */

        static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
        static readonly IntPtr HWND_BOTTOM = new IntPtr(1);
        static readonly IntPtr HWND_TOP = new IntPtr(0);

        private Process gameProcess =  new Process();

        private About aboutBox = new About();
        private SCMessage mess = new SCMessage("");
        
        public MainForm(int eyeTrackerArg, Configuration argConfig)
        {
            configuration = argConfig;
            eyeTrackerType = eyeTrackerArg;

            gazeOverlay = new GazeOverlay();
            //GetConfiguration();
            
            InitializeComponent();
            InitializeMyComponents();
            BuildApplicationComponents();
          
            StartEyeTracker();
            this.Activate();
        }


        
        private void InitializeMyComponents()
        {
            //CursorFactory.CreateBlankCursor();
            
            //set no cursor            
            //Cursor.Hide();
            centreScreen = new Point(Screen.PrimaryScreen.Bounds.Width / 2, Screen.PrimaryScreen.Bounds.Height / 2);

            mouseEmulationPos = new Point();
            dwellPos = new Point();

            //GetConfiguration();

            scModule = new SCModule(configuration, gazeOverlay);
            gazeEngine = new GazeEngine(configuration.BufferSize, configuration.PixelRange, configuration.CursorUpdate, 75,
                configuration.DwellClickDelay, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            gazeEngine.DwellEvent += new DwellEventHandler(gazeEngine_DwellEvent);
            gazeEngine.OffScreenGlanceEvent += new OffScreenGlanceEventHandler(gazeEngine_OffScreenGlanceEvent);
            gazeEngine.DwellBottomEvent += new DwellBottomEventHandler(gazeEngine_DwellBottomEvent);
            gazeEngine.DwellLeftEvent += new DwellLeftEventHandler(gazeEngine_DwellLeftEvent);
            gazeEngine.DwellRightEvent += new DwellRightEventHandler(gazeEngine_DwellRightEvent);
            gazeEngine.DwellTopEvent += new DwellTopEventHandler(gazeEngine_DwellTopEvent);


            // Setup keyboard hook and event handler
            keyboardHook = new KeyboardHook();
            keyboardHook.KeyboardEvent += new KeyboardHook.KeyboardEventHandler(keyboardHook_KeyboardEvent);
            keyboardHook.InstallHook();

            modeSet = scModule.GetCurrentModeSet();
            modeCollection = modeSet.GetActiveModeCollection();
            
            PostionModeLabels();

            labelDwellTimeValue.Text = configuration.DwellClickDelay.ToString() + " m/s";
            gazeOverlay.SetOverlayStatus(true);

            dwellTimer = new TimeSpan(0, 0, 0, 0, configuration.DwellClickDelay);         
        }

        private void StartEyeTracker()
        {
            switch (eyeTrackerType)
            {
                case 0:
                    //mouse
                    useOverlay = true;
                    gazeOverlay.SetOverlayStatus(true);
                    configuration.OffScreenGlanceDist = -1;
                    gazeEngine.SetOffScreenBuffer(-1);
                    break;
                case 1:
                    //itu gazetracker
                    useOverlay = true;
                    gazeOverlay.SetOverlayStatus(true);
                    // try/catch connect to udp server
                    ituClient = new Client();
                    ituClient.OnGazeData += new Client.GazeDataHandler(ituClient_OnGazeData);
                    ituClient.Port = 64555;
                    ituClient.MouseRedirect = false;
                    ituClient.Start();
                    break;
                case 2:
                    //tobii
                    useOverlay = true;
                    gazeOverlay.SetOverlayStatus(true);

                    // register all the tobii events

                    // Set up the calibration procedure object and it's events
                    tetCalibProc = new TetCalibProcClass();
                    _ITetCalibProcEvents_Event tetCalibProcEvents = (_ITetCalibProcEvents_Event)tetCalibProc;
                    tetCalibProcEvents.OnCalibrationEnd += new _ITetCalibProcEvents_OnCalibrationEndEventHandler(tetCalibProcEvents_OnCalibrationEnd);
                    tetCalibProcEvents.OnKeyDown += new _ITetCalibProcEvents_OnKeyDownEventHandler(tetCalibProcEvents_OnKeyDown);

                    // Set up the TET client object and it's events
                    myTetClient = new TetClientClass();
                    _ITetClientEvents_Event tetClientEvents = (_ITetClientEvents_Event)myTetClient;
                    tetClientEvents.OnTrackingStarted += new _ITetClientEvents_OnTrackingStartedEventHandler(tetClientEvents_OnTrackingStarted);
                    tetClientEvents.OnTrackingStopped += new _ITetClientEvents_OnTrackingStoppedEventHandler(tetClientEvents_OnTrackingStopped);
                    tetClientEvents.OnGazeData += new _ITetClientEvents_OnGazeDataEventHandler(tetClientEvents_OnGazeData);

                    try
                    {
                        Console.WriteLine(configuration.EyeTrackerAddress.ToString());
                        // Connect to the TET server if necessary
                        if (!myTetClient.IsConnected)
                            myTetClient.Connect(configuration.EyeTrackerAddress.ToString(), (int)TetConstants.TetConstants_DefaultServerPort, TetSynchronizationMode.TetSynchronizationMode_Local);

                        // Start tracking gaze data
                        myTetClient.StartTracking();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        
                    }
                    break;
                case 3:
                    //smi
                    break;
                case 4:
                    //development mode
                    gazeOverlay.SetOverlayStatus(false);
                    configuration.OffScreenGlanceDist = -1;
                    gazeEngine.SetOffScreenBuffer(-1);
                    useOverlay = false;
                    break;

            }
        }

        private void timerDef_Tick(object sender, EventArgs e)
        {
                switch (eyeTrackerType)
                {
                    case 0:
                        //mouse
                        GetCursorPos(ref mouseEmulationPos);
                        gazeEngine.AddPositionMovingAvg(((1 / (float)Screen.PrimaryScreen.Bounds.Width) * mouseEmulationPos.X),
                            ((1 / (float)Screen.PrimaryScreen.Bounds.Height) * mouseEmulationPos.Y));
                        scModule.DataIn(new Point(Convert.ToInt32(Math.Round(Screen.PrimaryScreen.Bounds.Width * Convert.ToDouble(gazeEngine.GetMouseEmulationX())))
                            , Convert.ToInt32(Math.Round(Screen.PrimaryScreen.Bounds.Height * Convert.ToDouble(gazeEngine.GetMouseEmulationY())))));
                        break;
                    case 1:
                        //itu gazetracker
                        gazeEngine.AddPositionMovingAvg(x, y);
                        mousePosition = ConvertToMouseCoords(gazeEngine.GetPositionXMovingAvg(), gazeEngine.GetPositionYMovingAvg());
                        scModule.DataIn(mousePosition);
                        break;
                    case 2:
                        //tobii                
                        gazeEngine.AddPositionMovingAvg(x, y);
                        mousePosition = ConvertToMouseCoords(gazeEngine.GetPositionXMovingAvg(), gazeEngine.GetPositionYMovingAvg());
                        scModule.DataIn(mousePosition);
                        break;
                    case 3:
                        //smi
                        break;
                    case 4:
                        //development mode
                        GetCursorPos(ref mouseEmulationPos);
                        gazeEngine.AddPositionMovingAvg(((1 / (float)Screen.PrimaryScreen.Bounds.Width) * mouseEmulationPos.X),
                            ((1 / (float)Screen.PrimaryScreen.Bounds.Height) * mouseEmulationPos.Y));
                        scModule.DataIn(new Point(Convert.ToInt32(Math.Round(Screen.PrimaryScreen.Bounds.Width * Convert.ToDouble(gazeEngine.GetMouseEmulationX())))
                            , Convert.ToInt32(Math.Round(Screen.PrimaryScreen.Bounds.Height * Convert.ToDouble(gazeEngine.GetMouseEmulationY())))));
                        break;
                }
            
        }

        #region Snap Clutch Interface Button Dwells

        // thread which checks if there is a dwell on the button
        private void CheckDwell()
        {
            if (!snapClutchOn)
            {
                if (!useSwitch && !aboutBox.Visible && !mess.Visible)
                {
                    while (dwellStarted)
                    {
                        if ((DateTime.Now - start) > dwellTimer)
                        {
                            start = DateTime.Now;
                            dwellPos = new Point(Convert.ToInt32(Math.Round(Screen.PrimaryScreen.Bounds.Width * Convert.ToDouble(gazeEngine.GetMouseEmulationX())))
                                , Convert.ToInt32(Math.Round(Screen.PrimaryScreen.Bounds.Height * Convert.ToDouble(gazeEngine.GetMouseEmulationY()))));

                            SnapClutchSounds.Click2();
                            MouseEvent.LeftClickPoint(dwellPos.X, dwellPos.Y);
                            //Console.WriteLine("we are clicking!");
                        }
                    }
                }

            }
        }

        #endregion

        #region Tracking events

            #region Tobii tracking events

            private void tetClientEvents_OnTrackingStarted()
            {
            }

            private void tetClientEvents_OnTrackingStopped(int hr)
            {
                if (hr != (int)TetHResults.ITF_S_OK) MessageBox.Show(string.Format("Error {0} occured while tracking.", hr), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            private void tetClientEvents_OnGazeData(ref TetGazeData gazeData)
            {
                // Use data only if both left and right eye was found by the eye tracker
                if ((gazeData.validity_lefteye == 0 && gazeData.validity_righteye == 0) && gazeOn)
                {
                    // Let the x, y and distance be the right and left eye average
                    x = (gazeData.x_gazepos_lefteye + gazeData.x_gazepos_righteye) / 2;
                    y = (gazeData.y_gazepos_lefteye + gazeData.y_gazepos_righteye) / 2;
                }


            }

            #endregion

            #region Tobii Calibration events


            private void tetCalibProcEvents_OnCalibrationEnd(int result)
            {
                // Calibration ended, hide the calibration window and update the calibration plot
                tetCalibProc.WindowVisible = false;
                gazeOverlay.SetOverlayStatus(true);
            }

            private void tetCalibProcEvents_OnKeyDown(int virtualKeyCode)
            {
                // Interrupt the calibration on key events
                if (tetCalibProc.IsCalibrating) tetCalibProc.InterruptCalibration(); // Will trigger OnCalibrationEnd
            }


            /// <summary>
            /// Starts calibration in either calibration or recalibration mode.
            /// </summary>
            /// <param name="isRecalibrating">whether to use recalibration or not.</param>
            private void Calibrate(bool isRecalibrating)
            {
                // Connect the calibration procedure if necessary
                if (!tetCalibProc.IsConnected) 
                    tetCalibProc.Connect(configuration.EyeTrackerAddress.ToString(), (int)TetConstants.TetConstants_DefaultServerPort);

                // Initiate number of points to be calibrated
                tetCalibProc.NumPoints = TetNumCalibPoints.TetNumCalibPoints_5;

                // Initiate window properties and start calibration
                tetCalibProc.WindowTopmost = false;
                tetCalibProc.WindowVisible = true;
                tetCalibProc.StartCalibration(isRecalibrating ? TetCalibType.TetCalibType_Recalib : TetCalibType.TetCalibType_Calib, false);
            }

            #endregion

            #region ITU Gazedriver tracking events

            private void ituClient_OnGazeData(GazeData gData)
            {
                //Console.WriteLine("X: " + gData.GazePositionX / Screen.PrimaryScreen.Bounds.Width);
                //Console.WriteLine("X: " + gData.GazePositionX / Screen.PrimaryScreen.Bounds.X + " Y:" + gData.GazePositionY / Screen.PrimaryScreen.Bounds.Y);
                x = (float) gData.GazePositionX / Screen.PrimaryScreen.Bounds.Width;
                y = (float) gData.GazePositionY / Screen.PrimaryScreen.Bounds.Height;
            }

            #endregion

            #region SMI tracking events

        #endregion

            #endregion

        private Point ConvertToMouseCoords(float argX, float argY)
        {
            //float tobiiX = argX;
            //float tobiiY = argY;

            //double dx = Math.Round(Screen.PrimaryScreen.Bounds.Width * Convert.ToDouble(argX));
            //double dy = Math.Round(Screen.PrimaryScreen.Bounds.Height * Convert.ToDouble(argY));

            //int x = Convert.ToInt32(Math.Round(Screen.PrimaryScreen.Bounds.Width * Convert.ToDouble(argX)));
            //int y = Convert.ToInt32(Math.Round(Screen.PrimaryScreen.Bounds.Height * Convert.ToDouble(argY)));

            Point tempMousePos = new Point(Convert.ToInt32(Math.Round(Screen.PrimaryScreen.Bounds.Width * Convert.ToDouble(argX)))
                , Convert.ToInt32(Math.Round(Screen.PrimaryScreen.Bounds.Height * Convert.ToDouble(argY))));

            return tempMousePos;
        }

        private void EyeTrackingStart()
        {
            gazeOn = true;

            switch (eyeTrackerType)
            {
                case 0:
                    //mouse
                    configuration.OffScreenGlanceDist = -1;
                    gazeEngine.SetOffScreenBuffer(-1);
                    break;
                case 1:
                    //itu gazetracker
                    break;
                case 2:
                    //tobii
                    try
                    {
                        // Connect to the TET server if necessary
                        if (!myTetClient.IsConnected)
                            myTetClient.Connect(configuration.EyeTrackerAddress.ToString(), (int)TetConstants.TetConstants_DefaultServerPort, TetSynchronizationMode.TetSynchronizationMode_Local);

                        // Start tracking gaze data
                        myTetClient.StartTracking();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
                case 3:
                    //smi
                    break;
            }
        }

        private void EyeTrackingStop()
        {
            gazeOn = false;

            switch (eyeTrackerType)
            {
                case 0:
                    //mouse
                    break;
                case 1:
                    //itu gazetracker
                    ituClient.Stop();
                    ituClient.Dispose();
                    break;
                case 2:
                    //tobii
                    try
                    {
                        if (myTetClient.IsTracking)
                        {
                            myTetClient.StopTracking();
                            myTetClient.Disconnect();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
                case 3:
                    //smi
                    break;
                case 4:
                    //development mode
                    break;
            }
        }


        private void CheckAboutBoxOpen()
        {
            if (aboutBox.Visible)
                aboutBox.Hide();
        }

        

        #region Gaze event handlers

        // dwell event handler
        private void gazeEngine_DwellEvent(object o, DwellEventArgs e)
        {
            if (!useSwitch)
            {
                dwellPos = new Point(Convert.ToInt32(Math.Round(Screen.PrimaryScreen.Bounds.Width * Convert.ToDouble(gazeEngine.GetMouseEmulationX())))
                    , Convert.ToInt32(Math.Round(Screen.PrimaryScreen.Bounds.Height * Convert.ToDouble(gazeEngine.GetMouseEmulationY()))));

                if (!snapClutchOn)
                {


                }
                else
                {

                    scModule.SetDwellPos(dwellPos);
                    scModule.Dwell(false, false);
                }
            }
        }

        // off screen glance event handler
        private void gazeEngine_OffScreenGlanceEvent(object o, OffScreenGlanceEventArgs e)
        {
                if (!snapClutchOn)
                {
                }
                else
                {
                    scModule.OffScreenGlance(e.Glance);
                }
            
        }

        // off screen dwell event handler
        private void gazeEngine_DwellTopEvent(object o, DwellTopEventArgs e)
        {
                if (snapClutchOn)
                {
                    SnapClutchSounds.OffScreenDwell();
                    //KeyEvent.AltTab();

                    if (useG9Text)
                    {
                        if (scModule.GetG9Active())
                        {
                            //g9 is active so turn it off
                            scModule.SetG9Active(false, useSwitch, useOverlay);
                        }
                        else
                        {
                            //g9 isn't on so turn it on!
                            //CursorFactory.Create(Application.StartupPath + CursorSnapClutch.BlankCursor());
                            scModule.SetG9Active(true, useSwitch, useOverlay);

                        }
                    }
                }
            
        }

        // off screen dwell event handler
        private void gazeEngine_DwellRightEvent(object o, DwellRightEventArgs e)
        {
            
                if (snapClutchOn)
                {
                    SnapClutchSounds.OffScreenDwell();

                    if (useG9Text)
                    {
                        if (scModule.GetG9Active())
                        {
                            //g9 is active so turn it off
                            scModule.SetG9Active(false, useSwitch, useOverlay);
                        }
                        else
                        {
                            //g9 isn't on so turn it on!
                            //CursorFactory.Create(Application.StartupPath + CursorSnapClutch.BlankCursor());
                            scModule.SetG9Active(true, useSwitch, useOverlay);

                        }
                    }
                }
            
        }

        // off screen dwell event handler
        private void gazeEngine_DwellLeftEvent(object o, DwellLeftEventArgs e)
        {
                if (snapClutchOn)
                {
                    SnapClutchSounds.OffScreenDwell();
                    if (useG9Text)
                    {
                        if (scModule.GetG9Active())
                        {
                            //g9 is active so turn it off
                            scModule.SetG9Active(false, useSwitch, useOverlay);
                        }
                        else
                        {
                            //g9 isn't on so turn it on!
                            //CursorFactory.Create(Application.StartupPath + CursorSnapClutch.BlankCursor());
                            scModule.SetG9Active(true, useSwitch, useOverlay);

                        }
                    }
                }
            
        }

        // off screen dwell event handler
        private void gazeEngine_DwellBottomEvent(object o, DwellBottomEventArgs e)
        {
                if (snapClutchOn)
                {
                    SnapClutchSounds.OffScreenDwell();
                    //CursorFactory.CreateBlankCursor();
                    CursorFactory.Create(Application.StartupPath + CursorSnapClutch.BlankCursor());

                    // stop snap clutch and show interface
                    snapClutchOn = false;
                    scModule.SetG9Active(false, useSwitch, useOverlay);

                    if(eyeTrackerType == 4)
                        gazeOverlay.SetOverlayStatus(false);
                    else
                        gazeOverlay.SetOverlayStatus(true); // change to true later when i am not using the mouse!!!


                    buttonStartStop.BackColor = Color.FromArgb(192, 192, 255);
                    button1Go.BackColor = Color.FromArgb(192, 192, 255);

                    this.Visible = true;
                    this.WindowState = FormWindowState.Maximized;
                    this.Activate();
                    scModule.StopSnapClutch();
                }
            
        }



        #endregion

        #region Interface methods

        // build all the interface components based on screen resolution
        private void BuildApplicationComponents()
        {
            //buttonSwitch.Hide();

            double panelWidthFactor = 0.47;
            double panelHeightFactor = 0.9;
            double panelGapFactor = 0.98;
            gapSize = Screen.PrimaryScreen.Bounds.Width * (1 - panelGapFactor);
            Rectangle rect = new Rectangle();

            
            rectangleShapeLeft.Width = (int)(Convert.ToDouble(Screen.PrimaryScreen.Bounds.Width) * panelWidthFactor);
            rectangleShapeLeft.Height = (int)((Convert.ToDouble(Screen.GetWorkingArea(rect).Height)) - (gapSize * 2.5));
            rectangleShapeLeft.Left = (int)(Convert.ToDouble(Screen.PrimaryScreen.Bounds.Width) * (1 - panelGapFactor));
            rectangleShapeLeft.Top = (int)gapSize;

            rectangleShapeRight.Width = (int)(Convert.ToDouble(Screen.PrimaryScreen.Bounds.Width) * panelWidthFactor);
            rectangleShapeRight.Height = (int)((Convert.ToDouble(Screen.PrimaryScreen.Bounds.Height) * panelHeightFactor) * 0.8);
            rectangleShapeRight.Left = rectangleShapeLeft.Right + ((int)(Convert.ToDouble(Screen.PrimaryScreen.Bounds.Width) * (1 - panelGapFactor)));
            rectangleShapeRight.Top = (int)gapSize;



            labelSettings.Left = rectangleShapeLeft.Left + (rectangleShapeLeft.Width / 2) - (labelSettings.Width / 2);
            labelSettings.Top = (int)(Convert.ToDouble(rectangleShapeLeft.Top + gapSize));
            labelModes.Left = rectangleShapeRight.Left + (rectangleShapeRight.Width / 2) - (labelModes.Width / 2);
            labelModes.Top = (int)(Convert.ToDouble(rectangleShapeRight.Top + gapSize));

            buttonAbout.Width = (int)(Convert.ToDouble((rectangleShapeRight.Width / 3) - (gapSize / 2)));
            buttonAbout.Height = (int)(Convert.ToDouble((rectangleShapeLeft.Height - rectangleShapeRight.Height) - gapSize));
            buttonAbout.Left = rectangleShapeRight.Left;
            buttonAbout.Top = (int)(Convert.ToDouble(rectangleShapeRight.Bottom + gapSize));
            gazeOverlay.AddZone("Button About", buttonAbout.Location, buttonAbout.Width, buttonAbout.Height + 30);
            rectAbout = new Rectangle(buttonAbout.Left, buttonAbout.Top, buttonAbout.Width, buttonAbout.Height);

            buttonExit.Width = (int)(Convert.ToDouble((rectangleShapeRight.Width / 3) - (gapSize / 2)));
            buttonExit.Height = (int)(Convert.ToDouble((rectangleShapeLeft.Height - rectangleShapeRight.Height) - gapSize));
            buttonExit.Left = (int)(Convert.ToDouble(buttonAbout.Right + gapSize));
            buttonExit.Top = (int)(Convert.ToDouble(rectangleShapeRight.Bottom + gapSize));
            gazeOverlay.AddZone("Button Exit", buttonExit.Location, buttonExit.Width, buttonExit.Height + 30);
            rectExit = new Rectangle(buttonExit.Left, buttonExit.Top, buttonExit.Width, buttonExit.Height);

            buttonStartStop.Width = (int)(Convert.ToDouble((rectangleShapeRight.Width / 3) - (gapSize / 2)));
            buttonStartStop.Height = (int)(Convert.ToDouble((rectangleShapeLeft.Height - rectangleShapeRight.Height) - gapSize));
            buttonStartStop.Left = (int)(Convert.ToDouble(buttonExit.Right + gapSize));
            buttonStartStop.Top = (int)(Convert.ToDouble(rectangleShapeRight.Bottom + gapSize));
            gazeOverlay.AddZone("Button Start Stop", buttonStartStop.Location, buttonStartStop.Width, buttonStartStop.Height + 30);
            rectStartStop = new Rectangle(buttonStartStop.Left, buttonStartStop.Top, buttonStartStop.Width, buttonStartStop.Height);

            buttonModeWindows.Width = (int)(Convert.ToDouble((rectangleShapeRight.Width / 3) - (gapSize * 1.5)));
            buttonModeWindows.Left = (int)(Convert.ToDouble(rectangleShapeRight.Left + gapSize));
            buttonModeWindows.Top = (int)(Convert.ToDouble(labelModes.Bottom + gapSize));
            buttonModeWindows.Height = buttonAbout.Height;
            gazeOverlay.AddZone("Button Mode Windows", buttonModeWindows.Location, buttonModeWindows.Width, buttonModeWindows.Height + 30);
            rectWindows = new Rectangle(buttonModeWindows.Left, buttonModeWindows.Top, buttonModeWindows.Width, buttonModeWindows.Height);

            buttonModeMMORPG.Width = (int)(Convert.ToDouble((rectangleShapeRight.Width / 3) - (gapSize * 1.5)));
            buttonModeMMORPG.Left = (int)(Convert.ToDouble(buttonModeWindows.Right + gapSize));
            buttonModeMMORPG.Top = (int)(Convert.ToDouble(labelModes.Bottom + gapSize));
            buttonModeMMORPG.Height = buttonAbout.Height;
            gazeOverlay.AddZone("Button Mode MMORPG", buttonModeMMORPG.Location, buttonModeMMORPG.Width, buttonModeMMORPG.Height + 30);
            rectMMORPG = new Rectangle(buttonModeMMORPG.Left, buttonModeMMORPG.Top, buttonModeMMORPG.Width, buttonModeMMORPG.Height);

            buttonModeMMORPGVariant.Width = (int)(Convert.ToDouble((rectangleShapeRight.Width / 3) - (gapSize * 1.5)));
            buttonModeMMORPGVariant.Left = (int)(Convert.ToDouble(buttonModeMMORPG.Right + gapSize));
            buttonModeMMORPGVariant.Top = (int)(Convert.ToDouble(labelModes.Bottom + gapSize));
            buttonModeMMORPGVariant.Height = buttonAbout.Height;
            gazeOverlay.AddZone("Button Mode MMORPG Variant", buttonModeMMORPGVariant.Location, buttonModeMMORPGVariant.Width, buttonModeMMORPGVariant.Height + 30);
            rectMMORPG2 = new Rectangle(buttonModeMMORPGVariant.Left, buttonModeMMORPGVariant.Top, buttonModeMMORPGVariant.Width, buttonModeMMORPGVariant.Height);           

            pictureBoxMonitor.Height = (int)(Convert.ToDouble(rectangleShapeRight.Height / 3));
            pictureBoxMonitor.Width = (int)(Convert.ToDouble(rectangleShapeRight.Width / 3));
            pictureBoxMonitor.Left = (int)(Convert.ToDouble((rectangleShapeRight.Left + (rectangleShapeRight.Width / 2) - (pictureBoxMonitor.Width / 2))));
            pictureBoxMonitor.Top = (int)(Convert.ToDouble(rectangleShapeRight.Height / 2));

            rectangleShapeDwellTime.Width = (int)(Convert.ToDouble((rectangleShapeLeft.Width / 2) - gapSize));
            rectangleShapeDwellTime.Height = buttonAbout.Height / 2;
            rectangleShapeDwellTime.Left = (int)(Convert.ToDouble(rectangleShapeLeft.Left + gapSize));
            rectangleShapeDwellTime.Top = (int)(Convert.ToDouble(labelSettings.Bottom + gapSize));

            labelDwellTime.Left = (int)(Convert.ToDouble(rectangleShapeDwellTime.Left + gapSize));
            labelDwellTime.Top = (int)(Convert.ToDouble(rectangleShapeDwellTime.Top + (rectangleShapeDwellTime.Height / 2) - (labelDwellTime.Height / 2)));

            labelDwellTimeValue.Left = (int)(Convert.ToDouble(labelDwellTime.Right + gapSize));
            labelDwellTimeValue.Top = (int)(Convert.ToDouble(labelDwellTime.Top + (labelDwellTime.Height / 2) - (labelDwellTimeValue.Height / 2)));

            buttonDwellMinus.Width = (int)(Convert.ToDouble((rectangleShapeDwellTime.Width / 2) - gapSize));
            buttonDwellMinus.Height = rectangleShapeDwellTime.Height;
            buttonDwellMinus.Left = (int)(Convert.ToDouble(rectangleShapeDwellTime.Right + gapSize));
            buttonDwellMinus.Top = rectangleShapeDwellTime.Top;
            gazeOverlay.AddZone("Dwell Minus", buttonDwellMinus.Location, buttonDwellMinus.Width, buttonDwellMinus.Height + 30);
            rectDwellMinus = new Rectangle(buttonDwellMinus.Left, buttonDwellMinus.Top, buttonDwellMinus.Width, buttonDwellMinus.Height);

            buttonDwellPlus.Width = (int)(Convert.ToDouble((rectangleShapeDwellTime.Width / 2) - gapSize));
            buttonDwellPlus.Height = rectangleShapeDwellTime.Height;
            buttonDwellPlus.Left = (int)(Convert.ToDouble(buttonDwellMinus.Right + gapSize));
            buttonDwellPlus.Top = rectangleShapeDwellTime.Top;
            gazeOverlay.AddZone("Dwell Plus", buttonDwellPlus.Location, buttonDwellPlus.Width, buttonDwellPlus.Height + 30);
            rectDwellPlus = new Rectangle(buttonDwellPlus.Left, buttonDwellPlus.Top, buttonDwellPlus.Width, buttonDwellPlus.Height);

            buttonTextOnOff.Width = buttonModeWindows.Width;
            buttonTextOnOff.Height = buttonModeWindows.Height;
            buttonTextOnOff.Left = (int)(Convert.ToDouble(rectangleShapeLeft.Left + (rectangleShapeLeft.Width / 4) + (rectangleShapeLeft.Width / 2) - (buttonTextOnOff.Width / 2) - gapSize));
            buttonTextOnOff.Top = (int)(Convert.ToDouble(buttonDwellPlus.Bottom + gapSize));
            gazeOverlay.AddZone("G9 Text", buttonTextOnOff.Location, buttonTextOnOff.Width, buttonTextOnOff.Height + 30);
            rectG9Text = new Rectangle(buttonTextOnOff.Left, buttonTextOnOff.Top, buttonTextOnOff.Width, buttonTextOnOff.Height);

            buttonRecalibrate.Width = buttonModeWindows.Width;
            buttonRecalibrate.Height = buttonModeWindows.Height;
            buttonRecalibrate.Left = (int)(Convert.ToDouble(rectangleShapeLeft.Left + (rectangleShapeLeft.Width / 4) - (buttonRecalibrate.Width / 2) + gapSize));
            buttonRecalibrate.Top = (int)(Convert.ToDouble(buttonDwellPlus.Bottom + gapSize));
            gazeOverlay.AddZone("Recalibrate", buttonRecalibrate.Location, buttonRecalibrate.Width, buttonRecalibrate.Height + 30);
            rectRecal = new Rectangle(buttonRecalibrate.Left, buttonRecalibrate.Top, buttonRecalibrate.Width, buttonRecalibrate.Height);

            rectangleShapeLauncher.Width = (int)(Convert.ToDouble(rectangleShapeLeft.Width - (gapSize * 2)));
            rectangleShapeLauncher.Height = (int)(Convert.ToDouble(rectangleShapeLeft.Height - buttonTextOnOff.Bottom - gapSize));
            rectangleShapeLauncher.Left = (int)(Convert.ToDouble(rectangleShapeLeft.Left + gapSize));
            rectangleShapeLauncher.Top = (int)(Convert.ToDouble(buttonTextOnOff.Bottom + gapSize));

            labelAppLauncher.Left = rectangleShapeLeft.Left + (rectangleShapeLeft.Width / 2) - (labelAppLauncher.Width / 2);
            labelAppLauncher.Top = (int)(Convert.ToDouble(rectangleShapeLauncher.Top + gapSize));

            int rectShapeLauncherHeight = rectangleShapeLauncher.Bottom - labelAppLauncher.Bottom;
            
            // game one controls
            rectangleShapeGame1.Width = (int)(Convert.ToDouble(rectangleShapeLauncher.Width) * 0.50);
            rectangleShapeGame1.Height = (int)(Convert.ToDouble(rectShapeLauncherHeight * 0.25));
            rectangleShapeGame1.Left = rectangleShapeLauncher.Left + (rectangleShapeLauncher.Width / 2) - (rectangleShapeGame1.Width / 2);
            rectangleShapeGame1.Top = (int)(Convert.ToDouble(labelAppLauncher.Bottom + (rectShapeLauncherHeight * 0.05)));

            button1A.Width = (int)(Convert.ToDouble((rectangleShapeGame1.Width / 2) * 0.9));
            button1A.Height = (int)(Convert.ToDouble(rectShapeLauncherHeight * 0.25));
            button1A.Left = rectangleShapeGame1.Left;
            button1A.Top = (int)(Convert.ToDouble(rectangleShapeGame1.Bottom + (rectangleShapeGame1.Bottom * 0.05)));
            gazeOverlay.AddZone("Button 1A", button1A.Location, button1A.Width, button1A.Height + 30);
            rectProfileA = new Rectangle(button1A.Left, button1A.Top, button1A.Width, button1A.Height);

            button1B.Width = (int)(Convert.ToDouble((rectangleShapeGame1.Width / 2) * 0.9));
            button1B.Height = (int)(Convert.ToDouble(rectShapeLauncherHeight * 0.25));
            button1B.Left = rectangleShapeGame1.Right - button1B.Width;
            button1B.Top = (int)(Convert.ToDouble(rectangleShapeGame1.Bottom + (rectangleShapeGame1.Bottom * 0.05)));
            gazeOverlay.AddZone("Button 1B", button1B.Location, button1B.Width, button1B.Height + 30);
            rectProfileB = new Rectangle(button1B.Left, button1B.Top, button1B.Width, button1B.Height);

            button1Go.Width = rectangleShapeGame1.Width;
            button1Go.Height = (int)(Convert.ToDouble(rectShapeLauncherHeight * 0.25));
            button1Go.Left = rectangleShapeGame1.Left;
            button1Go.Top = (int)(Convert.ToDouble(button1B.Bottom + (rectShapeLauncherHeight * 0.05)));
            gazeOverlay.AddZone("Button Go", button1Go.Location, button1Go.Width, button1Go.Height + 30);
            rectProfileGo = new Rectangle(button1Go.Left, button1Go.Top, button1Go.Width, button1Go.Height);


            // game scroll controls
            buttonGameUp.Width = (int)(Convert.ToDouble(rectangleShapeLauncher.Width) * 0.15);
            buttonGameUp.Height = (int)(Convert.ToDouble(buttonGameUp.Width));
            buttonGameUp.Left = (int)(Convert.ToDouble((rectangleShapeGame1.Right + (rectangleShapeLauncher.Width) * 0.05)));
            buttonGameUp.Top = (int)(Convert.ToDouble(labelAppLauncher.Bottom + (rectShapeLauncherHeight * 0.05)));
            gazeOverlay.AddZone("Button Game Up", buttonGameUp.Location, buttonGameUp.Width, buttonGameUp.Height + 30);
            rectProfileUp = new Rectangle(buttonGameUp.Left, buttonGameUp.Top, buttonGameUp.Width, buttonGameUp.Height);

            buttonGameDown.Width = buttonGameUp.Width;
            buttonGameDown.Height = buttonGameUp.Height;
            buttonGameDown.Left = buttonGameUp.Left;
            buttonGameDown.Top = (int)(Convert.ToDouble(buttonGameUp.Bottom + (rectShapeLauncherHeight * 0.05)));
            gazeOverlay.AddZone("Button Game Down", buttonGameDown.Location, buttonGameDown.Width, buttonGameDown.Height + 30);
            rectProfileDown = new Rectangle(buttonGameDown.Left, buttonGameDown.Top, buttonGameDown.Width, buttonGameDown.Height);


            PostionModeLabels();
        }

        private void PostionModeLabels()
        {
            UpdateGamesList();

            CursorFactory.Create(Application.StartupPath + CursorSnapClutch.BlankCursor());
            //CursorFactory.CreateBlankCursor();

            // first set active mode buttons
            if (modeSet.IsProfileAActive_Windows())
            {
                buttonModeWindows.BackColor = Color.FromArgb(128, 255, 128);

                buttonModeMMORPG.BackColor = Color.FromArgb(192, 192, 255);
                buttonModeMMORPGVariant.BackColor = Color.FromArgb(192, 192, 255);
                button1A.BackColor = Color.FromArgb(192, 192, 255);
                button1B.BackColor = Color.FromArgb(192, 192, 255);

            }
            else if (modeSet.IsProfileBActive_MMORPGa())
            {
                buttonModeMMORPG.BackColor = Color.FromArgb(128, 255, 128);

                buttonModeWindows.BackColor = Color.FromArgb(192, 192, 255);
                buttonModeMMORPGVariant.BackColor = Color.FromArgb(192, 192, 255);
                button1A.BackColor = Color.FromArgb(192, 192, 255);
                button1B.BackColor = Color.FromArgb(192, 192, 255);
            }
            else if (modeSet.IsProfileCActive_MMORPGb())
            {
                buttonModeMMORPGVariant.BackColor = Color.FromArgb(128, 255, 128);

                buttonModeMMORPG.BackColor = Color.FromArgb(192, 192, 255);                
                buttonModeWindows.BackColor = Color.FromArgb(192, 192, 255);
                button1A.BackColor = Color.FromArgb(192, 192, 255);
                button1B.BackColor = Color.FromArgb(192, 192, 255);
            }

            labelModesBottom.Text = modeSet.GetActiveModeCollection().GetModeBottom().modeName;
            labelModesTop.Text = modeSet.GetActiveModeCollection().GetModeTop().modeName;
            labelModesLeft.Text = modeSet.GetActiveModeCollection().GetModeLeft().modeName;
            labelModesRight.Text = modeSet.GetActiveModeCollection().GetModeRight().modeName;

            labelModesTop.Left = (int)(Convert.ToDouble(pictureBoxMonitor.Left + (pictureBoxMonitor.Width / 2) - (labelModesTop.Width / 2)));
            labelModesTop.Top = (int)(Convert.ToDouble(pictureBoxMonitor.Top - labelModesTop.Height - (gapSize / 2)));

            labelModesBottom.Left = (int)(Convert.ToDouble(pictureBoxMonitor.Left + (pictureBoxMonitor.Width / 2) - (labelModesBottom.Width / 2)));
            labelModesBottom.Top = (int)(Convert.ToDouble(pictureBoxMonitor.Bottom + (gapSize / 2)));

            labelModesLeft.Left = (int)(Convert.ToDouble(pictureBoxMonitor.Left - (gapSize / 2) - labelModesLeft.Width));
            labelModesLeft.Top = (int)(Convert.ToDouble(pictureBoxMonitor.Top) + (pictureBoxMonitor.Height / 2) - (labelModesLeft.Height / 2));

            labelModesRight.Left = (int)(Convert.ToDouble(pictureBoxMonitor.Right + (gapSize / 2)));
            labelModesRight.Top = (int)(Convert.ToDouble(pictureBoxMonitor.Top) + (pictureBoxMonitor.Height / 2) - (labelModesRight.Height / 2));


        }

        private void MainFormV2_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Visible = false;
                notifyIcon1.Visible = true;
            }
            else if (this.WindowState == FormWindowState.Maximized)
            {
                this.Visible = true;
                notifyIcon1.Visible = false;
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Visible = true;
            notifyIcon1.Visible = false;
        }


        #endregion

        #region Button events

            #region general button

        private void button_MouseEnter(object sender, EventArgs e)
        {
            if (!dwellStarted)
            {
                start = DateTime.Now;
                dwellStarted = true;
                dwellThread = new Thread(new ThreadStart(CheckDwell));
                dwellThread.Start();
            }
            else
            {
            }

            if(!mess.Visible && !aboutBox.Visible)
                CursorFactory.Create(Application.StartupPath + CursorSnapClutch.Pulse());
        }

        private void button_MouseLeave(object sender, EventArgs e)
        {
            if (dwellStarted)
            {
                dwellThread.Abort();
                dwellStarted = false;
            }

            CursorFactory.Create(Application.StartupPath + CursorSnapClutch.BlankCursor());
        }

        #endregion

            #region windows

        //********************* mode windows button *********************
            private void buttonModeWindows_MouseEnter(object sender, EventArgs e)
            {                
                if (!dwellStarted)
                {
                    start = DateTime.Now;
                    dwellStarted = true;
                    dwellThread = new Thread(new ThreadStart(CheckDwell));
                    dwellThread.Start();
                }
                else
                {
                }

                CursorFactory.Create(Application.StartupPath + CursorSnapClutch.Pulse());
                //CursorFactory.CreatePulseCursor();
                //this.Cursor = CursorFactory.Create(Application.StartupPath + CursorSnapClutch.Pulse());
            }

            private void buttonModeWindows_MouseLeave(object sender, EventArgs e)
            {
                if (dwellStarted)
                {
                    dwellThread.Abort();
                    dwellStarted = false;
                }

                CursorFactory.Create(Application.StartupPath + CursorSnapClutch.BlankCursor());
                //CursorFactory.CreateBlankCursor();
            }

            private void buttonModeWindows_Click(object sender, EventArgs e)
            {
                if (!mess.Visible && !aboutBox.Visible)
                {
                    CheckAboutBoxOpen();
                    modeSet.ActivateProfileA_Windows();
                    PostionModeLabels();
                }
            }

            #endregion

            #region mmorpg

            private void buttonModeMMORPG_Click(object sender, EventArgs e)
            {
                if (!mess.Visible && !aboutBox.Visible)
                {
                    CheckAboutBoxOpen();
                    modeSet.ActivateProfileB_MMORPGa();
                    PostionModeLabels();
                }
            }

            #endregion

            #region mmorpg variant

            private void buttonModeMMORPGVariant_Click(object sender, EventArgs e)
            {
                if (!mess.Visible && !aboutBox.Visible)
                {
                    CheckAboutBoxOpen();
                    modeSet.ActivateProfileC_MMORPGb();
                    PostionModeLabels();
                }
            }

            #endregion

            #region about

            //********************* about button *********************

            private void buttonAbout_Click(object sender, EventArgs e)
            {

                if (aboutBox.Visible)
                    aboutBox.Hide();
                else
                    aboutBox.Show();
            }

            #endregion

            #region start stop

            ////********************* start stop button *********************
            //private void buttonStartStop_MouseEnter(object sender, EventArgs e)
            //{
            //    if (!dwellStarted)
            //    {
            //        start = DateTime.Now;
            //        dwellStarted = true;
            //        dwellThread = new Thread(new ThreadStart(CheckDwell));
            //        dwellThread.Start();
            //    }
            //    else
            //    {
            //    }
            //    CursorFactory.Create(Application.StartupPath + CursorSnapClutch.Pulse());
            //    //CursorFactory.CreatePulseCursor();
            //}

            //private void buttonStartStop_MouseLeave(object sender, EventArgs e)
            //{
            //    if (dwellStarted)
            //    {
            //        dwellThread.Abort();
            //        dwellStarted = false;
            //    }
            //    CursorFactory.Create(Application.StartupPath + CursorSnapClutch.BlankCursor());
            //    //CursorFactory.CreateBlankCursor();
            //}

            private void buttonStartStop_Click(object sender, EventArgs e)
            {
                StartSnapClutchModes();
                //if (!mess.Visible && !aboutBox.Visible)
                //{
                //    CheckAboutBoxOpen();
                //    // start snap clutch
                //    if (!snapClutchOn)
                //    {
                //        gazeOverlay.SetOverlayStatus(false);

                //        snapClutchOn = true;

                //        buttonStartStop.BackColor = Color.Red;

                //        // pass on settings
                //        gazeEngine.SetDwellClickTime(configuration.DwellClickDelay);

                //        // minimise application... eventually minimise this to icon bar and not task bar!
                //        this.WindowState = FormWindowState.Minimized;
                //        gazeEngine.SetOffScreenBuffer(configuration.OffScreenGlanceDist);
                //        // set default mode collection mode
                //        scModule.StartSnapClutch();

                //        Shell32.ShellClass shell = new Shell32.ShellClass();
                //        shell.MinimizeAll();

                //        if (dwellStarted)
                //        {
                //            dwellThread.Abort();
                //            dwellStarted = false;
                //        }
                //    }
                //}
            }

            #endregion

            #region exit

            //********************* exit button *********************
            private void buttonExit_Click(object sender, EventArgs e)
            {
                if (!mess.Visible && !aboutBox.Visible)
                {
                    CheckAboutBoxOpen();
                    EyeTrackingStop();
                    keyboardHook.UninstallHook();
                    keyboardHook.Dispose();
                    CursorFactory.Create(Application.StartupPath + CursorSnapClutch.DefaultCursor());
                    //CursorFactory.CreateDefaultCursor();
                    this.Dispose();
                }
            }

            private void buttonExit_MouseEnter(object sender, EventArgs e)
            {
                if (!dwellStarted)
                {
                    start = DateTime.Now;
                    dwellStarted = true;
                    dwellThread = new Thread(new ThreadStart(CheckDwell));
                    dwellThread.Start();
                }
                else
                {
                }

                if (!mess.Visible && !aboutBox.Visible)
                    CursorFactory.Create(Application.StartupPath + CursorSnapClutch.Pulse());
                //CursorFactory.CreatePulseCursor();
            }

            private void buttonExit_MouseLeave(object sender, EventArgs e)
            {
                if (dwellStarted)
                {
                    dwellThread.Abort();
                    dwellStarted = false;
                }
                if (!this.Disposing)
                    CursorFactory.Create(Application.StartupPath + CursorSnapClutch.BlankCursor());
                //CursorFactory.CreateBlankCursor();
            }

            #endregion

            #region dwell minus

            //********************* dwell time minus button *********************
            private void buttonDwellMinus_Click(object sender, EventArgs e)
            {
                if (!mess.Visible && !aboutBox.Visible)
                {
                    CheckAboutBoxOpen();
                    configuration.DwellClickDelay -= dwellIncrement;
                    labelDwellTimeValue.Text = configuration.DwellClickDelay.ToString() + " m/s";
                    buttonDwellMinus.BackColor = Color.FromArgb(128, 255, 128);
                    CursorFactory.Create(Application.StartupPath + CursorSnapClutch.BlankCursor());
                    //CursorFactory.CreateBlankCursor();
                }
            }

            private void buttonDwellMinus_MouseLeave(object sender, EventArgs e)
            {
                if (dwellStarted)
                {
                    dwellThread.Abort();
                    dwellStarted = false;
                }
                buttonDwellMinus.BackColor = Color.FromArgb(192, 192, 255);
                CursorFactory.Create(Application.StartupPath + CursorSnapClutch.BlankCursor());
            }

            #endregion

            #region dwell plus

            //********************* dwell time plus button *********************
            private void buttonDwellPlus_Click(object sender, EventArgs e)
            {
                if (!mess.Visible && !aboutBox.Visible)
                {
                    CheckAboutBoxOpen();
                    configuration.DwellClickDelay += dwellIncrement;
                    labelDwellTimeValue.Text = configuration.DwellClickDelay.ToString() + " m/s";
                    buttonDwellPlus.BackColor = Color.FromArgb(128, 255, 128);
                    CursorFactory.Create(Application.StartupPath + CursorSnapClutch.BlankCursor());
                    //CursorFactory.CreateBlankCursor();
                }
            }

            private void buttonDwellPlus_MouseLeave(object sender, EventArgs e)
            {
                if (dwellStarted)
                {
                    dwellThread.Abort();
                    dwellStarted = false;
                }
                buttonDwellPlus.BackColor = Color.FromArgb(192, 192, 255);
                CursorFactory.Create(Application.StartupPath + CursorSnapClutch.BlankCursor());
            }

            #endregion

            #region g9

            //********************* g9 text button *********************
            private void buttonTextOnOff_Click(object sender, EventArgs e)
            {
                if (!mess.Visible && !aboutBox.Visible)
                {
                    CheckAboutBoxOpen();
                    if (useG9Text)
                    {
                        useG9Text = false;

                        buttonTextOnOff.BackColor = Color.FromArgb(192, 192, 255);
                        buttonTextOnOff.Text = "G9 Text Off";

                    }
                    else
                    {
                        useG9Text = true;
                        buttonTextOnOff.BackColor = Color.FromArgb(128, 255, 128);
                        buttonTextOnOff.Text = "G9 Text On";
                    }
                    CursorFactory.Create(Application.StartupPath + CursorSnapClutch.BlankCursor());
                    //CursorFactory.CreateBlankCursor();
                }
            }


            #endregion

            #region wow

            //********************* wow button *********************
            //private void buttonWow_MouseEnter(object sender, EventArgs e)
            //{
            //    if (!dwellStarted)
            //    {
            //        start = DateTime.Now;
            //        dwellStarted = true;
            //        dwellThread = new Thread(new ThreadStart(CheckDwell));
            //        dwellThread.Start();
            //    }
            //    else
            //    {
            //    }

            //    CursorFactory.Create(Application.StartupPath + CursorSnapClutch.Pulse());
            //    //CursorFactory.CreatePulseCursor();
            //    //this.Cursor = CursorFactory.Create(Application.StartupPath + CursorSnapClutch.Pulse());
            //}

            //private void buttonWow_MouseLeave(object sender, EventArgs e)
            //{
            //    if (dwellStarted)
            //    {
            //        dwellThread.Abort();
            //        dwellStarted = false;
            //    }

            //    CursorFactory.Create(Application.StartupPath + CursorSnapClutch.BlankCursor());
            //    //CursorFactory.CreateBlankCursor();
            //}

            // part of the application launcher and is so disabled for now
            //private void buttonWow_Click(object sender, EventArgs e)
            //{
            //    CheckAboutBoxOpen();
            //    bool WoWWindowOpen = false;

            //    IntPtr hWnd = FindWindow("World of Warcraft", "World of Warcraft");
            //    if (!hWnd.Equals(IntPtr.Zero))
            //    {
            //        // SW_SHOWMAXIMIZED to maximize the window
            //        // SW_SHOWMINIMIZED to minimize the window
            //        // SW_SHOWNORMAL to make the window be normal size
            //        ShowWindowAsync(hWnd, SW_SHOWMAXIMIZED);
            //        SetWindowPos(hWnd, HWND_TOP, 0, 0, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, SWP_SHOWWINDOW);
            //        WoWWindowOpen = true;
            //    }

            //    if (WoWWindowOpen)
            //    {
            //        // start snap clutch
            //        if (!snapClutchOn)
            //        {
            //            gazeOverlay.SetOverlayStatus(false);

            //            snapClutchOn = true;

            //            buttonStartStop.BackColor = Color.Red;

            //            // pass on settings
            //            gazeEngine.SetDwellClickTime(configuration.DwellClickDelay);

            //            // minimise application... eventually minimise this to icon bar and not task bar!
            //            this.WindowState = FormWindowState.Minimized;
            //            gazeEngine.SetOffScreenBuffer(configuration.OffScreenGlanceDist);

            //            scModule.StartSnapClutch();
            //        }
            //        SetWindowPos(hWnd, HWND_TOP, 0, 0, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, SWP_SHOWWINDOW);
            //    }
            //}

            //#endregion

            //#region sl

            ////********************* sl button *********************
            ////private void buttonSL_MouseEnter(object sender, EventArgs e)
            ////{
            ////    if (!dwellStarted)
            ////    {
            ////        start = DateTime.Now;
            ////        dwellStarted = true;
            ////        dwellThread = new Thread(new ThreadStart(CheckDwell));
            ////        dwellThread.Start();
            ////    }
            ////    else
            ////    {
            ////    }

            ////    CursorFactory.Create(Application.StartupPath + CursorSnapClutch.Pulse());
            ////    //CursorFactory.CreatePulseCursor();
            ////    //this.Cursor = CursorFactory.Create(Application.StartupPath + CursorSnapClutch.Pulse());
            ////}

            ////private void buttonSL_MouseLeave(object sender, EventArgs e)
            ////{
            ////    if (dwellStarted)
            ////    {
            ////        dwellThread.Abort();
            ////        dwellStarted = false;
            ////    }

            ////    CursorFactory.Create(Application.StartupPath + CursorSnapClutch.BlankCursor());
            ////    //CursorFactory.CreateBlankCursor();
            ////}

            //private void buttonSL_Click(object sender, EventArgs e)
            //{
            //    CheckAboutBoxOpen();
            //    bool SLWindowOpen = false;

            //    IntPtr hWnd = FindWindow("Second Life", "Second Life");
            //    if (!hWnd.Equals(IntPtr.Zero))
            //    {
            //        // SW_SHOWMAXIMIZED to maximize the window
            //        // SW_SHOWMINIMIZED to minimize the window
            //        // SW_SHOWNORMAL to make the window be normal size
            //        ShowWindowAsync(hWnd, SW_SHOWMAXIMIZED);
            //        SetWindowPos(hWnd, HWND_TOP, 0, 0, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, SWP_SHOWWINDOW);
            //        SLWindowOpen = true;
            //    }

            //    if (SLWindowOpen)
            //    {
            //        // start snap clutch
            //        if (!snapClutchOn)
            //        {
            //            gazeOverlay.SetOverlayStatus(false);

            //            snapClutchOn = true;

            //            buttonStartStop.BackColor = Color.Red;

            //            // pass on settings
            //            gazeEngine.SetDwellClickTime(configuration.DwellClickDelay);

            //            // minimise application... eventually minimise this to icon bar and not task bar!
            //            this.WindowState = FormWindowState.Minimized;
            //            gazeEngine.SetOffScreenBuffer(configuration.OffScreenGlanceDist);
            //            // set default mode collection mode
            //            modeSet.ActivateMMORPGModeVariantCollection2Active();
            //            scModule.StartSnapClutch();
            //        }
            //        SetWindowPos(hWnd, HWND_TOP, 0, 0, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, SWP_SHOWWINDOW);
            //    }
            //}

            //#endregion


            //#region solitaire

            ////********************* sl button *********************
            ////private void buttonSolitaire_MouseEnter(object sender, EventArgs e)
            ////{
            ////    if (!dwellStarted)
            ////    {
            ////        start = DateTime.Now;
            ////        dwellStarted = true;
            ////        dwellThread = new Thread(new ThreadStart(CheckDwell));
            ////        dwellThread.Start();
            ////    }
            ////    else
            ////    {
            ////    }

            ////    CursorFactory.Create(Application.StartupPath + CursorSnapClutch.Pulse());
            ////    //CursorFactory.CreatePulseCursor();
            ////    //this.Cursor = CursorFactory.Create(Application.StartupPath + CursorSnapClutch.Pulse());
            ////}

            ////private void buttonSolitaire_MouseLeave(object sender, EventArgs e)
            ////{

            ////}

            //private void buttonSolitaire_Click(object sender, EventArgs e)
            //{
            //    CheckAboutBoxOpen();

            //    bool solWindowOpen = false;

            //    //is the window open
            //    Process slProcess = new Process();
            //    Process[] pArray = System.Diagnostics.Process.GetProcesses();
            //    foreach (Process proc in pArray)
            //    {
            //        //Console.WriteLine(proc.ToString());
            //        if (proc.ToString().Contains("Sol"))
            //        {
            //            slProcess = proc;
            //            solWindowOpen = true;
            //        }

            //    }

 
            //    if(solWindowOpen)
            //    {
            //        SetWindowPos(slProcess.MainWindowHandle, HWND_TOP, 0, 0, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, SWP_SHOWWINDOW);
                    
            //        // start snap clutch
            //        if (!snapClutchOn)
            //        {
            //            gazeOverlay.SetOverlayStatus(false);

            //            snapClutchOn = true;

            //            buttonStartStop.BackColor = Color.Red;

            //            // pass on settings
            //            gazeEngine.SetDwellClickTime(configuration.DwellClickDelay);

            //            // minimise application... eventually minimise this to icon bar and not task bar!
            //            this.WindowState = FormWindowState.Minimized;
            //            gazeEngine.SetOffScreenBuffer(configuration.OffScreenGlanceDist);
            //            // set default mode collection mode
            //            modeSet.ActivateWindowsModeCollection();
            //            scModule.StartSnapClutch();
            //            scModule.SetGlance(2);
            //        }
            //    }
            //    else
            //    {
            //        System.Diagnostics.Process.Start(@"Sol");

            //        Process slProcess2 = new Process();
            //        Process[] pArray2 = System.Diagnostics.Process.GetProcesses();
            //        foreach (Process proc in pArray2)
            //        {
            //            //Console.WriteLine(proc.ToString());
            //            if (proc.ToString().Contains("Sol"))
            //            {
            //                slProcess2 = proc;
            //                solWindowOpen = true;
            //            }

            //        }
            //        SetWindowPos(slProcess2.MainWindowHandle, HWND_TOP, 0, 0, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, SWP_SHOWWINDOW);
            //        // start snap clutch
            //        if (!snapClutchOn)
            //        {
            //            gazeOverlay.SetOverlayStatus(false);

            //            snapClutchOn = true;

            //            buttonStartStop.BackColor = Color.Red;

            //            // pass on settings
            //            gazeEngine.SetDwellClickTime(configuration.DwellClickDelay);

            //            // minimise application... eventually minimise this to icon bar and not task bar!
            //            this.WindowState = FormWindowState.Minimized;
            //            gazeEngine.SetOffScreenBuffer(configuration.OffScreenGlanceDist);
            //            // set default mode collection mode
            //            modeSet.ActivateWindowsModeCollection();
            //            scModule.StartSnapClutch();
            //            scModule.SetGlance(2);
            //        }
            //    }





            //}

            #endregion

            #region recalibrate

            private void buttonRecalibrate_Click(object sender, EventArgs e)
            {
                if (!mess.Visible && !aboutBox.Visible)
                {
                    CheckAboutBoxOpen();


                    switch (eyeTrackerType)
                    {
                        case 0:
                            //mouse
                            break;
                        case 1:
                            //itu gazetracker
                            break;
                        case 2:
                            //tobii                
                            //gazeOverlay.SetOverlayStatus(false);
                            try
                            {
                                Calibrate(false);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            break;
                        case 3:
                            //smi
                            break;
                    }


                }


            }

            #endregion

            #region switch

            private void buttonSwitch_Click(object sender, EventArgs e)
            {
                if (!mess.Visible && !aboutBox.Visible)
                {
                    if (!useSwitch)
                    {
                        useSwitch = true;
                        //buttonSwitch.BackColor = Color.Green;
                    }
                    else
                    {
                        useSwitch = false;
                        //buttonSwitch.BackColor = Color.Gray;
                    }
                }
            }
            #endregion

        #endregion

        #region Other stuff

            // event handler for system key events
        private void keyboardHook_KeyboardEvent(KeyboardEvents kEvent, Keys key)
        {
            
            // F10 key to stop eye tracker and give mouse control
            // TO DO:
            if ((key.Equals(Keys.F10)) && (kEvent.ToString().Equals("KeyDown")))
            {
                if (gazeOn)
                {
                    //EyeTrackingStop();
                }
                else
                {
                    //EyeTrackingStart();
                }
            }

            // Space bar for AAC simulation
            if ((key.Equals(Keys.F12)) && (kEvent.ToString().Equals("KeyDown")) && !switchDown)
            {
                // check for the correct mode
                if (useSwitch)
                {
                    switchDown = true;

                    dwellPos = new Point(Convert.ToInt32(Math.Round(Screen.PrimaryScreen.Bounds.Width * Convert.ToDouble(gazeEngine.GetMouseEmulationX())))
                        , Convert.ToInt32(Math.Round(Screen.PrimaryScreen.Bounds.Height * Convert.ToDouble(gazeEngine.GetMouseEmulationY()))));

                    if (!snapClutchOn)
                    {
                        // this is so we can also use the switch in the SC interface
                        SnapClutchSounds.Click2();
                        MouseEvent.LeftDown();
                    }
                    else
                    {
                        scModule.SetDwellPos(dwellPos);
                        scModule.Dwell(true, true);
                    }
                }
            }

            // Space bar for AAC simulation
            if ((key.Equals(Keys.F12)) && (kEvent.ToString().Equals("KeyUp")) && switchDown)
            {
                // check for the correct mode
                if (useSwitch)
                {
                    switchDown = false;

                    dwellPos = new Point(Convert.ToInt32(Math.Round(Screen.PrimaryScreen.Bounds.Width * Convert.ToDouble(gazeEngine.GetMouseEmulationX())))
                          , Convert.ToInt32(Math.Round(Screen.PrimaryScreen.Bounds.Height * Convert.ToDouble(gazeEngine.GetMouseEmulationY()))));

                    if (!snapClutchOn)
                    {
                        // this is so we can also use the switch in the SC interface
                        SnapClutchSounds.Click2();
                        MouseEvent.LeftUp();
                    }
                    else
                    {
                        scModule.SetDwellPos(dwellPos);
                        scModule.Dwell(true, false);
                    }
                }
            }

        }


        #endregion





        //application switching is disabled in this release
        #region application switching


        private void UpdateGamesList()
        {
            CompatibleGame thisGame = modeSet.GetGameList()[currentGame];
            
            // update game labels in gui
            labelGame1.Text = thisGame.GetName();
            labelGame1.Left = rectangleShapeGame1.Left + (rectangleShapeGame1.Width / 2) - (labelGame1.Width / 2);
            labelGame1.Top = rectangleShapeGame1.Top + (rectangleShapeGame1.Height / 2) - (labelGame1.Height / 2);
        }

        private void buttonGameUp_Click(object sender, EventArgs e)
        {
            if (!mess.Visible && !aboutBox.Visible)
            {
                currentGame++;
                if (currentGame == modeSet.GetGameList().Count)
                    currentGame = 0;

                modeSet.ActivateGameProfile(currentGame, 1);
                buttonModeWindows.BackColor = Color.FromArgb(192, 192, 255);
                buttonModeMMORPG.BackColor = Color.FromArgb(192, 192, 255);
                buttonModeMMORPGVariant.BackColor = Color.FromArgb(192, 192, 255);
                button1B.BackColor = Color.FromArgb(192, 192, 255);
                button1A.BackColor = Color.FromArgb(128, 255, 128);
                PostionModeLabels();
                UpdateGamesList();
            }
        }

        private void buttonGameDown_Click(object sender, EventArgs e)
        {
            if (!mess.Visible && !aboutBox.Visible)
            {
                currentGame--;
                if (currentGame < 0)
                    currentGame = modeSet.GetGameList().Count - 1;

                modeSet.ActivateGameProfile(currentGame, 1);
                buttonModeWindows.BackColor = Color.FromArgb(192, 192, 255);
                buttonModeMMORPG.BackColor = Color.FromArgb(192, 192, 255);
                buttonModeMMORPGVariant.BackColor = Color.FromArgb(192, 192, 255);
                button1B.BackColor = Color.FromArgb(192, 192, 255);
                button1A.BackColor = Color.FromArgb(128, 255, 128);
                PostionModeLabels();
                UpdateGamesList();
            }
        }

        private void button1A_Click(object sender, EventArgs e)
        {
            if (!mess.Visible && !aboutBox.Visible)
            {
                modeSet.ActivateGameProfile(currentGame, 1);

                buttonModeWindows.BackColor = Color.FromArgb(192, 192, 255);
                buttonModeMMORPG.BackColor = Color.FromArgb(192, 192, 255);
                buttonModeMMORPGVariant.BackColor = Color.FromArgb(192, 192, 255);
                button1B.BackColor = Color.FromArgb(192, 192, 255);
                button1A.BackColor = Color.FromArgb(128, 255, 128);

                PostionModeLabels();
            }
        }

        private void button1B_Click(object sender, EventArgs e)
        {
            if (!mess.Visible && !aboutBox.Visible)
            {
                modeSet.ActivateGameProfile(currentGame, 2);

                buttonModeWindows.BackColor = Color.FromArgb(192, 192, 255);
                buttonModeMMORPG.BackColor = Color.FromArgb(192, 192, 255);
                buttonModeMMORPGVariant.BackColor = Color.FromArgb(192, 192, 255);
                button1B.BackColor = Color.FromArgb(128, 255, 128);
                button1A.BackColor = Color.FromArgb(192, 192, 255);

                PostionModeLabels();
            }
        }

        private void button1Go_Click(object sender, EventArgs e)
        {
            if (!mess.Visible && !aboutBox.Visible)
            {
                // check game is running
                // if not then display warning box saying change to windows mode and start game
                // if is then start snap clutch and swith to game
                try
                {
                    StartSnapClutchModes();
                    gameProcess = new Process();
                    Process[] pArray = System.Diagnostics.Process.GetProcesses();
                    foreach (Process proc in pArray)
                    {
                        if (proc.MainWindowTitle.Contains(modeSet.GetGameList()[currentGame].GetName()))
                            gameProcess = proc;

                    }

                    //IntPtr hWnd = FindWindow(modeSet.GetGameList()[currentGame].GetName(), modeSet.GetGameList()[currentGame].GetName());
                    if (!gameProcess.MainWindowHandle.Equals(IntPtr.Zero))
                    {
                        // SW_SHOWMAXIMIZED to maximize the window
                        // SW_SHOWMINIMIZED to minimize the window
                        // SW_SHOWNORMAL to make the window be normal size
                        ShowWindowAsync(gameProcess.MainWindowHandle, SW_SHOWMAXIMIZED);
                        SetWindowPos(gameProcess.MainWindowHandle, HWND_TOP, 0, 0, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, SWP_SHOWWINDOW);
                    }
                }
                catch
                {
                    StopSnapClutchModes();
                    mess = new SCMessage(modeSet.GetGameList()[currentGame].GetName());
                    mess.Show();
                }

            }
                
                            
        }


        #endregion

        private void StartSnapClutchModes()
        {
            if (!mess.Visible && !aboutBox.Visible)
            {
                // start snap clutch
                if (!snapClutchOn)
                {
                    gazeOverlay.SetOverlayStatus(false);

                    snapClutchOn = true;

                    buttonStartStop.BackColor = Color.Red;
                    button1Go.BackColor = Color.Red;

                    // pass on settings
                    gazeEngine.SetDwellClickTime(configuration.DwellClickDelay);

                    // minimise application... eventually minimise this to icon bar and not task bar!
                    this.WindowState = FormWindowState.Minimized;
                    gazeEngine.SetOffScreenBuffer(configuration.OffScreenGlanceDist);
                    // set default mode collection mode
                    scModule.StartSnapClutch();

                    Shell32.ShellClass shell = new Shell32.ShellClass();
                    shell.MinimizeAll();

                    if (dwellStarted)
                    {
                        dwellThread.Abort();
                        dwellStarted = false;
                    }
                }
            }
        }

        private void StopSnapClutchModes()
        {
            if (snapClutchOn)
            {
                SnapClutchSounds.OffScreenDwell();
                //CursorFactory.CreateBlankCursor();
                CursorFactory.Create(Application.StartupPath + CursorSnapClutch.BlankCursor());

                // stop snap clutch and show interface
                snapClutchOn = false;
                scModule.SetG9Active(false, useSwitch, useOverlay);

                if (eyeTrackerType == 4)
                    gazeOverlay.SetOverlayStatus(false);
                else
                    gazeOverlay.SetOverlayStatus(true); // change to true later when i am not using the mouse!!!


                buttonStartStop.BackColor = Color.FromArgb(192, 192, 255);
                button1Go.BackColor = Color.FromArgb(192, 192, 255);

                //SetWindowPos(gameProcess.MainWindowHandle, HWND_NOTOPMOST, 0, 0, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, SWP_NOACTIVATE);


                this.Visible = true;
                this.WindowState = FormWindowState.Maximized;
                this.Activate();
                scModule.StopSnapClutch();
            }
        }

    }
}
