using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace JilbMetricsParser
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "C++ files|*.cpp;*.h;*.hpp;*.cxx|All files|*.*";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtCode.Text = File.ReadAllText(ofd.FileName);
                }
            }
        }

        private void btnAnalyze_Click(object sender, EventArgs e)
        {
            string code = txtCode.Text;

            string cleaned = RemoveCommentsAndStrings(code);

            int CL, CLI, Nop;
            CalculateJilbMetrics(cleaned, out CL, out CLI, out Nop);

            double cl = Nop > 0 ? (double)CL / Nop : 0.0;

            lblCL.Text = $"CL (абсолютная): {CL}";
            lblCLI.Text = $"CLI (макс. вложенность): {CLI}";
            lblcl.Text = $"cl (относительная): {cl:F3}";
            lblNop.Text = $"Nop (число операторов): {Nop}";
        }

        private string RemoveCommentsAndStrings(string code)
        {
            code = Regex.Replace(code, @"//.*?$", "", RegexOptions.Multiline);
            code = Regex.Replace(code, @"/\*.*?\*/", "", RegexOptions.Singleline);
            code = Regex.Replace(code, "\"(\\\\.|[^\"\\\\])*\"", "\"\"", RegexOptions.Singleline);
            code = Regex.Replace(code, "'(\\\\.|[^'\\\\])+'", "''", RegexOptions.Singleline);
            return code;
        }

        private void CalculateJilbMetrics(string code, out int CL, out int CLI, out int Nop)
        {
            CL = 0;
            CLI = 0;
            Nop = 0;

            int currentDepth = 0;

            foreach (char c in code)
            {
                if (c == ';')
                    Nop++;
            }

            var tokens = Regex.Split(code, @"(\s+|[\(\){};])");

            bool lastWasDo = false;
            int switchCaseCount = 0;
            bool insideSwitch = false;

            foreach (var raw in tokens)
            {
                string token = raw.Trim();
                if (string.IsNullOrEmpty(token))
                    continue;

                if (token == "{")
                {
                    currentDepth++;
                    CLI = Math.Max(CLI, currentDepth);
                    continue;
                }

                if (token == "}")
                {
                    currentDepth = Math.Max(0, currentDepth - 1);
                    continue;
                }

                if (token == "for" || token == "while")
                {
                    CL++;
                    currentDepth++;
                    CLI = Math.Max(CLI, currentDepth);
                    continue;
                }

                if (token == "do")
                {
                    CL++;
                    currentDepth++;
                    CLI = Math.Max(CLI, currentDepth);
                    lastWasDo = true;
                    continue;
                }

                if (token == "while" && lastWasDo)
                {
                    lastWasDo = false;
                    currentDepth = Math.Max(0, currentDepth - 1);
                    continue;
                }

                if (token == "if")
                {
                    CL++;
                    currentDepth++;
                    CLI = Math.Max(CLI, currentDepth);
                    continue;
                }

                if (token == "switch")
                {
                    insideSwitch = true;
                    switchCaseCount = 0;
                    currentDepth++;
                    CLI = Math.Max(CLI, currentDepth);
                    continue;
                }

                if (insideSwitch && token == "case")
                {
                    switchCaseCount++;
                    continue;
                }

                if (insideSwitch && token == "}")
                {
                    if (switchCaseCount > 0)
                        CL += (switchCaseCount - 1);

                    insideSwitch = false;
                    currentDepth = Math.Max(0, currentDepth - 1);
                }
            }
        }
    }
}
