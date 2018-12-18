using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GUI
{
    public partial class NoticeForm : Form
    {
        public NoticeForm(string message)
        {
            InitializeComponent();
            //this.TopMost = true;
            label1.Text = message;
            this.Height = 141 + label1.Height;
            this.Text = "";

            Button btn1 = new Button();
            btn1.Parent = panel2;
            btn1.Text = "OK";
            btn1.Location = new Point(201, 3);
            btn1.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
            btn1.Size = new Size(75, 23);
            btn1.Click += button1_Click;
        }

        public NoticeForm(string message, NoticeButtons buttons, string[] buttonsLabels)
        {
            InitializeComponent();
            //this.TopMost = true;
            label1.Text = message;
            this.Height = 141 + label1.Height;
            this.Text = "";

            Button btn1 = new Button();
            btn1.Parent = panel2;
            btn1.Text = buttonsLabels[0];
            btn1.Location = new Point(201, 3);
            btn1.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
            btn1.Size = new Size(75, 23);
            btn1.Click += button2_Click;

            if (buttons == NoticeButtons.OK_CANCEL)
            {
                Button btn2 = new Button();
                btn2.Parent = panel2;
                btn2.Text = buttonsLabels[1];
                if(btn2.Text != "Dodaj komentarz do JIRA")
                {
                    btn2.Location = new Point(80, 3);
                    btn2.Size = new Size(75, 23);
                } 
                else
                {
                    btn2.Location = new Point(65, 3);
                    btn2.Size = new Size(136, 23);
                }
                btn2.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
                btn2.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                btn2.Click += button1_Click;
            }
        }

        public NoticeForm(string message, string title)
        {
            InitializeComponent();
            //this.TopMost = true;
            label1.Text = message;
            this.Height = 141 + label1.Height;
            this.Text = title;

            Button btn1 = new Button();
            btn1.Parent = panel2;
            btn1.Text = "OK";
            btn1.Location = new Point(201, 3);
            btn1.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
            btn1.Size = new Size(75, 23);
            btn1.Click += button1_Click;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        public static DialogResult ShowNotice(string message)
        {
            var nf = new NoticeForm(message);
            return nf.ShowDialog();
        }

        public static DialogResult ShowNotice(string message, string title)
        {
            var nf = new NoticeForm(message, title);
            return nf.ShowDialog();
        }

        public static DialogResult ShowNotice(string message, NoticeButtons buttons, string[] buttonsLabels)
        {
            var nf = new NoticeForm(message, buttons, buttonsLabels);
            return nf.ShowDialog();
        }
    }

    public enum NoticeButtons
    {
        OK,
        OK_CANCEL
    }
}
