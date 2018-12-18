using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Utility;

namespace GUI
{
    public partial class Error : Form
    {
        public Error()
        {
            InitializeComponent();
        }
        public Error(String name)
        {
            InitializeComponent();
            this.Name = name;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void buttonSzczegoly_Click(object sender, EventArgs e)
        {
            if (richTextBoxSzczegoly.Visible == true)
            {   //to normal
                this.Size = new Size(362, 195);
                //richTextBoxSzczegoly.Size = new Size(10, 10);                
                this.buttonOK.Location = new Point(267, 133);
                this.buttonLog.Location = new Point(12, 133);
                richTextBoxSzczegoly.Visible = false;
                this.Refresh();
            }
            else
            {   //enlarge
                this.Size = new Size(362, 400);
                richTextBoxSzczegoly.Size = new Size(329, 200);                
                this.buttonOK.Location = new Point(267, 344);
                this.buttonLog.Location = new Point(12, 344);
                richTextBoxSzczegoly.Visible = true;
                this.Refresh();
            }
        }
        public void setLabel(String s)
        {
            labelBlad.Text = s;
        }
        public String getLabel()
        {
            return labelBlad.Text;
        }

        public void setSzczegoly(String s)
        {
            richTextBoxSzczegoly.Text = s;
        }
        public String getSzczegoly()
        {
            return richTextBoxSzczegoly.Text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void buttonLog_Click(object sender, EventArgs e)
        {
            try
            {
                new Log().readLog();
            }
            catch(MyCustomException ex)
            {
                openErrorForm("Błąd w GUI ", ex, this);
            }
        }


        public void openErrorForm(String label, Exception ex, Form form)
        {
            DateTime CurrTime = DateTime.Now;
            DateTime.Now.ToString("dd/MM/yyyy - h:MM tt");

            this.setLabel("[" + CurrTime + "]\n" + ex.Source + " - " + form.Name + "\n" + label);
            this.setSzczegoly(ex.Message + ex.StackTrace);
            Log logFile = new Log();
            logFile.saveError(this.getLabel() + "\n" + this.getSzczegoly());

            this.ShowDialog();
        }



    }
}


