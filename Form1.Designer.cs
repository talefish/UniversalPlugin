using System.Windows.Forms;

namespace Doffish {
    partial class Form1 {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.FileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ReloadMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CloseMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SplitMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.ExitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.HelpMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SplitMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.AboutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RefreshMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.PluginVer = new System.Windows.Forms.ToolStripStatusLabel();
            this.GameVer = new System.Windows.Forms.ToolStripStatusLabel();
            this.copyright = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileMenuItem,
            this.HelpMenuItem,
            this.RefreshMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(544, 25);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // FileMenuItem
            // 
            this.FileMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenMenuItem,
            this.ReloadMenuItem,
            this.CloseMenuItem,
            this.SplitMenuItem1,
            this.ExitMenuItem});
            this.FileMenuItem.Name = "FileMenuItem";
            this.FileMenuItem.Size = new System.Drawing.Size(58, 21);
            this.FileMenuItem.Text = "文件(&F)";
            // 
            // OpenMenuItem
            // 
            this.OpenMenuItem.Name = "OpenMenuItem";
            this.OpenMenuItem.Size = new System.Drawing.Size(151, 22);
            this.OpenMenuItem.Text = "打开配置(&O)...";
            this.OpenMenuItem.Click += new System.EventHandler(this.OpenMenuItem_Click);
            // 
            // ReloadMenuItem
            // 
            this.ReloadMenuItem.Name = "ReloadMenuItem";
            this.ReloadMenuItem.Size = new System.Drawing.Size(151, 22);
            this.ReloadMenuItem.Text = "重新加载(&R)";
            this.ReloadMenuItem.Click += new System.EventHandler(this.ReloadMenuItem_Click);
            // 
            // CloseMenuItem
            // 
            this.CloseMenuItem.Name = "CloseMenuItem";
            this.CloseMenuItem.Size = new System.Drawing.Size(151, 22);
            this.CloseMenuItem.Text = "卸载配置(&C)";
            this.CloseMenuItem.Click += new System.EventHandler(this.CloseMenuItem_Click);
            // 
            // SplitMenuItem1
            // 
            this.SplitMenuItem1.Name = "SplitMenuItem1";
            this.SplitMenuItem1.Size = new System.Drawing.Size(148, 6);
            // 
            // ExitMenuItem
            // 
            this.ExitMenuItem.Name = "ExitMenuItem";
            this.ExitMenuItem.Size = new System.Drawing.Size(151, 22);
            this.ExitMenuItem.Text = "退出程序(&X)";
            this.ExitMenuItem.Click += new System.EventHandler(this.ExitMenuItem_Click);
            // 
            // HelpMenuItem
            // 
            this.HelpMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ViewMenuItem,
            this.SplitMenuItem2,
            this.AboutMenuItem});
            this.HelpMenuItem.Name = "HelpMenuItem";
            this.HelpMenuItem.Size = new System.Drawing.Size(61, 21);
            this.HelpMenuItem.Text = "帮助(&H)";
            // 
            // ViewMenuItem
            // 
            this.ViewMenuItem.Name = "ViewMenuItem";
            this.ViewMenuItem.Size = new System.Drawing.Size(155, 22);
            this.ViewMenuItem.Text = "查看帮助(&V)";
            this.ViewMenuItem.Click += new System.EventHandler(this.ViewMenuItem_Click);
            // 
            // SplitMenuItem2
            // 
            this.SplitMenuItem2.Name = "SplitMenuItem2";
            this.SplitMenuItem2.Size = new System.Drawing.Size(152, 6);
            // 
            // AboutMenuItem
            // 
            this.AboutMenuItem.Name = "AboutMenuItem";
            this.AboutMenuItem.Size = new System.Drawing.Size(155, 22);
            this.AboutMenuItem.Text = "关于 Plugin(&A)";
            this.AboutMenuItem.Click += new System.EventHandler(this.AboutMenuItem_Click);
            // 
            // RefreshMenuItem
            // 
            this.RefreshMenuItem.Name = "RefreshMenuItem";
            this.RefreshMenuItem.Size = new System.Drawing.Size(60, 21);
            this.RefreshMenuItem.Text = "刷新(&R)";
            this.RefreshMenuItem.Click += new System.EventHandler(this.RefreshMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.PluginVer,
            this.GameVer,
            this.copyright});
            this.statusStrip1.Location = new System.Drawing.Point(0, 359);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(544, 22);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // PluginVer
            // 
            this.PluginVer.AutoSize = false;
            this.PluginVer.Name = "PluginVer";
            this.PluginVer.Size = new System.Drawing.Size(200, 17);
            this.PluginVer.Text = "PluginVer";
            this.PluginVer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // GameVer
            // 
            this.GameVer.AutoSize = false;
            this.GameVer.Name = "GameVer";
            this.GameVer.Size = new System.Drawing.Size(200, 17);
            this.GameVer.Text = "GameVer";
            this.GameVer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // copyright
            // 
            this.copyright.Name = "copyright";
            this.copyright.Size = new System.Drawing.Size(129, 17);
            this.copyright.Spring = true;
            this.copyright.Text = "ForgotFish©2020";
            this.copyright.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 25);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(100);
            this.panel1.Size = new System.Drawing.Size(544, 334);
            this.panel1.TabIndex = 5;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(544, 381);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MaximumSize = new System.Drawing.Size(1120, 420);
            this.MinimumSize = new System.Drawing.Size(560, 420);
            this.Name = "Form1";
            this.Text = "Plugin";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ToolStripMenuItem AboutMenuItem;
        private ToolStripMenuItem CloseMenuItem;
        private ToolStripStatusLabel copyright;
        private ToolStripMenuItem ExitMenuItem;
        private ToolStripMenuItem FileMenuItem;
        private ToolStripStatusLabel GameVer;
        private ToolStripMenuItem HelpMenuItem;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem OpenMenuItem;
        private Panel panel1;
        private ToolStripStatusLabel PluginVer;
        private ToolStripMenuItem RefreshMenuItem;
        private ToolStripMenuItem ReloadMenuItem;
        private ToolStripSeparator SplitMenuItem1;
        private ToolStripSeparator SplitMenuItem2;
        private StatusStrip statusStrip1;
        private ToolTip toolTip1;
        private ToolStripMenuItem ViewMenuItem;
    }
}