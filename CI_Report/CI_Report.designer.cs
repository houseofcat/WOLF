namespace Wolf.CI_Report
{
    partial class CI_Report
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
            this.dgvCompInfo = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCompInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvCompInfo
            // 
            this.dgvCompInfo.AllowUserToAddRows = false;
            this.dgvCompInfo.AllowUserToDeleteRows = false;
            this.dgvCompInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCompInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCompInfo.Location = new System.Drawing.Point(0, 0);
            this.dgvCompInfo.Name = "dgvCompInfo";
            this.dgvCompInfo.ReadOnly = true;
            this.dgvCompInfo.RowHeadersVisible = false;
            this.dgvCompInfo.Size = new System.Drawing.Size(900, 464);
            this.dgvCompInfo.TabIndex = 0;
            this.dgvCompInfo.VirtualMode = true;
            // 
            // AllComputerInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 464);
            this.Controls.Add(this.dgvCompInfo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "AllComputerInfo";
            this.ShowIcon = false;
            this.Text = "Computer Info Report (v0.001)";
            ((System.ComponentModel.ISupportInitialize)(this.dgvCompInfo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvCompInfo;
    }
}