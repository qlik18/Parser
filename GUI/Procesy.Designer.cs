namespace GUI
{
    partial class Procesy
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
            this.label1 = new System.Windows.Forms.Label();
            this.tb_ProcessId = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbTmp = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbBlad = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbRozwiazanie = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.btn_Dodaj = new System.Windows.Forms.Button();
            this.cbTypProcesu = new System.Windows.Forms.ComboBox();
            this.btnSettings = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Id procesu";
            // 
            // tb_ProcessId
            // 
            this.tb_ProcessId.Location = new System.Drawing.Point(15, 35);
            this.tb_ProcessId.Name = "tb_ProcessId";
            this.tb_ProcessId.Size = new System.Drawing.Size(259, 20);
            this.tb_ProcessId.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Typ procesu";
            // 
            // cbTmp
            // 
            this.cbTmp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTmp.FormattingEnabled = true;
            this.cbTmp.Items.AddRange(new object[] {
            "Procesy"});
            this.cbTmp.Location = new System.Drawing.Point(153, 70);
            this.cbTmp.Name = "cbTmp";
            this.cbTmp.Size = new System.Drawing.Size(121, 21);
            this.cbTmp.TabIndex = 3;
            this.cbTmp.Visible = false;
            this.cbTmp.SelectedIndexChanged += new System.EventHandler(this.cbTmp_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 144);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Błąd";
            // 
            // cbBlad
            // 
            this.cbBlad.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBlad.FormattingEnabled = true;
            this.cbBlad.Items.AddRange(new object[] {
            "Query has timed out.",
            "Kod kreskowy nie może byc dluższy niz 18 znaków.",
            "Error creating bean with name \'kontenerZadan\'"});
            this.cbBlad.Location = new System.Drawing.Point(15, 160);
            this.cbBlad.Name = "cbBlad";
            this.cbBlad.Size = new System.Drawing.Size(259, 21);
            this.cbBlad.TabIndex = 5;
            this.cbBlad.SelectedIndexChanged += new System.EventHandler(this.cbBlad_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 208);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Rozwiązanie";
            // 
            // cbRozwiazanie
            // 
            this.cbRozwiazanie.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRozwiazanie.FormattingEnabled = true;
            this.cbRozwiazanie.Items.AddRange(new object[] {
            "Rerun.",
            "Rerun + NastępnyTimeout.",
            "Anulowanie procesu.",
            "Anulowanie zamówienia.",
            "Cofnięcie do wcześniejszego węzła.",
            "Skrócenie kodu kreskowego.",
            "Ustawienie NastępnegoTimeoutu."});
            this.cbRozwiazanie.Location = new System.Drawing.Point(15, 224);
            this.cbRozwiazanie.Name = "cbRozwiazanie";
            this.cbRozwiazanie.Size = new System.Drawing.Size(259, 21);
            this.cbRozwiazanie.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 259);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Dodatkowe info";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(15, 275);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(259, 148);
            this.richTextBox1.TabIndex = 9;
            this.richTextBox1.Text = "";
            // 
            // btn_Dodaj
            // 
            this.btn_Dodaj.Location = new System.Drawing.Point(15, 429);
            this.btn_Dodaj.Name = "btn_Dodaj";
            this.btn_Dodaj.Size = new System.Drawing.Size(259, 35);
            this.btn_Dodaj.TabIndex = 10;
            this.btn_Dodaj.Text = "Dodaj";
            this.btn_Dodaj.UseVisualStyleBackColor = true;
            this.btn_Dodaj.Click += new System.EventHandler(this.btn_Dodaj_Click);
            // 
            // cbTypProcesu
            // 
            this.cbTypProcesu.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTypProcesu.FormattingEnabled = true;
            this.cbTypProcesu.Location = new System.Drawing.Point(15, 97);
            this.cbTypProcesu.Name = "cbTypProcesu";
            this.cbTypProcesu.Size = new System.Drawing.Size(259, 21);
            this.cbTypProcesu.TabIndex = 11;
            this.cbTypProcesu.SelectedIndexChanged += new System.EventHandler(this.cbTypProcesu_SelectedIndexChanged);
            // 
            // btnSettings
            // 
            this.btnSettings.Location = new System.Drawing.Point(15, 470);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(259, 23);
            this.btnSettings.TabIndex = 12;
            this.btnSettings.Text = "Ustawienia";
            this.btnSettings.UseVisualStyleBackColor = true;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // Procesy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(290, 508);
            this.Controls.Add(this.btnSettings);
            this.Controls.Add(this.cbTypProcesu);
            this.Controls.Add(this.btn_Dodaj);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cbRozwiazanie);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cbBlad);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbTmp);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tb_ProcessId);
            this.Controls.Add(this.label1);
            this.Name = "Procesy";
            this.Text = "Procesy";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_ProcessId;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbTmp;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbBlad;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbRozwiazanie;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button btn_Dodaj;
        private System.Windows.Forms.ComboBox cbTypProcesu;
        private System.Windows.Forms.Button btnSettings;
    }
}