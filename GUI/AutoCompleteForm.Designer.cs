namespace GUI
{
    partial class AutoCompleteForm
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
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.idKontraktuTB = new System.Windows.Forms.TextBox();
            this.tbKategoria = new System.Windows.Forms.ComboBox();
            this.tbSystem = new System.Windows.Forms.ComboBox();
            this.tbTyp = new System.Windows.Forms.ComboBox();
            this.tbRodzaj = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(16, 252);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(243, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Uzupełnij zgłoszenia";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Id kontraktu:";
            // 
            // idKontraktuTB
            // 
            this.idKontraktuTB.Location = new System.Drawing.Point(86, 10);
            this.idKontraktuTB.Name = "idKontraktuTB";
            this.idKontraktuTB.Size = new System.Drawing.Size(173, 20);
            this.idKontraktuTB.TabIndex = 2;
            // 
            // tbKategoria
            // 
            this.tbKategoria.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tbKategoria.FormattingEnabled = true;
            this.tbKategoria.Location = new System.Drawing.Point(16, 110);
            this.tbKategoria.Name = "tbKategoria";
            this.tbKategoria.Size = new System.Drawing.Size(243, 21);
            this.tbKategoria.TabIndex = 44;
            this.tbKategoria.SelectedIndexChanged += new System.EventHandler(this.tbKategoria_SelectedIndexChanged);
            // 
            // tbSystem
            // 
            this.tbSystem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tbSystem.FormattingEnabled = true;
            this.tbSystem.Location = new System.Drawing.Point(16, 60);
            this.tbSystem.Name = "tbSystem";
            this.tbSystem.Size = new System.Drawing.Size(243, 21);
            this.tbSystem.TabIndex = 43;
            // 
            // tbTyp
            // 
            this.tbTyp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tbTyp.FormattingEnabled = true;
            this.tbTyp.Location = new System.Drawing.Point(16, 215);
            this.tbTyp.Name = "tbTyp";
            this.tbTyp.Size = new System.Drawing.Size(243, 21);
            this.tbTyp.TabIndex = 41;
            // 
            // tbRodzaj
            // 
            this.tbRodzaj.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tbRodzaj.DropDownWidth = 180;
            this.tbRodzaj.FormattingEnabled = true;
            this.tbRodzaj.Location = new System.Drawing.Point(16, 164);
            this.tbRodzaj.Name = "tbRodzaj";
            this.tbRodzaj.Size = new System.Drawing.Size(243, 21);
            this.tbRodzaj.TabIndex = 42;
            this.tbRodzaj.SelectedIndexChanged += new System.EventHandler(this.tbRodzaj_SelectedIndexChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(13, 199);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(25, 13);
            this.label12.TabIndex = 37;
            this.label12.Text = "Typ";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(13, 148);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(40, 13);
            this.label11.TabIndex = 38;
            this.label11.Text = "Rodzaj";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(13, 94);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(52, 13);
            this.label10.TabIndex = 39;
            this.label10.Text = "Kategoria";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(13, 43);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(41, 13);
            this.label15.TabIndex = 40;
            this.label15.Text = "System";
            // 
            // AutoCompleteForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(271, 286);
            this.Controls.Add(this.tbKategoria);
            this.Controls.Add(this.tbSystem);
            this.Controls.Add(this.tbTyp);
            this.Controls.Add(this.tbRodzaj);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.idKontraktuTB);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Name = "AutoCompleteForm";
            this.Text = "AutoCompleteForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox idKontraktuTB;
        private System.Windows.Forms.ComboBox tbKategoria;
        private System.Windows.Forms.ComboBox tbSystem;
        private System.Windows.Forms.ComboBox tbTyp;
        private System.Windows.Forms.ComboBox tbRodzaj;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label15;
    }
}