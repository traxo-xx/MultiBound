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
        public virtual bool Verbose { get { return true; } set { } }

        public Celestial defaultCelestial
        {
            get {
                List<sector> s = new List<sector>();
                s.Add(new sector("alpha", "Alpha Sector", 71707887027, "Alpha", null, null));
                s.Add(new sector("beta", "Beta Sector", 912044941247, "Beta", null, null));
                s.Add(new sector("gamma", "Gamma Sector", 877461781495, "Gamma", null, null));
                s.Add(new sector("delta", "Delta Sector", 914461284455, "Delta", null, null));
                s.Add(new sector("sectorx", "X Sector", 427469981495, "X", null, null));
                Celestial c = new Celestial(12, 50, -100000000, 100000000, -100000000, 100000000, s.ToArray());
                return c;
            }
        }
            
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
        private void newConsoleLine(string NewLine, System.Drawing.Color TextColor, bool _Verbose)
        {
            if (rtConsole.InvokeRequired)
            {
                rtConsole.Invoke(new Action<string, System.Drawing.Color, bool>(newConsoleLine), NewLine, TextColor, Verbose);
                return;
            }
            if (_Verbose == false | Verbose == true)
            {
                rtConsole.SelectionColor = TextColor;
                rtConsole.Select(rtConsole.Text.Length, 0);
                rtConsole.AppendText(NewLine + Environment.NewLine);
                rtConsole.Select(rtConsole.Text.Length, 0);
                rtConsole.ScrollToCaret();
            }
        }
    }
}
