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
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.IO;

namespace SnapClutch.SCTools
{

    public class CursorFactory
    {
        [DllImport("user32.dll", EntryPoint = "LoadCursorFromFileW",
             CharSet = CharSet.Unicode)]
        private static extern IntPtr LoadCursorFromFile(String str);

        [DllImport("user32.dll")]
        private static extern IntPtr LoadCursor(IntPtr hInstance, int lpCursorName);

        [DllImport("user32.dll")]
        static extern bool SetSystemCursor(IntPtr hcur, uint id);

        private const uint OCR_NORMAL = 32512;
        const string DummyCursorFilename = "Dummy.cur";

        public static Cursor Create(string filename)
        {
            IntPtr hCursor;
            Cursor result = null;

            try
            {
                hCursor = LoadCursorFromFile(filename);
                if (!IntPtr.Zero.Equals(hCursor))
                {
                    result = new Cursor(hCursor);
                    bool ret_val = SetSystemCursor(hCursor, OCR_NORMAL);
                }
                else
                {
                    throw new ApplicationException("Could not create cursor from file " + filename);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("error is: " + ex.ToString());
            }

            return result;
        }

    //    public static void CreatePulseCursor()
    //    {            
    //        // Write temp. file for generating our cursor from resources
            
    //        FileStream dummyFile = File.Create(DummyCursorFilename);
    //        dummyFile.Write(Properties.Resources.Pulse , 0, Properties.Resources.Pulse.Length);
    //        dummyFile.Close();

    //        CreateTheCursor();
    //    }

    //    public static void CreateDefaultCursor()
    //    {
    //        // Write temp. file for generating our cursor from resources

    //        FileStream dummyFile = File.Create(DummyCursorFilename);
    //        dummyFile.Write(Properties.Resources.Default, 0, Properties.Resources.Default.Length);
    //        dummyFile.Close();

    //        CreateTheCursor();
    //    }

    //    public static void CreateDragActiveCursor()
    //    {
    //        // Write temp. file for generating our cursor from resources

    //        FileStream dummyFile = File.Create(DummyCursorFilename);
    //        dummyFile.Write(Properties.Resources.DragActive, 0, Properties.Resources.DragActive.Length);
    //        dummyFile.Close();

    //        CreateTheCursor();
    //    }

    //    public static void CreateDragInActiveCursor()
    //    {
    //        // Write temp. file for generating our cursor from resources

    //        FileStream dummyFile = File.Create(DummyCursorFilename);
    //        dummyFile.Write(Properties.Resources.DragInactive, 0, Properties.Resources.DragInactive.Length);
    //        dummyFile.Close();

    //        CreateTheCursor();
    //    }

    //    public static void CreateEyeControlOffCursor()
    //    {
    //        // Write temp. file for generating our cursor from resources

    //        try
    //        {
    //            FileStream dummyFile = File.Create(DummyCursorFilename);
    //            dummyFile.Write(Properties.Resources.EyeControlOff, 0, Properties.Resources.EyeControlOff.Length);
    //            dummyFile.Close();
    //        }
    //        catch(Exception ex)
    //        {
    
    //            Console.WriteLine(ex.ToString());
    //        }
    //        CreateTheCursor();
    //    }

    //    public static void CreateEyeControlOffSmallCursor()
    //    {
    //        // Write temp. file for generating our cursor from resources

    //        FileStream dummyFile = File.Create(DummyCursorFilename);
    //        dummyFile.Write(Properties.Resources.EyeControlOffSmall, 0, Properties.Resources.EyeControlOffSmall.Length);
    //        dummyFile.Close();

    //        CreateTheCursor();
    //    }

    //    public static void CreateLocomotionCursor()
    //    {
    //        // Write temp. file for generating our cursor from resources

    //        FileStream dummyFile = File.Create(DummyCursorFilename);
    //        dummyFile.Write(Properties.Resources.Locomotion, 0, Properties.Resources.Locomotion.Length);
    //        dummyFile.Close();

    //        CreateTheCursor();
    //    }

    //    public static void CreateBlankCursor()
    //    {
    //        // Write temp. file for generating our cursor from resources

    //        FileStream dummyFile = File.Create(DummyCursorFilename);
    //        dummyFile.Write(Properties.Resources.BlankCursor, 0, Properties.Resources.BlankCursor.Length);
    //        dummyFile.Close();

    //        CreateTheCursor();
    //    }

    //    public static void CreateLeftDwellClickCursor()
    //    {
    //        // Write temp. file for generating our cursor from resources

    //        FileStream dummyFile = File.Create(DummyCursorFilename);
    //        dummyFile.Write(Properties.Resources.DwellClickLeft, 0, Properties.Resources.DwellClickLeft.Length);
    //        dummyFile.Close();

    //        CreateTheCursor();
    //    }

    //    public static void CreateRightDwellClickCursor()
    //    {
    //        // Write temp. file for generating our cursor from resources

    //        FileStream dummyFile = File.Create(DummyCursorFilename);
    //        dummyFile.Write(Properties.Resources.DwellClickRight, 0, Properties.Resources.DwellClickRight.Length);
    //        dummyFile.Close();

    //        CreateTheCursor();
    //    }

    //    private static void CreateTheCursor()
    //    {
    //        IntPtr hCursor;
    //        Cursor result = null;

    //        // Now load cursor and set it
    //        hCursor = LoadCursorFromFile(DummyCursorFilename);

    //        if (!IntPtr.Zero.Equals(hCursor))
    //        {
    //            result = new Cursor(hCursor);
    //            bool ret_val = SetSystemCursor(hCursor, OCR_NORMAL);
    //        }
    //        else
    //        {
    //            throw new ApplicationException("Could not create cursor from file ");
    //        }

    //        // And kill dummy file again
    //        File.Delete(DummyCursorFilename);
    //    }
    }
}
