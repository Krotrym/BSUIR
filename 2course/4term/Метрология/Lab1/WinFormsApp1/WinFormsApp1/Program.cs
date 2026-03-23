using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace GilbMetricsFinal
{
    public class MainForm : Form
    {
        private TextBox txtCode;
        private Button btnAnalyze;
        private RichTextBox txtResult;

        public MainForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Анализатор метрик Джилба (C++)";
            this.Size = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 10);

            txtCode = new TextBox
            {
                Location = new Point(12, 12),
                Size = new Size(860, 300),
                Multiline = true,
                ScrollBars = ScrollBars.Both,
                Font = new Font("Consolas", 10),
                Text = @"#include <iostream>
using namespace std;

int main() {
    int x = 2;
    
    // Пример 1: for с if внутри
    // CL = 2 (for + if), CLI = 2 (for + if)
    for (int i = 0; i < 5; i++) {
        if (i % 2 == 0) {
            cout << i << endl;
        }
    }
    
    // Пример 2: switch с 3 case
    // CL = 3 (3 case), CLI = 2
    switch (x) {
        case 1:
            cout << ""One"" << endl;
            break;
        case 2:
            cout << ""Two"" << endl;
            break;
        case 3:
            cout << ""Three"" << endl;
            break;
    }
    
    // Пример 3: switch с case и if
    // CL = 4 (3 case + if), CLI = 3
    switch (x) {
        case 1:
            cout << ""One"" << endl;
            break;
        case 2:
            cout << ""Two"" << endl;
            break;
        case 3:
            if (x > 0) {
                cout << ""Three"" << endl;
            }
            break;
    }
    
    // Пример 4: switch с case, for и if
    // CL = 5 (3 case + for + if), CLI = 4
    switch (x) {
        case 1:
            break;
        case 2:
            break;
        case 3:
            for (int i = 0; i < 5; i++) {
                if (i % 2 == 0) {
                    cout << i << endl;
                }
            }
            break;
    }
    
    return 0;
}"
            };

            btnAnalyze = new Button
            {
                Text = "Анализировать",
                Location = new Point(12, 320),
                Size = new Size(150, 30),
                BackColor = Color.LightBlue
            };
            btnAnalyze.Click += BtnAnalyze_Click;

            txtResult = new RichTextBox
            {
                Location = new Point(12, 360),
                Size = new Size(860, 200),
                ReadOnly = true,
                Font = new Font("Consolas", 10),
                BackColor = Color.White
            };

            this.Controls.Add(txtCode);
            this.Controls.Add(btnAnalyze);
            this.Controls.Add(txtResult);
        }

        private void BtnAnalyze_Click(object sender, EventArgs e)
        {
            try
            {
                string code = txtCode.Text;
                var metrics = AnalyzeGilbMetrics(code);
                DisplayMetrics(metrics);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private class GilbMetrics
        {
            public int CL { get; set; }
            public int TotalStatements { get; set; }
            public double cl => TotalStatements > 0 ? (double)CL / TotalStatements : 0;
            public int CLI { get; set; }
            public Dictionary<string, int> Details { get; set; } = new Dictionary<string, int>();
        }

        // Класс для хранения информации о switch
        private class SwitchInfo
        {
            public int BraceLevelAtEntry { get; set; }
            public int DepthAtEntry { get; set; }
            public int CaseCount { get; set; }
        }

        private GilbMetrics AnalyzeGilbMetrics(string code)
        {
            string cleaned = PreprocessCode(code);
            var metrics = new GilbMetrics();

            int cl = 0;
            int depth = 0;
            int maxDepth = 0;
            int braceLevel = 0;
            var details = new Dictionary<string, int>();

            Stack<string> depthStack = new Stack<string>();
            Stack<SwitchInfo> switchStack = new Stack<SwitchInfo>();

            string[] lines = cleaned.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].Trim();
                if (string.IsNullOrWhiteSpace(line)) continue;
                if (line.StartsWith("#")) continue;

                int openBraces = line.Count(c => c == '{');
                int closeBraces = line.Count(c => c == '}');

                string[] words = Regex.Split(line, @"(\W+)").Where(w => !string.IsNullOrEmpty(w)).ToArray();

                for (int j = 0; j < words.Length; j++)
                {
                    string word = words[j].Trim();
                    if (string.IsNullOrWhiteSpace(word) || Regex.IsMatch(word, @"^\W+$")) continue;

                    // Обработка for, while, do (теперь увеличивают глубину)
                    if (word == "for" || word == "while" || word == "do")
                    {
                        cl++;
                        depth++;
                        depthStack.Push("loop");
                        if (depth > maxDepth) maxDepth = depth;
                        if (details.ContainsKey(word)) details[word]++;
                        else details[word] = 1;
                    }
                    // Обработка if
                    else if (word == "if")
                    {
                        bool isElseIf = j > 1 && words[j - 2] == "else";
                        if (!isElseIf)
                        {
                            cl++;
                            depth++;
                            depthStack.Push("if");
                            if (depth > maxDepth) maxDepth = depth;
                            if (details.ContainsKey("if")) details["if"]++;
                            else details["if"] = 1;
                        }
                        else
                        {
                            cl++;
                            if (details.ContainsKey("else if")) details["else if"]++;
                            else details["else if"] = 1;
                        }
                    }
                    // Обработка switch
                    else if (word == "switch")
                    {
                        var si = new SwitchInfo
                        {
                            BraceLevelAtEntry = braceLevel,
                            DepthAtEntry = depth,
                            CaseCount = 0
                        };
                        switchStack.Push(si);
                    }
                    // Обработка case/default
                    else if (word == "case" || word == "default")
                    {
                        if (switchStack.Count > 0)
                        {
                            var si = switchStack.Peek();
                            si.CaseCount++;
                            cl++;
                            if (details.ContainsKey("case")) details["case"]++;
                            else details["case"] = 1;

                            if (si.CaseCount > 1)
                            {
                                depth++;
                                depthStack.Push("case");
                                if (depth > maxDepth) maxDepth = depth;
                            }
                        }
                    }
                }

                braceLevel += openBraces;

                for (int j = 0; j < closeBraces; j++)
                {
                    braceLevel--;

                    // Закрытие if или loop
                    if (depthStack.Count > 0 && (depthStack.Peek() == "if" || depthStack.Peek() == "loop"))
                    {
                        depthStack.Pop();
                        depth--;
                    }

                    // Проверка закрытия switch
                    if (switchStack.Count > 0)
                    {
                        var si = switchStack.Peek();
                        if (braceLevel == si.BraceLevelAtEntry)
                        {
                            int caseToRemove = si.CaseCount - 1;
                            for (int k = 0; k < caseToRemove; k++)
                            {
                                if (depthStack.Count > 0 && depthStack.Peek() == "case")
                                {
                                    depthStack.Pop();
                                    depth--;
                                }
                                else
                                {
                                    if (depth > 0) depth--;
                                }
                            }
                            depth = si.DepthAtEntry;
                            switchStack.Pop();
                        }
                    }
                }
            }

            int totalStatements = CountStatements(cleaned);

            metrics.CL = cl;
            metrics.TotalStatements = totalStatements;
            metrics.CLI = maxDepth;
            metrics.Details = details;

            return metrics;
        }

        private string PreprocessCode(string code)
        {
            code = Regex.Replace(code, @"^\s*#.*$", "", RegexOptions.Multiline);
            code = Regex.Replace(code, @"using\s+namespace\s+[^;]+;", "", RegexOptions.Singleline);
            code = Regex.Replace(code, @""".*?(?<!\\)""", "\"\"", RegexOptions.Singleline);
            code = Regex.Replace(code, @"'.*?(?<!\\)'", "''", RegexOptions.Singleline);
            code = Regex.Replace(code, @"/\*.*?\*/", "", RegexOptions.Singleline);
            code = Regex.Replace(code, @"//.*$", "", RegexOptions.Multiline);
            return code;
        }

        private int CountStatements(string code)
        {
            string noFor = Regex.Replace(code, @"for\s*\([^;]*;[^;]*;[^)]*\)", " for_special ", RegexOptions.Singleline);
            int semicolons = Regex.Matches(noFor, ";").Count;

            string[] controlKeywords = { "if", "switch", "for", "while", "do" };
            int controlCount = 0;
            foreach (string kw in controlKeywords)
            {
                controlCount += Regex.Matches(code, $@"\b{kw}\b").Count;
            }

            return semicolons + controlCount;
        }

        private void DisplayMetrics(GilbMetrics metrics)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("========== МЕТРИКИ ДЖИЛБА ==========");
            sb.AppendLine();
            sb.AppendLine($"Абсолютная сложность (CL): {metrics.CL}");
            sb.AppendLine($"Общее количество операторов: {metrics.TotalStatements}");
            sb.AppendLine($"Относительная сложность (cl): {metrics.cl:F4}");
            sb.AppendLine($"Максимальный уровень вложенности (CLI): {metrics.CLI}");
            sb.AppendLine();
            sb.AppendLine("Детализация условных операторов:");
            foreach (var kv in metrics.Details)
            {
                sb.AppendLine($"  {kv.Key}: {kv.Value}");
            }

            txtResult.Text = sb.ToString();
        }

        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}