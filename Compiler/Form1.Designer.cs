namespace Compiler
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.rtbIde = new System.Windows.Forms.RichTextBox();
            this.btnCompile = new System.Windows.Forms.Button();
            this.lstBxErrors = new System.Windows.Forms.ListBox();
            this.btnStop = new System.Windows.Forms.Button();
            this.rtBOutput = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnGenerateAssembly = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rtbIde
            // 
            this.rtbIde.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbIde.Location = new System.Drawing.Point(15, 12);
            this.rtbIde.Name = "rtbIde";
            this.rtbIde.Size = new System.Drawing.Size(1224, 368);
            this.rtbIde.TabIndex = 0;
            this.rtbIde.Text = "";
            // 
            // btnCompile
            // 
            this.btnCompile.Location = new System.Drawing.Point(15, 387);
            this.btnCompile.Name = "btnCompile";
            this.btnCompile.Size = new System.Drawing.Size(1064, 52);
            this.btnCompile.TabIndex = 1;
            this.btnCompile.Text = "Run";
            this.btnCompile.UseVisualStyleBackColor = true;
            this.btnCompile.Click += new System.EventHandler(this.btnCompile_Click);
            // 
            // lstBxErrors
            // 
            this.lstBxErrors.Font = new System.Drawing.Font("Consolas", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstBxErrors.FormattingEnabled = true;
            this.lstBxErrors.ItemHeight = 24;
            this.lstBxErrors.Location = new System.Drawing.Point(12, 474);
            this.lstBxErrors.Name = "lstBxErrors";
            this.lstBxErrors.Size = new System.Drawing.Size(502, 196);
            this.lstBxErrors.TabIndex = 2;
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(1085, 386);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(46, 53);
            this.btnStop.TabIndex = 3;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // rtBOutput
            // 
            this.rtBOutput.Font = new System.Drawing.Font("Myanmar Text", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtBOutput.Location = new System.Drawing.Point(520, 474);
            this.rtBOutput.Name = "rtBOutput";
            this.rtBOutput.Size = new System.Drawing.Size(719, 214);
            this.rtBOutput.TabIndex = 4;
            this.rtBOutput.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 442);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Error(s)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(517, 442);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Output";
            // 
            // btnGenerateAssembly
            // 
            this.btnGenerateAssembly.Location = new System.Drawing.Point(1137, 387);
            this.btnGenerateAssembly.Name = "btnGenerateAssembly";
            this.btnGenerateAssembly.Size = new System.Drawing.Size(102, 52);
            this.btnGenerateAssembly.TabIndex = 7;
            this.btnGenerateAssembly.Text = "Gen Assembly";
            this.btnGenerateAssembly.UseVisualStyleBackColor = true;
            this.btnGenerateAssembly.Click += new System.EventHandler(this.btnGenerateAssembly_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1251, 692);
            this.Controls.Add(this.btnGenerateAssembly);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rtBOutput);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.lstBxErrors);
            this.Controls.Add(this.btnCompile);
            this.Controls.Add(this.rtbIde);
            this.Name = "Form1";
            this.Text = "deemLang -Compiler";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtbIde;
        private System.Windows.Forms.Button btnCompile;
        private System.Windows.Forms.ListBox lstBxErrors;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.RichTextBox rtBOutput;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnGenerateAssembly;
    }
}

