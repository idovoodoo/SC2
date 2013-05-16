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
using System.Drawing;
using System.Windows.Forms;
using SnapClutch.Config;
using SnapClutch.SCTools;
using SnapClutch.Modes;
using SnapClutch.G9CSharp;
using System.IO;
using SnapClutch.Modes.EyeGuitar;
using SpeechLib;


namespace SnapClutch
{
    public class SCModule : Form
    {
        private Point gazePos;
        private Configuration mConfiguration;
        private int glance = 0;
        //private Point tgParkPos;
        private Point dwellPos;
        private string name;
        private SpVoice voice;

        private List<Mode> listOfModes;
        private ModeCollection myModeCollection;
        //windows modes
        private static ModeDwellClickLeft myModeDwellClickLeft;
        private static ModeDwellClickRight myModeDwellClickRight;
        private static ModeOffSmallCursor myModeOffSmallCursor;
        private static ModeLeftDrag myModeLeftDrag;
        private static ModeBlank myModeBlank;
        //key mmo modes
        private static ModeKeyLocomotionCatA myModeKeyLocomotionCatA;
        private static ModeKeyLookAroundCatA myModeKeyLookAroundCatA;
        private static ModeKeyLocomotionCatB myModeKeyLocomotionCatB;
        private static ModeKeyLookAroundCatB myModeKeyLookAroundCatB; 
        //mouse mmo modes
        private static ModeMouseLocomotionCatA myModeMouseLocomotionCatA;
        private static ModeMouseLookAroundCatA myModeMouseLookAroundCatA;
        private static ModeMouseLocomotionCatB myModeMouseLocomotionCatB;
        private static ModeMouseLookAroundCatB myModeMouseLookAroundCatB; 
        //eyeguitar
        private static ModeStrings myModeStrings;
        //minecraft
        private static ModeMinecraftLocoGazeOnly myModeMinecraftLocoGazeOnly;
        private static ModeMinecraftLookGazeOnly myModeMinecraftLookGazeOnly;
        private static ModeMinecraftCBlock myModeMinecraftCBlock;

        public static ModeIndicator myModeIndicator;

        public static MOUSESTATE msBefore, msAfter;

        private bool g9TextOn = false;

        public G9TextFormCS myG9TextForm;
        private GazeOverlay gazeOverlay, gazeOverlayG9Text;
        private bool useSpeech = false;

