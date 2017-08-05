namespace Serial
{
    partial class CtlMain
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
        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.Header = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.LabelBauds = new System.Windows.Forms.Label();
            this.NumBauds = new System.Windows.Forms.NumericUpDown();
            this.LabelPort = new System.Windows.Forms.Label();
            this.TxtPort = new System.Windows.Forms.TextBox();
            this.CBoxGenEventsOnReceive = new System.Windows.Forms.CheckBox();
            this.LabelSaved = new System.Windows.Forms.Label();
            this.NumConcatenationInterval = new System.Windows.Forms.NumericUpDown();
            this.CBoxConcateStrings = new System.Windows.Forms.CheckBox();
            this.CBoxDTR = new System.Windows.Forms.CheckBox();
            this.PanelSerialRead = new System.Windows.Forms.Panel();
            this.ElementToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.PluginIcon = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.NumBauds)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumConcatenationInterval)).BeginInit();
            this.PanelSerialRead.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PluginIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // Header
            // 
            this.Header.AutoSize = true;
            this.Header.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Header.Location = new System.Drawing.Point(60, 8);
            this.Header.Name = "Header";
            this.Header.Size = new System.Drawing.Size(59, 25);
            this.Header.TabIndex = 0;
            this.Header.Text = "Serial";
            this.ElementToolTip.SetToolTip(this.Header, "Left Click opens plugin path");
            this.Header.Click += new System.EventHandler(this.Header_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(67)))), ((int)(((byte)(110)))));
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.ForeColor = System.Drawing.SystemColors.Window;
            this.btnSave.Location = new System.Drawing.Point(232, 8);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(136, 25);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Save options";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // LabelBauds
            // 
            this.LabelBauds.AutoSize = true;
            this.LabelBauds.Location = new System.Drawing.Point(195, 64);
            this.LabelBauds.Name = "LabelBauds";
            this.LabelBauds.Size = new System.Drawing.Size(131, 15);
            this.LabelBauds.TabIndex = 9;
            this.LabelBauds.Text = "Port frequency (Bauds):";
            // 
            // NumBauds
            // 
            this.NumBauds.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.NumBauds.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.NumBauds.ForeColor = System.Drawing.SystemColors.Control;
            this.NumBauds.Location = new System.Drawing.Point(198, 83);
            this.NumBauds.Maximum = new decimal(new int[] {
            1215752191,
            23,
            0,
            0});
            this.NumBauds.Name = "NumBauds";
            this.NumBauds.Size = new System.Drawing.Size(131, 23);
            this.NumBauds.TabIndex = 10;
            // 
            // LabelPort
            // 
            this.LabelPort.AutoSize = true;
            this.LabelPort.Location = new System.Drawing.Point(11, 64);
            this.LabelPort.Name = "LabelPort";
            this.LabelPort.Size = new System.Drawing.Size(132, 15);
            this.LabelPort.TabIndex = 11;
            this.LabelPort.Text = "Port name (e.g. COM1):";
            this.ElementToolTip.SetToolTip(this.LabelPort, "Leave blank to enable Arduino \r\nautoscan function (Experimental)");
            // 
            // TxtPort
            // 
            this.TxtPort.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.TxtPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TxtPort.ForeColor = System.Drawing.SystemColors.Control;
            this.TxtPort.Location = new System.Drawing.Point(14, 82);
            this.TxtPort.Name = "TxtPort";
            this.TxtPort.Size = new System.Drawing.Size(130, 23);
            this.TxtPort.TabIndex = 12;
            this.ElementToolTip.SetToolTip(this.TxtPort, "Leave blank to enable Arduino \r\nautoscan function (Experimental)\r\n");
            // 
            // CBoxGenEventsOnReceive
            // 
            this.CBoxGenEventsOnReceive.AutoSize = true;
            this.CBoxGenEventsOnReceive.Location = new System.Drawing.Point(10, 19);
            this.CBoxGenEventsOnReceive.Name = "CBoxGenEventsOnReceive";
            this.CBoxGenEventsOnReceive.Size = new System.Drawing.Size(343, 19);
            this.CBoxGenEventsOnReceive.TabIndex = 13;
            this.CBoxGenEventsOnReceive.Text = "Gen event \"Serial.Received\" when message comes from port";
            this.ElementToolTip.SetToolTip(this.CBoxGenEventsOnReceive, "Message is stored in {1} of event.");
            this.CBoxGenEventsOnReceive.UseVisualStyleBackColor = true;
            this.CBoxGenEventsOnReceive.CheckedChanged += new System.EventHandler(this.CBoxGenEventsOnReceive_CheckedChanged);
            // 
            // LabelSaved
            // 
            this.LabelSaved.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LabelSaved.AutoSize = true;
            this.LabelSaved.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.LabelSaved.ForeColor = System.Drawing.Color.Gold;
            this.LabelSaved.Location = new System.Drawing.Point(117, 10);
            this.LabelSaved.Name = "LabelSaved";
            this.LabelSaved.Size = new System.Drawing.Size(109, 21);
            this.LabelSaved.TabIndex = 15;
            this.LabelSaved.Text = "Options saved";
            this.LabelSaved.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.LabelSaved.Visible = false;
            // 
            // NumConcatenationInterval
            // 
            this.NumConcatenationInterval.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.NumConcatenationInterval.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.NumConcatenationInterval.ForeColor = System.Drawing.SystemColors.Control;
            this.NumConcatenationInterval.Location = new System.Drawing.Point(228, 60);
            this.NumConcatenationInterval.Maximum = new decimal(new int[] {
            1215752191,
            23,
            0,
            0});
            this.NumConcatenationInterval.Name = "NumConcatenationInterval";
            this.NumConcatenationInterval.Size = new System.Drawing.Size(131, 23);
            this.NumConcatenationInterval.TabIndex = 18;
            // 
            // CBoxConcateStrings
            // 
            this.CBoxConcateStrings.AutoSize = true;
            this.CBoxConcateStrings.Location = new System.Drawing.Point(10, 53);
            this.CBoxConcateStrings.Name = "CBoxConcateStrings";
            this.CBoxConcateStrings.Size = new System.Drawing.Size(212, 34);
            this.CBoxConcateStrings.TabIndex = 19;
            this.CBoxConcateStrings.Text = "Concat strings that came from port\r\nwith interval less, than:";
            this.CBoxConcateStrings.UseVisualStyleBackColor = true;
            this.CBoxConcateStrings.CheckedChanged += new System.EventHandler(this.CBoxConcateStrings_CheckedChanged);
            // 
            // CBoxDTR
            // 
            this.CBoxDTR.AutoSize = true;
            this.CBoxDTR.Location = new System.Drawing.Point(14, 141);
            this.CBoxDTR.Name = "CBoxDTR";
            this.CBoxDTR.Size = new System.Drawing.Size(244, 19);
            this.CBoxDTR.TabIndex = 20;
            this.CBoxDTR.Text = "Restart Arduino on COM port connection";
            this.ElementToolTip.SetToolTip(this.CBoxDTR, "This function also called Data Terminal Ready (DTR) signal.");
            this.CBoxDTR.UseVisualStyleBackColor = true;
            // 
            // PanelSerialRead
            // 
            this.PanelSerialRead.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PanelSerialRead.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.PanelSerialRead.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PanelSerialRead.Controls.Add(this.CBoxGenEventsOnReceive);
            this.PanelSerialRead.Controls.Add(this.NumConcatenationInterval);
            this.PanelSerialRead.Controls.Add(this.CBoxConcateStrings);
            this.PanelSerialRead.Location = new System.Drawing.Point(3, 207);
            this.PanelSerialRead.Name = "PanelSerialRead";
            this.PanelSerialRead.Size = new System.Drawing.Size(373, 103);
            this.PanelSerialRead.TabIndex = 21;
            // 
            // ElementToolTip
            // 
            this.ElementToolTip.AutoPopDelay = 5000;
            this.ElementToolTip.InitialDelay = 100;
            this.ElementToolTip.ReshowDelay = 100;
            // 
            // PluginIcon
            // 
            this.PluginIcon.Location = new System.Drawing.Point(9, 8);
            this.PluginIcon.Name = "PluginIcon";
            this.PluginIcon.Size = new System.Drawing.Size(45, 45);
            this.PluginIcon.TabIndex = 23;
            this.PluginIcon.TabStop = false;
            this.ElementToolTip.SetToolTip(this.PluginIcon, "Left Click opens plugin path");
            this.PluginIcon.Click += new System.EventHandler(this.Header_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 195);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 15);
            this.label1.TabIndex = 22;
            this.label1.Text = "Reading from port";
            // 
            // ctlMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.Controls.Add(this.PluginIcon);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CBoxDTR);
            this.Controls.Add(this.LabelSaved);
            this.Controls.Add(this.TxtPort);
            this.Controls.Add(this.LabelPort);
            this.Controls.Add(this.NumBauds);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.Header);
            this.Controls.Add(this.LabelBauds);
            this.Controls.Add(this.PanelSerialRead);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ForeColor = System.Drawing.Color.Gainsboro;
            this.Name = "ctlMain";
            this.Size = new System.Drawing.Size(379, 502);
            ((System.ComponentModel.ISupportInitialize)(this.NumBauds)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumConcatenationInterval)).EndInit();
            this.PanelSerialRead.ResumeLayout(false);
            this.PanelSerialRead.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PluginIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
        private System.Windows.Forms.Label Header;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label LabelBauds;
        private System.Windows.Forms.NumericUpDown NumBauds;
        private System.Windows.Forms.Label LabelPort;
        private System.Windows.Forms.TextBox TxtPort;
        private System.Windows.Forms.CheckBox CBoxGenEventsOnReceive;
        private System.Windows.Forms.Label LabelSaved;
        private System.Windows.Forms.NumericUpDown NumConcatenationInterval;
        private System.Windows.Forms.CheckBox CBoxConcateStrings;
        private System.Windows.Forms.CheckBox CBoxDTR;
        private System.Windows.Forms.Panel PanelSerialRead;
        private System.Windows.Forms.ToolTip ElementToolTip;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox PluginIcon;
    }
}
