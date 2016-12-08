namespace cpsLIB
{
    partial class FrmStatusLog
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
            this.dGV_Log = new System.Windows.Forms.DataGridView();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoScrollToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.freezeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dGV_Log)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dGV_Log
            // 
            this.dGV_Log.AllowUserToOrderColumns = true;
            this.dGV_Log.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGV_Log.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dGV_Log.Location = new System.Drawing.Point(0, 24);
            this.dGV_Log.Name = "dGV_Log";
            this.dGV_Log.Size = new System.Drawing.Size(1094, 560);
            this.dGV_Log.TabIndex = 3;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearToolStripMenuItem,
            this.showAllToolStripMenuItem,
            this.autoScrollToolStripMenuItem,
            this.freezeToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1094, 24);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            this.clearToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.clearToolStripMenuItem.Text = "clear";
            this.clearToolStripMenuItem.Click += new System.EventHandler(this.clearToolStripMenuItem_Click);
            // 
            // showAllToolStripMenuItem
            // 
            this.showAllToolStripMenuItem.Name = "showAllToolStripMenuItem";
            this.showAllToolStripMenuItem.Size = new System.Drawing.Size(62, 20);
            this.showAllToolStripMenuItem.Text = "show all";
            this.showAllToolStripMenuItem.Click += new System.EventHandler(this.showAllToolStripMenuItem_Click);
            // 
            // autoScrollToolStripMenuItem
            // 
            this.autoScrollToolStripMenuItem.Name = "autoScrollToolStripMenuItem";
            this.autoScrollToolStripMenuItem.Size = new System.Drawing.Size(74, 20);
            this.autoScrollToolStripMenuItem.Text = "auto scroll";
            this.autoScrollToolStripMenuItem.Click += new System.EventHandler(this.autoScrollToolStripMenuItem_Click);
            // 
            // freezeToolStripMenuItem
            // 
            this.freezeToolStripMenuItem.Name = "freezeToolStripMenuItem";
            this.freezeToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.freezeToolStripMenuItem.Text = "freeze";
            this.freezeToolStripMenuItem.Click += new System.EventHandler(this.freezeToolStripMenuItem_Click);
            // 
            // FrmStatusLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1094, 584);
            this.Controls.Add(this.dGV_Log);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FrmStatusLog";
            this.Text = "Status Log";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmStatusLog_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dGV_Log)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dGV_Log;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem autoScrollToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem freezeToolStripMenuItem;
    }
}