namespace OracleSoft
{
    partial class Oracle
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
                this.CPU.Dispose();
                this.CAC.Dispose();
                this.SYS.Dispose();
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabCentral = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.dgvCac = new System.Windows.Forms.DataGridView();
            this.lblCPU = new System.Windows.Forms.Label();
            this.lblSys = new System.Windows.Forms.Label();
            this.dgvSys = new System.Windows.Forms.DataGridView();
            this.dgvCPU = new System.Windows.Forms.DataGridView();
            this.btnProcWatch = new System.Windows.Forms.Button();
            this.tabStorage = new System.Windows.Forms.TabPage();
            this.lblPhysDisk = new System.Windows.Forms.Label();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.lblLogDisk = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btnDiskWatch = new System.Windows.Forms.Button();
            this.tabMemory = new System.Windows.Forms.TabPage();
            this.tabNet = new System.Windows.Forms.TabPage();
            this.tabSoft = new System.Windows.Forms.TabPage();
            this.tabProcesses = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabCentral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCac)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSys)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCPU)).BeginInit();
            this.tabStorage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabCentral);
            this.tabControl1.Controls.Add(this.tabStorage);
            this.tabControl1.Controls.Add(this.tabMemory);
            this.tabControl1.Controls.Add(this.tabNet);
            this.tabControl1.Controls.Add(this.tabSoft);
            this.tabControl1.Controls.Add(this.tabProcesses);
            this.tabControl1.Location = new System.Drawing.Point(4, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(851, 638);
            this.tabControl1.TabIndex = 1;
            // 
            // tabCentral
            // 
            this.tabCentral.BackColor = System.Drawing.Color.DimGray;
            this.tabCentral.Controls.Add(this.label2);
            this.tabCentral.Controls.Add(this.dgvCac);
            this.tabCentral.Controls.Add(this.lblCPU);
            this.tabCentral.Controls.Add(this.lblSys);
            this.tabCentral.Controls.Add(this.dgvSys);
            this.tabCentral.Controls.Add(this.dgvCPU);
            this.tabCentral.Controls.Add(this.btnProcWatch);
            this.tabCentral.ForeColor = System.Drawing.Color.White;
            this.tabCentral.Location = new System.Drawing.Point(4, 22);
            this.tabCentral.Name = "tabCentral";
            this.tabCentral.Size = new System.Drawing.Size(843, 612);
            this.tabCentral.TabIndex = 2;
            this.tabCentral.Text = "Central Processing";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1, 406);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "Cache Info:";
            // 
            // dgvCac
            // 
            this.dgvCac.AllowUserToAddRows = false;
            this.dgvCac.AllowUserToDeleteRows = false;
            this.dgvCac.AllowUserToOrderColumns = true;
            this.dgvCac.AllowUserToResizeRows = false;
            this.dgvCac.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvCac.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCac.Location = new System.Drawing.Point(4, 422);
            this.dgvCac.MultiSelect = false;
            this.dgvCac.Name = "dgvCac";
            this.dgvCac.ReadOnly = true;
            this.dgvCac.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvCac.RowHeadersVisible = false;
            this.dgvCac.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCac.Size = new System.Drawing.Size(833, 128);
            this.dgvCac.TabIndex = 15;
            // 
            // lblCPU
            // 
            this.lblCPU.AutoSize = true;
            this.lblCPU.Location = new System.Drawing.Point(2, 34);
            this.lblCPU.Name = "lblCPU";
            this.lblCPU.Size = new System.Drawing.Size(78, 13);
            this.lblCPU.TabIndex = 14;
            this.lblCPU.Text = "Processor Info:";
            // 
            // lblSys
            // 
            this.lblSys.AutoSize = true;
            this.lblSys.Location = new System.Drawing.Point(2, 259);
            this.lblSys.Name = "lblSys";
            this.lblSys.Size = new System.Drawing.Size(65, 13);
            this.lblSys.TabIndex = 13;
            this.lblSys.Text = "System Info:";
            // 
            // dgvSys
            // 
            this.dgvSys.AllowUserToAddRows = false;
            this.dgvSys.AllowUserToDeleteRows = false;
            this.dgvSys.AllowUserToOrderColumns = true;
            this.dgvSys.AllowUserToResizeRows = false;
            this.dgvSys.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvSys.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSys.Location = new System.Drawing.Point(5, 275);
            this.dgvSys.MultiSelect = false;
            this.dgvSys.Name = "dgvSys";
            this.dgvSys.ReadOnly = true;
            this.dgvSys.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvSys.RowHeadersVisible = false;
            this.dgvSys.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSys.Size = new System.Drawing.Size(833, 128);
            this.dgvSys.TabIndex = 12;
            // 
            // dgvCPU
            // 
            this.dgvCPU.AllowUserToAddRows = false;
            this.dgvCPU.AllowUserToDeleteRows = false;
            this.dgvCPU.AllowUserToOrderColumns = true;
            this.dgvCPU.AllowUserToResizeRows = false;
            this.dgvCPU.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvCPU.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCPU.Location = new System.Drawing.Point(5, 50);
            this.dgvCPU.MultiSelect = false;
            this.dgvCPU.Name = "dgvCPU";
            this.dgvCPU.ReadOnly = true;
            this.dgvCPU.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvCPU.RowHeadersVisible = false;
            this.dgvCPU.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCPU.Size = new System.Drawing.Size(833, 206);
            this.dgvCPU.TabIndex = 11;
            // 
            // btnProcWatch
            // 
            this.btnProcWatch.ForeColor = System.Drawing.Color.Black;
            this.btnProcWatch.Location = new System.Drawing.Point(5, 3);
            this.btnProcWatch.Name = "btnProcWatch";
            this.btnProcWatch.Size = new System.Drawing.Size(161, 28);
            this.btnProcWatch.TabIndex = 9;
            this.btnProcWatch.Text = "Start Monitoring";
            this.btnProcWatch.UseVisualStyleBackColor = true;
            this.btnProcWatch.Click += new System.EventHandler(this.ClickEventHandler);
            // 
            // tabStorage
            // 
            this.tabStorage.BackColor = System.Drawing.Color.DimGray;
            this.tabStorage.Controls.Add(this.lblPhysDisk);
            this.tabStorage.Controls.Add(this.dataGridView2);
            this.tabStorage.Controls.Add(this.lblLogDisk);
            this.tabStorage.Controls.Add(this.dataGridView1);
            this.tabStorage.Controls.Add(this.btnDiskWatch);
            this.tabStorage.ForeColor = System.Drawing.Color.White;
            this.tabStorage.Location = new System.Drawing.Point(4, 22);
            this.tabStorage.Name = "tabStorage";
            this.tabStorage.Padding = new System.Windows.Forms.Padding(3);
            this.tabStorage.Size = new System.Drawing.Size(883, 612);
            this.tabStorage.TabIndex = 0;
            this.tabStorage.Text = "Storage I/O";
            // 
            // lblPhysDisk
            // 
            this.lblPhysDisk.AutoSize = true;
            this.lblPhysDisk.Location = new System.Drawing.Point(3, 319);
            this.lblPhysDisk.Name = "lblPhysDisk";
            this.lblPhysDisk.Size = new System.Drawing.Size(94, 13);
            this.lblPhysDisk.TabIndex = 16;
            this.lblPhysDisk.Text = "Physical Disk Info:";
            // 
            // dataGridView2
            // 
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(6, 335);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.Size = new System.Drawing.Size(1120, 271);
            this.dataGridView2.TabIndex = 15;
            // 
            // lblLogDisk
            // 
            this.lblLogDisk.AutoSize = true;
            this.lblLogDisk.Location = new System.Drawing.Point(2, 34);
            this.lblLogDisk.Name = "lblLogDisk";
            this.lblLogDisk.Size = new System.Drawing.Size(89, 13);
            this.lblLogDisk.TabIndex = 14;
            this.lblLogDisk.Text = "Logical Disk Info:";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(5, 50);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(1120, 263);
            this.dataGridView1.TabIndex = 13;
            // 
            // btnDiskWatch
            // 
            this.btnDiskWatch.Enabled = false;
            this.btnDiskWatch.ForeColor = System.Drawing.Color.Black;
            this.btnDiskWatch.Location = new System.Drawing.Point(5, 3);
            this.btnDiskWatch.Name = "btnDiskWatch";
            this.btnDiskWatch.Size = new System.Drawing.Size(131, 23);
            this.btnDiskWatch.TabIndex = 11;
            this.btnDiskWatch.Text = "Start Monitoring";
            this.btnDiskWatch.UseVisualStyleBackColor = true;
            // 
            // tabMemory
            // 
            this.tabMemory.BackColor = System.Drawing.Color.DimGray;
            this.tabMemory.Location = new System.Drawing.Point(4, 22);
            this.tabMemory.Name = "tabMemory";
            this.tabMemory.Size = new System.Drawing.Size(883, 612);
            this.tabMemory.TabIndex = 3;
            this.tabMemory.Text = "Memory I/O";
            // 
            // tabNet
            // 
            this.tabNet.BackColor = System.Drawing.Color.DimGray;
            this.tabNet.Location = new System.Drawing.Point(4, 22);
            this.tabNet.Name = "tabNet";
            this.tabNet.Size = new System.Drawing.Size(883, 612);
            this.tabNet.TabIndex = 5;
            this.tabNet.Text = "Net I/O";
            // 
            // tabSoft
            // 
            this.tabSoft.BackColor = System.Drawing.Color.DimGray;
            this.tabSoft.Location = new System.Drawing.Point(4, 22);
            this.tabSoft.Name = "tabSoft";
            this.tabSoft.Size = new System.Drawing.Size(883, 612);
            this.tabSoft.TabIndex = 4;
            this.tabSoft.Text = "Soft I/O";
            // 
            // tabProcesses
            // 
            this.tabProcesses.BackColor = System.Drawing.Color.DimGray;
            this.tabProcesses.Location = new System.Drawing.Point(4, 22);
            this.tabProcesses.Name = "tabProcesses";
            this.tabProcesses.Size = new System.Drawing.Size(883, 612);
            this.tabProcesses.TabIndex = 1;
            this.tabProcesses.Text = "Processes";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(4, 648);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "I am all about that bass.";
            // 
            // lblVersion
            // 
            this.lblVersion.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lblVersion.AutoSize = true;
            this.lblVersion.ForeColor = System.Drawing.Color.White;
            this.lblVersion.Location = new System.Drawing.Point(385, 648);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(13, 13);
            this.lblVersion.TabIndex = 3;
            this.lblVersion.Text = "v";
            // 
            // Oracle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(858, 666);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "Oracle";
            this.ShowIcon = false;
            this.Text = "Oracle";
            this.tabControl1.ResumeLayout(false);
            this.tabCentral.ResumeLayout(false);
            this.tabCentral.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCac)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSys)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCPU)).EndInit();
            this.tabStorage.ResumeLayout(false);
            this.tabStorage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabProcesses;
        private System.Windows.Forms.TabPage tabCentral;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnProcWatch;
        private System.Windows.Forms.DataGridView dgvCPU;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.DataGridView dgvSys;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dgvCac;
        private System.Windows.Forms.Label lblCPU;
        private System.Windows.Forms.Label lblSys;
        private System.Windows.Forms.TabPage tabSoft;
        private System.Windows.Forms.TabPage tabNet;
        private System.Windows.Forms.TabPage tabStorage;
        private System.Windows.Forms.Button btnDiskWatch;
        private System.Windows.Forms.TabPage tabMemory;
        private System.Windows.Forms.Label lblPhysDisk;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.Label lblLogDisk;
        private System.Windows.Forms.DataGridView dataGridView1;
    }
}

