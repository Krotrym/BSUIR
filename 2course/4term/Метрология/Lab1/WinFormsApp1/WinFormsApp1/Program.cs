    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;
    using System.Drawing;

    namespace HalsteadMetricsParser
    {
        public partial class MainForm : Form
        {
            private TextBox txtCodeInput;
            private Button btnAnalyze;
            private DataGridView dgvResults;
            private GroupBox grpMetrics;
            private GroupBox grpExtendedMetrics;
            private Label lblProgramLength, lblProgramVocabulary, lblProgramVolume;
            private Label lblN1, lblN2, lblEta1, lblEta2;

            private readonly HashSet<string> keywords = new HashSet<string>
            {
                "int", "double", "float", "char", "bool", "void", "long", "short",
                "const", "static", "virtual", "explicit", "inline", "volatile",
                "public", "private", "protected", "class", "struct", "enum", "union",
                "namespace", "using", "typedef", "template", "typename",
                "include", "define", "ifdef", "ifndef", "endif", "defined",
                "auto", "register", "extern", "mutable", "friend", "operator",
                "this", "override", "final", "default", "delete",
                "true", "false", "nullptr", "NULL", "RAND_MAX", "M_PI"
            };

            private readonly List<KeyValuePair<string, string>> operatorPatterns = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(@"\+\+", "++"),
                new KeyValuePair<string, string>("--", "--"),
                new KeyValuePair<string, string>("==", "=="),
                new KeyValuePair<string, string>("!=", "!="),
                new KeyValuePair<string, string>("<=", "<="),
                new KeyValuePair<string, string>(">=", ">="),
                new KeyValuePair<string, string>("&&", "&&"),
                new KeyValuePair<string, string>(@"\|\|", "||"),
                new KeyValuePair<string, string>("<<=", "<<="),
                new KeyValuePair<string, string>(">>=", ">>="),
                new KeyValuePair<string, string>("+=", "+="),
                new KeyValuePair<string, string>("-=", "-="),
                new KeyValuePair<string, string>("*=", "*="),
                new KeyValuePair<string, string>("/=", "/="),
                new KeyValuePair<string, string>("%=", "%="),
                new KeyValuePair<string, string>("&=", "&="),
                new KeyValuePair<string, string>(@"\|=", "|="),
                new KeyValuePair<string, string>("^=", "^="),
                new KeyValuePair<string, string>("<<", "<<"),
                new KeyValuePair<string, string>(">>", ">>"),
                new KeyValuePair<string, string>("::", "::"),
                new KeyValuePair<string, string>(@"\+", "+"),
                new KeyValuePair<string, string>("-", "-"),
                new KeyValuePair<string, string>(@"\*", "*"),
                new KeyValuePair<string, string>("/", "/"),
                new KeyValuePair<string, string>("%", "%"),
                new KeyValuePair<string, string>("=", "="),
                new KeyValuePair<string, string>("<", "<"),
                new KeyValuePair<string, string>(">", ">"),
                new KeyValuePair<string, string>("&", "&"),
                new KeyValuePair<string, string>(@"\|", "|"),
                new KeyValuePair<string, string>(@"\^", "^"),
                new KeyValuePair<string, string>("~", "~"),
                new KeyValuePair<string, string>("!", "!"),
                new KeyValuePair<string, string>(";", ";"),
                new KeyValuePair<string, string>(",", ","),
                new KeyValuePair<string, string>(@"\?", "?"),
                new KeyValuePair<string, string>(":", ":")
            };

            private readonly Dictionary<string, string> controlOperators = new Dictionary<string, string>
            {
                {@"\bif\b", "if"},
                {@"\belse\b", "else"},
                {@"\bfor\b", "for"},
                {@"\bwhile\b", "while"},
                {@"\bdo\b", "do"},
                {@"\bswitch\b", "switch"},
                {@"\bdefault\b", "default"},
                {@"\bbreak\b", "break"},
                {@"\bcontinue\b", "continue"},
                {@"\breturn\b", "return"},
                {@"\bgoto\b", "goto"},
                {@"\bnew\b", "new"},
                {@"\bdelete\b", "delete"},
                {@"\bsizeof\b", "sizeof"},
                {@"\bthrow\b", "throw"},
                {@"\btry\b", "try"},
                {@"\bcatch\b", "catch"}
            };

            public MainForm()
            {
                InitializeComponent();
            }

            private void InitializeComponent()
            {
                this.Text = "Анализатор метрик Холстеда для C++";
                this.Size = new Size(1200, 900);
                this.StartPosition = FormStartPosition.CenterScreen;

                GroupBox grpInput = new GroupBox
                {
                    Text = "Исходный код программы на C++",
                    Location = new Point(10, 10),
                    Size = new Size(1160, 250)
                };

                txtCodeInput = new TextBox
                {
                    Location = new Point(10, 20),
                    Size = new Size(1140, 190),
                    Multiline = true,
                    ScrollBars = ScrollBars.Both,
                    Font = new Font("Consolas", 10),
                    Text = GetDefaultCode()
                };

                btnAnalyze = new Button
                {
                    Text = "Выполнить анализ",
                    Location = new Point(10, 215),
                    Size = new Size(150, 25),
                    BackColor = Color.LightBlue
                };
                btnAnalyze.Click += BtnAnalyze_Click;

                grpInput.Controls.Add(txtCodeInput);
                grpInput.Controls.Add(btnAnalyze);

                grpMetrics = new GroupBox
                {
                    Text = "Базовые метрики Холстеда",
                    Location = new Point(10, 270),
                    Size = new Size(800, 550)
                };

                dgvResults = new DataGridView
                {
                    Location = new Point(10, 20),
                    Size = new Size(780, 520),
                    AllowUserToAddRows = false,
                    AllowUserToDeleteRows = false,
                    ReadOnly = true,
                    RowHeadersVisible = false,
                    AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
                };

                dgvResults.Columns.Add("j", "j");
                dgvResults.Columns.Add("Operator", "Оператор");
                dgvResults.Columns.Add("f1j", "f₁ⱼ");
                dgvResults.Columns.Add("i", "i");
                dgvResults.Columns.Add("Operand", "Операнд");
                dgvResults.Columns.Add("f2i", "f₂ᵢ");

                dgvResults.Columns[0].Width = 40;
                dgvResults.Columns[1].Width = 200;
                dgvResults.Columns[2].Width = 50;
                dgvResults.Columns[3].Width = 40;
                dgvResults.Columns[4].Width = 200;
                dgvResults.Columns[5].Width = 50;

                grpMetrics.Controls.Add(dgvResults);

                grpExtendedMetrics = new GroupBox
                {
                    Text = "Расширенные метрики Холстеда",
                    Location = new Point(820, 270),
                    Size = new Size(350, 550)
                };

                lblEta1 = new Label { Location = new Point(10, 30), Size = new Size(330, 25), Font = new Font("Arial", 10), Text = "η₁ (уникальных операторов):" };
                lblEta2 = new Label { Location = new Point(10, 60), Size = new Size(330, 25), Font = new Font("Arial", 10), Text = "η₂ (уникальных операндов):" };
                lblN1 = new Label { Location = new Point(10, 90), Size = new Size(330, 25), Font = new Font("Arial", 10), Text = "N₁ (всего операторов):" };
                lblN2 = new Label { Location = new Point(10, 120), Size = new Size(330, 25), Font = new Font("Arial", 10), Text = "N₂ (всего операндов):" };

                Label line1 = new Label { Location = new Point(10, 150), Size = new Size(330, 2), BackColor = Color.Gray };

                lblProgramVocabulary = new Label
                {
                    Location = new Point(10, 160),
                    Size = new Size(330, 30),
                    Font = new Font("Arial", 11, FontStyle.Bold),
                    ForeColor = Color.Blue,
                    Text = "Словарь программы (η = η₁ + η₂):"
                };

                lblProgramLength = new Label
                {
                    Location = new Point(10, 200),
                    Size = new Size(330, 30),
                    Font = new Font("Arial", 11, FontStyle.Bold),
                    ForeColor = Color.Blue,
                    Text = "Длина программы (N = N₁ + N₂):"
                };

                lblProgramVolume = new Label
                {
                    Location = new Point(10, 240),
                    Size = new Size(330, 60),
                    Font = new Font("Arial", 11, FontStyle.Bold),
                    ForeColor = Color.Blue,
                    Text = "Объем программы (V = N · log₂η):"
                };

                Label formulas = new Label
                {
                    Location = new Point(10, 320),
                    Size = new Size(330, 200),
                    Font = new Font("Arial", 9),
                    Text = ""
                };

                grpExtendedMetrics.Controls.AddRange(new Control[] {
                    lblEta1, lblEta2, lblN1, lblN2, line1,
                    lblProgramVocabulary, lblProgramLength, lblProgramVolume,
                    formulas
                });

                this.Controls.Add(grpInput);
                this.Controls.Add(grpMetrics);
                this.Controls.Add(grpExtendedMetrics);
            }

            private string GetDefaultCode()
            {
                return @"// Программа для вычисления числа Пи
    #include <iostream>
    #include <cmath>
    #include <cstdlib>
    #include <ctime>
    #include <iomanip>
    using namespace std;

    const int ITER = 1000000;

    double monteCarloPi() {
        int inside = 0;
        for (int i = 0; i < ITER; ++i) {
            double x = (double)rand() / RAND_MAX;
            double y = (double)rand() / RAND_MAX;
            if (x * x + y * y <= 1.0) inside++;
        }
        return 4.0 * inside / ITER;
    }

    double leibnizPi() {
        double sum = 0.0;
        for (int n = 0; n < ITER; ++n) {
            sum += (n % 2 ? -1.0 : 1.0) / (2 * n + 1);
        }
        return 4.0 * sum;
    }

    double wallisPi() {
        double prod = 1.0;
        for (int n = 1; n <= ITER; ++n) {
            prod *= (2.0 * n) / (2.0 * n - 1) * (2.0 * n) / (2.0 * n + 1);
        }
        return 2.0 * prod;
    }

    double eulerPi() {
        double sum = 0.0;
        for (int n = 1; n <= ITER; ++n) {
            sum += 1.0 / (n * n);
        }
        return sqrt(6.0 * sum);
    }

    double machinPi() {
        return 16.0 * atan(1.0 / 5.0) - 4.0 * atan(1.0 / 239.0);
    }

    double archimedesPi() {
        double a = 1.0, b = 1.0 / sqrt(2.0), t = 0.25, p = 1.0;
        for (int i = 0; i < 5; ++i) {
            double an = (a + b) / 2.0;
            double bn = sqrt(a * b);
            double tn = t - p * (a - an) * (a - an);
            double pn = 2.0 * p;
            a = an; b = bn; t = tn; p = pn;
        }
        return (a + b) * (a + b) / (4.0 * t);
    }

    void printResult(const char* name, double pi) {
        double err = fabs(pi - M_PI);
        cout << left << setw(12) << name << "" ""
             << fixed << setprecision(8) << pi
             << "" err: "" << scientific << setprecision(2) << err << endl;
    }

    int main() {
        srand(time(NULL));
        int cmd;
        do {
            cout << ""\n1. Монте-Карло\n2. Лейбниц\n3. Валлис\n4. Эйлер\n"";
            cout << ""5. Мэчин\n6. Архимед\n7. Все методы\n0. Выход\n"";
            cin >> cmd;
            switch (cmd) {
                case 1: printResult(""Монте-Карло"", monteCarloPi()); break;
                case 2: printResult(""Лейбниц"", leibnizPi()); break;
                case 3: printResult(""Валлис"", wallisPi()); break;
                case 4: printResult(""Эйлер"", eulerPi()); break;
                case 5: printResult(""Мэчин"", machinPi()); break;
                case 6: printResult(""Архимед"", archimedesPi()); break;
                case 7:
                    printResult(""Монте-Карло"", monteCarloPi());
                    printResult(""Лейбниц"", leibnizPi());
                    printResult(""Валлис"", wallisPi());
                    printResult(""Эйлер"", eulerPi());
                    printResult(""Мэчин"", machinPi());
                    printResult(""Архимед"", archimedesPi());
                    break;
            }
        } while (cmd != 0);
        return 0;
    }";
            }

            private void BtnAnalyze_Click(object sender, EventArgs e)
            {
                try
                {
                    string code = txtCodeInput.Text;
                    HalsteadMetrics metrics = AnalyzeCode(code);
                    DisplayResults(metrics);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            private HalsteadMetrics AnalyzeCode(string code)
            {
                var metrics = new HalsteadMetrics();

                string codeNoPreproc = Regex.Replace(code, @"^\s*#.*$", "", RegexOptions.Multiline);
                string codeNoComments = RemoveComments(codeNoPreproc);
                string codeNoStrings = RemoveStringsAndChars(codeNoComments);

                HashSet<string> functionNames = new HashSet<string>();
                var funcMatches = Regex.Matches(codeNoStrings, @"\b([a-zA-Z_][a-zA-Z0-9_]*)\s*\(");
                foreach (Match m in funcMatches)
                {
                    string name = m.Groups[1].Value;
                    if (!keywords.Contains(name) && !controlOperators.ContainsValue(name) &&
                        name != "if" && name != "for" && name != "while" && name != "switch" && name != "return")
                    {
                        functionNames.Add(name);
                    }
                }

                var operatorCounts = new Dictionary<string, int>();

                foreach (string func in functionNames)
                {
                    var matches = Regex.Matches(codeNoStrings, @"\b" + func + @"\s*\(");
                    if (matches.Count > 0)
                        operatorCounts[func + "()"] = matches.Count;
                }

                foreach (var op in controlOperators)
                {
                    var matches = Regex.Matches(codeNoStrings, op.Key);
                    if (matches.Count > 0)
                        operatorCounts[op.Value] = matches.Count;
                }

                string codeWithoutCalls = codeNoStrings;
                foreach (string func in functionNames)
                {
                    codeWithoutCalls = Regex.Replace(codeWithoutCalls, @"\b" + func + @"\s*\([^)]*\)", " FUNC_CALL ");
                }
                var parenPairs = Regex.Matches(codeWithoutCalls, @"\([^)]*\)");
                if (parenPairs.Count > 0)
                    operatorCounts["( )"] = parenPairs.Count;

                int braceCount = Regex.Matches(codeNoStrings, @"\{").Count;
                if (braceCount > 0)
                    operatorCounts["{ }"] = braceCount;

                var symbolCounts = new Dictionary<string, int>();
                string processedCode = codeNoStrings;
                int markerCounter = 0;

                var filteredPatterns = operatorPatterns.Where(p => p.Value != ".").ToList();

                foreach (var pattern in filteredPatterns)
                {
                    string regex = pattern.Key;
                    string opName = pattern.Value;
                    try
                    {
                        var matches = Regex.Matches(processedCode, regex);
                        if (matches.Count > 0)
                        {
                            symbolCounts[opName] = matches.Count;
                            foreach (Match m in matches.Cast<Match>().OrderByDescending(x => x.Index))
                            {
                                string marker = $"__OP{markerCounter++}__";
                                processedCode = processedCode.Remove(m.Index, m.Length).Insert(m.Index, marker);
                            }
                        }
                    }
                    catch { }
                }

                foreach (var kv in symbolCounts)
                {
                    if (operatorCounts.ContainsKey(kv.Key))
                        operatorCounts[kv.Key] += kv.Value;
                    else
                        operatorCounts[kv.Key] = kv.Value;
                }

                var operandCounts = new Dictionary<string, int>();

                var identifiers = Regex.Matches(codeNoStrings, @"\b[a-zA-Z_][a-zA-Z0-9_]*\b");
                foreach (Match m in identifiers)
                {
                    string id = m.Value;

                    if (id == "case") continue;

                    bool isKeyword = keywords.Contains(id);
                    bool isFunction = functionNames.Contains(id);
                    bool isControl = controlOperators.ContainsValue(id);
                    bool isSymbolOp = filteredPatterns.Any(p => p.Value == id) || id == "(" || id == ")" || id == "{" || id == "}";

                    if (!isKeyword && !isFunction && !isControl && !isSymbolOp)
                    {
                        if (operandCounts.ContainsKey(id))
                            operandCounts[id]++;
                        else
                            operandCounts[id] = 1;
                    }
                }

                var numbers = Regex.Matches(codeNoStrings, @"\b\d+\.?\d*\b");
                foreach (Match m in numbers)
                {
                    string num = m.Value;
                    if (operandCounts.ContainsKey(num))
                        operandCounts[num]++;
                    else
                        operandCounts[num] = 1;
                }

                var stringMatches = Regex.Matches(codeNoComments, @"""[^""]*""");
                foreach (Match m in stringMatches)
                {
                    string str = m.Value;
                    if (operandCounts.ContainsKey(str))
                        operandCounts[str]++;
                    else
                        operandCounts[str] = 1;
                }

                var charMatches = Regex.Matches(codeNoStrings, @"'[^']'");
                foreach (Match m in charMatches)
                {
                    string ch = m.Value;
                    if (operandCounts.ContainsKey(ch))
                        operandCounts[ch]++;
                    else
                        operandCounts[ch] = 1;
                }

                foreach (string constName in new[] { "true", "false", "NULL", "nullptr", "RAND_MAX", "M_PI" })
                {
                    var constMatches = Regex.Matches(codeNoComments, @"\b" + constName + @"\b");
                    if (constMatches.Count > 0)
                    {
                        if (operandCounts.ContainsKey(constName))
                            operandCounts[constName] += constMatches.Count;
                        else
                            operandCounts[constName] = constMatches.Count;
                    }
                }

                metrics.Operators = operatorCounts;
                metrics.Operands = operandCounts;
                metrics.UniqueOperators = operatorCounts.Count;
                metrics.UniqueOperands = operandCounts.Count;
                metrics.TotalOperators = operatorCounts.Sum(x => x.Value);
                metrics.TotalOperands = operandCounts.Sum(x => x.Value);

                metrics.ProgramLength = metrics.TotalOperators + metrics.TotalOperands;
                metrics.ProgramVocabulary = metrics.UniqueOperators + metrics.UniqueOperands;
                metrics.ProgramVolume = metrics.ProgramLength * Math.Log2(metrics.ProgramVocabulary);

                return metrics;
            }

            private string RemoveComments(string code)
            {
                code = Regex.Replace(code, @"/\*.*?\*/", "", RegexOptions.Singleline);
                code = Regex.Replace(code, @"//.*?$", "", RegexOptions.Multiline);
                return code;
            }

            private string RemoveStringsAndChars(string code)
            {
                code = Regex.Replace(code, @"""[^""]*""", "\"\"", RegexOptions.Singleline);
                code = Regex.Replace(code, @"'[^']'", "''", RegexOptions.Singleline);
                return code;
            }

            private void DisplayResults(HalsteadMetrics metrics)
            {
                dgvResults.Rows.Clear();

                var operators = metrics.Operators.OrderBy(x => x.Key).ToList();
                var operands = metrics.Operands.OrderBy(x => x.Key).ToList();

                int maxRows = Math.Max(operators.Count, operands.Count);

                for (int i = 0; i < maxRows; i++)
                {
                    string j = i < operators.Count ? (i + 1).ToString() : "";
                    string op = i < operators.Count ? operators[i].Key : "";
                    string f1j = i < operators.Count ? operators[i].Value.ToString() : "";

                    string idx = i < operands.Count ? (i + 1).ToString() : "";
                    string operand = i < operands.Count ? operands[i].Key : "";
                    string f2i = i < operands.Count ? operands[i].Value.ToString() : "";

                    dgvResults.Rows.Add(j, op, f1j, idx, operand, f2i);
                }

                dgvResults.Rows.Add("", "", "", "", "", "");
                dgvResults.Rows.Add($"η₁ = {metrics.UniqueOperators}", "", $"N₁ = {metrics.TotalOperators}",
                                    $"η₂ = {metrics.UniqueOperands}", "", $"N₂ = {metrics.TotalOperands}");

                int lastRow = dgvResults.Rows.Count - 1;
                dgvResults.Rows[lastRow].DefaultCellStyle.BackColor = Color.LightYellow;
                dgvResults.Rows[lastRow].DefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);

                lblEta1.Text = $"η₁ (уникальных операторов): {metrics.UniqueOperators}";
                lblEta2.Text = $"η₂ (уникальных операндов): {metrics.UniqueOperands}";
                lblN1.Text = $"N₁ (всего операторов): {metrics.TotalOperators}";
                lblN2.Text = $"N₂ (всего операндов): {metrics.TotalOperands}";

                lblProgramVocabulary.Text = $"Словарь программы (η = η₁ + η₂): {metrics.ProgramVocabulary}";
                lblProgramLength.Text = $"Длина программы (N = N₁ + N₂): {metrics.ProgramLength}";
                lblProgramVolume.Text = $"Объем программы (V = N · log₂η): {metrics.ProgramVolume:F2}";
            }

            private class HalsteadMetrics
            {
                public Dictionary<string, int> Operators { get; set; } = new Dictionary<string, int>();
                public Dictionary<string, int> Operands { get; set; } = new Dictionary<string, int>();
                public int UniqueOperators { get; set; }
                public int UniqueOperands { get; set; }
                public int TotalOperators { get; set; }
                public int TotalOperands { get; set; }
                public int ProgramLength { get; set; }
                public int ProgramVocabulary { get; set; }
                public double ProgramVolume { get; set; }
            }

            [STAThread]
            static void Main()
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
        }
    }