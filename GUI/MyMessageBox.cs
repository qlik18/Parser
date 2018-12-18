using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Entities;

namespace GUI
{
    public partial class MyMessageBox : Form
    {
        public MyMessageBox()
        {
            InitializeComponent();
        }

        public MyMessageBox(List<IssueMessage> lista)
        {
            InitializeComponent();
            StringBuilder newIssue = new StringBuilder();
            newIssue.AppendLine("Pojawiły się nowe zgłoszenia<br>");
            StringBuilder warningIssue = new StringBuilder();

            foreach (IssueMessage item in lista)
            {
                if(item.isNew)
                    newIssue.AppendLine("nr: "+item.issuenumber+"<br>");
                if (item.isWarning)
                {
                    TimeSpan span = DateTime.Now.Subtract(item.date);
                    warningIssue.AppendLine("nr: " + item.issuenumber + " Czeka już: " +
                        item.date.Subtract(DateTime.Now).Minutes.ToString() + " minut<br>");
                }
            }
            if (warningIssue.Length != 0)
            {
                warningIssue.Insert(0, "<p>Zgłoszenie długo nie odbierane:</p>");
                warningIssue.Insert(0, "<div style='color: red'>");
                
                warningIssue.AppendLine("</div>");
            }
            this.webBrowser1.Navigate("about:blank");
            HtmlDocument doc = this.webBrowser1.Document;
            doc.Write(string.Empty);
            this.webBrowser1.DocumentText = newIssue.AppendLine(warningIssue.ToString()).ToString();
        }

        public MyMessageBox(string message)
        {
            InitializeComponent();
            this.webBrowser1.Navigate("about:blank");
            HtmlDocument doc = this.webBrowser1.Document;
            doc.Write(string.Empty);
            this.webBrowser1.DocumentText = message;
            this.TopMost = true;
        }

        public MyMessageBox(string message, string title)
        {
            InitializeComponent();
            this.webBrowser1.Navigate("about:blank");
            HtmlDocument doc = this.webBrowser1.Document;
            doc.Write(string.Empty);
            this.webBrowser1.DocumentText = message;
            this.Text = title;
            this.TopMost = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }

}
