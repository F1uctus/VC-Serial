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
            this.btSave = new System.Windows.Forms.Button();
            this.cbGenEventsOnReceive = new System.Windows.Forms.CheckBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.lvPorts = new System.Windows.Forms.ListView();
            this.lbAvailablePorts = new System.Windows.Forms.Label();
            this.btnUpdatePortsList = new System.Windows.Forms.Button();
            this.lbGreen = new System.Windows.Forms.Label();
            this.lbYellow = new System.Windows.Forms.Label();
            this.cbEnableLogging = new System.Windows.Forms.CheckBox();
            this.lvLog = new System.Windows.Forms.ListView();
            this.cbLogAutoscroll = new System.Windows.Forms.CheckBox();
            this.tbSendMessage = new System.Windows.Forms.TextBox();
            this.btSend = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btSave
            // 
            this.btSave.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btSave.FlatAppearance.BorderSize = 0;
            this.btSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btSave.ForeColor = System.Drawing.Color.White;
            this.btSave.Location = new System.Drawing.Point(0, 0);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(450, 30);
            this.btSave.TabIndex = 5;
            this.btSave.Text = "Save options";
            this.btSave.UseVisualStyleBackColor = false;
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // cbGenEventsOnReceive
            // 
            this.cbGenEventsOnReceive.AutoSize = true;
            this.cbGenEventsOnReceive.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbGenEventsOnReceive.Location = new System.Drawing.Point(3, 38);
            this.cbGenEventsOnReceive.Name = "cbGenEventsOnReceive";
            this.cbGenEventsOnReceive.Size = new System.Drawing.Size(404, 64);
            this.cbGenEventsOnReceive.TabIndex = 13;
            this.cbGenEventsOnReceive.Text = "Enable \"Serial.Received\" event when plugin receives message from port.\r\nEvent pay" +
    "loads:\r\n    {1} - Port name from which message came\r\n    {2} - Message";
            this.cbGenEventsOnReceive.UseVisualStyleBackColor = true;
            this.cbGenEventsOnReceive.CheckedChanged += new System.EventHandler(this.cbGenEventsOnReceive_CheckedChanged);
            // 
            // toolTip
            // 
            this.toolTip.AutoPopDelay = 5000;
            this.toolTip.InitialDelay = 100;
            this.toolTip.ReshowDelay = 100;
            // 
            // lvPorts
            // 
            this.lvPorts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvPorts.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.lvPorts.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvPorts.ForeColor = System.Drawing.Color.Gainsboro;
            this.lvPorts.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvPorts.Location = new System.Drawing.Point(-1, 195);
            this.lvPorts.MultiSelect = false;
            this.lvPorts.Name = "lvPorts";
            this.lvPorts.Size = new System.Drawing.Size(452, 82);
            this.lvPorts.TabIndex = 24;
            this.lvPorts.UseCompatibleStateImageBehavior = false;
            this.lvPorts.View = System.Windows.Forms.View.Details;
            this.lvPorts.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lvPorts_MouseClick);
            this.lvPorts.Resize += new System.EventHandler(this.lvPorts_Resize);
            // 
            // lbAvailablePorts
            // 
            this.lbAvailablePorts.AutoSize = true;
            this.lbAvailablePorts.Location = new System.Drawing.Point(3, 111);
            this.lbAvailablePorts.Name = "lbAvailablePorts";
            this.lbAvailablePorts.Size = new System.Drawing.Size(391, 15);
            this.lbAvailablePorts.TabIndex = 26;
            this.lbAvailablePorts.Text = "Availiable COM ports: (Right-click on port name to perform action on it.)";
            // 
            // btnUpdatePortsList
            // 
            this.btnUpdatePortsList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdatePortsList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnUpdatePortsList.FlatAppearance.BorderSize = 0;
            this.btnUpdatePortsList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdatePortsList.ForeColor = System.Drawing.Color.White;
            this.btnUpdatePortsList.Location = new System.Drawing.Point(0, 159);
            this.btnUpdatePortsList.Name = "btnUpdatePortsList";
            this.btnUpdatePortsList.Size = new System.Drawing.Size(450, 30);
            this.btnUpdatePortsList.TabIndex = 27;
            this.btnUpdatePortsList.Text = "Update list";
            this.btnUpdatePortsList.UseVisualStyleBackColor = false;
            this.btnUpdatePortsList.Click += new System.EventHandler(this.UpdatePortsListView);
            // 
            // lbGreen
            // 
            this.lbGreen.AutoSize = true;
            this.lbGreen.ForeColor = System.Drawing.Color.LimeGreen;
            this.lbGreen.Location = new System.Drawing.Point(3, 135);
            this.lbGreen.Name = "lbGreen";
            this.lbGreen.Size = new System.Drawing.Size(147, 15);
            this.lbGreen.TabIndex = 28;
            this.lbGreen.Text = "Connected ports are green";
            // 
            // lbYellow
            // 
            this.lbYellow.AutoSize = true;
            this.lbYellow.ForeColor = System.Drawing.Color.Gold;
            this.lbYellow.Location = new System.Drawing.Point(166, 135);
            this.lbYellow.Name = "lbYellow";
            this.lbYellow.Size = new System.Drawing.Size(124, 15);
            this.lbYellow.TabIndex = 29;
            this.lbYellow.Text = "Selected port is yellow";
            // 
            // cbEnableLogging
            // 
            this.cbEnableLogging.AutoSize = true;
            this.cbEnableLogging.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbEnableLogging.Location = new System.Drawing.Point(3, 286);
            this.cbEnableLogging.Name = "cbEnableLogging";
            this.cbEnableLogging.Size = new System.Drawing.Size(206, 19);
            this.cbEnableLogging.TabIndex = 14;
            this.cbEnableLogging.Text = "Enable selected serial port logging";
            this.cbEnableLogging.UseVisualStyleBackColor = true;
            this.cbEnableLogging.CheckedChanged += new System.EventHandler(this.cbEnableLogging_CheckedChanged);
            // 
            // lvLog
            // 
            this.lvLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.lvLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvLog.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lvLog.ForeColor = System.Drawing.Color.Gainsboro;
            this.lvLog.FullRowSelect = true;
            this.lvLog.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvLog.Location = new System.Drawing.Point(-1, 315);
            this.lvLog.MultiSelect = false;
            this.lvLog.Name = "lvLog";
            this.lvLog.Size = new System.Drawing.Size(452, 101);
            this.lvLog.TabIndex = 32;
            this.lvLog.UseCompatibleStateImageBehavior = false;
            this.lvLog.View = System.Windows.Forms.View.Details;
            this.lvLog.Resize += new System.EventHandler(this.lvLog_Resize);
            // 
            // cbLogAutoscroll
            // 
            this.cbLogAutoscroll.AutoSize = true;
            this.cbLogAutoscroll.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbLogAutoscroll.Location = new System.Drawing.Point(228, 286);
            this.cbLogAutoscroll.Name = "cbLogAutoscroll";
            this.cbLogAutoscroll.Size = new System.Drawing.Size(101, 19);
            this.cbLogAutoscroll.TabIndex = 33;
            this.cbLogAutoscroll.Text = "Log autoscroll";
            this.cbLogAutoscroll.UseVisualStyleBackColor = true;
            this.cbLogAutoscroll.CheckedChanged += new System.EventHandler(this.cbLogAutoscroll_CheckedChanged);
            // 
            // tbSendMessage
            // 
            this.tbSendMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSendMessage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.tbSendMessage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbSendMessage.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbSendMessage.ForeColor = System.Drawing.Color.Gainsboro;
            this.tbSendMessage.Location = new System.Drawing.Point(3, 422);
            this.tbSendMessage.Name = "tbSendMessage";
            this.tbSendMessage.Size = new System.Drawing.Size(362, 25);
            this.tbSendMessage.TabIndex = 34;
            this.tbSendMessage.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbSendMessage_KeyPress);
            // 
            // btSend
            // 
            this.btSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btSend.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btSend.FlatAppearance.BorderSize = 0;
            this.btSend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btSend.ForeColor = System.Drawing.Color.White;
            this.btSend.Location = new System.Drawing.Point(371, 422);
            this.btSend.Name = "btSend";
            this.btSend.Size = new System.Drawing.Size(76, 25);
            this.btSend.TabIndex = 35;
            this.btSend.Text = "Send";
            this.btSend.UseVisualStyleBackColor = false;
            this.btSend.Click += new System.EventHandler(this.btSend_Click);
            // 
            // CtlMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.Controls.Add(this.btSend);
            this.Controls.Add(this.tbSendMessage);
            this.Controls.Add(this.cbLogAutoscroll);
            this.Controls.Add(this.cbGenEventsOnReceive);
            this.Controls.Add(this.lvLog);
            this.Controls.Add(this.cbEnableLogging);
            this.Controls.Add(this.lbYellow);
            this.Controls.Add(this.lbGreen);
            this.Controls.Add(this.lbAvailablePorts);
            this.Controls.Add(this.btnUpdatePortsList);
            this.Controls.Add(this.lvPorts);
            this.Controls.Add(this.btSave);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ForeColor = System.Drawing.Color.Gainsboro;
            this.MinimumSize = new System.Drawing.Size(450, 450);
            this.Name = "CtlMain";
            this.Size = new System.Drawing.Size(450, 450);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
        private Button btSave;
        private CheckBox cbGenEventsOnReceive;
        private ToolTip toolTip;
        private Label lbAvailablePorts;
        private Button btnUpdatePortsList;
		private Label lbGreen;
		private Label lbYellow;
		private ListView lvPorts;
        private CheckBox cbEnableLogging;
        private ListView lvLog;
        private CheckBox cbLogAutoscroll;
        private TextBox tbSendMessage;
        private Button btSend;
    }
}
