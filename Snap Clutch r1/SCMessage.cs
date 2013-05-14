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
using System.Threading;
using SnapClutch.SCTools;

namespace SnapClutch
{
    public partial class SCMessage : Form
    {

        public SCMessage(string argGame)
        {
            InitializeComponent();
            labelMessage.Text = argGame;
            AlignText();
        }

        private void AlignText()
        {
            labelText.Left = this.Width / 2 - labelText.Width / 2;
            labelMessage.Left = this.Width / 2 - labelMessage.Width / 2;
            labelText2.Left = this.Width / 2 - labelText2.Width / 2;
            labelText3.Left = this.Width / 2 - labelText3.Width / 2;
            buttonClose.Left = this.Width / 2 - buttonClose.Width / 2;
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            CursorFactory.Create(Application.StartupPath + CursorSnapClutch.BlankCursor());
            this.Close();
        }

        private void buttonClose_MouseEnter(object sender, EventArgs e)
        {
            CursorFactory.Create(Application.StartupPath + CursorSnapClutch.Pulse());
        }

        private void buttonClose_MouseLeave(object sender, EventArgs e)
        {
            CursorFactory.Create(Application.StartupPath + CursorSnapClutch.BlankCursor());
        }
    }
}
