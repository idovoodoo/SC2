using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using SnapClutch.SCTools;

namespace SnapClutch.G9CSharp
{
    public partial class G9TextFormCS : Form
    {
        int dwellTime = 700;
        DateTime start;
        TimeSpan dwellTimer;
        bool dwellStarted = false;
        bool useSwitch = false;
        Thread dwellThread;
        enum G9Keys
        {
            test, spc, abc, def,
            ghi, jkl, mno,
            pqrs, tuv, wxyz,
            del, cyc, symb, send, back
        };
        int activeKey = 0;
        enum Symbols { fullstop, exclamation, question };
        G9DictionaryCS g9Dictionary;
        G9IteratorCS g9Iter;
        //string baseText;
        string currentString;
        //bool symbolStarted = false;
        GazeOverlay gazeOverlay = new GazeOverlay();

        public G9TextFormCS(int argDwellTime)
        {
            dwellTime = argDwellTime;

            CheckForIllegalCrossThreadCalls = false;

            InitializeComponent();
            dwellTimer = new TimeSpan(0, 0, 0, 0, dwellTime);
            g9Dictionary = new G9DictionaryCS();
            g9Iter = g9Dictionary.Iterator();

            labelAbcPreview.Text = "";
            labelDefPreview.Text = "";
            labelGhiPreview.Text = "";
            labelJklPreview.Text = "";
            labelMnoPreview.Text = "";
            labelPqrsPreview.Text = "";
            labelTuvPreview.Text = "";
            labelWxyzPreview.Text = "";
            labelCyclePreview.Text = "";
            labelSendPreview.Text = "";
            labelSymbolPreview.Text = "";
            labelDelPreview.Text = "";

            labelAbcPreview2.Text = "";
            labelDefPreview2.Text = "";
            labelGhiPreview2.Text = "";
            labelJklPreview2.Text = "";
            labelMnoPreview2.Text = "";
            labelPqrsPreview2.Text = "";
            labelTuvPreview2.Text = "";
            labelWxyzPreview2.Text = "";
            labelCyclePreview2.Text = "";
            labelSendPreview2.Text = "";
            labelDelPreview2.Text = "";

            labelAbcPreview.Hide();
            labelDefPreview.Hide();
            labelGhiPreview.Hide();
            labelJklPreview.Hide();
            labelMnoPreview.Hide();
            labelPqrsPreview.Hide();
            labelTuvPreview.Hide();
            labelWxyzPreview.Hide();
            labelCyclePreview.Hide();
            labelSendPreview.Hide();
            labelSymbolPreview.Hide();
            labelDelPreview.Hide();

            labelAbcPreview2.Hide();
            labelDefPreview2.Hide();
            labelGhiPreview2.Hide();
            labelJklPreview2.Hide();
            labelMnoPreview2.Hide();
            labelPqrsPreview2.Hide();
            labelTuvPreview2.Hide();
            labelWxyzPreview2.Hide();
            labelDelPreview2.Hide();
            labelCyclePreview2.Hide();
            labelSendPreview2.Hide();
            labelSymbolPreview2.Hide();


            CreateGazeOverlays();
        }

        private void CreateGazeOverlays()
        {
            gazeOverlay.AddZone("abc", PointToScreen(buttonAbc.Location), buttonAbc.Width, buttonAbc.Height);
            gazeOverlay.AddZone("def", PointToScreen(buttonDef.Location), buttonDef.Width, buttonDef.Height);
            gazeOverlay.AddZone("ghi", PointToScreen(buttonGhi.Location), buttonGhi.Width, buttonGhi.Height);
            gazeOverlay.AddZone("jkl", PointToScreen(buttonJkl.Location), buttonJkl.Width, buttonJkl.Height);
            gazeOverlay.AddZone("mno", PointToScreen(buttonMno.Location), buttonMno.Width, buttonMno.Height);
            gazeOverlay.AddZone("pqrs", PointToScreen(buttonPqrs.Location), buttonPqrs.Width, buttonPqrs.Height);
            gazeOverlay.AddZone("tuv", PointToScreen(buttonTuv.Location), buttonTuv.Width, buttonTuv.Height);
            gazeOverlay.AddZone("wxyz", PointToScreen(buttonWxyz.Location), buttonWxyz.Width, buttonWxyz.Height);
            gazeOverlay.AddZone("delete", PointToScreen(buttonDelete.Location), buttonDelete.Width, buttonDelete.Height);
            gazeOverlay.AddZone("cycle", PointToScreen(buttonCycle.Location), buttonCycle.Width, buttonCycle.Height);
            gazeOverlay.AddZone("send", PointToScreen(buttonSend.Location), buttonSend.Width, buttonSend.Height);
            gazeOverlay.AddZone("symbols", PointToScreen(buttonSymbols.Location), buttonSymbols.Width, buttonSymbols.Height);
            gazeOverlay.AddZone("back", PointToScreen(buttonBackspace.Location), buttonBackspace.Width, buttonBackspace.Height);
            gazeOverlay.AddZone("startfinish", PointToScreen(buttonStartFinish.Location), buttonStartFinish.Width, buttonStartFinish.Height);


        }

        public GazeOverlay GetGazeOverlay()
        {
            return gazeOverlay;
        }

        // thread which checks if there is a dwell on the button
        private void CheckDwell()
        {
            while (dwellStarted)
            {
                if ((DateTime.Now - start) > dwellTimer)
                {
                    KeyActivated(activeKey);
                    //dwellStarted = false;

                    UpdatePreviews();

                    start = DateTime.Now;
                }
            }
        }

        private void UpdatePreviews()
        {
            labelAbcPreview.Text = currentString;
            labelDefPreview.Text = currentString;
            labelGhiPreview.Text = currentString;
            labelJklPreview.Text = currentString;
            labelMnoPreview.Text = currentString;
            labelPqrsPreview.Text = currentString;
            labelTuvPreview.Text = currentString;
            labelWxyzPreview.Text = currentString;
            labelCyclePreview.Text = currentString;
            labelSendPreview.Text = currentString;
            labelSymbolPreview.Text = currentString;
            labelDelPreview.Text = currentString;

            labelAbcPreview2.Text = currentString;
            labelDefPreview2.Text = currentString;
            labelGhiPreview2.Text = currentString;
            labelJklPreview2.Text = currentString;
            labelMnoPreview2.Text = currentString;
            labelPqrsPreview2.Text = currentString;
            labelTuvPreview2.Text = currentString;
            labelWxyzPreview2.Text = currentString;
            labelDelPreview2.Text = currentString;
            labelCyclePreview2.Text = currentString;
            labelSendPreview2.Text = currentString;
            labelSymbolPreview2.Text = currentString;

            textBoxSafeArea.Text = currentString;
        }

        #region button abc

