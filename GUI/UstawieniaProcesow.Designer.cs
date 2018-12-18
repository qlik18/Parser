namespace GUI
{
    partial class UstawieniaProcesow
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbNewSolution = new System.Windows.Forms.RadioButton();
            this.rbNewError = new System.Windows.Forms.RadioButton();
            this.rbNewProcess = new System.Windows.Forms.RadioButton();
            this.rbBounds = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.rbBounds);
            this.groupBox1.Controls.Add(this.rbNewSolution);
            this.groupBox1.Controls.Add(this.rbNewError);
            this.groupBox1.Controls.Add(this.rbNewProcess);
            this.groupBox1.Location = new System.Drawing.Point(3, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(176, 284);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Wybierz  czynność";
            // 
            // rbNewSolution
            // 
            this.rbNewSolution.AutoSize = true;
            this.rbNewSolution.Location = new System.Drawing.Point(6, 65);
            this.rbNewSolution.Name = "rbNewSolution";
            this.rbNewSolution.Size = new System.Drawing.Size(111, 17);
            this.rbNewSolution.TabIndex = 2;
            this.rbNewSolution.TabStop = true;
            this.rbNewSolution.Text = "Nowe rozwiązanie";
            this.rbNewSolution.UseVisualStyleBackColor = true;
            this.rbNewSolution.CheckedChanged += new System.EventHandler(this.rbNewProcess_CheckedChanged);
            // 
            // rbNewError
            // 
            this.rbNewError.AutoSize = true;
            this.rbNewError.Location = new System.Drawing.Point(6, 42);
            this.rbNewError.Name = "rbNewError";
            this.rbNewError.Size = new System.Drawing.Size(77, 17);
            this.rbNewError.TabIndex = 1;
            this.rbNewError.TabStop = true;
            this.rbNewError.Text = "Nowy błąd";
            this.rbNewError.UseVisualStyleBackColor = true;
            this.rbNewError.CheckedChanged += new System.EventHandler(this.rbNewProcess_CheckedChanged);
            // 
            // rbNewProcess
            // 
            this.rbNewProcess.AutoSize = true;
            this.rbNewProcess.Location = new System.Drawing.Point(6, 19);
            this.rbNewProcess.Name = "rbNewProcess";
            this.rbNewProcess.Size = new System.Drawing.Size(87, 17);
            this.rbNewProcess.TabIndex = 0;
            this.rbNewProcess.TabStop = true;
            this.rbNewProcess.Text = "Nowy proces";
            this.rbNewProcess.UseVisualStyleBackColor = true;
            this.rbNewProcess.CheckedChanged += new System.EventHandler(this.rbNewProcess_CheckedChanged);
            // 
            // rbBounds
            // 
            this.rbBounds.AutoSize = true;
            this.rbBounds.Location = new System.Drawing.Point(6, 88);
            this.rbBounds.Name = "rbBounds";
            this.rbBounds.Size = new System.Drawing.Size(79, 17);
            this.rbBounds.TabIndex = 1;
            this.rbBounds.TabStop = true;
            this.rbBounds.Text = "Powiązania";
            this.rbBounds.UseVisualStyleBackColor = true;
            // 
            // UstawieniaProcesow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(183, 299);
            this.Controls.Add(this.groupBox1);
            this.Name = "UstawieniaProcesow";
            this.Text = "UstawieniaProcesow";
            this.Load += new System.EventHandler(this.UstawieniaProcesow_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbNewSolution;
        private System.Windows.Forms.RadioButton rbNewError;
        private System.Windows.Forms.RadioButton rbNewProcess;
        private System.Windows.Forms.RadioButton rbBounds;

    }
}