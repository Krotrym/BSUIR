namespace JilbMetricsParser
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.txtCode = new System.Windows.Forms.TextBox();
            this.btnAnalyze = new System.Windows.Forms.Button();
            this.btnOpenFile = new System.Windows.Forms.Button();
            this.lblCL = new System.Windows.Forms.Label();
            this.lblcl = new System.Windows.Forms.Label();
            this.lblCLI = new System.Windows.Forms.Label();
            this.lblNop = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtCode
            // 
            this.txtCode.Font = new System.Drawing.Font("Consolas", 10F);
            this.txtCode.Location = new System.Drawing.Point(12, 12);
            this.txtCode.Multiline = true;
            this.txtCode.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtCode.Size = new System.Drawing.Size(760, 350);
            this.txtCode.TabIndex = 0;
            this.txtCode.WordWrap = false;
            // 
            // btnAnalyze
            // 
            this.btnAnalyze.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnAnalyze.Location = new System.Drawing.Point(12, 370);
            this.btnAnalyze.Size = new System.Drawing.Size(150, 35);
            this.btnAnalyze.TabIndex = 1;
            this.btnAnalyze.Text = "Анализировать";
            this.btnAnalyze.UseVisualStyleBackColor = true;
            this.btnAnalyze.Click += new System.EventHandler(this.btnAnalyze_Click);
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnOpenFile.Location = new System.Drawing.Point(170, 370);
            this.btnOpenFile.Size = new System.Drawing.Size(150, 35);
            this.btnOpenFile.TabIndex = 2;
            this.btnOpenFile.Text = "Открыть файл";
            this.btnOpenFile.UseVisualStyleBackColor = true;
            this.btnOpenFile.Click += new System.EventHandler(this.btnOpenFile_Click);
            // 
            // lblCL
            // 
            this.lblCL.AutoSize = true;
            this.lblCL.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblCL.Location = new System.Drawing.Point(12, 420);
            this.lblCL.Size = new System.Drawing.Size(150, 19);
            this.lblCL.TabIndex = 3;
            this.lblCL.Text = "CL (абсолютная): —";
            // 
            // lblcl
            // 
            this.lblcl.AutoSize = true;
            this.lblcl.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblcl.Location = new System.Drawing.Point(12, 450);
            this.lblcl.Size = new System.Drawing.Size(150, 19);
            this.lblcl.TabIndex = 4;
            this.lblcl.Text = "cl (относительная): —";
            // 
            // lblCLI
            // 
            this.lblCLI.AutoSize = true;
            this.lblCLI.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblCLI.Location = new System.Drawing.Point(12, 480);
            this.lblCLI.Size = new System.Drawing.Size(180, 19);
            this.lblCLI.TabIndex = 5;
            this.lblCLI.Text = "CLI (вложенность): —";
            // 
            // lblNop
            // 
            this.lblNop.AutoSize = true;
            this.lblNop.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblNop.Location = new System.Drawing.Point(12, 510);
            this.lblNop.Size = new System.Drawing.Size(170, 19);
            this.lblNop.TabIndex = 6;
            this.lblNop.Text = "Nop (операторов): —";
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.lblNop);
            this.Controls.Add(this.lblCLI);
            this.Controls.Add(this.lblcl);
            this.Controls.Add(this.lblCL);
            this.Controls.Add(this.btnOpenFile);
            this.Controls.Add(this.btnAnalyze);
            this.Controls.Add(this.txtCode);
            this.Name = "MainForm";
            this.Text = "Парсер метрики Джилба";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.Button btnAnalyze;
        private System.Windows.Forms.Button btnOpenFile;
        private System.Windows.Forms.Label lblCL;
        private System.Windows.Forms.Label lblcl;
        private System.Windows.Forms.Label lblCLI;
        private System.Windows.Forms.Label lblNop;
    }
}