        private void buttonAbc_MouseHover(object sender, EventArgs e)
        {
            CursorFactory.Create(Application.StartupPath + CursorSnapClutch.Pulse());
            //CursorFactory.CreatePulseCursor();

            activeKey = (int)G9Keys.abc;
            labelAbcPreview.Show();
            labelAbcPreview2.Show();
            if (!dwellStarted && !useSwitch)
            {
                start = DateTime.Now;
                dwellStarted = true;
                dwellThread = new Thread(new ThreadStart(CheckDwell));
                dwellThread.Start();
            }
            else
            {
            }
        }

        private void buttonAbc_MouseLeave(object sender, EventArgs e)
        {
            CursorFactory.Create(Application.StartupPath + CursorSnapClutch.BlankCursor());
            //CursorFactory.CreateBlankCursor();

            labelAbcPreview.Hide();
            labelAbcPreview2.Hide();
            if (dwellStarted)
            {
                dwellThread.Abort();
                dwellStarted = false;
            }
        }

        private void buttonAbc_Click(object sender, EventArgs e)
        {
            activeKey = (int)G9Keys.abc;
            KeyActivated(activeKey);
        }

        #endregion

        #region button def

        private void buttonDef_MouseHover(object sender, EventArgs e)
        {
            CursorFactory.Create(Application.StartupPath + CursorSnapClutch.Pulse());
            //CursorFactory.CreatePulseCursor();
            activeKey = (int)G9Keys.def;

            labelDefPreview.Show();
            labelDefPreview2.Show();
            if (!dwellStarted && !useSwitch)
            {
                start = DateTime.Now;
                dwellStarted = true;
                dwellThread = new Thread(new ThreadStart(CheckDwell));
                dwellThread.Start();
            }
            else
            {
            }
        }

        private void buttonDef_MouseLeave(object sender, EventArgs e)
        {
            CursorFactory.Create(Application.StartupPath + CursorSnapClutch.BlankCursor());
            //CursorFactory.CreateBlankCursor();
            labelDefPreview.Hide();
            labelDefPreview2.Hide();
            if (dwellStarted)
            {
                dwellThread.Abort();
                dwellStarted = false;
            }
        }

        private void buttonDef_Click(object sender, EventArgs e)
        {
            activeKey = (int)G9Keys.def;
            KeyActivated(activeKey);
        }

        #endregion

        #region button ghi

        private void buttonGhi_MouseHover(object sender, EventArgs e)
        {
            CursorFactory.Create(Application.StartupPath + CursorSnapClutch.Pulse());
            //CursorFactory.CreatePulseCursor();
            activeKey = (int)G9Keys.ghi;
            labelGhiPreview.Show();
            labelGhiPreview2.Show();
            if (!dwellStarted && !useSwitch)
            {
                start = DateTime.Now;
                dwellStarted = true;
                dwellThread = new Thread(new ThreadStart(CheckDwell));
                dwellThread.Start();
            }
            else
            {
            }
        }

        private void buttonGhi_MouseLeave(object sender, EventArgs e)
        {
            CursorFactory.Create(Application.StartupPath + CursorSnapClutch.BlankCursor());
            //CursorFactory.CreateBlankCursor();
            labelGhiPreview.Hide();
            labelGhiPreview2.Hide();
            if (dwellStarted)
            {
                dwellThread.Abort();
                dwellStarted = false;
            }
        }

        private void buttonGhi_Click(object sender, EventArgs e)
        {
            activeKey = (int)G9Keys.ghi;
            KeyActivated(activeKey);
        }

        #endregion

        #region button jkl

        private void buttonJkl_MouseHover(object sender, EventArgs e)
        {
            CursorFactory.Create(Application.StartupPath + CursorSnapClutch.Pulse());
            //CursorFactory.CreatePulseCursor();
            labelJklPreview.Show();
            labelJklPreview2.Show();
            activeKey = (int)G9Keys.jkl;

            if (!dwellStarted && !useSwitch)
            {
                start = DateTime.Now;
                dwellStarted = true;
                dwellThread = new Thread(new ThreadStart(CheckDwell));
                dwellThread.Start();
            }
            else
            {
            }
        }

        private void buttonJkl_MouseLeave(object sender, EventArgs e)
        {
            CursorFactory.Create(Application.StartupPath + CursorSnapClutch.BlankCursor());
            //CursorFactory.CreateBlankCursor();
            labelJklPreview.Hide();
            labelJklPreview2.Hide();
            if (dwellStarted)
            {
                dwellThread.Abort();
                dwellStarted = false;
            }
        }

        private void buttonJkl_Click(object sender, EventArgs e)
        {
            activeKey = (int)G9Keys.jkl;
            KeyActivated(activeKey);
        }

        #endregion

        #region button mno

        private void buttonMno_MouseHover(object sender, EventArgs e)
        {
            CursorFactory.Create(Application.StartupPath + CursorSnapClutch.Pulse());
            //CursorFactory.CreatePulseCursor();
            activeKey = (int)G9Keys.mno;
            labelMnoPreview.Show();
            labelMnoPreview2.Show();
            if (!dwellStarted && !useSwitch)
            {
                start = DateTime.Now;
                dwellStarted = true;
                dwellThread = new Thread(new ThreadStart(CheckDwell));
                dwellThread.Start();
            }
            else
            {
            }
        }

        private void buttonMno_MouseLeave(object sender, EventArgs e)
        {
            CursorFactory.Create(Application.StartupPath + CursorSnapClutch.BlankCursor());
            //CursorFactory.CreateBlankCursor();
            labelMnoPreview.Hide();
            labelMnoPreview2.Hide();
            if (dwellStarted)
            {
                dwellThread.Abort();
                dwellStarted = false;
            }
        }

        private void buttonMno_Click(object sender, EventArgs e)
        {
            activeKey = (int)G9Keys.mno;
            KeyActivated(activeKey);
        }

        #endregion

        #region button pqrs

        private void buttonPqrs_MouseHover(object sender, EventArgs e)
        {
            CursorFactory.Create(Application.StartupPath + CursorSnapClutch.Pulse());
            //CursorFactory.CreatePulseCursor();
            activeKey = (int)G9Keys.pqrs;
            labelPqrsPreview.Show();
            labelPqrsPreview2.Show();
            if (!dwellStarted && !useSwitch)
            {
                start = DateTime.Now;
                dwellStarted = true;
                dwellThread = new Thread(new ThreadStart(CheckDwell));
                dwellThread.Start();
            }
            else
            {
            }
        }

        private void buttonPqrs_MouseLeave(object sender, EventArgs e)
        {
            CursorFactory.Create(Application.StartupPath + CursorSnapClutch.BlankCursor());
            //CursorFactory.CreateBlankCursor();
            labelPqrsPreview.Hide();
            labelPqrsPreview2.Hide();
            if (dwellStarted)
            {
                dwellThread.Abort();
                dwellStarted = false;
            }
        }

