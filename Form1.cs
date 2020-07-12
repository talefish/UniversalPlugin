namespace Doffish {
    using Doffish.MemoryPlugin;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    public partial class Form1 : Form {
        private int buttonh = 0x19;
        private int buttonl = 10;
        private int buttont = 10;
        private int buttonw = 200;
        private int interval = 30;
        private Plugin plugin;
        private Dictionary<string, Dictionary<string, uint>> pluginMem = new Dictionary<string, Dictionary<string, uint>>();
        private int rows = 10;


        public Form1() {
            InitializeComponent();
        }

        private void AboutMenuItem_Click(object sender, EventArgs e) {
            MessageBox.Show("通用游戏内存修改器 v1.0.0\n\n无需编程基础，只要会用CE找基址，\n\n会写JSON文件即可。\n\nForgotFish\x00a92020", "关于 Plugin", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void addButton(PluginItem plugin) {
            Label control = new Label {
                Tag = plugin.dictKey,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleLeft,
                Text = plugin.name,
                Top = buttont,
                Left = buttonl,
                Height = buttonh,
                Width = buttonw - 100,
                Visible = true
            };
            control.Click += new EventHandler(Plugin_Click);
            toolTip1.SetToolTip(control, plugin.description);
            panel1.Controls.Add(control);
            Label label2 = new Label {
                Tag = plugin.dictKey,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleRight,
                Text = plugin.hotkey,
                Top = buttont,
                Left = buttonl + control.Width,
                Width = 100
            };
            label2.Click += new EventHandler(Plugin_Click);
            toolTip1.SetToolTip(label2, plugin.hotkey);
            panel1.Controls.Add(label2);
            Label label3 = new Label {
                Tag = plugin.dictKey,
                Name = plugin.dictKey,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleRight,
                ForeColor = GetColor(plugin.verify),
                Text = plugin.verify,
                Top = buttont,
                Left = (buttonl + control.Width) + 100,
                Width = 50
            };
            label3.Click += new EventHandler(Plugin_Click);
            panel1.Controls.Add(label3);
            base.ControlBox = true;
            if ((interval * (rows - 1)) <= buttont) {
                buttonl += buttonw + 0x4b;
                buttont -= (interval * rows) - interval;
            } else {
                buttont += interval;
            }
        }

        private void CloseMenuItem_Click(object sender, EventArgs e) {
            closePlugin();
        }

        private void ExitMenuItem_Click(object sender, EventArgs e) {
            base.Close();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e) {
            closePlugin();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
            closePlugin();
        }

        private Color GetColor(string memValue) {
            uint num;
            if (uint.TryParse(memValue, out num)) {
                if (num > 0) {
                    return ColorTranslator.FromHtml(plugin.actionColor);
                }
                return ColorTranslator.FromHtml(plugin.defaultColor);
            }
            if (memValue == "开启") {
                return ColorTranslator.FromHtml(plugin.actionColor);
            }
            return ColorTranslator.FromHtml(plugin.defaultColor);
        }


        private void initPlugin(string profile) {
            try {
                plugin = new Plugin(profile, base.Handle);
                initPlugin();
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Plugin", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void initPlugin() {
            try {
                if (plugin != null) {
                    rows = 10;
                    buttonl = 10;
                    buttont = 10;
                    buttonw = 200;
                    buttonh = 0x19;
                    interval = 30;
                    Text = plugin.title + " " + plugin.version;
                    PluginVer.Text = "PluginVer: " + plugin.version;
                    GameVer.Text = "GameVer: " + plugin.gamever;
                    pluginMem.Clear();
                    foreach (PluginItem item in plugin.plugins.Values) {
                        Dictionary<string, uint> dictionary;
                        plugin.read(item.dictKey, out dictionary);
                        pluginMem.Add(item.dictKey, dictionary);
                        addButton(item);
                    }
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Plugin", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void closePlugin() {
            try {
                panel1.Controls.Clear();
                plugin.Close();
                plugin = null;
                Text = "Plugin";
                PluginVer.Text = "PluginVer";
                GameVer.Text = "GameVer";
            } catch {
            }
        }

        private void OpenMenuItem_Click(object sender, EventArgs e) {
            if (plugin != null) {
                if (MessageBox.Show("是否关闭当前配置文件", "Plugin",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question,MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                    return;
                closePlugin();
            }

            OpenFileDialog dialog = new OpenFileDialog {
                Filter = "Plug文件|*.plug|Json文件|*.json|所有文件|*.*",
                RestoreDirectory = true,
                FilterIndex = 1
            };
            if (dialog.ShowDialog() == DialogResult.OK) {
                initPlugin(dialog.FileName);
            }
        }

        private void Plugin_Click(object sender, EventArgs e) {
            Dictionary<string, uint> dictionary;
            string plugin = ((Label)sender).Tag.ToString();
            this.plugin.realize(plugin, out dictionary);
            panel1.Controls[plugin].Text = this.plugin.plugins[plugin].verify;
            panel1.Controls[plugin].ForeColor = GetColor(this.plugin.plugins[plugin].verify);
        }

        private void RefreshMenuItem_Click(object sender, EventArgs e) {
            try {
                panel1.Controls.Clear();
                initPlugin();
            } catch {
            }
        }

        private void ReloadMenuItem_Click(object sender, EventArgs e) {
            try {
                panel1.Controls.Clear();
                plugin.reload();
                initPlugin();
            } catch {
            }
        }

        private void ViewMenuItem_Click(object sender, EventArgs e) {
            MessageBox.Show("实话说我懒得写帮助，\n\n为你导出一个Demo文件，自己理解好了\n\nDemo是Treasure Hunt的Plugin哦~", "帮助", MessageBoxButtons.OK, MessageBoxIcon.Question);
            SaveFileDialog dialog = new SaveFileDialog {
                Filter = "Plug文件|*.plug|Json文件|*.json|所有文件|*.*",
                FileName = "Demon",
                RestoreDirectory = true,
                FilterIndex = 1
            };
            do {
                if (dialog.ShowDialog() != DialogResult.OK) {
                    return;
                }
            }
            while (File.Exists(dialog.FileName) && (MessageBox.Show("文件已经存在，是否覆盖文件？", "导出Demo", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) != DialogResult.Yes));
            try {
                byte[] plugin = Properties.Resource.Plugin;
                FileStream stream1 = new FileStream(dialog.FileName, FileMode.Create);
                stream1.Write(plugin, 0, plugin.Length);
                stream1.Close();
                MessageBox.Show("导出成功", "导出Demo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            } catch (Exception exception) {
                MessageBox.Show((exception.Message == "") ? "导出Demo时遇到错误" : exception.Message, "导出Demo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        protected override void WndProc(ref Message msg) {
            if (msg.Msg == 0x312) {
                Dictionary<string, uint> dictionary;
                string str;
                int hotkey = msg.WParam.ToInt32();
                plugin.realize(hotkey, out dictionary, out str);
                panel1.Controls[str].Text = plugin.plugins[str].verify;
                panel1.Controls[str].ForeColor = GetColor(plugin.plugins[str].verify);
            }
            base.WndProc(ref msg);
        }
    }
}

