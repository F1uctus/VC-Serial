using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Serial
{
    partial class CtlMain
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;
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
			this.CBoxGenEventsOnReceive = new System.Windows.Forms.CheckBox();
			this.LabelSaved = new System.Windows.Forms.Label();
			this.NumConcatenationInterval = new System.Windows.Forms.NumericUpDown();
			this.CBoxConcateStrings = new System.Windows.Forms.CheckBox();
			this.PanelSerialRead = new System.Windows.Forms.Panel();
			this.ElementToolTip = new System.Windows.Forms.ToolTip(this.components);
			this.PluginIcon = new System.Windows.Forms.PictureBox();
			this.label1 = new System.Windows.Forms.Label();
			this.ListAllPorts = new System.Windows.Forms.ListView();
			this.LblAllPorts = new System.Windows.Forms.Label();
			this.btnUpdatePortsList = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.NumConcatenationInterval)).BeginInit();
			this.PanelSerialRead.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.PluginIcon)).BeginInit();
			this.SuspendLayout();
			// 
			// Header
			// 
			this.Header.AutoSize = true;
			this.Header.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.Header.Location = new System.Drawing.Point(60, 17);
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
			this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
			this.btnSave.FlatAppearance.BorderSize = 0;
			this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnSave.ForeColor = System.Drawing.Color.White;
			this.btnSave.Location = new System.Drawing.Point(232, 8);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(136, 25);
			this.btnSave.TabIndex = 5;
			this.btnSave.Text = "Save options";
			this.btnSave.UseVisualStyleBackColor = false;
			this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
			// 
			// CBoxGenEventsOnReceive
			// 
			this.CBoxGenEventsOnReceive.AutoSize = true;
			this.CBoxGenEventsOnReceive.Location = new System.Drawing.Point(10, 12);
			this.CBoxGenEventsOnReceive.Name = "CBoxGenEventsOnReceive";
			this.CBoxGenEventsOnReceive.Size = new System.Drawing.Size(360, 34);
			this.CBoxGenEventsOnReceive.TabIndex = 13;
			this.CBoxGenEventsOnReceive.Text = "Enable event \"Serial.Received\" when message comes from port.\r\n{1} - Port name,  {" +
    "2} - Message";
			this.CBoxGenEventsOnReceive.UseVisualStyleBackColor = true;
			this.CBoxGenEventsOnReceive.CheckedChanged += new System.EventHandler(this.CBoxGenEventsOnReceive_CheckedChanged);
			// 
			// LabelSaved
			// 
			this.LabelSaved.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.LabelSaved.AutoSize = true;
			this.LabelSaved.Font = new System.Drawing.Font("Segoe UI", 12F);
			this.LabelSaved.ForeColor = System.Drawing.Color.Gold;
			this.LabelSaved.Location = new System.Drawing.Point(246, 40);
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
			this.NumConcatenationInterval.Location = new System.Drawing.Point(225, 66);
			this.NumConcatenationInterval.Maximum = new decimal(new int[] {
            1215752191,
            23,
            0,
            0});
			this.NumConcatenationInterval.Name = "NumConcatenationInterval";
			this.NumConcatenationInterval.Size = new System.Drawing.Size(107, 23);
			this.NumConcatenationInterval.TabIndex = 18;
			// 
			// CBoxConcateStrings
			// 
			this.CBoxConcateStrings.AutoSize = true;
			this.CBoxConcateStrings.Location = new System.Drawing.Point(10, 59);
			this.CBoxConcateStrings.Name = "CBoxConcateStrings";
			this.CBoxConcateStrings.Size = new System.Drawing.Size(212, 34);
			this.CBoxConcateStrings.TabIndex = 19;
			this.CBoxConcateStrings.Text = "Concat strings that came from port\r\nwith interval less, than:";
			this.CBoxConcateStrings.UseVisualStyleBackColor = true;
			this.CBoxConcateStrings.CheckedChanged += new System.EventHandler(this.CBoxConcateStrings_CheckedChanged);
			// 
			// PanelSerialRead
			// 
			this.PanelSerialRead.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
			this.PanelSerialRead.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.PanelSerialRead.Controls.Add(this.label4);
			this.PanelSerialRead.Controls.Add(this.CBoxGenEventsOnReceive);
			this.PanelSerialRead.Controls.Add(this.NumConcatenationInterval);
			this.PanelSerialRead.Controls.Add(this.CBoxConcateStrings);
			this.PanelSerialRead.Location = new System.Drawing.Point(3, 95);
			this.PanelSerialRead.Name = "PanelSerialRead";
			this.PanelSerialRead.Size = new System.Drawing.Size(373, 104);
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
			this.label1.BackColor = System.Drawing.Color.Transparent;
			this.label1.Location = new System.Drawing.Point(5, 77);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(104, 15);
			this.label1.TabIndex = 22;
			this.label1.Text = "Reading from port";
			// 
			// ListAllPorts
			// 
			this.ListAllPorts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ListAllPorts.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
			this.ListAllPorts.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.ListAllPorts.ForeColor = System.Drawing.Color.Gainsboro;
			this.ListAllPorts.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.ListAllPorts.Location = new System.Drawing.Point(3, 234);
			this.ListAllPorts.MultiSelect = false;
			this.ListAllPorts.Name = "ListAllPorts";
			this.ListAllPorts.Size = new System.Drawing.Size(373, 223);
			this.ListAllPorts.TabIndex = 24;
			this.ListAllPorts.UseCompatibleStateImageBehavior = false;
			this.ListAllPorts.View = System.Windows.Forms.View.List;
			// 
			// LblAllPorts
			// 
			this.LblAllPorts.AutoSize = true;
			this.LblAllPorts.Location = new System.Drawing.Point(5, 216);
			this.LblAllPorts.Name = "LblAllPorts";
			this.LblAllPorts.Size = new System.Drawing.Size(355, 15);
			this.LblAllPorts.TabIndex = 26;
			this.LblAllPorts.Text = "Available COM ports:             is connected,               is also selected)";
			// 
			// btnUpdatePortsList
			// 
			this.btnUpdatePortsList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.btnUpdatePortsList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
			this.btnUpdatePortsList.FlatAppearance.BorderSize = 0;
			this.btnUpdatePortsList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnUpdatePortsList.ForeColor = System.Drawing.Color.White;
			this.btnUpdatePortsList.Location = new System.Drawing.Point(3, 463);
			this.btnUpdatePortsList.Name = "btnUpdatePortsList";
			this.btnUpdatePortsList.Size = new System.Drawing.Size(373, 25);
			this.btnUpdatePortsList.TabIndex = 27;
			this.btnUpdatePortsList.Text = "Update list";
			this.btnUpdatePortsList.UseVisualStyleBackColor = false;
			this.btnUpdatePortsList.Click += new System.EventHandler(this.UpdatePortsList);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.ForeColor = System.Drawing.Color.ForestGreen;
			this.label2.Location = new System.Drawing.Point(121, 216);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(38, 15);
			this.label2.TabIndex = 28;
			this.label2.Text = "Green";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.ForeColor = System.Drawing.Color.Goldenrod;
			this.label3.Location = new System.Drawing.Point(231, 216);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(42, 15);
			this.label3.TabIndex = 29;
			this.label3.Text = "Yellow";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(338, 69);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(26, 15);
			this.label4.TabIndex = 30;
			this.label4.Text = "ms.";
			// 
			// CtlMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoScroll = true;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.LblAllPorts);
			this.Controls.Add(this.btnUpdatePortsList);
			this.Controls.Add(this.ListAllPorts);
			this.Controls.Add(this.PluginIcon);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.LabelSaved);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.Header);
			this.Controls.Add(this.PanelSerialRead);
			this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.ForeColor = System.Drawing.Color.Gainsboro;
			this.Name = "CtlMain";
			this.Size = new System.Drawing.Size(379, 502);
			((System.ComponentModel.ISupportInitialize)(this.NumConcatenationInterval)).EndInit();
			this.PanelSerialRead.ResumeLayout(false);
			this.PanelSerialRead.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.PluginIcon)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }
        #endregion
        private Label Header;
        private Button btnSave;
        private CheckBox CBoxGenEventsOnReceive;
        private Label LabelSaved;
        private NumericUpDown NumConcatenationInterval;
        private CheckBox CBoxConcateStrings;
        private Panel PanelSerialRead;
        private ToolTip ElementToolTip;
        private Label label1;
        private PictureBox PluginIcon;
        private Label LblAllPorts;
        public ListView ListAllPorts;
        private Button btnUpdatePortsList;
		private Label label2;
		private Label label3;
		private Label label4;
	}
}