        private void buttonPqrs_Click(object sender, EventArgs e)
        {
            activeKey = (int)G9Keys.pqrs;
            KeyActivated(activeKey);
        }

        #endregion

        #region button tuv

        private void buttonTuv_MouseHover(object sender, EventArgs e)
        {
            CursorFactory.Create(Application.StartupPath + CursorSnapClutch.Pulse());
            //CursorFactory.CreatePulseCursor(); 
            activeKey = (int)G9Keys.tuv;
            labelTuvPreview.Show();
            labelTuvPreview2.Show();
            if (!dwellStarted && !useSwitch)
            {
                start = DateTime.Now;
                dwellStarted = true;
                dwellThread = new Thread(new ThreadStart(CheckDwell));
                dwellThread.Start();
            }
            else
            {
            }
        }

        private void buttonTuv_MouseLeave(object sender, EventArgs e)
        {
            CursorFactory.Create(Application.StartupPath + CursorSnapClutch.BlankCursor());
            //CursorFactory.CreateBlankCursor();
            labelTuvPreview.Hide();
            labelTuvPreview2.Hide();
            if (dwellStarted)
            {
                dwellThread.Abort();
                dwellStarted = false;
            }
        }

        private void buttonTuv_Click(object sender, EventArgs e)
        {
            activeKey = (int)G9Keys.tuv;
            KeyActivated(activeKey);
        }

        #endregion

        #region button wxyz

        private void buttonWxyz_MouseHover(object sender, EventArgs e)
        {
            CursorFactory.Create(Application.StartupPath + CursorSnapClutch.Pulse());
            //CursorFactory.CreatePulseCursor(); 
            activeKey = (int)G9Keys.wxyz;
            labelWxyzPreview.Show();
            labelWxyzPreview2.Show();
            if (!dwellStarted && !useSwitch)
            {
                start = DateTime.Now;
                dwellStarted = true;
                dwellThread = new Thread(new ThreadStart(CheckDwell));
                dwellThread.Start();
            }
            else
            {
            }
        }

        private void buttonWxyz_MouseLeave(object sender, EventArgs e)
        {
            CursorFactory.Create(Application.StartupPath + CursorSnapClutch.BlankCursor());
            //CursorFactory.CreateBlankCursor();
            labelWxyzPreview.Hide();
            labelWxyzPreview2.Hide();
            if (dwellStarted)
            {
                dwellThread.Abort();
                dwellStarted = false;
            }
        }

        private void buttonWxyz_Click(object sender, EventArgs e)
        {
            activeKey = (int)G9Keys.wxyz;
            KeyActivated(activeKey);
        }

        #endregion

        #region button send

        private void buttonSend_MouseHover(object sender, EventArgs e)
        {
            CursorFactory.Create(Application.StartupPath + CursorSnapClutch.Pulse());
            //CursorFactory.CreatePulseCursor(); 
            activeKey = (int)G9Keys.spc;
            labelSendPreview.Show();
            labelSendPreview2.Show();
            if (!dwellStarted && !useSwitch)
            {
                start = DateTime.Now;
                dwellStarted = true;
                dwellThread = new Thread(new ThreadStart(CheckDwell));
                dwellThread.Start();
            }
            else
            {
            }
        }

        private void buttonSend_MouseLeave(object sender, EventArgs e)
        {
            CursorFactory.Create(Application.StartupPath + CursorSnapClutch.BlankCursor());
            //CursorFactory.CreateBlankCursor();
            labelSendPreview.Hide();
            labelSendPreview2.Hide();
            if (dwellStarted)
            {
                dwellThread.Abort();
                dwellStarted = false;
            }
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            activeKey = (int)G9Keys.spc;
            KeyActivated(activeKey);
        }

        #endregion

        #region button delete

        private void buttonDelete_MouseHover(object sender, EventArgs e)
        {
            CursorFactory.Create(Application.StartupPath + CursorSnapClutch.Pulse());
            //CursorFactory.CreatePulseCursor(); 
            activeKey = (int)G9Keys.del;
            labelDelPreview.Show();
            labelDelPreview2.Show();

            if (!dwellStarted && !useSwitch)
            {
                start = DateTime.Now;
                dwellStarted = true;
                dwellThread = new Thread(new ThreadStart(CheckDwell));
                dwellThread.Start();
            }
            else
            {
            }
        }

