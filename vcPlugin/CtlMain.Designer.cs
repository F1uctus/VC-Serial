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
            this.components = new Container();
            this.Header = new Label();
            this.btnSave = new Button();
            this.CBoxGenEventsOnReceive = new CheckBox();
            this.LabelSaved = new Label();
            this.NumConcatenationInterval = new NumericUpDown();
            this.CBoxConcateStrings = new CheckBox();
            this.PanelSerialRead = new Panel();
            this.ElementToolTip = new ToolTip(this.components);
            this.PluginIcon = new PictureBox();
            this.label1 = new Label();
            this.ListAllPorts = new ListView();
            this.LblAllPorts = new Label();
            this.btnUpdatePortsList = new Button();
            ((ISupportInitialize)(this.NumConcatenationInterval)).BeginInit();
            this.PanelSerialRead.SuspendLayout();
            ((ISupportInitialize)(this.PluginIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // Header
            // 
            this.Header.AutoSize = true;
            this.Header.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(204)));
            this.Header.Location = new Point(60, 17);
            this.Header.Name = "Header";
            this.Header.Size = new Size(59, 25);
            this.Header.TabIndex = 0;
            this.Header.Text = "Serial";
            this.ElementToolTip.SetToolTip(this.Header, "Left Click opens plugin path");
            this.Header.Click += new EventHandler(this.Header_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.btnSave.BackColor = Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(67)))), ((int)(((byte)(110)))));
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = FlatStyle.Flat;
            this.btnSave.ForeColor = SystemColors.Window;
            this.btnSave.Location = new Point(232, 8);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new Size(136, 25);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Save options";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new EventHandler(this.BtnSave_Click);
            // 
            // CBoxGenEventsOnReceive
            // 
            this.CBoxGenEventsOnReceive.AutoSize = true;
            this.CBoxGenEventsOnReceive.Location = new Point(10, 12);
            this.CBoxGenEventsOnReceive.Name = "CBoxGenEventsOnReceive";
            this.CBoxGenEventsOnReceive.Size = new Size(346, 49);
            this.CBoxGenEventsOnReceive.TabIndex = 13;
            this.CBoxGenEventsOnReceive.Text = "Gen event \"Serial.Received\" when message comes from port.\r\n{1} - Port name\r\n{2} -" +
    " Message";
            this.ElementToolTip.SetToolTip(this.CBoxGenEventsOnReceive, "Port name is stored in {1} of event.\r\nMessage is stored in {2} of event.");
            this.CBoxGenEventsOnReceive.UseVisualStyleBackColor = true;
            this.CBoxGenEventsOnReceive.CheckedChanged += new EventHandler(this.CBoxGenEventsOnReceive_CheckedChanged);
            // 
            // LabelSaved
            // 
            this.LabelSaved.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.LabelSaved.AutoSize = true;
            this.LabelSaved.Font = new Font("Segoe UI", 12F);
            this.LabelSaved.ForeColor = Color.Gold;
            this.LabelSaved.Location = new Point(246, 40);
            this.LabelSaved.Name = "LabelSaved";
            this.LabelSaved.Size = new Size(109, 21);
            this.LabelSaved.TabIndex = 15;
            this.LabelSaved.Text = "Options saved";
            this.LabelSaved.TextAlign = ContentAlignment.TopRight;
            this.LabelSaved.Visible = false;
            // 
            // NumConcatenationInterval
            // 
            this.NumConcatenationInterval.BackColor = Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.NumConcatenationInterval.BorderStyle = BorderStyle.FixedSingle;
            this.NumConcatenationInterval.ForeColor = SystemColors.Control;
            this.NumConcatenationInterval.Location = new Point(228, 81);
            this.NumConcatenationInterval.Maximum = new decimal(new int[] {
            1215752191,
            23,
            0,
            0});
            this.NumConcatenationInterval.Name = "NumConcatenationInterval";
            this.NumConcatenationInterval.Size = new Size(131, 23);
            this.NumConcatenationInterval.TabIndex = 18;
            // 
            // CBoxConcateStrings
            // 
            this.CBoxConcateStrings.AutoSize = true;
            this.CBoxConcateStrings.Location = new Point(10, 74);
            this.CBoxConcateStrings.Name = "CBoxConcateStrings";
            this.CBoxConcateStrings.Size = new Size(212, 34);
            this.CBoxConcateStrings.TabIndex = 19;
            this.CBoxConcateStrings.Text = "Concat strings that came from port\r\nwith interval less, than:";
            this.CBoxConcateStrings.UseVisualStyleBackColor = true;
            this.CBoxConcateStrings.CheckedChanged += new EventHandler(this.CBoxConcateStrings_CheckedChanged);
            // 
            // PanelSerialRead
            // 
            this.PanelSerialRead.Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Left) 
            | AnchorStyles.Right)));
            this.PanelSerialRead.BackColor = Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.PanelSerialRead.BorderStyle = BorderStyle.FixedSingle;
            this.PanelSerialRead.Controls.Add(this.CBoxGenEventsOnReceive);
            this.PanelSerialRead.Controls.Add(this.NumConcatenationInterval);
            this.PanelSerialRead.Controls.Add(this.CBoxConcateStrings);
            this.PanelSerialRead.Location = new Point(3, 100);
            this.PanelSerialRead.Name = "PanelSerialRead";
            this.PanelSerialRead.Size = new Size(373, 124);
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
            this.PluginIcon.Location = new Point(9, 8);
            this.PluginIcon.Name = "PluginIcon";
            this.PluginIcon.Size = new Size(45, 45);
            this.PluginIcon.TabIndex = 23;
            this.PluginIcon.TabStop = false;
            this.ElementToolTip.SetToolTip(this.PluginIcon, "Left Click opens plugin path");
            this.PluginIcon.Click += new EventHandler(this.Header_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = Color.Transparent;
            this.label1.Location = new Point(5, 88);
            this.label1.Name = "label1";
            this.label1.Size = new Size(104, 15);
            this.label1.TabIndex = 22;
            this.label1.Text = "Reading from port";
            // 
            // ListAllPorts
            // 
            this.ListAllPorts.Anchor = ((AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom) 
            | AnchorStyles.Left) 
            | AnchorStyles.Right)));
            this.ListAllPorts.BackColor = Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.ListAllPorts.BorderStyle = BorderStyle.FixedSingle;
            this.ListAllPorts.ForeColor = Color.Gainsboro;
            this.ListAllPorts.HeaderStyle = ColumnHeaderStyle.None;
            this.ListAllPorts.Location = new Point(3, 256);
            this.ListAllPorts.Name = "ListAllPorts";
            this.ListAllPorts.Size = new Size(373, 138);
            this.ListAllPorts.TabIndex = 24;
            this.ListAllPorts.UseCompatibleStateImageBehavior = false;
            this.ListAllPorts.View = View.List;
            // 
            // LblAllPorts
            // 
            this.LblAllPorts.AutoSize = true;
            this.LblAllPorts.Location = new Point(5, 238);
            this.LblAllPorts.Name = "LblAllPorts";
            this.LblAllPorts.Size = new Size(239, 15);
            this.LblAllPorts.TabIndex = 26;
            this.LblAllPorts.Text = "Available COM ports: (Green are connected)";
            // 
            // btnUpdatePortsList
            // 
            this.btnUpdatePortsList.Anchor = ((AnchorStyles)(((AnchorStyles.Bottom | AnchorStyles.Left) 
            | AnchorStyles.Right)));
            this.btnUpdatePortsList.BackColor = Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(67)))), ((int)(((byte)(110)))));
            this.btnUpdatePortsList.FlatAppearance.BorderSize = 0;
            this.btnUpdatePortsList.FlatStyle = FlatStyle.Flat;
            this.btnUpdatePortsList.ForeColor = SystemColors.Window;
            this.btnUpdatePortsList.Location = new Point(3, 400);
            this.btnUpdatePortsList.Name = "btnUpdatePortsList";
            this.btnUpdatePortsList.Size = new Size(373, 25);
            this.btnUpdatePortsList.TabIndex = 27;
            this.btnUpdatePortsList.Text = "Update list";
            this.btnUpdatePortsList.UseVisualStyleBackColor = false;
            this.btnUpdatePortsList.Click += new EventHandler(this.UpdatePortsList);
            // 
            // CtlMain
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.Controls.Add(this.btnUpdatePortsList);
            this.Controls.Add(this.LblAllPorts);
            this.Controls.Add(this.ListAllPorts);
            this.Controls.Add(this.PluginIcon);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LabelSaved);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.Header);
            this.Controls.Add(this.PanelSerialRead);
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(204)));
            this.ForeColor = Color.Gainsboro;
            this.Name = "CtlMain";
            this.Size = new Size(379, 502);
            ((ISupportInitialize)(this.NumConcatenationInterval)).EndInit();
            this.PanelSerialRead.ResumeLayout(false);
            this.PanelSerialRead.PerformLayout();
            ((ISupportInitialize)(this.PluginIcon)).EndInit();
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
    }
}
