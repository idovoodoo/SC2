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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TetComp;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using SnapClutch.Config;
using SnapClutch.SCTools;
using SnapClutch.Modes;
using Kennedy.ManagedHooks;
using SpeechLib;
using System.IO;

namespace SnapClutch
{
    public partial class Start : Form
    {
        #region Declarations

        // import api dll for getting cursor pos
        [DllImport("user32.dll")]
        static extern bool GetCursorPos(ref Point pt);

        private enum EyeTracker { Mouse = 0, ITU = 1, Tobii = 2, SMI = 3, Dev = 4 };
        private int eyeTrackerType;
        private string configFileName = "configData.xml";
        private Configuration configuration = new Configuration();

        private ITetTrackStatus tetTrackStatus;
        private ITetCalibPlot tetCalibPlot;
        private TetCalibProc tetCalibProc;
        private ITetClient tetClient;
        private bool snapClutchOn = false;
        private TetServiceBrowser serviceBrowser;
        private List<TetServiceEntryWrapper> services = null;
        private GazeEngine gazeEngine;
        private SCModule scModule;
        private ModeCollection activeModeCollection;
        private List<Mode> myModeObjectList;
        private Point mouseEmulationPos;
        private Point mousePosition = new Point(0, 0);
        private float x, y;
        private SimpleStatus leftSimpleStatus, rightSimpleStatus;
        private KeyboardHook keyboardHook;
        private bool keyboardHooked = false;
        private bool useG9 = false;
        private bool G9On = false;
        private bool useSpeech = false;
        private About aboutForm = new About();
        private string userName = "";
        private StreamWriter sw;
        private StreamReader sr;

        //diagnostic test 
        SnapClutch.Diagnostic.Zones zones;
        SnapClutch.Diagnostic.SemiTranspZone tl, t, tr, l, c, r, bl, b, br;
        private bool showZones = true;
        private bool gameOn = false;
        private bool wKey = false;
        private bool aKey = false;
        private bool dKey = false;
        private bool sKey = false;
        private FixationEngine myFixationEngine;
        private int gazeTimeStamp;


