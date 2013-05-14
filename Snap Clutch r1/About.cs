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
using System.Reflection;
using System.Windows.Forms;
using SnapClutch.SCTools;

namespace SnapClutch
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
            labelText.Text = "Snap Clutch v" + Assembly.GetExecutingAssembly().GetName().Version.ToString() + " b";
            labelText.Left = this.Width / 2 - labelText.Width / 2;
            labelMessage.Left = this.Width / 2 - labelMessage.Width / 2;
            label2.Left = this.Width / 2 - label2.Width / 2;
            label3.Left = this.Width / 2 - label3.Width / 2;
            label5.Left = this.Width / 2 - label5.Width / 2;
            label6.Left = this.Width / 2 - label6.Width / 2;
            label7.Left = this.Width / 2 - label7.Width / 2;
            label8.Left = this.Width / 2 - label8.Width / 2;
            buttonClose.Left = this.Width / 2 - buttonClose.Width / 2;
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }


    }
}
