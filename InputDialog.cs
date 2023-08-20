using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HexoLocalTools
{
    public partial class InputDialog : Form
    {
        public static DialogResult Show(string tips, string content, out string input)
        {
            if (string.IsNullOrEmpty(content))
                content = string.Empty;
            string tempInput = content;
            input = content;
            InputDialog d = new InputDialog();
            d.SetTips(tips);
            d.SetContent(content);
            d.onCallback = (s) =>
            {
                tempInput = s;
            };
            DialogResult result = d.ShowDialog();
            input = tempInput;
            return result;
        }

        public Action<string> onCallback;

        public InputDialog()
        {
            InitializeComponent();
        }

        public void SetTips(string tips)
        {
            this.label1.Text = tips;
        }

        public void SetContent(string content)
        {
            this.inputBox.Text = content;
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            onCallback?.Invoke(inputBox.Text);
            DialogResult = DialogResult.OK;
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void inputBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Keys.Enter == (Keys)e.KeyChar)
            {
                onCallback?.Invoke(inputBox.Text);
                DialogResult = DialogResult.OK;
            }
        }
    }
}