        /// <summary>
        /// Constructor without overlay
        /// </summary>
        /// <param name="argConfiguration"></param>
        public SCModule(Configuration argConfiguration)
        {
            mConfiguration = argConfiguration;

            gazePos = new Point(0, 0);
            //tgParkPos = new Point(0, 0);
            voice = new SpVoice();

            myG9TextForm = new G9TextFormCS(mConfiguration.DwellClickDelay);
            gazeOverlayG9Text = myG9TextForm.GetGazeOverlay();
            gazeOverlayG9Text.SetOverlayStatus(true);
            myModeIndicator = new ModeIndicator();
            SetupModes();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="argConfiguration"></param>
        /// <param name="argGazeOverlay"></param>
        public SCModule(Configuration argConfiguration, GazeOverlay argGazeOverlay)
        {
            mConfiguration = argConfiguration;
            gazeOverlay = argGazeOverlay;
            
            gazePos = new Point(0, 0);
            //tgParkPos = new Point(0, 0);
            
            myG9TextForm = new G9TextFormCS(mConfiguration.DwellClickDelay);
            gazeOverlayG9Text = myG9TextForm.GetGazeOverlay();
            gazeOverlayG9Text.SetOverlayStatus(true);
            myModeIndicator = new ModeIndicator();
            SetupModes();
            }

        /// <summary>
        /// setup all available modes
        /// </summary>
        private void SetupModes()
        {
            myModeCollection = new ModeCollection();

            // windows modes
            myModeDwellClickLeft = new ModeDwellClickLeft();
            myModeDwellClickRight = new ModeDwellClickRight();
            myModeOffSmallCursor = new ModeOffSmallCursor();
            myModeLeftDrag = new ModeLeftDrag();
            myModeBlank = new ModeBlank();
            //key mmo modes
            myModeKeyLocomotionCatA = new ModeKeyLocomotionCatA();
            myModeKeyLookAroundCatA = new ModeKeyLookAroundCatA();
            myModeKeyLocomotionCatB = new ModeKeyLocomotionCatB();
            myModeKeyLookAroundCatB = new ModeKeyLookAroundCatB(); 
            //mouse mmo modes
            myModeMouseLocomotionCatA = new ModeMouseLocomotionCatA();
            myModeMouseLookAroundCatA = new ModeMouseLookAroundCatA();
            myModeMouseLocomotionCatB = new ModeMouseLocomotionCatB();
            myModeMouseLookAroundCatB = new ModeMouseLookAroundCatB(); 
            //eyeguitar
            myModeStrings = new ModeStrings();
            //minecraft
            myModeMinecraftLocoGazeOnly = new ModeMinecraftLocoGazeOnly();
            myModeMinecraftLookGazeOnly = new ModeMinecraftLookGazeOnly();
            myModeMinecraftCBlock = new ModeMinecraftCBlock();

            listOfModes = new List<Mode>();
            
            listOfModes.Add(myModeDwellClickLeft);
            listOfModes.Add(myModeDwellClickRight);
            listOfModes.Add(myModeOffSmallCursor);
            listOfModes.Add(myModeLeftDrag);
            listOfModes.Add(myModeBlank);
            listOfModes.Add(myModeKeyLocomotionCatA);
            listOfModes.Add(myModeKeyLookAroundCatA);
            listOfModes.Add(myModeKeyLocomotionCatB);
            listOfModes.Add(myModeKeyLookAroundCatB);
            listOfModes.Add(myModeMouseLocomotionCatA);
            listOfModes.Add(myModeMouseLookAroundCatA);
            listOfModes.Add(myModeMouseLocomotionCatB);
            listOfModes.Add(myModeMouseLookAroundCatB);
            listOfModes.Add(myModeStrings);
            listOfModes.Add(myModeMinecraftLocoGazeOnly);
            listOfModes.Add(myModeMinecraftLookGazeOnly);
            listOfModes.Add(myModeMinecraftCBlock);

            //setup default modes
            myModeCollection.SetModeLeft(myModeDwellClickLeft);
            myModeCollection.SetModeRight(myModeDwellClickRight);
            myModeCollection.SetModeTop(myModeLeftDrag);
            myModeCollection.SetModeBottom(myModeOffSmallCursor);
        }

        public void SetDefaultModes()
        {
            myModeCollection.SetModeLeft(myModeDwellClickLeft);
            myModeCollection.SetModeRight(myModeDwellClickRight);
            myModeCollection.SetModeTop(myModeLeftDrag);
            myModeCollection.SetModeBottom(myModeOffSmallCursor);
        }

        public void SetGestureModes()
        {
            myModeCollection.SetModeLeft(myModeOffSmallCursor);
            myModeCollection.SetModeRight(myModeOffSmallCursor);
            myModeCollection.SetModeTop(myModeOffSmallCursor);
            myModeCollection.SetModeBottom(myModeOffSmallCursor);
        }

        public void SetWarcraftModeLocomotion()
        {
            myModeCollection.SetModeLeft(myModeBlank);
            myModeCollection.SetModeRight(myModeBlank);
            myModeCollection.SetModeTop(myModeMouseLocomotionCatA);
            myModeCollection.SetModeBottom(myModeMouseLookAroundCatA);
        }

        public void SetWarcraftModeKeys()
        {
            myModeCollection.SetModeLeft(myModeDwellClickLeft);
            myModeCollection.SetModeRight(myModeDwellClickRight);
            myModeCollection.SetModeTop(myModeKeyLocomotionCatA);
            myModeCollection.SetModeBottom(myModeKeyLookAroundCatA);
        }

        public void SetWarcraftModeMouse()
        {
            myModeCollection.SetModeLeft(myModeDwellClickLeft);
            myModeCollection.SetModeRight(myModeDwellClickRight);
            myModeCollection.SetModeTop(myModeMouseLocomotionCatA);
            myModeCollection.SetModeBottom(myModeMouseLookAroundCatA);
        }

        public void SetMinecraftModeMouse()
        {
            myModeCollection.SetModeLeft(myModeBlank);
            myModeCollection.SetModeRight(myModeBlank);
            myModeCollection.SetModeTop(myModeMinecraftLocoGazeOnly);
            myModeCollection.SetModeBottom(myModeMinecraftLookGazeOnly);
        }

        public void SetBlankMode()
        {
            myModeCollection.SetModeLeft(myModeBlank);
            myModeCollection.SetModeRight(myModeBlank);
            myModeCollection.SetModeTop(myModeBlank);
            myModeCollection.SetModeBottom(myModeBlank);
        }

        /// <summary>
        /// Gets the current modecollection
        /// </summary>
        /// <returns></returns>
        public ModeCollection GetActiveModeCollection()
        {
            return myModeCollection;
        }

        /// <summary>
        /// Get the List<> of modes
        /// </summary>
        /// <returns></returns>
        public List<Mode> GetModeList()
        {
            return listOfModes;
        }

        /// <summary>
        /// Update the current modeset
        /// </summary>
        /// <param name="argModeCollection"></param>
        public void SetModeCollection(ModeCollection argModeCollection)
        {
            myModeCollection = argModeCollection;
        }

        /// <summary>
        /// Gets all of the modes
        /// </summary>
        /// <returns></returns>
        public List<Mode> GetModeObjects()
        {
            return listOfModes;
        }

        //public void SetGlance(int argGlance)
        //{
        //    glance = argGlance;
        //}
        

        /// <summary>
        /// Gaze data stream comes in here
        /// </summary>
        /// <param name="argPoint"></param>
        public void DataIn(Point argPoint)
        {
            gazePos = argPoint;
           

            if (g9TextOn)
            {
                //CursorFactory.Create(Application.StartupPath + CursorSnapClutch.BlankCursor());
                Cursor.Position = gazeOverlayG9Text.GetPosition(gazePos);
            }
            else
            {

                switch (glance)
                {
                    case 0:
                        // off
                        //Cursor.Position = gazeOverlay.GetPosition(argPoint);
                        //msAfter = myModeSnapClutch.Execute(gazePos, msBefore, gazeOverlay);
                        msBefore = msAfter;
                        break;
                    case 1:
                        //top
                        msAfter = myModeCollection.GetModeTop().Execute(gazePos, msBefore, gazeOverlay);                       
                        msBefore = msAfter;
                        break;
                    case 2:
                        // bottom
                        msAfter = myModeCollection.GetModeBottom().Execute(gazePos, msBefore, gazeOverlay);
                        msBefore = msAfter;
                        break;
                    case 3:
                        // left
                        msAfter = myModeCollection.GetModeLeft().Execute(gazePos, msBefore, gazeOverlay);  
                        msBefore = msAfter;
                        break;
                    case 4:
                        // right
                        msAfter = myModeCollection.GetModeRight().Execute(gazePos, msBefore, gazeOverlay);   
                        msBefore = msAfter;
                        break;
                    case 5:
                        //top2
                        //msAfter = modeSet.GetActiveModeCollection().GetModeTop2().Execute(gazePos, msBefore, gazeOverlay);
                        //msBefore = msAfter;
                        break;
                    case 6:
                        // bottom2
                        //msAfter = modeSet.GetActiveModeCollection().GetModeBottom2().Execute(gazePos, msBefore, gazeOverlay);
                        //msBefore = msAfter;
                        break;
                    case 7:
                        // left2
                        //msAfter = modeSet.GetActiveModeCollection().GetModeLeft2().Execute(gazePos, msBefore, gazeOverlay);
                        //msBefore = msAfter;
                        break;
                    case 8:
                        // right2
                        //msAfter = modeSet.GetActiveModeCollection().GetModeRight2().Execute(gazePos, msBefore, gazeOverlay);
                        //msBefore = msAfter;
                        break;
                }
            }            
        }

        // start THE clutch
        public void StartSnapClutch()
        {
            // start snap clutch in safest mode, which is at the bottom!
            glance = 0;

            //string n = string.Format("{0:ddMMyyyy_hh-mm}", DateTime.Now);

            //create a new file
            //fixationWriter = new StreamWriter(name.ToString() + "_snap clutch fixations_" + n + ".txt");
        }

        // stop THE clutch
        public void StopSnapClutch()
        {
            glance = 0;
            ResetMouseStates();
            myModeIndicator.Hide();
            
            //fixationWriter.Close();
        }

        //public void StopStreamWriter()
        //{
        //    try
        //    {
        //        fixationWriter.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        // sets if g9 is active or not
        public void SetG9Active(bool argG9TextOn, bool argSwitch, bool argOverlay)
        {
            g9TextOn = argG9TextOn;
            myG9TextForm.UseSwitch(argSwitch);
            myG9TextForm.SetOverlayStatus(argOverlay);

            if (g9TextOn)
            {
                // turn on G9
                myG9TextForm.Show();
                // turn off modes
                glance = 0;
                // hide mode indication
                myModeIndicator.Hide();
                //CursorFactory.Create(Application.StartupPath + CursorSnapClutch.BlankCursor());
                ResetMouseStates();
                //CursorFactory.CreateBlankCursor();

            }
            else
            {
                // turn off G9
                myG9TextForm.Hide();
                // reset mode
                glance = 0;
            }
        }

        /// <summary>
        /// a fixation is detected so lets record it in the log file!
        /// </summary>
        /// <param name="argPoint"></param>
        public void Fixation(FixationDataPoint argPoint)
        {
            if (glance == 0)
            {
            }
            else { }
           }

        // gets the status of g9
        public bool GetG9Active()
        {
            return g9TextOn;
        }

        public void Blink(int argEye)
        {
            //0 = left eye
            //1 = right eye

            switch (glance)
            {
                case 0:
                    if (g9TextOn)
                    {
                    }
                    break;
                case 1:
                    //top
                    // g9 text overrides all other modes until it is switched off
                    if (g9TextOn)
                    {
                    }
                    else
                    {
                        //msAfter = modeSet.GetActiveModeCollection().GetModeTop().BlinkEvent(dwellPos, argEye);
                        msBefore = msAfter;
                    }
                    break;
                case 2:
                    //bottom
                    if (g9TextOn)
                    {
                    }
                    else
                    {
                        //msAfter = modeSet.GetActiveModeCollection().GetModeBottom().BlinkEvent(dwellPos, argEye);
                        msBefore = msAfter;
                    }
                    break;
                case 3:
                    //left
                    if (g9TextOn)
                    {
                    }
                    else
                    {
                        //msAfter = modeSet.GetActiveModeCollection().GetModeLeft().BlinkEvent(dwellPos, argEye);
                        msBefore = msAfter;

                    }
                    break;
                case 4:
                    //right
                    if (g9TextOn)
                    {
                    }
                    else
                    {
                        //msAfter = modeSet.GetActiveModeCollection().GetModeRight().BlinkEvent(dwellPos, argEye);
                        msBefore = msAfter;
                    }
                    break;
                case 5:
                    //top toggle
                    // g9 text overrides all other modes until it is switched off
                    if (g9TextOn)
                    {
                    }
                    else
                    {
                        //msAfter = modeSet.GetActiveModeCollection().GetModeTop2().BlinkEvent(dwellPos, argEye);
                        msBefore = msAfter;
                    }
                    break;
                case 6:
                    //bottom toggle
                    if (g9TextOn)
                    {
                    }
                    else
                    {
                        //msAfter = modeSet.GetActiveModeCollection().GetModeBottom2().BlinkEvent(dwellPos, argEye);
                        msBefore = msAfter;
                    }
                    break;
                case 7:
                    //left toggle
                    if (g9TextOn)
                    {
                    }
                    else
                    {
                        //msAfter = modeSet.GetActiveModeCollection().GetModeLeft2().BlinkEvent(dwellPos, argEye);
                        msBefore = msAfter;

                    }
                    break;
                case 8:
                    //right toggle
                    if (g9TextOn)
                    {
                    }
                    else
                    {
                        //msAfter = modeSet.GetActiveModeCollection().GetModeRight2().BlinkEvent(dwellPos, argEye);
                        msBefore = msAfter;
                    }
                    break;
            }

        }

        /// <summary>
        /// When a dwell occurs... deal with it here
        /// </summary>
        /// <param name="argSwitch"></param>
        /// <param name="argDown"></param>
        public void Dwell(bool argSwitch, bool argDown)
        {
                switch (glance)
                {
                    case 0:
                        if (g9TextOn)
                        {
                            if(argSwitch && !argDown)
                                myG9TextForm.SwitchPress();
                        }
                        break;
                    case 1:
                        //top
                        // g9 text overrides all other modes until it is switched off
                        if (g9TextOn)
                        {
                            if (argSwitch && !argDown)
                                myG9TextForm.SwitchPress();
                        }
                        else
                        {

                            msAfter = myModeCollection.GetModeTop().SomeEvent(dwellPos, argSwitch, argDown);
                            msBefore = msAfter;
                            //Console.WriteLine("DWELL!");
                        }
                        break;
                    case 2:
                        //bottom
                        if (g9TextOn)
                        {
                            if (argSwitch && !argDown)
                                myG9TextForm.SwitchPress(); 
                        }
                        else
                        {
                            msAfter = myModeCollection.GetModeBottom().SomeEvent(dwellPos, argSwitch, argDown);
                            msBefore = msAfter;
                        }
                        break;
                    case 3:
                        //left
                        if (g9TextOn)
                        {
                            if (argSwitch && !argDown)
                                myG9TextForm.SwitchPress();
                        }
                        else
                        {
                            msAfter = myModeCollection.GetModeLeft().SomeEvent(dwellPos, argSwitch, argDown);
                            msBefore = msAfter;

                        }
                        break;
                    case 4:
                        //right
                        if (g9TextOn)
                        {
                            if (argSwitch && !argDown)
                                myG9TextForm.SwitchPress();
                        }
                        else
                        {
                            msAfter = myModeCollection.GetModeRight().SomeEvent(dwellPos, argSwitch, argDown);
                            msBefore = msAfter;
                        }
                        break;
                    case 5:
                        ////top toggle
                        //// g9 text overrides all other modes until it is switched off
                        //if (g9TextOn)
                        //{
                        //    if (argSwitch && !argDown)
                        //        myG9TextForm.SwitchPress();
                        //}
                        //else
                        //{
                        //    //msAfter = modeSet.GetActiveModeCollection().GetModeTop2().SomeEvent(dwellPos, argSwitch, argDown);
                        //    msBefore = msAfter;
                        //}
                        break;
                    case 6:
                        ////bottom toggle
                        //if (g9TextOn)
                        //{
                        //    if (argSwitch && !argDown)
                        //        myG9TextForm.SwitchPress();
                        //}
                        //else
                        //{
                        //    //msAfter = modeSet.GetActiveModeCollection().GetModeBottom2().SomeEvent(dwellPos, argSwitch, argDown);
                        //    msBefore = msAfter;
                        //}
                        break;
                    case 7:
                        //left toggle
                        //if (g9TextOn)
                        //{
                        //    if (argSwitch && !argDown)
                        //        myG9TextForm.SwitchPress();
                        //}
                        //else
                        //{
                        //    msAfter = modeSet.GetActiveModeCollection().GetModeLeft2().SomeEvent(dwellPos, argSwitch, argDown);
                        //    msBefore = msAfter;

                        //}
                        break;
                    case 8:
                        //right toggle
                        //if (g9TextOn)
                        //{
                        //    if (argSwitch && !argDown)
                        //        myG9TextForm.SwitchPress();
                        //}
                        //else
                        //{
                        //    msAfter = modeSet.GetActiveModeCollection().GetModeRight2().SomeEvent(dwellPos, argSwitch, argDown);
                        //    msBefore = msAfter;
                        //}
                        break;
                }
        }


        public void SetDwellPos(Point argDwellPos)
        {
            dwellPos = argDwellPos;
        }

        /// <summary>
        /// Tells module that we want to use speech
        /// </summary>
        /// <param name="speech"></param>
        public void UseSpeech(bool speech)
        {
            useSpeech = speech;
        }

        // what to do on off screen glance
        public void OffScreenGlance(int argGlance)
        { 
            if (g9TextOn)
            {}
            else
            {
                //glance off left
                if (argGlance == 3)
                {
                    if (glance == 3)
                    {
                    }
                    else
                    {
                        
                        glance = 3;
                        SetModeIndicatorLeft();
                        myModeCollection.SetActiveMode(3);
                        Console.WriteLine("glance left");
                        ResetMouseStates();
                        if(useSpeech)
                            voice.Speak(myModeCollection.GetModeLeft().modeName, SpeechVoiceSpeakFlags.SVSFlagsAsync);
                    }
                }
                // glance off right
                else if (argGlance == 4)
                {
                    if (glance == 4)
                    {
                    }
                    else
                    {
                        glance = 4;
                        SetModeIndicatorRight();
                        myModeCollection.SetActiveMode(4);
                        Console.WriteLine("glance right");
                        ResetMouseStates();
                        if (useSpeech)
                            voice.Speak(myModeCollection.GetModeRight().modeName, SpeechVoiceSpeakFlags.SVSFlagsAsync);
                    }
                }

                // glance off top
                else if (argGlance == 1)
                {
                    if (glance == 1)
                    {
                    }
                    else
                    {
                        glance = 1;
                        SetModeIndicatorTop();
                        myModeCollection.SetActiveMode(1);
                        Console.WriteLine("glance top");
                        ResetMouseStates();
                        if (useSpeech)
                            voice.Speak(myModeCollection.GetModeTop().modeName, SpeechVoiceSpeakFlags.SVSFlagsAsync);
                    }
                }

                // glance off bottom
                else if (argGlance == 2)
                {
                    if (glance == 2)
                    {
                    }
                    else
                    {
                        glance = 2;
                        SetModeIndicatorBottom();
                        myModeCollection.SetActiveMode(2);
                        Console.WriteLine("glance bottom");
                        ResetMouseStates();
                        if (useSpeech)
                            voice.Speak(myModeCollection.GetModeBottom().modeName, SpeechVoiceSpeakFlags.SVSFlagsAsync);
                    }
                }
                // glance off left toggle
                else if (argGlance == 7)
                {
                    //KeyEvent.KeysUp();
                    //modeSet.GetActiveModeCollection().SetActiveMode(7);
                    //glance = 7;
                    //SetModeIndicatorLeft();
                    //voice.Speak(modeSet.GetActiveModeCollection().GetModeLeft2().modeName, SpeechVoiceSpeakFlags.SVSFlagsAsync);

                    ////Console.WriteLine("glance left second");
                }
                // glance off right toggle
                else if (argGlance == 8)
                {
                    
                    //KeyEvent.KeysUp();
                    //modeSet.GetActiveModeCollection().SetActiveMode(8);
                    //glance = 8;
                    //SetModeIndicatorRight();
                    //voice.Speak(modeSet.GetActiveModeCollection().GetModeRight2().modeName, SpeechVoiceSpeakFlags.SVSFlagsAsync);

                    ////Console.WriteLine("glance right second");
                }
                // glance off top toggle
                else if (argGlance == 5)
                {
                    
                    //ResetMouseStates();
                    //KeyEvent.KeysUp();
                    //modeSet.GetActiveModeCollection().SetActiveMode(5);
                    //glance = 5;
                    //SetModeIndicatorTop();
                    //voice.Speak(modeSet.GetActiveModeCollection().GetModeTop2().modeName, SpeechVoiceSpeakFlags.SVSFlagsAsync);

                    //Console.WriteLine("glance top second");
                }
                // glance off bottom toggle
                else if (argGlance == 6)
                {
                    //ResetMouseStates();
                    //KeyEvent.KeysUp();
                    //modeSet.GetActiveModeCollection().SetActiveMode(6);
                    //glance = 6;
                    //SetModeIndicatorBottom();
                    //voice.Speak(modeSet.GetActiveModeCollection().GetModeBottom2().modeName, SpeechVoiceSpeakFlags.SVSFlagsAsync);

                    //Console.WriteLine("glance bottom second");
                }
            }
        }

        public void SetConfig(Configuration argConfig)
        {
            mConfiguration = argConfig;
        }

        public void ResetMouseStates()
        {
            if (msBefore.goingForward)
            {
                MouseEvent.RightUp();
                msBefore.rightButton = false;
            }

            if (msBefore.leftButton)
            {
                MouseEvent.LeftUp();
                msBefore.leftButton = false;
            }
            if (msBefore.rightButton)
            {
                MouseEvent.RightUp();
                msBefore.rightButton = false;
            }

            if (msAfter.leftButton)
            {
                MouseEvent.LeftUp();
                msAfter.leftButton = false;
            }
            if (msAfter.rightButton)
            {
                MouseEvent.RightUp();
                msAfter.rightButton = false;
            }

            if (msAfter.goingForward)
            {
                MouseEvent.RightUp();
                msBefore.rightButton = false;
            }

            if (msBefore.wKey)
            {
                KeyEvent.WKeyUp();
                msBefore.wKey = false;
            }
            if (msBefore.aKey)
            {
                KeyEvent.AKeyUp();
                msBefore.aKey = false;
            }
            if (msBefore.sKey)
            {
                KeyEvent.SKeyUp();
                msBefore.sKey = false;
            }
            if (msBefore.dKey)
            {
                KeyEvent.DKeyUp();
                msBefore.dKey = false;
            }

            if (msAfter.wKey)
            {
                KeyEvent.WKeyUp();
                msAfter.wKey = false;
            }
            if (msAfter.aKey)
            {
                KeyEvent.AKeyUp();
                msAfter.aKey = false;
            }
            if (msAfter.sKey)
            {
                KeyEvent.SKeyUp();
                msAfter.sKey = false;
            }
            if (msAfter.dKey)
            {
                KeyEvent.DKeyUp();
                msAfter.dKey = false;
            }

            //KeyEvent.EscKeyDown();
            //KeyEvent.EscKeyUp();
        }

        public void SetName(string argName)
        {
            name = argName;
        }

 
        #region Mode Indicators


        private void SetModeIndicatorTop()
        {
            if (!myModeIndicator.Visible)
                myModeIndicator.Show();

            //myModeIndicator.TopMost = true;
            //myModeIndicator.SetName(modeSet.GetActiveModeCollection().GetModeTop().modeName);
            //myModeIndicator.BackColor.Equals(Color.Green);
            myModeIndicator.Top = 0;
            myModeIndicator.Left = 0;
            myModeIndicator.Width = Screen.PrimaryScreen.Bounds.Width;
            myModeIndicator.Height = 10;
        }

        private void SetModeIndicatorLeft()
        {
            if (!myModeIndicator.Visible)
                myModeIndicator.Show();

            //myModeIndicator.TopMost = true;
            //myModeIndicator.SetName(modeSet.GetActiveModeCollection().GetModeLeft().modeName);
            //myModeIndicator.BackColor.Equals(Color.Green);
            myModeIndicator.Top = 0;
            myModeIndicator.Left = 0;
            myModeIndicator.Width = 10;
            myModeIndicator.Height = Screen.PrimaryScreen.Bounds.Height;
        }

        private void SetModeIndicatorRight()
        {
            if (!myModeIndicator.Visible)
                myModeIndicator.Show();

            //myModeIndicator.TopMost = true;
            //myModeIndicator.SetName(modeSet.GetActiveModeCollection().GetModeRight().modeName);
            //myModeIndicator.BackColor.Equals(Color.Green);
            myModeIndicator.Top = 0;
            myModeIndicator.Left = Screen.PrimaryScreen.Bounds.Width - 10;
            myModeIndicator.Width = 10;
            myModeIndicator.Height = Screen.PrimaryScreen.Bounds.Height;
        }

        private void SetModeIndicatorBottom()
        {
            if (!myModeIndicator.Visible)
                myModeIndicator.Show();

            //myModeIndicator.TopMost = true;
            //myModeIndicator.SetName(modeSet.GetActiveModeCollection().GetModeBottom().modeName);
            //myModeIndicator.BackColor.Equals(Color.Green);
            myModeIndicator.Top = Screen.PrimaryScreen.Bounds.Height - 10;
            myModeIndicator.Left = 0;
            myModeIndicator.Width = Screen.PrimaryScreen.Bounds.Width;
            myModeIndicator.Height = 10;
        }

        #endregion

    }

}