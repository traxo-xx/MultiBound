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
            star = new StarServer(21025);
            star.newConsoleLine += newConsoleLine;
        }
        private void newConsoleLine(string NewLine)
        {
            if (rtConsole.InvokeRequired)
            {
                rtConsole.Invoke(new Action<string>(newConsoleLine), NewLine);
                return;
            } 
            rtConsole.Text += Environment.NewLine + NewLine;
            rtConsole.Select(rtConsole.Text.Length, 0);
            rtConsole.ScrollToCaret();
        }
    }
}
