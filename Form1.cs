using HexoLocalTools.Tools;
using System.Diagnostics;
using System.IO.Packaging;

namespace HexoLocalTools
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GloalTools.HideHorizontalScrollBar(this.tempView);
            GloalTools.HideHorizontalScrollBar(this.fileView);

            ReadNote();

        }

        private void fileView_Resize(object sender, EventArgs e)
        {
            GloalTools.HideHorizontalScrollBar(this.tempView);
            GloalTools.HideHorizontalScrollBar(this.fileView);
        }


        private void hexoDocBtn_Click_1(object sender, EventArgs e)
        {
            GloalTools.ExeCmd("start https://hexo.io/zh-cn/docs/ &exit");
        }

        private void hexoTempBtn_Click(object sender, EventArgs e)
        {
            GloalTools.ExeCmd("start https://hexo.io/themes/ &exit");
        }

        private void openSite_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog d = new FolderBrowserDialog();
            if (d.ShowDialog() == DialogResult.OK)
            {
                string path = d.SelectedPath;
                //路径检测
                string packagePath = Path.Combine(path, "package.json");
                string configPath = Path.Combine(path,  "_config.yml");

                if (!File.Exists(configPath) || !File.Exists(packagePath))
                {
                    MessageBox.Show("站点异常，未能找到package和_config文件！！", "错误", MessageBoxButtons.OK);
                    return;
                }
                OpenSite(path);
            }
        }

        private void newSite_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog d = new FolderBrowserDialog();
            var result = d.ShowDialog();
            if (result == DialogResult.OK)
            {
                string path = d.SelectedPath;
                if (string.IsNullOrEmpty(path))
                    return;
                GloalTools.CreateSite(path, ShowStautsLabel);
                OpenSite(path);
            }
        }

        private void cleanBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(GloalTools.currentSite))
                return;
            GloalTools.ExeCmd(string.Format("cd {0}&hexo clean&exit",GloalTools.currentSite), ShowStautsLabel);
        }

        private void generateBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(GloalTools.currentSite))
                return;
            GloalTools.ExeCmd(string.Format("cd {0}&hexo g&exit", GloalTools.currentSite), ShowStautsLabel);
        }

        private void localPreview_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(GloalTools.currentSite))
                return;
            string cmd = string.Format("cd {0}&hexo s&exit", GloalTools.currentSite);
            GloalTools.ExeCmd(cmd, ShowStautsLabel,false);
            GloalTools.ExeCmd("start http://localhost:4000/ &exit", ShowStautsLabel);
        }

        private void onePreview_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(GloalTools.currentSite))
                return;
            string cmd = string.Format("cd {0}&hexo clean&hexo g&hexo s&exit",GloalTools.currentSite);
            GloalTools.ExeCmd(cmd, ShowStautsLabel, false);
            GloalTools.ExeCmd("start http://localhost:4000/ &exit", ShowStautsLabel);
        }

        private void deployBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(GloalTools.currentSite))
                return;
            string cmd = string.Format("cd {0}&hexo d&exit", GloalTools.currentSite);
            GloalTools.ExeCmd(cmd, ShowStautsLabel);
        }

        private void oneDeploy_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(GloalTools.currentSite))
                return;
            string cmd = string.Format("cd {0}&hexo clean&hexo g&hexo d", GloalTools.currentSite);
            GloalTools.ExeCmd(cmd, ShowStautsLabel);
        }

        private void editSiteConfig_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(GloalTools.currentSite))
                return;
            string configPath = Path.Combine(GloalTools.currentSite, "_config.yml");
            if (!File.Exists(configPath))
            {
                MessageBox.Show(string.Format("路径：{0} 不存在！！", configPath));
                return;
            }
            Process.Start("notepad.exe",configPath);
        }

        private void openSitePath_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(GloalTools.currentSite))
                return;
            Process.Start("Explorer.exe", GloalTools.currentSite);
        }

        #region 模版列表

        private string currentSelectTemp = "";

        private void openTempPath_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(GloalTools.currentSite))
                return;
            string tempPath = Path.Combine(GloalTools.currentSite, "scaffolds");
            if (!Directory.Exists(tempPath))
            {
                MessageBox.Show(string.Format("路径：{0} 不存在！！", tempPath));
                return;
            }

            Process.Start("Explorer.exe",tempPath);
        }

        private void onRefresh_Click(object sender, EventArgs e)
        {
            RefreshTemp();
        }


        private void tempView_MouseClick(object sender, MouseEventArgs e)
        {
            var items = this.tempView.SelectedItems;
            if (items == null || items.Count <= 0)
                return;
            var target = items[0];
            currentSelectTemp = target.Text;
            if (e.Button != MouseButtons.Right)
                return;
            this.tempRightMenu.Show(this.tempView,e.Location);
        }

        private void 新建ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (InputDialog.Show("请输入名称：", "new File", out string fileName) != DialogResult.OK)
                return;

            var items = this.tempView.SelectedItems;
            if (items == null || items.Count <= 0)
                return;
            var target = items[0];

            string cmd = string.Format("cd {2}&hexo n {0} \"{1}\"&exit", Path.GetFileNameWithoutExtension(target.Text), fileName,GloalTools.currentSite);

            GloalTools.ExeCmd(cmd, ShowStautsLabel);
            RefreshFiels();
        }


        private void RefreshTemp()
        {
            if (string.IsNullOrEmpty(GloalTools.currentSite))
                return;
            string tempPath = Path.Combine(GloalTools.currentSite, "scaffolds/");
            string[] files = Directory.GetFiles(tempPath, "*", SearchOption.AllDirectories);
            this.tempView.Items.Clear();
            for (int i = 0; i < files.Length; i++)
            {
                string p = files[i];
                if (string.IsNullOrEmpty(p))
                    continue;

                string name = p.Replace("\\", "/").Replace(tempPath.Replace("\\", "/"), "");
                var item = this.tempView.Items.Add(name);
                item.Selected = name.Equals(currentSelectTemp);
            }
        }

        #endregion

        #region 文章列表

        private string currentSelectFile = "";

        private void openFilePathBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(GloalTools.currentSite))
                return;
            string filePath = Path.Combine(GloalTools.currentSite, "source/");
            if (!Directory.Exists(filePath))
            {
                MessageBox.Show(string.Format("路径：{0} 不存在！！", filePath));
                return;
            }
            Process.Start("Explorer.exe", filePath);
        }


        private void refreshFileBtn_Click(object sender, EventArgs e)
        {
            RefreshFiels();
        }

        private void fileView_MouseClick(object sender, MouseEventArgs e)
        {
            string filePath = Path.Combine(GloalTools.currentSite, "source/");
            var items = this.fileView.SelectedItems;
            if (items == null || items.Count <= 0)
                return;
            var target = items[0];
            SaveEditorFile();
            currentSelectFile = target.Text;

            if (e.Button == MouseButtons.Right)
                this.fileRightMenu.Show(this.fileView, e.Location);
            if (e.Button == MouseButtons.Left)
            {
                
                string fullPath = Path.Combine(filePath,currentSelectFile);
                if (!File.Exists(fullPath))
                {
                    MessageBox.Show(string.Format("路径：{0} 不存在！！", fullPath));
                    RefreshFiels();
                    return;
                }
                this.label1.Text = "当前编辑："+currentSelectFile;

                this.fileEditor.Text = File.ReadAllText(fullPath);
            }
        }


        private void fileEditor_TextChanged(object sender, EventArgs e)
        {
            this.label1.Text = "当前编辑：" + currentSelectFile+" (未保存)";
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var items = this.fileView.SelectedItems;
            if (items == null || items.Count <= 0)
                return;
            var target = items[0];
            string filePath = Path.Combine(GloalTools.currentSite, "source/");
            string fullPath = Path.Combine(filePath,target.Text);
            if (!File.Exists(fullPath))
            {
                MessageBox.Show(string.Format("路径：{0} 不存在！！", filePath));
                RefreshFiels();
                return;
            }

            File.Delete(fullPath);
            RefreshFiels();
            
        }

        private void RefreshFiels()
        {
            if (string.IsNullOrEmpty(GloalTools.currentSite))
                return;
            string filePath = Path.Combine(GloalTools.currentSite, "source/");
            string[] files = Directory.GetFiles(filePath, "*.md", SearchOption.AllDirectories);
            this.fileView.Items.Clear();
            for (int i = 0; i < files.Length; i++)
            {
                string p = files[i];
                if (string.IsNullOrEmpty(p))
                    continue;

                string name = p.Replace("\\", "/").Replace(filePath.Replace("\\", "/"), "");
                var item = this.fileView.Items.Add(name);
                item.Selected = name.Equals(currentSelectFile);
            }
        }

        private void SaveEditorFile()
        {
            if (!string.IsNullOrEmpty(currentSelectFile))
            {
                string filePath = Path.Combine(GloalTools.currentSite, "source/");
                string fullPath = Path.Combine(filePath, currentSelectFile);
                File.WriteAllText(fullPath, fileEditor.Text);
                this.label1.Text = "当前编辑：" + currentSelectFile;
            }
        }

        #endregion

        private void OpenSite(string path)
        {
            SaveEditorFile();
            this.label1.Text = "当前编辑：";
            currentSelectFile = "";
            currentSelectTemp = "";
            this.fileEditor.Text = "";

            ShowStautsLabel(string.Format("打开站点：{0}", path));
            GloalTools.currentSite = path;
            this.Text = "Hexo 快捷工具 - " + Path.GetFileName(path);
            RefreshTemp();
            RefreshFiels();
        }



        #region stauts Label

        private long preShowTime = 0;

        private void ShowStautsLabel(string text)
        {
            this.statusLabel.Text = text;
            preShowTime = DateTime.Now.Second;
        }

        private void statusTick_Tick(object sender, EventArgs e)
        {
            if ((DateTime.Now.Second - preShowTime) >= 3)
                this.statusLabel.Text = "";
        }

        #endregion

        private void ReadNote()
        {
            string path = Path.Combine(Environment.CurrentDirectory, "note.txt");
            if (File.Exists(path))
            {
                this.noteBox.Text = File.ReadAllText(path);
                return;
            }
            this.noteBox.Text = "";
        }

        private void SaveNote()
        {
            this.label2.Text = "状态：已保存";
            string path = Path.Combine(Environment.CurrentDirectory, "note.txt");
            File.WriteAllText(path,this.noteBox.Text);
        }


        private void openNotePath_Click(object sender, EventArgs e)
        {
            string path = Path.Combine(Environment.CurrentDirectory, "");
            Process.Start("Explorer.exe", path);

        }
        private void noteBox_TextChanged(object sender, EventArgs e)
        {
            this.label2.Text = "状态：未保存";
        }

        

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                SaveEditorFile();
                SaveNote();
            }
        }

    }
}