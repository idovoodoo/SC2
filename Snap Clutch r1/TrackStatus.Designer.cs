namespace SnapClutch
{
    partial class TrackStatus
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TrackStatus));
            this.axTetTrackStatus = new AxTetComp.AxTetTrackStatus();
            ((System.ComponentModel.ISupportInitialize)(this.axTetTrackStatus)).BeginInit();
            this.SuspendLayout();
            // 
            // axTetTrackStatus
            // 
            this.axTetTrackStatus.Enabled = true;
            this.axTetTrackStatus.Location = new System.Drawing.Point(12, 11);
            this.axTetTrackStatus.Name = "axTetTrackStatus";
            this.axTetTrackStatus.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axTetTrackStatus.OcxState")));
            this.axTetTrackStatus.Size = new System.Drawing.Size(192, 192);
            this.axTetTrackStatus.TabIndex = 1;
            this.axTetTrackStatus.TabStop = false;
            // 
            // TrackStatus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(217, 214);
            this.Controls.Add(this.axTetTrackStatus);
            this.Name = "TrackStatus";
            this.Opacity = 0.5;
            this.Text = "TrackStatus";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.axTetTrackStatus)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AxTetComp.AxTetTrackStatus axTetTrackStatus;

    }
}