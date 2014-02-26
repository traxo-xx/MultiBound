using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultiBound
{
    public partial class MainForm : Form
    {
        public StarServer star;
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            star = new StarServer();
            star.newConsoleLine += newConsoleLine;
        }
        private void newConsoleLine(string NewLine, System.Drawing.Color TextColor)
        {
            if (rtConsole.InvokeRequired)
            {
                rtConsole.Invoke(new Action<string, System.Drawing.Color>(newConsoleLine), NewLine, TextColor);
                return;
            }
            rtConsole.SelectionColor = TextColor;
            rtConsole.Select(rtConsole.Text.Length, 0);
            rtConsole.AppendText(NewLine + Environment.NewLine);
            rtConsole.Select(rtConsole.Text.Length, 0);
            rtConsole.ScrollToCaret();
        }
    }
}
