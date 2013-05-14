using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TetComp;

namespace SnapClutch
{
    public partial class TrackStatus : Form
    {
        private string connectionString = "";
        private ITetTrackStatus tetTrackStatus;

        public TrackStatus(string argConnectionString)
        {
            InitializeComponent();
            connectionString = argConnectionString;
            tetTrackStatus = (ITetTrackStatus)axTetTrackStatus.GetOcx();
            this.Show();
        }

        public void StartTrackStatus()
        {
            try
            {
                if (!tetTrackStatus.IsConnected)
                    tetTrackStatus.Connect(connectionString, (int)TetConstants.TetConstants_DefaultServerPort);

                if (!tetTrackStatus.IsTracking) 
                    tetTrackStatus.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void StopTrackStatus()
        {
            try
            {
                if (tetTrackStatus.IsTracking) 
                    tetTrackStatus.Stop();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