        private void buttonDelete_MouseLeave(object sender, EventArgs e)
        {
            CursorFactory.Create(Application.StartupPath + CursorSnapClutch.BlankCursor());
            //CursorFactory.CreateBlankCursor();
            labelDelPreview.Hide();
            labelDelPreview2.Hide();
            if (dwellStarted)
            {
                dwellThread.Abort();
                dwellStarted = false;
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            activeKey = (int)G9Keys.del;
            KeyActivated(activeKey);
        }

        #endregion

        #region button cycle

        private void buttonCycle_MouseHover(object sender, EventArgs e)
        {
            CursorFactory.Create(Application.StartupPath + CursorSnapClutch.Pulse());
            //CursorFactory.CreatePulseCursor(); 
            activeKey = (int)G9Keys.cyc;
            labelCyclePreview.Show();
            labelCyclePreview2.Show();
            if (!dwellStarted && !useSwitch)
            {
                start = DateTime.Now;
                dwellStarted = true;
                dwellThread = new Thread(new ThreadStart(CheckDwell));
                dwellThread.Start();
            }
            else
            {
            }
        }

        private void buttonCycle_MouseLeave(object sender, EventArgs e)
        {
            CursorFactory.Create(Application.StartupPath + CursorSnapClutch.BlankCursor());
            //CursorFactory.CreateBlankCursor();
            labelCyclePreview.Hide();
            labelCyclePreview2.Hide();
            if (dwellStarted)
            {
                dwellThread.Abort();
                dwellStarted = false;
            }
        }

        private void buttonCycle_Click(object sender, EventArgs e)
        {
            activeKey = (int)G9Keys.cyc;
            KeyActivated(activeKey);
        }

        #endregion

        #region button symbols

        private void buttonSymbols_MouseHover(object sender, EventArgs e)
        {
            CursorFactory.Create(Application.StartupPath + CursorSnapClutch.Pulse());
            //CursorFactory.CreatePulseCursor(); 
            activeKey = (int)G9Keys.symb;
            labelSymbolPreview.Show();
            labelSymbolPreview2.Show();

            if (!dwellStarted && !useSwitch)
            {
                start = DateTime.Now;
                dwellStarted = true;
                dwellThread = new Thread(new ThreadStart(CheckDwell));
                dwellThread.Start();
            }
            else
            {
            }
        }

        private void buttonSymbols_MouseLeave(object sender, EventArgs e)
        {
            CursorFactory.Create(Application.StartupPath + CursorSnapClutch.BlankCursor());
            //CursorFactory.CreateBlankCursor();
            labelSymbolPreview.Hide();
            labelSymbolPreview2.Hide();

            if (dwellStarted)
            {
                dwellThread.Abort();
                dwellStarted = false;
            }
        }

        private void buttonSymbols_Click(object sender, EventArgs e)
        {
            activeKey = (int)G9Keys.symb;
            KeyActivated(activeKey);
        }

        #endregion

        #region button start/finish

        private void buttonStartFinish_MouseHover(object sender, EventArgs e)
        {
            CursorFactory.Create(Application.StartupPath + CursorSnapClutch.Pulse());
            //CursorFactory.CreatePulseCursor(); 
            activeKey = (int)G9Keys.send;
            if (!dwellStarted && !useSwitch)
            {
                start = DateTime.Now;
                dwellStarted = true;
                dwellThread = new Thread(new ThreadStart(CheckDwell));
                dwellThread.Start();
            }
            else
            {
            }
        }

        private void buttonStartFinish_MouseLeave(object sender, EventArgs e)
        {
            CursorFactory.Create(Application.StartupPath + CursorSnapClutch.BlankCursor());
            //CursorFactory.CreateBlankCursor();
            if (dwellStarted)
            {
                dwellThread.Abort();
                dwellStarted = false;
            }
        }

        private void buttonStartFinish_Click(object sender, EventArgs e)
        {
            activeKey = (int)G9Keys.send;
            KeyActivated(activeKey);
        }

        #endregion

        #region button backspace

        private void buttonBack_MouseHover(object sender, EventArgs e)
        {
            CursorFactory.Create(Application.StartupPath + CursorSnapClutch.Pulse());
            //CursorFactory.CreatePulseCursor(); 
            activeKey = (int)G9Keys.back;
            if (!dwellStarted && !useSwitch)
            {
                start = DateTime.Now;
                dwellStarted = true;
                dwellThread = new Thread(new ThreadStart(CheckDwell));
                dwellThread.Start();
            }
            else
            {
            }
        }

        private void buttonBack_MouseLeave(object sender, EventArgs e)
        {
            CursorFactory.Create(Application.StartupPath + CursorSnapClutch.BlankCursor());
            //CursorFactory.CreateBlankCursor();
            if (dwellStarted)
            {
                dwellThread.Abort();
                dwellStarted = false;
            }
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            activeKey = (int)G9Keys.back;
            KeyActivated(activeKey);
        }

        #endregion

        public void SwitchPress()
        {
            KeyActivated(activeKey);

        }

        public void SetOverlayStatus(bool argStatus)
        {
            gazeOverlay.SetOverlayStatus(argStatus);
        }

        private void KeyActivated(int argKey)
        {

            SnapClutchSounds.Click2();
            //textBoxSafeArea.BackColor = Color.FromArgb(128, 255, 128);

            // cycle words
            if (argKey == (int)G9Keys.cyc)
            {

                Console.WriteLine("attempting cycle " + currentString);
                //if (g9Iter.MoveNext())
                //{
                try
                {

                    currentString = (string)g9Iter.Cycle();
                    Console.WriteLine("new word is " + currentString);
                    //textBoxEnteredText.Text = baseText + currentString;
                }
                catch
                {
                    Console.WriteLine("cant do it!");
                }
                //else
                //{                   
                //}
            }
            // delete letter
            else if (argKey == (int)G9Keys.del)
            {
                g9Iter.PreviousLevel();
                if (g9Iter.MoveNext())
                {
                    currentString = (string)g9Iter.Current;
                    //textBoxEnteredText.Text = baseText + currentString;
                }
            }
            // symbol... well just the question mark!
            else if (argKey == (int)G9Keys.symb)
            {
                //string symb = "";
                //g9Iter = g9Dictionary.iterator();                
                //g9Iter.nextLevel(1);

                //if (g9Iter.hasNext())
                //{
                //    currentString = (string)g9Iter.next();
                //}
                //else
                //{
                //    g9Iter.prevLevel();
                //}

                currentString += "? ";

                //currentString += symb + " ";
            }
            // space
            else if (argKey == (int)G9Keys.spc)
            {
                // send string to application and reset
                KeyEvent.SendString(currentString + " ");

                g9Iter = g9Dictionary.Iterator();
                //baseText += currentString + " ";
                currentString = "";
                //textBoxEnteredText.Text = baseText + currentString;
            }
            // enter
            else if (argKey == (int)G9Keys.send)
            {
                // send string to application and reset
                //KeyEvent.SendString(currentString + " ");

                //g9Iter = g9Dictionary.iterator();
                //baseText += currentString + " ";
                //currentString = "";
                //textBoxEnteredText.Text = baseText + currentString;
                KeyEvent.ReturnKeyDown();
                KeyEvent.ReturnKeyUp();


            }
            // backspace
            else if (argKey == (int)G9Keys.back)
            {
                KeyEvent.BackspaceKeyDown();
                KeyEvent.BackspaceKeyUp();
            }
            // lettered keys
            else
            {
                g9Iter.NextLevel(argKey);
                if (g9Iter.MoveNext())
                {
                    currentString = (string)g9Iter.Current;
                    //textBoxEnteredText.Text = baseText + currentString;

                }
                else
                {
                    g9Iter.PreviousLevel();
                }
            }
            UpdatePreviews();
            //textBoxSafeArea.BackColor = Color.FromArgb(255, 192, 128);
        }

        private void rectangleShapeSafeArea_MouseEnter(object sender, EventArgs e)
        {
            CursorFactory.Create(Application.StartupPath + CursorSnapClutch.BlankCursor());
            //CursorFactory.CreateBlankCursor();
        }

        public void UseSwitch(bool argSwitch)
        {
            useSwitch = argSwitch;
        }
    }
}


//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Text;
//using System.Windows.Forms;
//using System.Threading;
//using SnapClutch.SCTools;

//namespace SnapClutch.G9CSharp
//{
//    public partial class G9TextFormCS : Form
//    {
//        int dwellTime = 700;
//        DateTime start;
//        TimeSpan dwellTimer;
//        bool dwellStarted = false;
//        bool useSwitch = false;
//        Thread dwellThread;
//        enum G9Keys {test, spc,  abc, def,
//                   ghi,  jkl, mno,
//                   pqrs, tuv, wxyz,
//                   del,  cyc, symb, send, back};
//        int activeKey = 0;
//        enum Symbols { fullstop, exclamation, question };
//        G9DictionaryCS g9Dictionary;
//        G9IteratorCS g9Iter;
//        //string baseText;
//        string currentString;
//        //bool symbolStarted = false;
//        GazeOverlay gazeOverlay = new GazeOverlay();
        
//        public G9TextFormCS(int argDwellTime)
//        {
//            dwellTime = argDwellTime;

//            CheckForIllegalCrossThreadCalls = false;

//            InitializeComponent();
//            dwellTimer = new TimeSpan(0, 0, 0, 0, dwellTime);
//            g9Dictionary = new G9DictionaryCS();
//            g9Iter = g9Dictionary.Iterator();

//            labelAbcPreview.Text = "";
//            labelDefPreview.Text = "";
//            labelGhiPreview.Text = "";
//            labelJklPreview.Text = "";
//            labelMnoPreview.Text = "";
//            labelPqrsPreview.Text = "";
//            labelTuvPreview.Text = "";
//            labelWxyzPreview.Text = "";
//            labelCyclePreview.Text = "";
//            labelSendPreview.Text = "";
//            labelSymbolPreview.Text = "";
//            labelDelPreview.Text = "";

//            labelAbcPreview2.Text = "";
//            labelDefPreview2.Text = "";
//            labelGhiPreview2.Text = "";
//            labelJklPreview2.Text = "";
//            labelMnoPreview2.Text = "";
//            labelPqrsPreview2.Text = "";
//            labelTuvPreview2.Text = "";
//            labelWxyzPreview2.Text = "";
//            labelCyclePreview2.Text = "";
//            labelSendPreview2.Text = "";
//            labelDelPreview2.Text = "";

//            labelAbcPreview.Hide();
//            labelDefPreview.Hide();
//            labelGhiPreview.Hide();
//            labelJklPreview.Hide();
//            labelMnoPreview.Hide();
//            labelPqrsPreview.Hide();
//            labelTuvPreview.Hide();
//            labelWxyzPreview.Hide();
//            labelCyclePreview.Hide();
//            labelSendPreview.Hide();
//            labelSymbolPreview.Hide();
//            labelDelPreview.Hide();

//            labelAbcPreview2.Hide();
//            labelDefPreview2.Hide();
//            labelGhiPreview2.Hide();
//            labelJklPreview2.Hide();
//            labelMnoPreview2.Hide();
//            labelPqrsPreview2.Hide();
//            labelTuvPreview2.Hide();
//            labelWxyzPreview2.Hide();
//            labelDelPreview2.Hide();
//            labelCyclePreview2.Hide();
//            labelSendPreview2.Hide();
//            labelSymbolPreview2.Hide();


//            CreateGazeOverlays();
//        }

//        private void CreateGazeOverlays()
//        {
//            gazeOverlay.AddZone("abc", PointToScreen(buttonAbc.Location), buttonAbc.Width, buttonAbc.Height);
//            gazeOverlay.AddZone("def", PointToScreen(buttonDef.Location), buttonDef.Width, buttonDef.Height);
//            gazeOverlay.AddZone("ghi", PointToScreen(buttonGhi.Location), buttonGhi.Width, buttonGhi.Height);
//            gazeOverlay.AddZone("jkl", PointToScreen(buttonJkl.Location), buttonJkl.Width, buttonJkl.Height);
//            gazeOverlay.AddZone("mno", PointToScreen(buttonMno.Location), buttonMno.Width, buttonMno.Height);
//            gazeOverlay.AddZone("pqrs", PointToScreen(buttonPqrs.Location), buttonPqrs.Width, buttonPqrs.Height);
//            gazeOverlay.AddZone("tuv", PointToScreen(buttonTuv.Location), buttonTuv.Width, buttonTuv.Height);
//            gazeOverlay.AddZone("wxyz", PointToScreen(buttonWxyz.Location), buttonWxyz.Width, buttonWxyz.Height);
//            gazeOverlay.AddZone("delete", PointToScreen(buttonDelete.Location), buttonDelete.Width, buttonDelete.Height);
//            gazeOverlay.AddZone("cycle", PointToScreen(buttonCycle.Location), buttonCycle.Width, buttonCycle.Height);
//            gazeOverlay.AddZone("send", PointToScreen(buttonSend.Location), buttonSend.Width, buttonSend.Height);
//            gazeOverlay.AddZone("symbols", PointToScreen(buttonSymbols.Location), buttonSymbols.Width, buttonSymbols.Height);
//            gazeOverlay.AddZone("back", PointToScreen(buttonBackspace.Location), buttonBackspace.Width, buttonBackspace.Height);
//            gazeOverlay.AddZone("startfinish", PointToScreen(buttonStartFinish.Location), buttonStartFinish.Width, buttonStartFinish.Height);
            
            
//        }

//        public GazeOverlay GetGazeOverlay()
//        {
//            return gazeOverlay;
//        }

//        public void SetOverlayStatus(bool argStatus)
//        {
//            gazeOverlay.SetOverlayStatus(argStatus);
//        }

//        // thread which checks if there is a dwell on the button
//        private void CheckDwell()
//        {
//            while (dwellStarted)
//            {
//                if ((DateTime.Now - start) > dwellTimer)
//                {
//                    KeyActivated(activeKey);                 
//                    //dwellStarted = false;

//                    UpdatePreviews();

//                    start = DateTime.Now;
//                }
//            }
//        }

//        private void UpdatePreviews()
//        {
//            labelAbcPreview.Text = currentString;
//            labelDefPreview.Text = currentString;
//            labelGhiPreview.Text = currentString;
//            labelJklPreview.Text = currentString;
//            labelMnoPreview.Text = currentString;
//            labelPqrsPreview.Text = currentString;
//            labelTuvPreview.Text = currentString;
//            labelWxyzPreview.Text = currentString;
//            labelCyclePreview.Text = currentString;
//            labelSendPreview.Text = currentString;
//            labelSymbolPreview.Text = currentString;
//            labelDelPreview.Text = currentString;

//            labelAbcPreview2.Text = currentString;
//            labelDefPreview2.Text = currentString;
//            labelGhiPreview2.Text = currentString;
//            labelJklPreview2.Text = currentString;
//            labelMnoPreview2.Text = currentString;
//            labelPqrsPreview2.Text = currentString;
//            labelTuvPreview2.Text = currentString;
//            labelWxyzPreview2.Text = currentString;
//            labelDelPreview2.Text = currentString;
//            labelCyclePreview2.Text = currentString;
//            labelSendPreview2.Text = currentString;
//            labelSymbolPreview2.Text = currentString;

//            textBoxSafeArea.Text = currentString;
//        }

//        #region button abc

//        private void buttonAbc_MouseHover(object sender, EventArgs e)
//        {
//            CursorFactory.Create(Application.StartupPath + CursorSnapClutch.Pulse());
//            //CursorFactory.CreatePulseCursor();

//            activeKey = (int) G9Keys.abc;
//            labelAbcPreview.Show();
//            labelAbcPreview2.Show();
//            if (!dwellStarted && !useSwitch)
//            {
//                start = DateTime.Now;
//                dwellStarted = true;
//                dwellThread = new Thread(new ThreadStart(CheckDwell));
//                dwellThread.Start();
//            }
//            else
//            {
//            }
//        }

//        private void buttonAbc_MouseLeave(object sender, EventArgs e)
//        {
//            CursorFactory.Create(Application.StartupPath + CursorSnapClutch.BlankCursor());
//            //CursorFactory.CreateBlankCursor();

//            labelAbcPreview.Hide();
//            labelAbcPreview2.Hide();
//            if (dwellStarted)
//            {
//                dwellThread.Abort();
//                dwellStarted = false;
//            }
//        }

//        private void buttonAbc_Click(object sender, EventArgs e)
//        {
//            activeKey = (int)G9Keys.abc;
//            KeyActivated(activeKey);
//        }

//        #endregion

//        #region button def

//        private void buttonDef_MouseHover(object sender, EventArgs e)
//        {
//            CursorFactory.Create(Application.StartupPath + CursorSnapClutch.Pulse());
//            //CursorFactory.CreatePulseCursor();
//            activeKey = (int) G9Keys.def;

//            labelDefPreview.Show();
//            labelDefPreview2.Show();
//            if (!dwellStarted && !useSwitch)
//            {
//                start = DateTime.Now;
//                dwellStarted = true;
//                dwellThread = new Thread(new ThreadStart(CheckDwell));
//                dwellThread.Start();
//            }
//            else
//            {
//            }
//        }

//        private void buttonDef_MouseLeave(object sender, EventArgs e)
//        {
//            CursorFactory.Create(Application.StartupPath + CursorSnapClutch.BlankCursor());
//            //CursorFactory.CreateBlankCursor();
//            labelDefPreview.Hide();
//            labelDefPreview2.Hide();
//            if (dwellStarted)
//            {
//                dwellThread.Abort();
//                dwellStarted = false;
//            }
//        }

//        private void buttonDef_Click(object sender, EventArgs e)
//        {
//            activeKey = (int)G9Keys.def;
//            KeyActivated(activeKey);
//        }

//        #endregion

//        #region button ghi

//        private void buttonGhi_MouseHover(object sender, EventArgs e)
//        {
//            CursorFactory.Create(Application.StartupPath + CursorSnapClutch.Pulse());
//            //CursorFactory.CreatePulseCursor();
//            activeKey = (int)G9Keys.ghi;
//            labelGhiPreview.Show();
//            labelGhiPreview2.Show();
//            if (!dwellStarted && !useSwitch)
//            {
//                start = DateTime.Now;
//                dwellStarted = true;
//                dwellThread = new Thread(new ThreadStart(CheckDwell));
//                dwellThread.Start();
//            }
//            else
//            {
//            }
//        }

//        private void buttonGhi_MouseLeave(object sender, EventArgs e)
//        {
//            CursorFactory.Create(Application.StartupPath + CursorSnapClutch.BlankCursor());
//            //CursorFactory.CreateBlankCursor();
//            labelGhiPreview.Hide();
//            labelGhiPreview2.Hide();
//            if (dwellStarted)
//            {
//                dwellThread.Abort();
//                dwellStarted = false;
//            }
//        }

//        private void buttonGhi_Click(object sender, EventArgs e)
//        {
//            activeKey = (int)G9Keys.ghi;
//            KeyActivated(activeKey);
//        }

//        #endregion

//        #region button jkl

//        private void buttonJkl_MouseHover(object sender, EventArgs e)
//        {
//            CursorFactory.Create(Application.StartupPath + CursorSnapClutch.Pulse());
//            //CursorFactory.CreatePulseCursor();
//            labelJklPreview.Show();
//            labelJklPreview2.Show();
//            activeKey = (int)G9Keys.jkl;

//            if (!dwellStarted && !useSwitch)
//            {
//                start = DateTime.Now;
//                dwellStarted = true;
//                dwellThread = new Thread(new ThreadStart(CheckDwell));
//                dwellThread.Start();
//            }
//            else
//            {
//            }
//        }

//        private void buttonJkl_MouseLeave(object sender, EventArgs e)
//        {
//            CursorFactory.Create(Application.StartupPath + CursorSnapClutch.BlankCursor());
//            //CursorFactory.CreateBlankCursor();
//            labelJklPreview.Hide();
//            labelJklPreview2.Hide();
//            if (dwellStarted)
//            {
//                dwellThread.Abort();
//                dwellStarted = false;
//            }
//        }

//        private void buttonJkl_Click(object sender, EventArgs e)
//        {
//            activeKey = (int)G9Keys.jkl;
//            KeyActivated(activeKey);
//        }

//        #endregion

//        #region button mno

//        private void buttonMno_MouseHover(object sender, EventArgs e)
//        {            
//            CursorFactory.Create(Application.StartupPath + CursorSnapClutch.Pulse());
//            //CursorFactory.CreatePulseCursor();
//            activeKey = (int)G9Keys.mno;
//            labelMnoPreview.Show();
//            labelMnoPreview2.Show();
//            if (!dwellStarted && !useSwitch)
//            {
//                start = DateTime.Now;
//                dwellStarted = true;
//                dwellThread = new Thread(new ThreadStart(CheckDwell));
//                dwellThread.Start();
//            }
//            else
//            {
//            }
//        }

//        private void buttonMno_MouseLeave(object sender, EventArgs e)
//        {
//            CursorFactory.Create(Application.StartupPath + CursorSnapClutch.BlankCursor());
//            //CursorFactory.CreateBlankCursor();
//            labelMnoPreview.Hide();
//            labelMnoPreview2.Hide();
//            if (dwellStarted)
//            {
//                dwellThread.Abort();
//                dwellStarted = false;
//            }
//        }

//        private void buttonMno_Click(object sender, EventArgs e)
//        {
//            activeKey = (int)G9Keys.mno;
//            KeyActivated(activeKey);
//        }

//        #endregion

//        #region button pqrs

//        private void buttonPqrs_MouseHover(object sender, EventArgs e)
//        {
//            CursorFactory.Create(Application.StartupPath + CursorSnapClutch.Pulse());
//            //CursorFactory.CreatePulseCursor();
//            activeKey = (int)G9Keys.pqrs;
//            labelPqrsPreview.Show();
//            labelPqrsPreview2.Show();
//            if (!dwellStarted && !useSwitch)
//            {
//                start = DateTime.Now;
//                dwellStarted = true;
//                dwellThread = new Thread(new ThreadStart(CheckDwell));
//                dwellThread.Start();
//            }
//            else
//            {
//            }
//        }

//        private void buttonPqrs_MouseLeave(object sender, EventArgs e)
//        {
//            CursorFactory.Create(Application.StartupPath + CursorSnapClutch.BlankCursor());
//            //CursorFactory.CreateBlankCursor();
//            labelPqrsPreview.Hide();
//            labelPqrsPreview2.Hide();
//            if (dwellStarted)
//            {
//                dwellThread.Abort();
//                dwellStarted = false;
//            }
//        }

//        private void buttonPqrs_Click(object sender, EventArgs e)
//        {
//            activeKey = (int)G9Keys.pqrs;
//            KeyActivated(activeKey);
//        }

//        #endregion

//        #region button tuv

//        private void buttonTuv_MouseHover(object sender, EventArgs e)
//        {
//            CursorFactory.Create(Application.StartupPath + CursorSnapClutch.Pulse());
//            //CursorFactory.CreatePulseCursor(); 
//            activeKey = (int)G9Keys.tuv;
//            labelTuvPreview.Show();
//            labelTuvPreview2.Show();
//            if (!dwellStarted && !useSwitch)
//            {
//                start = DateTime.Now;
//                dwellStarted = true;
//                dwellThread = new Thread(new ThreadStart(CheckDwell));
//                dwellThread.Start();
//            }
//            else
//            {
//            }
//        }

//        private void buttonTuv_MouseLeave(object sender, EventArgs e)
//        {
//            CursorFactory.Create(Application.StartupPath + CursorSnapClutch.BlankCursor());
//            //CursorFactory.CreateBlankCursor();
//            labelTuvPreview.Hide();
//            labelTuvPreview2.Hide();
//            if (dwellStarted)
//            {
//                dwellThread.Abort();
//                dwellStarted = false;
//            }
//        }

//        private void buttonTuv_Click(object sender, EventArgs e)
//        {
//            activeKey = (int)G9Keys.tuv;
//            KeyActivated(activeKey);
//        }

//        #endregion

//        #region button wxyz

//        private void buttonWxyz_MouseHover(object sender, EventArgs e)
//        {
//            CursorFactory.Create(Application.StartupPath + CursorSnapClutch.Pulse());
//            //CursorFactory.CreatePulseCursor(); 
//            activeKey = (int)G9Keys.wxyz;
//            labelWxyzPreview.Show();
//            labelWxyzPreview2.Show();
//            if (!dwellStarted && !useSwitch)
//            {
//                start = DateTime.Now;
//                dwellStarted = true;
//                dwellThread = new Thread(new ThreadStart(CheckDwell));
//                dwellThread.Start();
//            }
//            else
//            {
//            }
//        }

//        private void buttonWxyz_MouseLeave(object sender, EventArgs e)
//        {
//            CursorFactory.Create(Application.StartupPath + CursorSnapClutch.BlankCursor());
//            //CursorFactory.CreateBlankCursor();
//            labelWxyzPreview.Hide();
//            labelWxyzPreview2.Hide();
//            if (dwellStarted)
//            {
//                dwellThread.Abort();
//                dwellStarted = false;
//            }
//        }

//        private void buttonWxyz_Click(object sender, EventArgs e)
//        {
//            activeKey = (int)G9Keys.wxyz;
//            KeyActivated(activeKey);
//        }

//        #endregion

//        #region button send

//        private void buttonSend_MouseHover(object sender, EventArgs e)
//        {
//            CursorFactory.Create(Application.StartupPath + CursorSnapClutch.Pulse());
//            //CursorFactory.CreatePulseCursor(); 
//            activeKey = (int)G9Keys.spc;
//            labelSendPreview.Show();
//            labelSendPreview2.Show();
//            if (!dwellStarted && !useSwitch)
//            {
//                start = DateTime.Now;
//                dwellStarted = true;
//                dwellThread = new Thread(new ThreadStart(CheckDwell));
//                dwellThread.Start();
//            }
//            else
//            {
//            }
//        }

//        private void buttonSend_MouseLeave(object sender, EventArgs e)
//        {
//            CursorFactory.Create(Application.StartupPath + CursorSnapClutch.BlankCursor());
//            //CursorFactory.CreateBlankCursor();
//            labelSendPreview.Hide();
//            labelSendPreview2.Hide();
//            if (dwellStarted)
//            {
//                dwellThread.Abort();
//                dwellStarted = false;
//            }
//        }

//        private void buttonSend_Click(object sender, EventArgs e)
//        {
//            activeKey = (int)G9Keys.spc;
//            KeyActivated(activeKey);
//        }

//        #endregion

//        #region button delete

//        private void buttonDelete_MouseHover(object sender, EventArgs e)
//        {
//            CursorFactory.Create(Application.StartupPath + CursorSnapClutch.Pulse());
//            //CursorFactory.CreatePulseCursor(); 
//            activeKey = (int)G9Keys.del;
//            labelDelPreview.Show();
//            labelDelPreview2.Show();

//            if (!dwellStarted && !useSwitch)
//            {
//                start = DateTime.Now;
//                dwellStarted = true;
//                dwellThread = new Thread(new ThreadStart(CheckDwell));
//                dwellThread.Start();
//            }
//            else
//            {
//            }
//        }

//        private void buttonDelete_MouseLeave(object sender, EventArgs e)
//        {
//            CursorFactory.Create(Application.StartupPath + CursorSnapClutch.BlankCursor());
//            //CursorFactory.CreateBlankCursor();
//            labelDelPreview.Hide();
//            labelDelPreview2.Hide();
//            if (dwellStarted)
//            {
//                dwellThread.Abort();
//                dwellStarted = false;
//            }
//        }

//        private void buttonDelete_Click(object sender, EventArgs e)
//        {
//            activeKey = (int)G9Keys.del;
//            KeyActivated(activeKey);
//        }

//        #endregion

//        #region button cycle

//        private void buttonCycle_MouseHover(object sender, EventArgs e)
//        {
//            CursorFactory.Create(Application.StartupPath + CursorSnapClutch.Pulse());
//            //CursorFactory.CreatePulseCursor(); 
//            activeKey = (int)G9Keys.cyc;
//            labelCyclePreview.Show();
//            labelCyclePreview2.Show();
//            if (!dwellStarted && !useSwitch)
//            {
//                start = DateTime.Now;
//                dwellStarted = true;
//                dwellThread = new Thread(new ThreadStart(CheckDwell));
//                dwellThread.Start();
//            }
//            else
//            {
//            }
//        }

//        private void buttonCycle_MouseLeave(object sender, EventArgs e)
//        {
//            CursorFactory.Create(Application.StartupPath + CursorSnapClutch.BlankCursor());
//            //CursorFactory.CreateBlankCursor();
//            labelCyclePreview.Hide();
//            labelCyclePreview2.Hide();
//            if (dwellStarted)
//            {
//                dwellThread.Abort();
//                dwellStarted = false;
//            }
//        }

//        private void buttonCycle_Click(object sender, EventArgs e)
//        {
//            activeKey = (int)G9Keys.cyc;
//            KeyActivated(activeKey);
//        }

//        #endregion

//        #region button symbols

//        private void buttonSymbols_MouseHover(object sender, EventArgs e)
//        {
//            CursorFactory.Create(Application.StartupPath + CursorSnapClutch.Pulse());
//            //CursorFactory.CreatePulseCursor(); 
//            activeKey = (int)G9Keys.symb;
//            labelSymbolPreview.Show();
//            labelSymbolPreview2.Show();

//            if (!dwellStarted && !useSwitch)
//            {
//                start = DateTime.Now;
//                dwellStarted = true;
//                dwellThread = new Thread(new ThreadStart(CheckDwell));
//                dwellThread.Start();
//            }
//            else
//            {
//            }
//        }

//        private void buttonSymbols_MouseLeave(object sender, EventArgs e)
//        {
//            CursorFactory.Create(Application.StartupPath + CursorSnapClutch.BlankCursor());
//            //CursorFactory.CreateBlankCursor();
//            labelSymbolPreview.Hide();
//            labelSymbolPreview2.Hide();

//            if (dwellStarted)
//            {
//                dwellThread.Abort();
//                dwellStarted = false;
//            }
//        }

//        private void buttonSymbols_Click(object sender, EventArgs e)
//        {
//            activeKey = (int)G9Keys.symb;
//            KeyActivated(activeKey);
//        }

//        #endregion

//        #region button start/finish

//        private void buttonStartFinish_MouseHover(object sender, EventArgs e)
//        {
//            CursorFactory.Create(Application.StartupPath + CursorSnapClutch.Pulse());
//            //CursorFactory.CreatePulseCursor(); 
//            activeKey = (int)G9Keys.send;
//            if (!dwellStarted && !useSwitch)
//            {
//                start = DateTime.Now;
//                dwellStarted = true;
//                dwellThread = new Thread(new ThreadStart(CheckDwell));
//                dwellThread.Start();
//            }
//            else
//            {
//            }
//        }

//        private void buttonStartFinish_MouseLeave(object sender, EventArgs e)
//        {
//            CursorFactory.Create(Application.StartupPath + CursorSnapClutch.BlankCursor());
//            //CursorFactory.CreateBlankCursor();
//            if (dwellStarted)
//            {
//                dwellThread.Abort();
//                dwellStarted = false;
//            }
//        }

//        private void buttonStartFinish_Click(object sender, EventArgs e)
//        {
//            activeKey = (int)G9Keys.send;
//            KeyActivated(activeKey);
//        }

//        #endregion

//        #region button backspace

//        private void buttonBack_MouseHover(object sender, EventArgs e)
//        {
//            CursorFactory.Create(Application.StartupPath + CursorSnapClutch.Pulse());
//            //CursorFactory.CreatePulseCursor(); 
//            activeKey = (int)G9Keys.back;
//            if (!dwellStarted && !useSwitch)
//            {
//                start = DateTime.Now;
//                dwellStarted = true;
//                dwellThread = new Thread(new ThreadStart(CheckDwell));
//                dwellThread.Start();
//            }
//            else
//            {
//            }
//        }

//        private void buttonBack_MouseLeave(object sender, EventArgs e)
//        {
//            CursorFactory.Create(Application.StartupPath + CursorSnapClutch.BlankCursor());
//            //CursorFactory.CreateBlankCursor();
//            if (dwellStarted)
//            {
//                dwellThread.Abort();
//                dwellStarted = false;
//            }
//        }

//        private void buttonBack_Click(object sender, EventArgs e)
//        {
//            activeKey = (int)G9Keys.back;
//            KeyActivated(activeKey);
//        }

//        #endregion

//        public void SwitchPress()
//        {
//            KeyActivated(activeKey);

//        }

//        private void KeyActivated(int argKey)
//        {
            
//            SnapClutchSounds.Click2();
//            //textBoxSafeArea.BackColor = Color.FromArgb(128, 255, 128);
            
//            // cycle words
//            if (argKey == (int)G9Keys.cyc)
//            {

//                Console.WriteLine ("attempting cycle " + currentString);
//                //if (g9Iter.MoveNext())
//                //{
//                    try
//                    {
                    
//                    currentString = (string)g9Iter.Cycle();
//                    Console.WriteLine("new word is " + currentString);
//                    //textBoxEnteredText.Text = baseText + currentString;
//                    }
//                    catch
//                    {
//                      Console.WriteLine("cant do it!");
//                    }
//                //else
//                //{                   
//                //}
//            }
//            // delete letter
//            else if (argKey == (int)G9Keys.del)
//            {
//                g9Iter.PreviousLevel();
//                if (g9Iter.MoveNext())
//                {
//                    currentString = (string)g9Iter.Current;
//                    //textBoxEnteredText.Text = baseText + currentString;
//                }
//            }
//            // symbol... well just the question mark!
//            else if (argKey == (int)G9Keys.symb)
//            {
//                //string symb = "";
//                //g9Iter = g9Dictionary.iterator();                
//                //g9Iter.nextLevel(1);

//                //if (g9Iter.hasNext())
//                //{
//                //    currentString = (string)g9Iter.next();
//                //}
//                //else
//                //{
//                //    g9Iter.prevLevel();
//                //}

//                currentString += "? ";

//                //currentString += symb + " ";
//            }
//            // space
//            else if (argKey == (int)G9Keys.spc)
//            {
//                // send string to application and reset
//                KeyEvent.SendString(currentString + " ");

//                g9Iter = g9Dictionary.Iterator();
//                //baseText += currentString + " ";
//                currentString = "";
//                //textBoxEnteredText.Text = baseText + currentString;
//            }
//            // enter
//            else if (argKey == (int)G9Keys.send)
//            {
//                // send string to application and reset
//                //KeyEvent.SendString(currentString + " ");

//                //g9Iter = g9Dictionary.iterator();
//                //baseText += currentString + " ";
//                //currentString = "";
//                //textBoxEnteredText.Text = baseText + currentString;
//                KeyEvent.ReturnKeyDown();
//                KeyEvent.ReturnKeyUp();


//            }
//            // backspace
//            else if (argKey == (int)G9Keys.back)
//            {
//                KeyEvent.BackspaceKeyDown();
//                KeyEvent.BackspaceKeyUp();
//            }
//            // lettered keys
//            else
//            {                
//                g9Iter.NextLevel(argKey);
//                if (g9Iter.MoveNext())
//                {
//                    currentString = (string)g9Iter.Current;
//                    //textBoxEnteredText.Text = baseText + currentString;
                   
//                }
//                else
//                {
//                    g9Iter.PreviousLevel();
//                }
//            }
//            UpdatePreviews();
//            //textBoxSafeArea.BackColor = Color.FromArgb(255, 192, 128);
//        }

//        private void rectangleShapeSafeArea_MouseEnter(object sender, EventArgs e)
//        {
//            CursorFactory.Create(Application.StartupPath + CursorSnapClutch.BlankCursor());
//            //CursorFactory.CreateBlankCursor();
//        }

//        public void UseSwitch(bool argSwitch)
//        {
//            useSwitch = argSwitch;
//        }
//    }
//}