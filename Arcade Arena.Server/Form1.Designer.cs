
namespace Arcade_Arena.Server
{
    partial class Form1
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
            this.grpServerControls = new System.Windows.Forms.GroupBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.grpServerStatusLog = new System.Windows.Forms.GroupBox();
            this.dgwServerStatusLog = new System.Windows.Forms.DataGridView();
            this.clmId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmMessage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grpServerControls.SuspendLayout();
            this.grpServerStatusLog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgwServerStatusLog)).BeginInit();
            this.SuspendLayout();
            // 
            // grpServerControls
            // 
            this.grpServerControls.Controls.Add(this.btnStop);
            this.grpServerControls.Controls.Add(this.btnStart);
            this.grpServerControls.Location = new System.Drawing.Point(12, 12);
            this.grpServerControls.Name = "grpServerControls";
            this.grpServerControls.Size = new System.Drawing.Size(230, 54);
            this.grpServerControls.TabIndex = 0;
            this.grpServerControls.TabStop = false;
            this.grpServerControls.Text = "Server Controls";
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(6, 19);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(103, 23);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start Server";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(121, 18);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(103, 23);
            this.btnStop.TabIndex = 1;
            this.btnStop.Text = "Stop Server";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // grpServerStatusLog
            // 
            this.grpServerStatusLog.Controls.Add(this.dgwServerStatusLog);
            this.grpServerStatusLog.Location = new System.Drawing.Point(12, 72);
            this.grpServerStatusLog.Name = "grpServerStatusLog";
            this.grpServerStatusLog.Size = new System.Drawing.Size(776, 366);
            this.grpServerStatusLog.TabIndex = 1;
            this.grpServerStatusLog.TabStop = false;
            this.grpServerStatusLog.Text = "Server Status Log";
            // 
            // dgwServerStatusLog
            // 
            this.dgwServerStatusLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgwServerStatusLog.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmId,
            this.clmMessage});
            this.dgwServerStatusLog.Location = new System.Drawing.Point(7, 20);
            this.dgwServerStatusLog.Name = "dgwServerStatusLog";
            this.dgwServerStatusLog.Size = new System.Drawing.Size(763, 340);
            this.dgwServerStatusLog.TabIndex = 0;
            // 
            // clmId
            // 
            this.clmId.HeaderText = "Id";
            this.clmId.Name = "clmId";
            // 
            // clmMessage
            // 
            this.clmMessage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.clmMessage.HeaderText = "Message";
            this.clmMessage.Name = "clmMessage";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.grpServerStatusLog);
            this.Controls.Add(this.grpServerControls);
            this.Name = "Form1";
            this.Text = "Form1";
            this.grpServerControls.ResumeLayout(false);
            this.grpServerStatusLog.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgwServerStatusLog)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpServerControls;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.GroupBox grpServerStatusLog;
        private System.Windows.Forms.DataGridView dgwServerStatusLog;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmId;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmMessage;
    }
}

