namespace GUI
{
    partial class Error
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

        //public label {get}

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonLog = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.labelBlad = new System.Windows.Forms.Label();
            this.buttonSzczegoly = new System.Windows.Forms.Button();
            this.richTextBoxSzczegoly = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // buttonLog
            // 
            this.buttonLog.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonLog.Location = new System.Drawing.Point(12, 133);
            this.buttonLog.Name = "buttonLog";
            this.buttonLog.Size = new System.Drawing.Size(75, 23);
            this.buttonLog.TabIndex = 0;
            this.buttonLog.Text = "Plik logu";
            this.buttonLog.UseVisualStyleBackColor = true;
            this.buttonLog.Click += new System.EventHandler(this.buttonLog_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonOK.Location = new System.Drawing.Point(267, 133);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.button2_Click);
            // 
            // labelBlad
            // 
            this.labelBlad.AutoSize = true;
            this.labelBlad.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.labelBlad.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelBlad.Location = new System.Drawing.Point(12, 9);
            this.labelBlad.Name = "labelBlad";
            this.labelBlad.Size = new System.Drawing.Size(42, 16);
            this.labelBlad.TabIndex = 3;
            this.labelBlad.Text = "Błąd:";
            this.labelBlad.Click += new System.EventHandler(this.label1_Click);
            // 
            // buttonSzczegoly
            // 
            this.buttonSzczegoly.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonSzczegoly.Location = new System.Drawing.Point(12, 68);
            this.buttonSzczegoly.Name = "buttonSzczegoly";
            this.buttonSzczegoly.Size = new System.Drawing.Size(75, 23);
            this.buttonSzczegoly.TabIndex = 4;
            this.buttonSzczegoly.Text = "Szczegóły";
            this.buttonSzczegoly.UseVisualStyleBackColor = true;
            this.buttonSzczegoly.Click += new System.EventHandler(this.buttonSzczegoly_Click);
            // 
            // richTextBoxSzczegoly
            // 
            this.richTextBoxSzczegoly.BackColor = System.Drawing.Color.Ivory;
            this.richTextBoxSzczegoly.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.richTextBoxSzczegoly.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.richTextBoxSzczegoly.Location = new System.Drawing.Point(12, 97);
            this.richTextBoxSzczegoly.Name = "richTextBoxSzczegoly";
            this.richTextBoxSzczegoly.ReadOnly = true;
            this.richTextBoxSzczegoly.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.richTextBoxSzczegoly.Size = new System.Drawing.Size(329, 28);
            this.richTextBoxSzczegoly.TabIndex = 5;
            this.richTextBoxSzczegoly.Text = "";
            this.richTextBoxSzczegoly.Visible = false;
            // 
            // Error
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(354, 168);
            this.Controls.Add(this.richTextBoxSzczegoly);
            this.Controls.Add(this.buttonSzczegoly);
            this.Controls.Add(this.labelBlad);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonLog);
            this.Location = new System.Drawing.Point(100, 500);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(362, 400);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(362, 195);
            this.Name = "Error";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Error";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonLog;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Label labelBlad;
        private System.Windows.Forms.Button buttonSzczegoly;
        private System.Windows.Forms.RichTextBox richTextBoxSzczegoly;
    }
}