        private Point centrePoint = new Point((Screen.PrimaryScreen.WorkingArea.Width / 2), Screen.PrimaryScreen.WorkingArea.Height / 2);
        private int speedFactor = 7;
        private bool rightMouse = false;
        private bool isTurning = false;
        private bool usingGestures = false;
        private Gesture_Interface.MainForm gestureInterface = new Gesture_Interface.MainForm();

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public Start()
        {           
            InitializeComponent();
            GetConfiguration();
            CheckForEyeTracker();
            labelTitle.Text = "Snap Clutch v" + Assembly.GetExecutingAssembly().GetName().Version.ToString();

            // in case we lost the cursor previously.. then running the app will give us it back!
            CursorFactory.Create(Application.StartupPath + CursorSnapClutch.DefaultCursor());

            //setup up the gazeengine
            gazeEngine = new GazeEngine(configuration.BufferSize, configuration.PixelRange, configuration.CursorUpdate, 75,
                configuration.DwellClickDelay, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            gazeEngine.DwellEvent += new DwellEventHandler(gazeEngine_DwellEvent);
            gazeEngine.OffScreenGlanceEvent += new OffScreenGlanceEventHandler(gazeEngine_OffScreenGlanceEvent);
            gazeEngine.DwellBottomEvent += new DwellBottomEventHandler(gazeEngine_DwellBottomEvent);
            gazeEngine.DwellLeftEvent += new DwellLeftEventHandler(gazeEngine_DwellLeftEvent);
            gazeEngine.DwellRightEvent += new DwellRightEventHandler(gazeEngine_DwellRightEvent);
            gazeEngine.DwellTopEvent += new DwellTopEventHandler(gazeEngine_DwellTopEvent);

            scModule = new SCModule(configuration);
            activeModeCollection = scModule.GetActiveModeCollection();
            myModeObjectList = scModule.GetModeObjects();

            //get mode information and populate comboboxes
            foreach (Mode m in scModule.GetModeList())
            {
                comboBoxTop.Items.Add(m.GetModeName());
                comboBoxBottom.Items.Add(m.GetModeName());
                comboBoxLeft.Items.Add(m.GetModeName());
                comboBoxRight.Items.Add(m.GetModeName());
            }
            //set default modes in comboboxes
            
            SetComboBoxes();

            mouseEmulationPos = new Point();

            leftSimpleStatus = new SimpleStatus();
            rightSimpleStatus = new SimpleStatus();
            PositionSimpleStatus();
            HookKeyboard();

            //diagnostic test stuff
            tl = new SnapClutch.Diagnostic.SemiTranspZone();
            t = new SnapClutch.Diagnostic.SemiTranspZone();
            tr = new SnapClutch.Diagnostic.SemiTranspZone();
            l = new SnapClutch.Diagnostic.SemiTranspZone();
            c = new SnapClutch.Diagnostic.SemiTranspZone();
            r = new SnapClutch.Diagnostic.SemiTranspZone();
            bl = new SnapClutch.Diagnostic.SemiTranspZone();
            b = new SnapClutch.Diagnostic.SemiTranspZone();
            br = new SnapClutch.Diagnostic.SemiTranspZone();
            myFixationEngine = new FixationEngine();
        }

        private void SetComboBoxes()
        {
            comboBoxTop.SelectedItem = activeModeCollection.GetModeTop().GetModeName();
            comboBoxRight.SelectedItem = activeModeCollection.GetModeRight().GetModeName();
            comboBoxBottom.SelectedItem = activeModeCollection.GetModeBottom().GetModeName();
            comboBoxLeft.SelectedItem = activeModeCollection.GetModeLeft().GetModeName();
        }

        #region Others

        /// <summary>
        /// Update the messages textbox with information on the selected modes
        /// </summary>
        private void UpdateMessages()
        {
            richTextBox.Clear();

            // check listboxes for certain mode notes
            if (comboBoxTop.Text.Contains("Mouse") || comboBoxLeft.Text.Contains("Mouse")
                || comboBoxRight.Text.Contains("Mouse") || comboBoxBottom.Text.Contains("Mouse"))
            {
                richTextBox.AppendText("Mouse MMO modes are experimental!\n");
            }
            if (comboBoxTop.Text.Contains("Strings") || comboBoxLeft.Text.Contains("Strings")
                || comboBoxRight.Text.Contains("Strings") || comboBoxBottom.Text.Contains("Strings"))
            {
                richTextBox.AppendText("Strings on Speed mode is experimental!\n");
                richTextBox.AppendText("Resolution must be 1600 x 1200\n");
                richTextBox.AppendText("Set Strings on Speed to use keys A, B, C, D and E\n");
            }

            if (richTextBox.Text.Equals(""))
                richTextBox.AppendText("No Messages!");
        }


        /// <summary>
        /// Start the Eye Tracker
        /// </summary>
        private void StartEyeTracker()
        {
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
                    // register all the tobii events

                    // Set up the calibration procedure object and it's events
                    tetCalibProc = new TetCalibProcClass();
                    _ITetCalibProcEvents_Event tetCalibProcEvents = (_ITetCalibProcEvents_Event)tetCalibProc;
                    tetCalibProcEvents.OnCalibrationEnd += new _ITetCalibProcEvents_OnCalibrationEndEventHandler(tetCalibProcEvents_OnCalibrationEnd);
                    tetCalibProcEvents.OnKeyDown += new _ITetCalibProcEvents_OnKeyDownEventHandler(tetCalibProcEvents_OnKeyDown);
                    
                    // Set up the TET client object and it's events
                    tetClient = new TetClientClass();
                    _ITetClientEvents_Event tetClientEvents = (_ITetClientEvents_Event)tetClient;
                    tetClientEvents.OnTrackingStarted += new _ITetClientEvents_OnTrackingStartedEventHandler(tetClientEvents_OnTrackingStarted);
                    tetClientEvents.OnTrackingStopped += new _ITetClientEvents_OnTrackingStoppedEventHandler(tetClientEvents_OnTrackingStopped);
                    tetClientEvents.OnGazeData += new _ITetClientEvents_OnGazeDataEventHandler(tetClientEvents_OnGazeData);

                    //stop track status
                    try
                    {
                        if (tetTrackStatus.IsTracking)
                            tetTrackStatus.Stop();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    //start eye tracking data
                    try
                    {


                        // Connect to the TET server if necessary
                        if (!tetClient.IsConnected)
                            tetClient.Connect(GetConnectionString(), (int)TetConstants.TetConstants_DefaultServerPort, TetSynchronizationMode.TetSynchronizationMode_Local);

                        // Start tracking gaze data
                        tetClient.StartTracking();
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
                    configuration.OffScreenGlanceDist = -1;
                    gazeEngine.SetOffScreenBuffer(-1);
                    break;

            }

            if (usingGestures)
                gestureInterface.ShowZones(true);
        }

        /// <summary>
        /// Shuts down all eye trackers
        /// </summary>
        private void StopEyeTrackers()
        {
            CursorFactory.Create(Application.StartupPath + CursorSnapClutch.DefaultCursor());

            switch (eyeTrackerType)
            {
                case 0:
                    //mouse
                    break;
                case 1:
                    //itu gazetracker
                    //ituClient.Stop();
                    break;
                case 2:
                    //tobii
                    //stop track status control
                    try
                    {
                        if (tetTrackStatus.IsTracking)
                            tetTrackStatus.Stop();
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

            if (usingGestures)
                gestureInterface.ShowZones(false);
        }


        /// <summary>
        /// Timer to handle all of the gaze data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerGazeData_Tick(object sender, EventArgs e)
        {
            //FixationDataPoint fixData = myFixationEngine.testPoint(x * Screen.PrimaryScreen.Bounds.Width, y * Screen.PrimaryScreen.Bounds.Height, gazeTimeStamp);

            //if (fixData == null)
            //{
            //    //Console.WriteLine("nothing");
            //}
            //else
            //{
            //    scModule.Fixation(fixData);
            //    //counter++;
            //    //Console.WriteLine(counter + " start: " + fixData.GetFixationStartTime() + " finish: " + fixData.GetFixationEndTime());
            //    //Console.WriteLine(counter + " " + (fixData.GetFixationEndTime().TotalMilliseconds - fixData.GetFixationStartTime().TotalMilliseconds).ToString());
            //    //we have fixation so lets send it to the test module!

            //}
            if (snapClutchOn)
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
                        //gazeEngine.AddPositionMovingAvg(x, y);
                        //mousePosition = ConvertToMouseCoords(gazeEngine.GetPositionXMovingAvg(), gazeEngine.GetPositionYMovingAvg());
                        //scModule.DataIn(mousePosition);
                        break;
                    case 2:
                        //tobii                
                        gazeEngine.AddPositionMovingAvg(x, y);
                        mousePosition = ConvertToMouseCoords(gazeEngine.GetPositionXMovingAvg(), gazeEngine.GetPositionYMovingAvg());
                        scModule.DataIn(mousePosition);

                        //Console.WriteLine(x + " " + y);
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

                if (gameOn)
                {


                    FixationDataPoint fixData = myFixationEngine.testPoint(x * Screen.PrimaryScreen.Bounds.Width, y * Screen.PrimaryScreen.Bounds.Height, gazeTimeStamp);
                    //FixationDataPoint fixData = myFixationEngine.testPoint(ConvertToMouseCoords(x, y).X, ConvertToMouseCoords(x, y).Y, gazeTimeStamp);

                    if (fixData == null)
                    {
                        //Console.WriteLine("nothing");
                    }
                    else
                    {
                        Fixation(fixData);
                        //Console.WriteLine("yay... fixation");
                    }
                }
            }

            //Console.WriteLine((float) x + " " + (float)y);


        }

        /// <summary>
        /// position the simple status windows
        /// </summary>
        private void PositionSimpleStatus()
        {
            leftSimpleStatus.Show();
            leftSimpleStatus.Height = 25;
            leftSimpleStatus.Width = 25;
            leftSimpleStatus.Left = 0;
            leftSimpleStatus.Top = Screen.PrimaryScreen.Bounds.Height - leftSimpleStatus.Height;
            leftSimpleStatus.TopMost = true;

            rightSimpleStatus.Show();
            rightSimpleStatus.Width = 25;
            rightSimpleStatus.Height = 25;
            rightSimpleStatus.Left = leftSimpleStatus.Right + 15;
            rightSimpleStatus.Top = leftSimpleStatus.Top;
            //rightSimpleStatus.Left = Screen.PrimaryScreen.Bounds.Width - rightSimpleStatus.Width;
            //rightSimpleStatus.Top = Screen.PrimaryScreen.Bounds.Height - rightSimpleStatus.Height;

            rightSimpleStatus.TopMost = true;

        }

        /// <summary>
        /// Convert floats to mouse coordinates
        /// </summary>
        /// <param name="argX"></param>
        /// <param name="argY"></param>
        /// <returns></returns>
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

  

        /// <summary>
        /// Hook the keyboard to capture key events
        /// </summary>
        private void HookKeyboard()
        {
            // Setup keyboard hook and event handler
            keyboardHook = new KeyboardHook();
            keyboardHook.KeyboardEvent += new KeyboardHook.KeyboardEventHandler(keyboardHook_KeyboardEvent);
            keyboardHook.InstallHook();
            keyboardHooked = true;
        }

        /// <summary>
        /// Event handler for catching keyboard events
        /// </summary>
        /// <param name="kEvent"></param>
        /// <param name="key"></param>
        private void keyboardHook_KeyboardEvent(KeyboardEvents kEvent, Keys key)
        {
            // F10 key to stop eye tracker and give mouse control
            if ((key.Equals(Keys.F10)) && (kEvent.ToString().Equals("KeyDown")))
            {
                if (snapClutchOn)
                {
                    snapClutchOn = false;
                    StopEyeTrackers();
                    this.WindowState = FormWindowState.Normal;
                    this.Show();
                    scModule.StopSnapClutch();
                }
                else
                {
                    snapClutchOn = true;
                    StartEyeTracker();
                    // minimise application... eventually minimise this to icon bar and not task bar!
                    this.WindowState = FormWindowState.Minimized;
                    // pass on settings
                    gazeEngine.SetDwellClickTime(configuration.DwellClickDelay);
                    gazeEngine.SetOffScreenBuffer(configuration.OffScreenGlanceDist);
                    // set default mode collection mode
                    scModule.StartSnapClutch();
                }


                if (gameOn)
                {
                  //  StopEyeTrackers();
                    //gameOn = false;
                    HideAllZones();
                }
                //else
                //{
                    //StartEyeTracker();
                    //gameOn = true;
                //}
            }


            // F5 key to show/hide overlays
            if ((key.Equals(Keys.F5)) && (kEvent.ToString().Equals("KeyDown")))
            {
                if (showZones)
                    HideAllZones();
                else
                    ShowZones();
            }
        }

        /// <summary>
        /// Gets configuration
        /// </summary>
        private void GetConfiguration()
        {
            try
            {
                configuration = (Configuration)XmlUtility.Deserialize(configuration.GetType(), configFileName);
            }
            catch
            {
                configuration = new Configuration();
            }

            //apply settings to textboxes?
            textBoxDwellTime.Text = configuration.DwellClickDelay.ToString();
        }

        #endregion


        #region GazeEngine Event Handlers

        private void gazeEngine_DwellTopEvent(object o, DwellTopEventArgs e)
        {
            if (snapClutchOn && useG9)
            {
                if(G9On)
                    scModule.SetG9Active(false, false, true);
                else
                    scModule.SetG9Active(true, false, true);
            }
        }

        void gazeEngine_DwellRightEvent(object o, DwellRightEventArgs e)
        {
            if (snapClutchOn)
            {
            }
        }

        void gazeEngine_DwellLeftEvent(object o, DwellLeftEventArgs e)
        {
            if (snapClutchOn)
            {
            }
        }

        void gazeEngine_DwellBottomEvent(object o, DwellBottomEventArgs e)
        {
            if (!gameOn)
            {
                if (snapClutchOn)
                {
                    snapClutchOn = false;
                    StopEyeTrackers();
                    this.WindowState = FormWindowState.Normal;
                    this.Show();
                    scModule.StopSnapClutch();
                }
            }
        }

        /// <summary>
        /// Event handler for glances
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        void gazeEngine_OffScreenGlanceEvent(object o, OffScreenGlanceEventArgs e)
        {
            if (snapClutchOn)
            {
                //Console.WriteLine(e.Glance.ToString());
                switch (e.Glance)
                {
                    case 1:
                        if (!comboBoxTop.Text.Equals("Blank"))
                            scModule.OffScreenGlance(e.Glance);
                        break;
                    case 2:
                        if (!comboBoxBottom.Text.Equals("Blank"))
                            scModule.OffScreenGlance(e.Glance);
                        break;
                    case 3:
                        if (!comboBoxLeft.Text.Equals("Blank"))
                            scModule.OffScreenGlance(e.Glance);
                        break;
                    case 4:
                        if (!comboBoxRight.Text.Equals("Blank"))
                            scModule.OffScreenGlance(e.Glance);
                        break;

                }
            }
        }

        /// <summary>
        /// Event handler for dwell
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        void gazeEngine_DwellEvent(object o, DwellEventArgs e)
        {

            if (snapClutchOn)
            {
                scModule.SetDwellPos(new Point(Convert.ToInt32(Math.Round(Screen.PrimaryScreen.Bounds.Width * Convert.ToDouble(gazeEngine.GetMouseEmulationX())))
                    , Convert.ToInt32(Math.Round(Screen.PrimaryScreen.Bounds.Height * Convert.ToDouble(gazeEngine.GetMouseEmulationY())))));
                scModule.Dwell(false, false);
            }
        }

        #endregion


        #region Eye Tracker Connection Checks

        /// <summary>
        /// Gets selected eye tracker from config file
        /// </summary>
        private void CheckForEyeTracker()
        {
            // first of all lets check with the config what eye tracker we should be using
            // are we using the mouse?
            if (configuration.EyeTracker == "Mouse")
            {
                eyeTrackerType = (int)EyeTracker.Mouse;
                RunMouseCheck();
                //voice.Speak("mouse", SpeechVoiceSpeakFlags.SVSFDefault);
            }
            // or are we using the itu gazetracker?
            else if (configuration.EyeTracker == "ITU")
            {
                eyeTrackerType = (int)EyeTracker.ITU;
                //RunITUCheck();
            }
            // or are we using a tobii?
            else if (configuration.EyeTracker == "Tobii")
            {
                eyeTrackerType = (int)EyeTracker.Tobii;
                RunTobiiCheck();

                //voice.Speak("Using Tobi eye tracker", SpeechVoiceSpeakFlags.SVSFDefault);
            }
            // or are we using an smi?
            else if (configuration.EyeTracker == "SMI")
            {
                eyeTrackerType = (int)EyeTracker.SMI;
                //RunSMICheck();
            }
            // or are we in development mode
            else if (configuration.EyeTracker == "Dev")
            {
                eyeTrackerType = (int)EyeTracker.Dev;
                RunDevCheck();
            }
        }


        /// <summary>
        /// Check to ensure mouse is working ok NYI!
        /// </summary>
        private void RunMouseCheck()
        {
            labelEyeTracker.Text = "Mouse";
        }

        /// <summary>
        /// Check to ensure dev mode is active NYI!
        /// </summary>
        private void RunDevCheck()
        {
            labelEyeTracker.Text = "Development Mode";
        }



        /// <summary>
        /// Check to ensure Tobii is working ok!
        /// </summary>
        private void RunTobiiCheck()
        {
            labelEyeTracker.Text = "Tobii";
            buttonContinue.Enabled = false;

            tetClient = new TetClientClass();
            services = new List<TetServiceEntryWrapper>();

            // Retreive underlying references to ActiveX controls
            tetTrackStatus = (ITetTrackStatus)axTetTrackStatus.GetOcx();
            tetCalibPlot = (ITetCalibPlot)axTetCalibPlot.GetOcx();          

            // Set up the calibration procedure object and it's events
            tetCalibProc = new TetCalibProcClass();
            _ITetCalibProcEvents_Event tetCalibProcEvents = (_ITetCalibProcEvents_Event)tetCalibProc;
            tetCalibProcEvents.OnCalibrationEnd += new _ITetCalibProcEvents_OnCalibrationEndEventHandler(tetCalibProcEvents_OnCalibrationEnd);
            tetCalibProcEvents.OnKeyDown += new _ITetCalibProcEvents_OnKeyDownEventHandler(tetCalibProcEvents_OnKeyDown);
            tetCalibPlot.AllowMouseInteraction = true;

            try
            {
                serviceBrowser = new TetServiceBrowserClass();
                serviceBrowser.OnServiceAdded += new _ITetServiceBrowserEvents_OnServiceAddedEventHandler(serviceBrowser_OnServiceAdded);
                serviceBrowser.OnServiceUpdated += new _ITetServiceBrowserEvents_OnServiceUpdatedEventHandler(serviceBrowser_OnServiceUpdated);
                serviceBrowser.OnServiceRemoved += new _ITetServiceBrowserEvents_OnServiceRemovedEventHandler(serviceBrowser_OnServiceRemoved);
                serviceBrowser.Start();
            }
            catch
            {
                MessageBox.Show("Tobii serviceBrowser couldn't be started");
            }

            //setup user calibration data
            sr = new StreamReader("users.dat");
            while(sr.Peek() >= 0)
            {
                comboBoxUsers.Items.Add(sr.ReadLine());
            }
            sr.Close();
            
        }

        #endregion


        #region Tobii

            #region Service tool to auto detect eye tracker

            private void UpdateEyetrackerCombo()
            {
                eyetrackers.Items.Clear();
                TetServiceEntryWrapper selected = eyetrackers.SelectedItem as TetServiceEntryWrapper;

                foreach (TetServiceEntryWrapper eyetracker in services)
                {
                    if (eyetracker.IsRunning)
                        eyetrackers.Items.Add(eyetracker);
                }

                if (eyetrackers.Items.Count == 0)
                {
                    UpdateButtons();
                    return;
                }

                if (selected != null)
                {
                    int index = eyetrackers.Items.IndexOf(selected);
                    if (index != -1)
                        eyetrackers.SelectedIndex = index;
                    else
                        eyetrackers.SelectedIndex = 0;
                }
                else
                {
                    eyetrackers.SelectedIndex = 0;
                }
                UpdateButtons();
            }

            private string GetConnectionString()
            {           
                return ((TetServiceEntryWrapper)eyetrackers.SelectedItem).Hostname;
            }

            private void serviceBrowser_OnServiceRemoved(string servicename)
            {
                services.Remove(new TetServiceEntryWrapper(servicename));
                UpdateEyetrackerCombo();
            }

            private void serviceBrowser_OnServiceUpdated(ref TetServiceEntry serviceEntry)
            {
                TetServiceEntryWrapper wrapper = new TetServiceEntryWrapper(serviceEntry);
                if (!services.Contains(wrapper))
                {
                    services.Add(wrapper);
                }
                else
                {
                    services.Remove(wrapper);
                    services.Add(wrapper);
                }
                UpdateEyetrackerCombo();
                UpdateCalibPlot();

            }

            private void serviceBrowser_OnServiceAdded(ref TetServiceEntry serviceEntry)
            {
                serviceBrowser_OnServiceUpdated(ref serviceEntry);
            }

            private void UpdateButtons()
            {
                trackStatusStartButton.Enabled = true;
                trackStatusStopButton.Enabled = true;
                calibrateButton.Enabled = true;
            }

            private void tetClientEvents_OnTrackingStarted()
            {
            }

            private void tetClientEvents_OnTrackingStopped(int hr)
            {
                if (hr != (int)TetHResults.ITF_S_OK) MessageBox.Show(string.Format("Error {0} occured while tracking.", hr), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            private void tetClientEvents_OnGazeData(ref TetGazeData gazeData)
            {
                //Console.WriteLine(gazeData.validity_lefteye.ToString());
                //gazeTimeStamp = gazeData.timestamp_microsec;
                gazeTimeStamp = gazeData.timestamp_microsec;
                try
                {
                    // Use data only if both left and right eye was found by the eye tracker
                    if ((gazeData.validity_lefteye == 0 && gazeData.validity_righteye == 0) && snapClutchOn)
                    {
                        // Let the x, y and distance be the right and left eye average
                        x = (gazeData.x_gazepos_lefteye + gazeData.x_gazepos_righteye) / 2;
                        y = (gazeData.y_gazepos_lefteye + gazeData.y_gazepos_righteye) / 2;

                        leftSimpleStatus.BackColor = Color.Green;
                        rightSimpleStatus.BackColor = Color.Green;
                    }
                    //check to see if we have a left wink
                    else if ((gazeData.validity_lefteye == 1 || (gazeData.validity_righteye == 4 && gazeData.validity_lefteye < 4)) && snapClutchOn)
                    {

                        x = gazeData.x_gazepos_lefteye;
                        y = gazeData.y_gazepos_lefteye;

                        leftSimpleStatus.BackColor = Color.Green;
                        rightSimpleStatus.BackColor = Color.Red;
                        //scModule.Blink(0);
                        //Console.WriteLine("Left wink!");
                    }
                    //check to see if we have a right wink
                    else if ((gazeData.validity_righteye == 1 || (gazeData.validity_lefteye == 4 && gazeData.validity_righteye < 4)) && snapClutchOn)
                    {
                        x = gazeData.x_gazepos_righteye;
                        y = gazeData.y_gazepos_righteye;

                        leftSimpleStatus.BackColor = Color.Red;
                        rightSimpleStatus.BackColor = Color.Green;
                        //scModule.Blink(1);
                        //Console.WriteLine("Right wink!");
                    }
                    else if ((gazeData.validity_lefteye == 4 && gazeData.validity_righteye == 4) && snapClutchOn)
                    {
                        x = (float)0.5;
                        y = (float)0.5;
                        //Console.WriteLine("lost eyes");

                        leftSimpleStatus.BackColor = Color.Red;
                        rightSimpleStatus.BackColor = Color.Red;
                    }
                }
                catch (Exception e)
                {
                    //string n = string.Format("{0:ddMMyyyy_hh-mm}", DateTime.Now);
                    //StreamWriter error = new StreamWriter("error mainform ongazedata " + n + ".txt");
                    //error.WriteLine(e.ToString());
                    //error.Close();

                }

                //Console.WriteLine(x + " " + y);

            }

            #endregion

            #region Calibration events

            private void tetCalibProcEvents_OnCalibrationEnd(int result)
            {
                // Calibration ended, hide the calibration window and update the calibration plot
                tetCalibProc.WindowVisible = false;
                UpdateCalibPlot();

                


            }

            private void tetCalibProcEvents_OnKeyDown(int virtualKeyCode)
            {
                // Interrupt the calibration on key events
                if (tetCalibProc.IsCalibrating) tetCalibProc.InterruptCalibration(); // Will trigger OnCalibrationEnd
            }

            #endregion

            #region Tracking and Calibration

            private void UpdateCalibPlot()
            {
                if (!tetCalibPlot.IsConnected)
                {
                    tetCalibPlot.Connect(GetConnectionString(), (int)TetConstants.TetConstants_DefaultServerPort);
                    tetCalibPlot.SetData(null); // Will use the currently stored calibration data
                }

                tetCalibPlot.UpdateData();

                buttonContinue.Enabled = true;
            }

            /// <summary>
            /// Starts calibration in either calibration or recalibration mode.
            /// </summary>
            /// <param name="isRecalibrating">whether to use recalibration or not.</param>
            private void Calibrate(bool isRecalibrating)
            {
                
                // Connect the calibration procedure if necessary
                if (!tetCalibProc.IsConnected) 
                    tetCalibProc.Connect(GetConnectionString(), (int)TetConstants.TetConstants_DefaultServerPort);

                // Initiate number of points to be calibrated
                tetCalibProc.NumPoints = TetNumCalibPoints.TetNumCalibPoints_5;
                tetCalibProc.PointSpeed = TetCalibPointSpeed.TetCalibPointSpeed_MediumSlow;
                tetCalibProc.PointSize = TetCalibPointSize.TetCalibPointSize_Large;
                

                // Initiate window properties and start calibration
                tetCalibProc.WindowTopmost = false;
                tetCalibProc.WindowVisible = true;
                tetCalibProc.StartCalibration(isRecalibrating ? TetCalibType.TetCalibType_Recalib : TetCalibType.TetCalibType_Calib, false);

                if (isRecalibrating)
                {
                    //tetCalibProc.CalibManager.SetRecalibPoints(tetCalibPlot.SelectedPoints);
                }

                
                
            }




            private void trackStatusStartButton_Click(object sender, System.EventArgs e)
		    {
			    try 
			    {
				    // Connect to the TET server if necessary
				    if (!tetTrackStatus.IsConnected) 
                        tetTrackStatus.Connect(GetConnectionString(), (int)TetConstants.TetConstants_DefaultServerPort);
    				
				    // Start the track status meter
				    if (!tetTrackStatus.IsTracking) tetTrackStatus.Start();
			    } 
			    catch (Exception ex) 
			    {
				    MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			    }
		    }

		    private void trackStatusStopButton_Click(object sender, System.EventArgs e)
		    {
			    try 
			    {
				    if (tetTrackStatus.IsTracking) tetTrackStatus.Stop();
			    } 
			    catch (Exception ex) 
			    {
				    MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			    }
		    }

		    private void calibrateButton_Click(object sender, System.EventArgs e)
		    {
			    try 
			    {
				    Calibrate(false);
			    }
			    catch (Exception ex) 
			    {
				    MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			    }
		    }

            private void buttonRecalibrate_Click(object sender, EventArgs e)
            {
                tetCalibProc.CalibManager.SetRecalibPoints(tetCalibPlot.SelectedPoints);
                try
                {
                    // Connect the calibration procedure if necessary
                    if (!tetCalibProc.IsConnected)
                        tetCalibProc.Connect(GetConnectionString(), (int)TetConstants.TetConstants_DefaultServerPort);

                    // Initiate window properties and start calibration
                    tetCalibProc.WindowTopmost = false;
                    tetCalibProc.WindowVisible = true;


                    tetCalibProc.StartCalibration(TetCalibType.TetCalibType_Recalib, false);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }

		    private void recalibrateButton_Click(object sender, System.EventArgs e)
		    {
			    try 
			    {
				    Calibrate(true);
			    }
			    catch (Exception ex) 
			    {
				    MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			    }
            }

            
            private void trackStartButton_Click(object sender, System.EventArgs e)
		    {
			    try 
			    {
				    // Connect to the TET server if necessary
				    if (!tetClient.IsConnected) tetClient.Connect(GetConnectionString(), (int)TetConstants.TetConstants_DefaultServerPort, TetSynchronizationMode.TetSynchronizationMode_Local);
    				
				    // Start tracking gaze data
				    tetClient.StartTracking();
			    }
			    catch (Exception ex) 
			    {
				    MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			    }
		    }

		    private void trackStopButton_Click(object sender, System.EventArgs e)
		    {
			    try 
			    {
				    if (tetClient.IsTracking) tetClient.StopTracking();
			    }
			    catch (Exception ex) 
			    {
				    MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			    }
		    }

		    #endregion

        #endregion


        #region Buttons

        /// <summary>
        /// Button to start Snap Clutch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonContinue_Click(object sender, EventArgs e)
        {
            snapClutchOn = true;
            StartEyeTracker();

            // minimise application... eventually minimise this to icon bar and not task bar!
            this.WindowState = FormWindowState.Minimized;
            // pass on settings
            gazeEngine.SetDwellClickTime(configuration.DwellClickDelay);
            gazeEngine.SetOffScreenBuffer(configuration.OffScreenGlanceDist);
            // set default mode collection mode
            scModule.StartSnapClutch();

            
        }

        /// <summary>
        /// Exit Snap Clutch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonExit_Click(object sender, EventArgs e)
        {
            snapClutchOn = false;
            StopEyeTrackers();
            this.Close();
        }


        /// <summary>
        /// Button to activate new mode set
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonActivateMode_Click(object sender, EventArgs e)
        {
            foreach (Mode myMode in myModeObjectList)
            {
                if (myMode.GetModeName().Equals(comboBoxTop.Text))
                {
                    activeModeCollection.SetModeTop(myMode);
                }
                if (myMode.GetModeName().Equals(comboBoxRight.Text))
                {
                    activeModeCollection.SetModeRight(myMode);
                }
                if (myMode.GetModeName().Equals(comboBoxBottom.Text))
                {
                    activeModeCollection.SetModeBottom(myMode);
                }
                if (myMode.GetModeName().Equals(comboBoxLeft.Text))
                {
                    activeModeCollection.SetModeLeft(myMode);
                }
            }

            scModule.SetModeCollection(activeModeCollection);

            MessageBox.Show("Mode Collection Updated. Please check messages for any important information!", "Snap Clutch");
            UpdateMessages();
        }

        
        /// <summary>
        /// Button to tell us about Snap CLutch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonAbout_Click(object sender, EventArgs e)
        {
            if (aboutForm.Visible)
                aboutForm.Hide();
            else
                aboutForm.Show();
        }

        /// <summary>
        /// Check box for setting G9
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (useG9)
                useG9 = false;
            else
                useG9 = true;
        }

        /// <summary>
        /// Check box for setting speech
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxSpeech_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxSpeech.Checked)
                useSpeech = true;
            else
                useSpeech = false;

            scModule.UseSpeech(useSpeech);
        }   
      

        #endregion

        private void axTetCalibPlot_OnSelectedPointsChanged(object sender, AxTetComp.DTetCalibPlotEvents_OnSelectedPointsChangedEvent e)
        {

        }

        /// <summary>
        /// Saves the calibration data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSave_Click(object sender, EventArgs e)
        {
            sw = new StreamWriter("users.dat");
            bool nameExists = false;

            for (int i = 0; i < comboBoxUsers.Items.Count; i++)
            {
                if (comboBoxUsers.Items[i].ToString().Equals(comboBoxUsers.Text))
                {
                    Console.WriteLine("exists");
                    nameExists = true;
                }
            }

            if(!nameExists)
                comboBoxUsers.Items.Add(comboBoxUsers.Text);

            for(int i = 0; i < comboBoxUsers.Items.Count; i++)
            {
                sw.WriteLine(comboBoxUsers.Items[i].ToString());
            }

            sw.Close();

            try
            {
                //Console.WriteLine("trying to write calib " + comboBoxUsers.Text);
                //tetCalibProc.CalibManager.SaveCalibrationToFile("calib_" + comboBoxUsers.Text, false);
                if (!tetClient.IsConnected) 
                    tetClient.Connect(GetConnectionString(), (int)TetConstants.TetConstants_DefaultServerPort, TetSynchronizationMode.TetSynchronizationMode_Local);

                tetClient.SaveCalibrationToFile("calib_" + comboBoxUsers.Text);

                MessageBox.Show("Calibration Saved", "Snap Clutch");
                //tetClient.Disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// Loads the calibration data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonLoad_Click(object sender, EventArgs e)
        {
            try
            {
                if (!tetClient.IsConnected)
                    tetClient.Connect(GetConnectionString(), (int)TetConstants.TetConstants_DefaultServerPort, TetSynchronizationMode.TetSynchronizationMode_Local);

                tetClient.ClearCalibration();
                tetClient.LoadCalibrationFromFile("calib_" + comboBoxUsers.Text);
                UpdateCalibPlot();
                MessageBox.Show("Calibration Loaded", "Snap Clutch");
                //tetClient.Disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void comboBoxUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        #region Diagnostic Test Stuff

        private void buttonSetControls_Click(object sender, EventArgs e)
        {
            scModule.SetBlankMode();

            SetComboBoxes();

            foreach (Mode myMode in myModeObjectList)
            {
                if (myMode.GetModeName().Equals(comboBoxTop.Text))
                {
                    activeModeCollection.SetModeTop(myMode);
                }
                if (myMode.GetModeName().Equals(comboBoxRight.Text))
                {
                    activeModeCollection.SetModeRight(myMode);
                }
                if (myMode.GetModeName().Equals(comboBoxBottom.Text))
                {
                    activeModeCollection.SetModeBottom(myMode);
                }
                if (myMode.GetModeName().Equals(comboBoxLeft.Text))
                {
                    activeModeCollection.SetModeLeft(myMode);
                }
            }

            scModule.SetModeCollection(activeModeCollection);

            MessageBox.Show("Mode Collection Updated. Please check messages for any important information!", "Snap Clutch");
            UpdateMessages();

            tl.SetKey(textBoxZone1.Text);
            t.SetKey(textBoxZone2.Text);
            tr.SetKey(textBoxZone3.Text);
            l.SetKey(textBoxZone4.Text);
            c.SetKey(textBoxZone5.Text);
            r.SetKey(textBoxZone6.Text);
            bl.SetKey(textBoxZone7.Text);
            b.SetKey(textBoxZone8.Text);
            br.SetKey(textBoxZone9.Text);

            if (showZones)
                HideAllZones();
            else
                ShowZones();

            SetUpZones();

            //ShowZones();
        }

        private void buttonStartCustom_Click(object sender, EventArgs e)
        {
            //zones = new SnapClutch.Diagnostic.Zones(500, 500, 0.9, 525);
            SetUpZones();
            gameOn = true;
            HideAllZones();
            //ShowZones();
            //StartEyeTracker();
            //this.WindowState = FormWindowState.Minimized;
        }

        /// <summary>
        /// Setup all of the semi transparent zones 
        /// </summary>
        public void SetUpZones()
        {
            zones = new SnapClutch.Diagnostic.Zones(500, 500, 0.9, 525);

            tl.Show();
            tl.Location = zones.topleft2.Location;
            tl.Size = zones.topleft2.Size;

            t.Show();
            t.Location = zones.top2.Location;
            t.Size = zones.top2.Size;

            tr.Show();
            tr.Location = zones.topright2.Location;
            tr.Size = zones.topright2.Size;

            l.Show();
            l.Location = zones.left2.Location;
            l.Size = zones.left2.Size;

            c.Show();
            c.Location = zones.centre2.Location;
            c.Size = zones.centre2.Size;

            r.Show();
            r.Location = zones.right2.Location;
            r.Size = zones.right2.Size;

            bl.Show();
            bl.Location = zones.bottomleft2.Location;
            bl.Size = zones.bottomleft2.Size;

            b.Show();
            b.Location = zones.bottom2.Location;
            b.Size = zones.bottom2.Size;

            br.Show();
            br.Location = zones.bottomright2.Location;
            br.Size = zones.bottomright2.Size;



        }

        /// <summary>
        /// Hides all of the semi transparent zones
        /// </summary>
        public void HideAllZones()
        {
            showZones = false;

            tl.Hide();
            t.Hide();
            tr.Hide();
            l.Hide();
            c.Hide();
            r.Hide();
            bl.Hide();
            b.Hide();
            br.Hide();

        }

        /// <summary>
        /// Shows all of the semi transparent zones
        /// </summary>
        private void ShowZones()
        {
            SetUpZones();
            showZones = true;

            if (tl.IsHidden())
                tl.Hide();
            else
                tl.Show();
            if (t.IsHidden())
                t.Hide();
            else
                t.Show();
            if (tr.IsHidden())
                tr.Hide();
            else
                tr.Show();
            if (l.IsHidden())
                l.Hide();
            else
                l.Show();
            if (c.IsHidden())
                c.Hide();
            else
                c.Show();
            if (r.IsHidden())
                r.Hide();
            else
                r.Show();
            if (bl.IsHidden())
                bl.Hide();
            else
                bl.Show();
            if (b.IsHidden())
                b.Hide();
            else
                b.Show();
            if (br.IsHidden())
                br.Hide();
            else
                br.Show();

            tl.ShowInTaskbar = false;
            t.ShowInTaskbar = false;
            tr.ShowInTaskbar = false;
            l.ShowInTaskbar = false;
            c.ShowInTaskbar = false;
            r.ShowInTaskbar = false;
            bl.ShowInTaskbar = false;
            b.ShowInTaskbar = false;
            br.ShowInTaskbar = false;
        }

        //if there is a fixation within the zone then send the direction control
        public void Fixation(FixationDataPoint argFixationPoint)
        {
            int zoneInt = 0;

            if (gameOn)
            {
                //try
                //{
                if (argFixationPoint == null)
                {
                }
                else
                {
                    zoneInt = Convert.ToInt16(zones.CheckDistanceZones(argFixationPoint));
                }

                //    throw new System.Exception();
                //}
                //catch
                //{
                //    Console.WriteLine("exception thrown");
                //}
                //Console.WriteLine("current zone = " + zoneInt);
                switch (zoneInt)
                {
                    case 1:
                        SendKey(tl.GetKey());
                        break;
                    case 2:
                        SendKey(t.GetKey());
                        break;
                    case 3:
                        SendKey(tr.GetKey());
                        break;
                    case 4:
                        SendKey(l.GetKey());
                        break;
                    case 5:
                        SendKey(c.GetKey());
                        break;
                    case 6:
                        SendKey(r.GetKey());
                        break;
                    case 7:
                        SendKey(bl.GetKey());
                        break;
                    case 8:
                        SendKey(b.GetKey());
                        break;
                    case 9:
                        SendKey(br.GetKey());
                        break;
                }
            }
        }

        private void SendKey(string argKey)
        {
            if (argKey.Equals("W"))
            {
                if (!wKey)
                {
                    KeyEvent.WKeyDown();
                    wKey = true;
                }
                if (aKey)
                {
                    KeyEvent.AKeyUp();
                    aKey = false;
                }
                if (sKey)
                {
                    KeyEvent.SKeyUp();
                    sKey = false;
                }
                if (dKey)
                {
                    KeyEvent.DKeyUp();
                    dKey = false;
                }
                if (rightMouse)
                    RightMouseStop();
            }
            if (argKey.Equals("A"))
            {

                if (!aKey)
                {
                    KeyEvent.AKeyDown();
                    aKey = true;
                }
                if (wKey)
                {
                    KeyEvent.WKeyUp();
                    wKey = false;
                }
                if (sKey)
                {
                    KeyEvent.SKeyUp();
                    sKey = false;
                }
                if (dKey)
                {
                    KeyEvent.DKeyUp();
                    dKey = false;
                }
            }
            if (argKey.Equals("S"))
            {
                if (!sKey)
                {
                    KeyEvent.SKeyDown();
                    sKey = true;
                }
                if (wKey)
                {
                    KeyEvent.WKeyUp();
                    wKey = false;
                }
                if (aKey)
                {
                    KeyEvent.AKeyUp();
                    aKey = false;
                }
                if (dKey)
                {
                    KeyEvent.DKeyUp();
                    dKey = false;
                }
            }
            if (argKey.Equals("D"))
            {
                if (!dKey)
                {
                    KeyEvent.DKeyDown();
                    dKey = true;
                }
                if (wKey)
                {
                    KeyEvent.WKeyUp();
                    wKey = false;
                }
                if (aKey)
                {
                    KeyEvent.AKeyUp();
                    aKey = false;
                }
                if (sKey)
                {
                    KeyEvent.SKeyUp();
                    sKey = false;
                }
            }
            if (argKey.Equals("WA"))
            {
                if (!wKey)
                {
                    KeyEvent.WKeyDown();
                    wKey = true;
                }
                if (!aKey)
                {
                    KeyEvent.AKeyDown();
                    aKey = true;
                }
                if (sKey)
                {
                    KeyEvent.SKeyUp();
                    sKey = false;
                }
                if (dKey)
                {
                    KeyEvent.DKeyUp();
                    dKey = false;
                }
            }
            if (argKey.Equals("WD"))
            {
                if (!wKey)
                {
                    KeyEvent.WKeyDown();
                    wKey = true;
                }
                if (!dKey)
                {
                    KeyEvent.DKeyDown();
                    dKey = true;
                }
                if (sKey)
                {
                    KeyEvent.SKeyUp();
                    sKey = false;
                }
                if (aKey)
                {
                    KeyEvent.AKeyUp();
                    aKey = false;
                }
            }
            if (argKey.Equals("X"))
                KeysUp();
            //KeysUp();
        }

        private void KeysUp()
        {
            KeyEvent.WKeyUp();
            wKey = false;
            KeyEvent.AKeyUp();
            aKey = false;
            KeyEvent.SKeyUp();
            sKey = false;
            KeyEvent.DKeyUp();
            dKey = false;
        }

        #endregion

        private void buttonLocoOnly_Click(object sender, EventArgs e)
        {
            scModule.SetWarcraftModeLocomotion();
            SetComboBoxes();
        }

        private void buttonWowSafe_Click(object sender, EventArgs e)
        {
            scModule.SetWarcraftModeKeys();
            SetComboBoxes();
        }

        private void buttonWowExp_Click(object sender, EventArgs e)
        {
            scModule.SetWarcraftModeMouse();
            SetComboBoxes();
        }

        private void buttonDefault_Click(object sender, EventArgs e)
        {
            scModule.SetDefaultModes();
            SetComboBoxes();
        }

        private void buttonMine_Click(object sender, EventArgs e)
        {
            scModule.SetMinecraftModeMouse();
            SetComboBoxes();
        }

        #region Mouse Controls

        private void TurnLeft()
        {
            Console.WriteLine("attempting left turn");

            MouseEvent.RightDownPoint(centrePoint.X, centrePoint.Y + 2);
            rightMouse = true;
            double offset = 0.0;

            offset = speedFactor * 0.8;


            Cursor.Position = new Point(centrePoint.X - Convert.ToInt16(offset), centrePoint.Y + 2);
            isTurning = true;

        }

        private void TurnRight()
        {
            MouseEvent.RightDownPoint(centrePoint.X, centrePoint.Y + 2);
            rightMouse = true;
            double offset = 0.0;

            offset = speedFactor * 0.8;

            Cursor.Position = new Point(centrePoint.X + Convert.ToInt16(offset), centrePoint.Y + 2);
            isTurning = true;
        }
    
        private void RightMouseStop()
        {
            if(rightMouse)
                MouseEvent.RightUpPoint(centrePoint.X, centrePoint.Y + 2);

            rightMouse = false;
            isTurning = false;
        }

        #endregion

        private void buttonGestures_Click(object sender, EventArgs e)
        {
            if (gestureInterface.Visible)
            {
                gestureInterface.Hide();

                gestureInterface.ShowGestureInfo(false);
            }
            else
            {
                gestureInterface = new Gesture_Interface.MainForm();
                gestureInterface.Show();
                //gestureInterface.ShowGestureInfo(true);
            }
        }

        private void buttonGesturePreset_Click(object sender, EventArgs e)
        {
            scModule.SetGestureModes();
            SetComboBoxes();
            //usingGestures = true;
        }

        private void checkBoxGestures_CheckedChanged(object sender, EventArgs e)
        {
            if (usingGestures)
            {
                gazeEngine.SetBufferSize(configuration.BufferSize);
                usingGestures = false;
            }
            else
            {
                gazeEngine.SetBufferSize(10);
                usingGestures = true;
            }
        }





    }
}
