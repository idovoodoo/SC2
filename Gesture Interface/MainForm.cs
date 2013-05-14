/*
 *  To do suggestions: If gaze zones twice to same area, it would still be concidered as one enter and 
 *                     would not break the making of a gesture. SM.
 *                     
 */

using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Gesture_Interface
{
    public partial class MainForm : Form
    {        
        SettingZone mySettingZone;
        ZoneA myZoneA;
        ZoneB myZoneB;
        ZoneC myZoneC;
        ZoneD myZoneD;
        ZoneE myZoneE;
        double enterOpacity;
        double leaveOpacity;
        enum GestureZone { NONE = 0, ZONEA = 1, ZONEB = 2, ZONEC = 3, ZONED = 4, ZONEE = 5};    
        bool gestureStart = false;
        bool zoneAActive = false;
        bool zoneBActive = false;
        bool zoneCActive = false;
        bool zoneDActive = false;
        bool zoneEActive = false;
        bool sequenceOk = false;
        int[] gestureString = new int[4] {0, 0, 0, 0};
        int zoneCounter = 0;
        int gestureTimeOutTimer = 2000;
        TimeSpan zeroTime = new TimeSpan (0, 0, 0, 0, 0);
        int activeZone = 0;
        Thread timeOutThread;
        DateTime startTimeOut;
        TimeSpan timeOutTimer;
        string gestureAsString;
        Hashtable keyCode = new Hashtable();
        Gesture gesture1A = new Gesture(5145);
        Gesture gesture1B = new Gesture(5150);
        Gesture gesture1C = new Gesture(5125);

        Gesture gesture2A = new Gesture(5215);
        Gesture gesture2B = new Gesture(5250);
        Gesture gesture2C = new Gesture(5235);

        Gesture gesture3A = new Gesture(5325);
        Gesture gesture3B = new Gesture(5350);
        Gesture gesture3C = new Gesture(5345);

        Gesture gesture4A = new Gesture(5415);
        Gesture gesture4B = new Gesture(5450);
        Gesture gesture4C = new Gesture(5435);

        GestureCollection gestureCollection;
        bool lockTimoutThread = false;
        bool showGestureInfo = true;
        GestureInfo gestureInfo;
        
        public MainForm()
        {
            CheckForIllegalCrossThreadCalls = false;
            StartPosition = 0;
            InitializeComponent();
            mySettingZone = new SettingZone();
            myZoneA = new ZoneA();
            myZoneB = new ZoneB();
            myZoneC = new ZoneC();
            myZoneD = new ZoneD();
            myZoneE = new ZoneE();
            trackBarOpacityEnter.Value = 2;
            trackBarOpacityLeave.Value = 1;
            enterOpacity = 0.35;
            leaveOpacity = 0.15;
            myZoneA.Opacity = leaveOpacity;
            myZoneB.Opacity = leaveOpacity;
            myZoneC.Opacity = leaveOpacity;
            myZoneD.Opacity = leaveOpacity;
            myZoneE.Opacity = leaveOpacity;
            timeOutThread = new Thread(new ThreadStart(CalculateTimeOut));

            gestureCollection = new GestureCollection();
            PopulateKeyCodeHashTable();
            gestureInfo = new GestureInfo();
            gestureInfo.Hide();
            GestureCommit();
            textBoxGestureTimeout_TextChanged(null, null);
        }

        public void ShowGestureInfo(bool argState)
        {
            if (argState)
                gestureInfo.Show();
            else
                gestureInfo.Hide();
        }

        // ************* register zone event handlers *************
        private void SetZoneEventHandlers()
        {
            myZoneA.MouseEnter += new EventHandler(myZoneA_MouseEnter);
            myZoneA.MouseLeave += new EventHandler(myZoneA_MouseLeave);
            myZoneB.MouseEnter += new EventHandler(myZoneB_MouseEnter);
            myZoneB.MouseLeave += new EventHandler(myZoneB_MouseLeave);
            myZoneC.MouseEnter += new EventHandler(myZoneC_MouseEnter);
            myZoneC.MouseLeave += new EventHandler(myZoneC_MouseLeave);
            myZoneD.MouseEnter += new EventHandler(myZoneD_MouseEnter);
            myZoneD.MouseLeave += new EventHandler(myZoneD_MouseLeave);
            myZoneE.MouseEnter += new EventHandler(myZoneE_MouseEnter);
            myZoneE.MouseLeave += new EventHandler(myZoneE_MouseLeave);
        }

        // ************* adds all the virtual key codes into a hash table *************
        private void PopulateKeyCodeHashTable()
        {
            keyCode.Add("VK_0", "0x30");
            keyCode.Add("VK_1", "0x31");
            keyCode.Add("VK_2", "0x32");
            keyCode.Add("VK_3", "0x33");
            keyCode.Add("VK_4", "0x34");
            keyCode.Add("VK_5", "0x35");
            keyCode.Add("VK_6", "0x36");
            keyCode.Add("VK_7", "0x37");
            keyCode.Add("VK_8", "0x38");
            keyCode.Add("VK_9", "0x49");

            keyCode.Add("VK_TAB", "0x09");

            keyCode.Add("VK_A", "0x41");
            keyCode.Add("VK_B", "0x42");
            keyCode.Add("VK_C", "0x43");
            keyCode.Add("VK_D", "0x44");
            keyCode.Add("VK_E", "0x45");
            keyCode.Add("VK_F", "0x46");
            keyCode.Add("VK_G", "0x47");
            keyCode.Add("VK_H", "0x48");
            keyCode.Add("VK_I", "0x49");
            keyCode.Add("VK_J", "0x4A");
            keyCode.Add("VK_K", "0x4B");
            keyCode.Add("VK_L", "0x4C");
            keyCode.Add("VK_M", "0x4D");
            keyCode.Add("VK_N", "0x4E");
            keyCode.Add("VK_O", "0x4F");
            keyCode.Add("VK_P", "0x50");
            keyCode.Add("VK_Q", "0x51");
            keyCode.Add("VK_R", "0x52");
            keyCode.Add("VK_S", "0x53");
            keyCode.Add("VK_T", "0x54");
            keyCode.Add("VK_U", "0x55");
            keyCode.Add("VK_V", "0x56");
            keyCode.Add("VK_W", "0x57");
            keyCode.Add("VK_X", "0x58");
            keyCode.Add("VK_Y", "0x59");
            keyCode.Add("VK_Z", "0x5A");

            

            // and populate the dropdown menus with the keys
            IDictionaryEnumerator myEnum = keyCode.GetEnumerator();
            while (myEnum.MoveNext())
            {
                comboBox1A.Items.Add(myEnum.Key);
                comboBox1B.Items.Add(myEnum.Key);
                comboBox1C.Items.Add(myEnum.Key);
                comboBox2A.Items.Add(myEnum.Key);
                comboBox2B.Items.Add(myEnum.Key);
                comboBox2C.Items.Add(myEnum.Key);
                comboBox3A.Items.Add(myEnum.Key);
                comboBox3B.Items.Add(myEnum.Key);
                comboBox3C.Items.Add(myEnum.Key);
                comboBox4A.Items.Add(myEnum.Key);
                comboBox4B.Items.Add(myEnum.Key);
                comboBox4C.Items.Add(myEnum.Key);
            }

            // set default keys
            comboBox1A.SelectedIndex = 21;
            comboBox1B.SelectedIndex = 30;
            comboBox1C.SelectedIndex = 22;
            comboBox2A.SelectedIndex = 12;
            comboBox2B.SelectedIndex = 30;
            comboBox2C.SelectedIndex = 11;
            comboBox3A.SelectedIndex = 4;
            comboBox3B.SelectedIndex = 1;
            comboBox3C.SelectedIndex = 5;
            comboBox4A.SelectedIndex = 2;
            comboBox4B.SelectedIndex = 1;
            comboBox4C.SelectedIndex = 3;
        }

        #region gestures

        // ************* assigns the chosen keys to a gesture *************
        private void AssignGestures(string argKey, int argZoneCode)
        {

            // get the matching key code from the hash table
            IDictionaryEnumerator myEnum = keyCode.GetEnumerator();
            while (myEnum.MoveNext())
            {
                if (myEnum.Key.Equals(argKey))
                {
                    switch (argZoneCode)
                    {
                        case 0:
                            break;
                        case 11:
                            gesture1A.SetKey(myEnum.Value.ToString());
                            gesture1A.SetName(argKey);
                            gestureCollection.AddGesture(gesture1A);
                            break;
                        case 12:
                            gesture1B.SetKey(myEnum.Value.ToString());
                            gesture1B.SetName(argKey);
                            gestureCollection.AddGesture(gesture1B);
                            break;
                        case 13:
                            gesture1C.SetKey(myEnum.Value.ToString());
                            gesture1C.SetName(argKey);
                            gestureCollection.AddGesture(gesture1C);
                            break;

                        case 21:
                            gesture2A.SetKey(myEnum.Value.ToString());
                            gesture2A.SetName(argKey);
                            gestureCollection.AddGesture(gesture2A);
                            break;
                        case 22:
                            gesture2B.SetKey(myEnum.Value.ToString());
                            gesture2B.SetName(argKey);
                            gestureCollection.AddGesture(gesture2B);
                            break;
                        case 23:
                            gesture2C.SetKey(myEnum.Value.ToString());
                            gesture2C.SetName(argKey);
                            gestureCollection.AddGesture(gesture2C);
                            break;

                        case 31:
                            gesture3A.SetKey(myEnum.Value.ToString());
                            gesture3A.SetName(argKey);
                            gestureCollection.AddGesture(gesture3A);
                            break;
                        case 32:
                            gesture3B.SetKey(myEnum.Value.ToString());
                            gesture3B.SetName(argKey);
                            gestureCollection.AddGesture(gesture3B);
                            break;
                        case 33:
                            gesture3C.SetKey(myEnum.Value.ToString());
                            gesture3C.SetName(argKey);
                            gestureCollection.AddGesture(gesture3C);
                            break;

                        case 41:
                            gesture4A.SetKey(myEnum.Value.ToString());
                            gesture4A.SetName(argKey);
                            gestureCollection.AddGesture(gesture4A);
                            break;
                        case 42:
                            gesture4B.SetKey(myEnum.Value.ToString());
                            gesture4B.SetName(argKey);
                            gestureCollection.AddGesture(gesture4B);
                            break;
                        case 43:
                            gesture4C.SetKey(myEnum.Value.ToString());
                            gesture4C.SetName(argKey);
                            gestureCollection.AddGesture(gesture4C);
                            break;
                    }

                    break;
                }
            }
        }

        // ************* checks the gesture string to see if it is valid *************
        private bool CheckGestureString()
        {
            gestureAsString = "";
            sequenceOk = true;

            // convert to string
            for (int i = 0; i < 4; i++)
            {
                gestureAsString += gestureString[i].ToString();
            }

            // not yet a gesture... but might be
            if (gestureString[0] == 0)
                sequenceOk = false;
            if (gestureString[1] == 0)
                sequenceOk = false;
            if (gestureString[2] == 0)
                sequenceOk = false;

            if (gestureString[0] == 5)
                if (gestureString[1] < 5)
                    if (gestureString[2] < 5)
                        if (gestureString[3] == 0)
                            sequenceOk = false;


            // if two zones are together then reset e.g. #22#
            if (gestureString[0] > 0)
                if (gestureString[0] == gestureString[1])
                {
                    AddText("Invalid gesture A");
                    ResetGesture();
                    sequenceOk = false;
                }
            if (gestureString[1] > 0)
                if (gestureString[1] == gestureString[2])
                {
                    AddText("Invalid gesture b");
                    ResetGesture();
                    sequenceOk = false;
                }
            if (gestureString[2] > 0)
                if (gestureString[2] == gestureString[3])
                {
                    AddText("Invalid gesture c");
                    ResetGesture();
                    sequenceOk = false;
                }

            // if gesture string does not end with a 5
            if (gestureString[0] == 5)
                if (gestureString[1] > 0)
                    if (gestureString[2] > 0)
                        if (gestureString[3] != 5 && gestureString[3] != 0)
                        {
                            AddText("Invalid gesture d");
                            ResetGesture();
                            sequenceOk = false;
                        }

            if (sequenceOk)
            {
                FindGesture(gestureAsString);
            }

            return sequenceOk;
        }

        // ************* search through the list of gestures *************
        private void FindGesture(string argGesture)
        {
            //AddText("*yay a gesture!*");
            AddText("Gesture string is : " + argGesture);

            gestureCollection.ExecuteGesture(argGesture);
            ResetGesture();
        }

        // ************* thread to calculate if the gesture has timed out *************
        private void CalculateTimeOut()
        {
            timeOutTimer = new TimeSpan(0, 0, 0, 0, gestureTimeOutTimer);
            AddText("Given timeout: " + gestureTimeOutTimer + " Time measured: " + DateTime.Now.Subtract(startTimeOut));

            //                 test for timeout                               test that timeout is legit
            if (((DateTime.Now - startTimeOut) > timeOutTimer) && DateTime.Now.Subtract(startTimeOut) > zeroTime)
            {
                AddText("*GESTURE TIME OUT*");
                ResetGesture();
            }
        }

        // ************* reset the gesture *************
        private void ResetGesture()
        {
            DeActivateZones();
            gestureString = new int[4] { 0, 0, 0, 0};
            zoneCounter = 0;
            gestureStart = false;
            AddText("*GESTURE RESET*");

            // reset all zone text labels
            myZoneA.UpdateLabel(" ");  // Some missing...? Theyr not even made on the other zones. Needed?

        }

        // ************* reset all zones to be inactive *************
        private void DeActivateZones()
        {
            zoneAActive = false;
            zoneBActive = false;
            zoneCActive = false;
            zoneDActive = false;
            zoneEActive = false;
            activeZone = 0;
        }

        // ************* counts the zones to find a gesture sequence *************
        private void GestureCounter()
        {
            switch (activeZone)
            {
                // no zone
                case 0:
                    break;

                // zone a
                case 1:
                    //Console.WriteLine("zone a!");
                    AddText("ZONE A " + zoneCounter);
                    DeActivateZones();
                    zoneAActive = true;

                    zoneCounter++;
                    if (zoneCounter > 4)
                    {
                        ResetGesture();
                    }

                    gestureString[zoneCounter - 1] = (int)GestureZone.ZONEA;
                    //AddText("enum test :" + (int)GestureZone.ZONEA);
                    AddText("gesture string :" + gestureString[0] + gestureString[1] + gestureString[2] + gestureString[3]);
                    CheckGestureString();
                    break;

                // zone b
                case 2:
                    //Console.WriteLine("zone b!");
                    AddText("ZONE B " + zoneCounter);
                    DeActivateZones();
                    zoneAActive = true;

                    // show what happens at zone 1 and 3
                    myZoneA.UpdateLabel(textBox2A.Text);

                    zoneCounter++;
                    if (zoneCounter > 4)
                    {
                        ResetGesture();
                    }

                    gestureString[zoneCounter - 1] = (int)GestureZone.ZONEB;
                    //AddText("enum test :" + (int)GestureZone.ZONEB);
                    AddText("gesture string :" + gestureString[0] + gestureString[1] + gestureString[2] + gestureString[3]);
                    CheckGestureString();
                    break;

                // zone c
                case 3:
                    //Console.WriteLine("zone c!");
                    AddText("ZONE C " + zoneCounter);
                    DeActivateZones();
                    zoneAActive = true;

                    zoneCounter++;
                    if (zoneCounter > 4)
                    {
                        ResetGesture();
                    }

                    gestureString[zoneCounter - 1] = (int)GestureZone.ZONEC;
                    //AddText("enum test :" + (int)GestureZone.ZONEC);
                    AddText("gesture string :" + gestureString[0] + gestureString[1] + gestureString[2] + gestureString[3]);
                    CheckGestureString();
                    break;

                // zone d
                case 4:
                    //Console.WriteLine("zone d!");
                    AddText("ZONE D " + zoneCounter);
                    DeActivateZones();
                    zoneAActive = true;

                    zoneCounter++;
                    if (zoneCounter > 4)
                    {
                        ResetGesture();
                    }

                    gestureString[zoneCounter - 1] = (int)GestureZone.ZONED;
                    //AddText("enum test :" + (int)GestureZone.ZONED);
                    AddText("gesture string :" + gestureString[0] + gestureString[1] + gestureString[2] + gestureString[3]);
                    CheckGestureString();
                    break;



                // zone e
                case 5:
                    //Console.WriteLine("zone e!");
                    AddText("ZONE E " + zoneCounter);
                    DeActivateZones();
                    zoneEActive = true;
                    zoneCounter++;

                    if (zoneCounter > 4)
                    {
                        ResetGesture();
                    }

                    if (!gestureStart)
                    {
                        gestureStart = true;
                        zoneCounter = 0;
                    }

                    else
                    {
                        gestureString[zoneCounter - 1] = (int)GestureZone.ZONEE;
                    }
                    //AddText("gesture enum test :" + (int)GestureZone.ZONEE);
                    AddText("gesture string :" + gestureString[0] + gestureString[1] + gestureString[2] + gestureString[3]);
                    CheckGestureString();
                    break;
            }
        }

        #endregion

        #region button, slider, etc events

        public void ShowZones(bool argState)
        {
            if (argState)
            {
                myZoneA.Show();
                myZoneB.Show();
                myZoneC.Show();
                myZoneD.Show();
                myZoneE.Show();
            }
            else
            {
                if (myZoneA.Visible) myZoneA.Hide();
                if (myZoneB.Visible) myZoneB.Hide();
                if (myZoneC.Visible) myZoneC.Hide();
                if (myZoneD.Visible) myZoneD.Hide();
                if (myZoneE.Visible) myZoneE.Hide();
            }
        }

        // ************* sets the zone to the current layout *************
        private void buttonApplyZone_Click(object sender, EventArgs e)
        {          
            myZoneA.SetZone(mySettingZone.GetPosition());
            myZoneB.SetZone(mySettingZone.GetPosition());
            myZoneC.SetZone(mySettingZone.GetPosition());
            myZoneD.SetZone(mySettingZone.GetPosition());
            myZoneE.SetZone(mySettingZone.GetPosition());
            myZoneA.Show();
            myZoneB.Show();
            myZoneC.Show();
            myZoneD.Show();
            myZoneE.Show();
            mySettingZone.Hide();
            SetZoneEventHandlers();
        }

        // ************* removes the zone from the current layout *************
        private void buttonHideZone_Click(object sender, EventArgs e)
        {
         //   myZoneA.SetZone(mySettingZone.GetPosition());
         //   myZoneB.SetZone(mySettingZone.GetPosition());
         //   myZoneC.SetZone(mySettingZone.GetPosition());
         //   myZoneD.SetZone(mySettingZone.GetPosition());
         //   myZoneE.SetZone(mySettingZone.GetPosition());
            myZoneA.Hide();
            myZoneB.Hide();
            myZoneC.Hide();
            myZoneD.Hide();
            myZoneE.Hide();
           // mySettingZone.Hide();
           // SetZoneEventHandlers();  //opposite handler shut down for hiding?
        }

        // ************* displays the setting zone for positioning *************
        private void buttonSettingZone_Click(object sender, EventArgs e)
        {
            if (mySettingZone.Visible)
            {
                mySettingZone.Hide();
            }
            else
            {                
                mySettingZone.Show();
            }
        }

        // ************* sets transparency of zones on enter *************
        private void trackBarOpacityEnter_Scroll(object sender, EventArgs e)
        {
            TrackBar tb = (TrackBar)sender;
            enterOpacity = ((double)tb.Value / tb.Maximum);
            Console.WriteLine("enter " + ((double)tb.Value / tb.Maximum));
        }

        // ************* sets transparency of zones on leave *************
        private void trackBarOpacityLeave_Scroll(object sender, EventArgs e)
        {
            TrackBar tb = (TrackBar)sender;
            leaveOpacity = ((double)tb.Value / tb.Maximum);

            myZoneA.Opacity = leaveOpacity;
            myZoneB.Opacity = leaveOpacity;
            myZoneC.Opacity = leaveOpacity;
            myZoneD.Opacity = leaveOpacity;
            myZoneE.Opacity = leaveOpacity;

            Console.WriteLine("leave " + ((double)tb.Value / tb.Maximum));
        }

        // ************* sets gesture timer *************
        private void textBoxGestureTimeout_TextChanged(object sender, EventArgs e)
        {
            try
            {
                gestureTimeOutTimer = Int16.Parse(textBoxGestureTimeout.Text);
            }
            catch (System.FormatException SysFormEx)
            {
                gestureTimeOutTimer = 0;
            }
            catch (Exception Ex)
            {
                AddText("Unexpected error parsing gesture timeout timer.");
            }
        }

        // ************* adds a new line of text to the textbox *************
        private void AddText(string message)
        {
            if (message == null)
            {
                return;
            }

            int length = textBoxInfo.Text.Length + message.Length;
            if (length >= textBoxInfo.MaxLength)
            {
                textBoxInfo.Text = "";
            }

            if (!message.EndsWith("\r\n"))
            {
                message += "\r\n";
            }

            textBoxInfo.Text = textBoxInfo.Text + message;

            textBoxInfo.SelectionStart = textBoxInfo.Text.Length;
            textBoxInfo.ScrollToCaret();
        }

        private void comboBox1A_SelectedIndexChanged(object sender, EventArgs e)
        {
            AssignGestures(comboBox1A.SelectedItem.ToString(), 11);
        }

        private void comboBox1B_SelectedIndexChanged(object sender, EventArgs e)
        {
            AssignGestures(comboBox1B.SelectedItem.ToString(), 12);
        }

        private void comboBox1C_SelectedIndexChanged(object sender, EventArgs e)
        {
            AssignGestures(comboBox1C.SelectedItem.ToString(), 13);
        }

        private void comboBox2A_SelectedIndexChanged(object sender, EventArgs e)
        {
            AssignGestures(comboBox2A.SelectedItem.ToString(), 21);
        }

        private void comboBox2B_SelectedIndexChanged(object sender, EventArgs e)
        {
            AssignGestures(comboBox2B.SelectedItem.ToString(), 22);
        }

        private void comboBox2C_SelectedIndexChanged(object sender, EventArgs e)
        {
            AssignGestures(comboBox2C.SelectedItem.ToString(), 23);
        }

        private void comboBox3A_SelectedIndexChanged(object sender, EventArgs e)
        {
            AssignGestures(comboBox3A.SelectedItem.ToString(), 31);
        }

        private void comboBox3B_SelectedIndexChanged(object sender, EventArgs e)
        {
            AssignGestures(comboBox3B.SelectedItem.ToString(), 32);
        }

        private void comboBox3C_SelectedIndexChanged(object sender, EventArgs e)
        {
            AssignGestures(comboBox3C.SelectedItem.ToString(), 33);
        }

        private void comboBox4A_SelectedIndexChanged(object sender, EventArgs e)
        {
            AssignGestures(comboBox4A.SelectedItem.ToString(), 41);
        }

        private void comboBox4B_SelectedIndexChanged(object sender, EventArgs e)
        {
            AssignGestures(comboBox4B.SelectedItem.ToString(), 42);
        }

        private void comboBox4C_SelectedIndexChanged(object sender, EventArgs e)
        {
            AssignGestures(comboBox4C.SelectedItem.ToString(), 43);
        }
        #endregion
    
        #region zone mouse enter/leave

        // ************* ZONE E enter **************
        private void myZoneE_MouseEnter(object sender, EventArgs e)
        {
  
            CalculateTimeOut();
            myZoneE.Opacity = enterOpacity;

            activeZone = (int)GestureZone.ZONEE;
            GestureCounter();
            
        }

        // ************* ZONE E leave **************
        private void myZoneE_MouseLeave(object sender, EventArgs e)
        {           
            myZoneE.Opacity = leaveOpacity;
            
            startTimeOut = DateTime.Now;
            gestureStart = true;
            activeZone = (int)GestureZone.ZONEE;
            GestureCounter();  
        }

        // ************* ZONE D enter **************
        private void myZoneD_MouseEnter(object sender, EventArgs e)
        {
            myZoneD.Opacity = enterOpacity;

            if (gestureStart)
            {
                activeZone = (int)GestureZone.ZONED;
                GestureCounter();
            }
        }

        // ************* ZONE D leave **************
        private void myZoneD_MouseLeave(object sender, EventArgs e)
        {
            myZoneD.Opacity = leaveOpacity;
        }

        // ************* ZONE C enter **************
        private void myZoneC_MouseEnter(object sender, EventArgs e)
        {
            myZoneC.Opacity = enterOpacity;
 
            if (gestureStart)
            {
                activeZone = (int)GestureZone.ZONEC;
                GestureCounter();
            }
        }

        // ************* ZONE C leave **************
        private void myZoneC_MouseLeave(object sender, EventArgs e)
        {
            myZoneC.Opacity = leaveOpacity;
        }

        // ************* ZONE B enter **************
        private void myZoneB_MouseEnter(object sender, EventArgs e)
        {
            myZoneB.Opacity = enterOpacity;

            if (gestureStart)
            {
                activeZone = (int)GestureZone.ZONEB;
                GestureCounter();
            }
        }

        // ************* ZONE B leave **************
        private void myZoneB_MouseLeave(object sender, EventArgs e)
        {
            myZoneB.Opacity = leaveOpacity;
        }

        // ************* ZONE A enter **************
        private void myZoneA_MouseEnter(object sender, EventArgs e)
        {
            myZoneA.Opacity = enterOpacity;

            if (gestureStart)
            {
                activeZone = (int)GestureZone.ZONEA;
                GestureCounter();
            }
        }

        // ************* ZONE A leave **************
        private void myZoneA_MouseLeave(object sender, EventArgs e)
        {
            myZoneA.Opacity = leaveOpacity;          
        }

        #endregion

        #region gesture info

        // ************* update the gesture info chart **************
        private void buttonGestureCommit_Click(object sender, EventArgs e)
        {
            GestureCommit();
        }

        private void GestureCommit()
        {
            string[] labelArray = new string[12];

            labelArray[0] = textBox1A.Text;
            labelArray[1] = textBox1B.Text;
            labelArray[2] = textBox1C.Text;
            labelArray[3] = textBox2A.Text;
            labelArray[4] = textBox2B.Text;
            labelArray[5] = textBox2C.Text;
            labelArray[6] = textBox3A.Text;
            labelArray[7] = textBox3B.Text;
            labelArray[8] = textBox3C.Text;
            labelArray[9] = textBox4A.Text;
            labelArray[10] = textBox4B.Text;
            labelArray[11] = textBox4C.Text;

            gestureInfo.UpdateLabels(labelArray);

            AssignGestures(comboBox1A.SelectedItem.ToString(), 11);
            AssignGestures(comboBox1B.SelectedItem.ToString(), 12);
            AssignGestures(comboBox1C.SelectedItem.ToString(), 13);
            AssignGestures(comboBox2A.SelectedItem.ToString(), 21);
            AssignGestures(comboBox2B.SelectedItem.ToString(), 22);
            AssignGestures(comboBox2C.SelectedItem.ToString(), 23);
            AssignGestures(comboBox3A.SelectedItem.ToString(), 31);
            AssignGestures(comboBox3B.SelectedItem.ToString(), 32);
            AssignGestures(comboBox3C.SelectedItem.ToString(), 33);
            AssignGestures(comboBox4A.SelectedItem.ToString(), 41);
            AssignGestures(comboBox4B.SelectedItem.ToString(), 42);
            AssignGestures(comboBox4C.SelectedItem.ToString(), 43);
        }

        private void checkBoxShowGestureInfo_CheckedChanged(object sender, EventArgs e)
        {
            if (gestureInfo.Visible)
                gestureInfo.Hide();
            else
                gestureInfo.Show();
        }

        #endregion

        private void MainForm_Load(object sender, EventArgs e)
        {

        }





    }
}