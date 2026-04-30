using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace GilbMetricParser
{
    public partial class MainForm : Form
    {
        private TextBox txtCodeInput;
        private Button btnAnalyze;
        private RichTextBox rtbResults;
        private Label lblStatus;
        private TextBox txtFilePath;
        private Button btnLoadFile;

        public MainForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Метрика Джилба - Анализатор C++ кода";
            this.Size = new Size(1200, 700);
            this.StartPosition = FormStartPosition.CenterScreen;

            Label lblFilePath = new Label
            {
                Text = "Путь к файлу C++:",
                Location = new Point(12, 15),
                Size = new Size(100, 25)
            };

            txtFilePath = new TextBox
            {
                Location = new Point(120, 12),
                Size = new Size(800, 25),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            btnLoadFile = new Button
            {
                Text = "Загрузить файл",
                Location = new Point(930, 10),
                Size = new Size(120, 30),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnLoadFile.Click += BtnLoadFile_Click;

            btnAnalyze = new Button
            {
                Text = "Анализировать код",
                Location = new Point(1060, 10),
                Size = new Size(120, 30),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnAnalyze.Click += BtnAnalyze_Click;

            Label lblCodeInput = new Label
            {
                Text = "C++ код для анализа:",
                Location = new Point(12, 45),
                Size = new Size(150, 25)
            };

            txtCodeInput = new TextBox
            {
                Location = new Point(12, 70),
                Size = new Size(1160, 300),
                Multiline = true,
                ScrollBars = ScrollBars.Both,
                Font = new Font("Consolas", 10),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            Label lblResults = new Label
            {
                Text = "Результаты анализа:",
                Location = new Point(12, 380),
                Size = new Size(150, 25)
            };

            rtbResults = new RichTextBox
            {
                Location = new Point(12, 405),
                Size = new Size(1160, 240),
                Font = new Font("Consolas", 10),
                ReadOnly = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom
            };

            lblStatus = new Label
            {
                Text = "Готов к работе",
                Location = new Point(12, 650),
                Size = new Size(800, 25),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left
            };

            this.Controls.AddRange(new Control[] {
                lblFilePath, txtFilePath, btnLoadFile, btnAnalyze,
                lblCodeInput, txtCodeInput, lblResults, rtbResults, lblStatus
            });

            this.Resize += (s, e) => {
                txtCodeInput.Width = this.ClientSize.Width - 24;
                rtbResults.Width = this.ClientSize.Width - 24;
                rtbResults.Height = this.ClientSize.Height - 460;
                lblStatus.Top = this.ClientSize.Height - 30;
                btnAnalyze.Left = this.ClientSize.Width - 130;
                btnLoadFile.Left = this.ClientSize.Width - 260;
                txtFilePath.Width = this.ClientSize.Width - 390;
            };
        }


        private void BtnLoadFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "C++ files (*.cpp;*.cxx;*.h;*.hpp)|*.cpp;*.cxx;*.h;*.hpp|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        txtFilePath.Text = openFileDialog.FileName;
                        string code = System.IO.File.ReadAllText(openFileDialog.FileName, Encoding.UTF8);
                        txtCodeInput.Text = code;
                        lblStatus.Text = $"Файл загружен: {openFileDialog.FileName}";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка загрузки файла: {ex.Message}", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnAnalyze_Click(object sender, EventArgs e)
        {
            try
            {
                string code = txtCodeInput.Text;
                if (string.IsNullOrWhiteSpace(code))
                {
                    MessageBox.Show("Введите код C++ для анализа!", "Предупреждение",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var metrics = CalculateGilbMetrics(code);
                DisplayResults(metrics);
                lblStatus.Text = "Анализ завершен успешно";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при анализе кода: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Ошибка при анализе";
            }
        }

        private GilbMetrics CalculateGilbMetrics(string code)
        {
            // Удаляем строковые литералы и комментарии
            string cleanCode = RemoveStringsAndComments(code);

            // Подсчет всех операторов программы
            int totalStatements = CountAllStatements(cleanCode);

            // Анализ условных операторов с учетом вложенности
            var conditionalAnalysis = AnalyzeConditionalStatements(cleanCode);

            int CL = conditionalAnalysis.TotalConditionalCount;
            double cl = totalStatements > 0 ? (double)CL / totalStatements : 0;
            int CLI = conditionalAnalysis.MaxNestingDepth;

            return new GilbMetrics
            {
                CL = CL,
                RelativeComplexity = cl,
                CLI = CLI,
                TotalStatements = totalStatements,
                RawIfCount = conditionalAnalysis.IfCount,
                SwitchCount = conditionalAnalysis.SwitchCount,
                LoopCount = conditionalAnalysis.LoopCount,
                SwitchCaseCounts = conditionalAnalysis.SwitchCaseCounts
            };
        }

        private string RemoveStringsAndComments(string code)
        {
            // Удаляем строковые литералы
            string result = Regex.Replace(code, @"""(\\.|[^""\\])*""", " \"\" ");
            result = Regex.Replace(result, @"'(\\.|[^'\\])*'", " ' ' ");

            // Удаляем однострочные комментарии
            result = Regex.Replace(result, @"//.*$", " ", RegexOptions.Multiline);

            // Удаляем многострочные комментарии
            result = Regex.Replace(result, @"/\*.*?\*/", " ", RegexOptions.Singleline);

            return result;
        }

        private int CountAllStatements(string code)
        {
            int statementCount = 0;

            // Разбиваем на строки и считаем операторы
            string[] lines = code.Split('\n');

            foreach (string line in lines)
            {
                string trimmedLine = line.Trim();
                if (string.IsNullOrWhiteSpace(trimmedLine))
                    continue;


                // Пропускаем директивы препроцессора и объявления
                if (trimmedLine.StartsWith("#") ||
                    trimmedLine.StartsWith("using ") ||
                    Regex.IsMatch(trimmedLine, @"^(class|struct|enum|namespace|template)\s+\w+"))
                    continue;

                // Пропускаем строки только с фигурными скобками
                if (trimmedLine == "{" || trimmedLine == "}" || trimmedLine == "};")
                    continue;

                // Пропускаем case и default (это не операторы)
                if (Regex.IsMatch(trimmedLine, @"^\bcase\s+") || trimmedLine.StartsWith("default:"))
                    continue;

                // Считаем точки с запятой (каждый оператор)
                // Но учитываем, что в одной строке может быть несколько операторов
                int semicolonCount = 0;
                bool inFor = false;

                for (int i = 0; i < trimmedLine.Length; i++)
                {
                    // Проверяем начало for
                    if (i < trimmedLine.Length - 3 &&
                        trimmedLine.Substring(i, 3).ToLower() == "for" &&
                        (i == 0 || !char.IsLetterOrDigit(trimmedLine[i - 1])))
                    {
                        inFor = true;
                    }

                    if (trimmedLine[i] == ';')
                    {
                        if (!inFor)
                        {
                            semicolonCount++;
                        }
                    }

                    if (inFor && trimmedLine[i] == ')')
                    {
                        inFor = false;
                    }
                }

                statementCount += semicolonCount;

                // Учитываем управляющие конструкции, которые не заканчиваются точкой с запятой
                // (if, else, switch, for, while, do) - но только если они не были учтены через ;
                if (Regex.IsMatch(trimmedLine, @"\bif\s*\(") && semicolonCount == 0)
                    statementCount++;
                if (Regex.IsMatch(trimmedLine, @"\belse\b") && !Regex.IsMatch(trimmedLine, @"\bif\b") && semicolonCount == 0)
                    statementCount++;
                if (Regex.IsMatch(trimmedLine, @"\bswitch\s*\(") && semicolonCount == 0)
                    statementCount++;
                if (Regex.IsMatch(trimmedLine, @"\bfor\s*\(") && semicolonCount == 0)
                    statementCount++;
                if (Regex.IsMatch(trimmedLine, @"\bwhile\s*\(") && semicolonCount == 0)
                    statementCount++;
                if (Regex.IsMatch(trimmedLine, @"\bdo\b") && semicolonCount == 0)
                    statementCount++;
                if (Regex.IsMatch(trimmedLine, @"\breturn\b") && semicolonCount == 0)
                    statementCount++;
            }

            return Math.Max(statementCount, 1);
        }

        private ConditionalAnalysis AnalyzeConditionalStatements(string code)
        {
            var analysis = new ConditionalAnalysis();
            bool flag = false;

            int maxDepth = 0;
            int caseCount = 0;
            bool insideSwitch = false;
            int switchBraceDepth = -1; // глубина фигурных скобок, где начался текущий switch
            bool lastWasDo = false;

            Stack<string> controlStack = new Stack<string>();
            int braceDepth = 0;

            var tokens = Regex.Matches(code,
                @"\bcase\b|\bdefault\b|\bif\b|\belse\s+if\b|\belse\b|\bfor\b|\bwhile\b|\bdo\b|\bswitch\b|{|}");

            foreach (Match m in tokens)
            {
                string t = m.Value;
                if(t == "default")
                    flag = true;

                switch (t)
                {
                    case "{":
                        braceDepth++;
                        break;

                    case "}":
                        // сначала уменьшаем глубину скобок
                        braceDepth = Math.Max(0, braceDepth - 1);


                        // если внутри switch и мы вышли за границу switch — закрываем switch
                        if (insideSwitch && braceDepth < switchBraceDepth)
                        {
                            insideSwitch = false;
                            caseCount = 0;
                            switchBraceDepth = -1;
                        }

                        // если есть управляющие конструкции в стеке — закрываем последнюю
                        if (controlStack.Count > 0)
                        {
                            controlStack.Pop();
                        }
                        break;

                    case "switch":
                        // switch начинается — запомним глубину скобок, где он откроется
                        // (текущая braceDepth + 1 будет глубиной блока switch после следующего '{')
                        insideSwitch = true;
                        caseCount = 0;
                        switchBraceDepth = braceDepth + 1;
                        break;

                    case "case":
                        analysis.TotalConditionalCount++;
                        if (insideSwitch)
                        {
                            caseCount++;
                            int caseDepth = caseCount; // case1 -> 1, case2 -> 2 ...
                            int controlDepth = Math.Max(0, controlStack.Count - 1);
                            int depth = controlDepth + caseDepth;
                            maxDepth = Math.Max(maxDepth, depth);
                        }
                        break;

                    case "default":
                        analysis.TotalConditionalCount++;
                        if (insideSwitch)
                        {
                            
                            int caseDepth = caseCount; // case1 -> 1, case2 -> 2 ...
                            int controlDepth = Math.Max(0, controlStack.Count - 1);
                            int depth = controlDepth + caseDepth;
                            maxDepth = Math.Max(maxDepth, depth);
                        }
                        break;

                    case "if":
                        analysis.TotalConditionalCount++;
                        controlStack.Push("if");

                        {
                            int caseDepth = insideSwitch ? caseCount : 0;
                            int controlDepth = Math.Max(0, controlStack.Count - 1);
                            int depth = controlDepth + caseDepth;
                            maxDepth = Math.Max(maxDepth, depth);
                        }
                        break;

                    case "else if":
                        analysis.TotalConditionalCount++;
                        controlStack.Push("else-if");

                        {
                            int caseDepth = insideSwitch ? caseCount : 0;
                            int controlDepth = Math.Max(0, controlStack.Count - 1);
                            int depth = controlDepth + caseDepth;
                            maxDepth = Math.Max(maxDepth, depth);
                        }
                        break;

                    case "else":
                        controlStack.Push("else");

                        {
                            int caseDepth = insideSwitch ? caseCount : 0;
                            int controlDepth = Math.Max(0, controlStack.Count - 1);
                            int depth = controlDepth + caseDepth;
                            maxDepth = Math.Max(maxDepth, depth);
                        }
                        break;

                    case "for":
                        //analysis.TotalConditionalCount++;
                       // break;

                    case "while":
                        analysis.TotalConditionalCount++;
                        if (lastWasDo)
                        {
                            // while после do — часть do-while, не добавляем новый уровень
                            lastWasDo = false;
                        }
                        else
                        {
                            analysis.LoopCount++;
                            controlStack.Push("loop");

                            int caseDepth = insideSwitch ? caseCount : 0;
                            int controlDepth = Math.Max(0, controlStack.Count - 1);
                            int depth = controlDepth + caseDepth;
                            maxDepth = Math.Max(maxDepth, depth);
                        }
                        break;

                    case "do":
                        analysis.LoopCount++;
                        controlStack.Push("do");
                        lastWasDo = true;


                        {
                            int caseDepth = insideSwitch ? caseCount : 0;
                            int controlDepth = Math.Max(0, controlStack.Count - 1);
                            int depth = controlDepth + caseDepth;
                            maxDepth = Math.Max(maxDepth, depth);
                        }
                        break;
                }
            }
            if(flag)
            analysis.TotalConditionalCount--;

            analysis.MaxNestingDepth = maxDepth;
            return analysis;
        }











        private void DisplayResults(GilbMetrics metrics)
        {
            rtbResults.Clear();
            rtbResults.SelectionFont = new Font("Consolas", 12, FontStyle.Bold);
            rtbResults.AppendText("РЕЗУЛЬТАТЫ АНАЛИЗА МЕТРИК ДЖИЛБА\n");
            rtbResults.AppendText("================================\n\n");

            rtbResults.SelectionFont = new Font("Consolas", 11);

            rtbResults.AppendText(" Абсолютная сложность программы (CL): ");
            rtbResults.SelectionFont = new Font("Consolas", 11, FontStyle.Bold);
            rtbResults.AppendText($"{metrics.CL}\n");

            rtbResults.SelectionFont = new Font("Consolas", 11);
            rtbResults.AppendText(" Относительная сложность программы (cl): ");
            rtbResults.SelectionFont = new Font("Consolas", 11, FontStyle.Bold);
            rtbResults.AppendText($"{metrics.RelativeComplexity:F4} ({metrics.RelativeComplexity:P2})\n");

            rtbResults.SelectionFont = new Font("Consolas", 11);
            rtbResults.AppendText(" Максимальный уровень вложенности (CLI): ");
            rtbResults.SelectionFont = new Font("Consolas", 11, FontStyle.Bold);
            rtbResults.AppendText($"{metrics.CLI}\n\n");

            rtbResults.SelectionFont = new Font("Consolas", 10, FontStyle.Italic);
            rtbResults.AppendText($"Общее количество операторов: {metrics.TotalStatements}\n");        
            if (metrics.SwitchCaseCounts.Count > 0)
            {
                rtbResults.AppendText($"└─ Количество CASE в SWITCH: {string.Join(", ", metrics.SwitchCaseCounts)}\n");
                rtbResults.AppendText($"   (каждый SWITCH с n case = n-1 операторов IF-THEN-ELSE)\n");
            }
            rtbResults.AppendText("\n");

            rtbResults.SelectionFont = new Font("Consolas", 10);
        }
    }

    public class ConditionalAnalysis
    {
        public int IfCount { get; set; }
        public int SwitchCount { get; set; }
        public int LoopCount { get; set; }
        public int TotalConditionalCount { get; set; }
        public int MaxNestingDepth { get; set; }
        public List<int> SwitchCaseCounts { get; set; } = new List<int>();
    }


    public class GilbMetrics
    {
        public int CL { get; set; }
        public double RelativeComplexity { get; set; }
        public int CLI { get; set; }
        public int TotalStatements { get; set; }
        public int RawIfCount { get; set; }
        public int SwitchCount { get; set; }
        public int LoopCount { get; set; }
        public List<int> SwitchCaseCounts { get; set; } = new List<int>();
    }

    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
