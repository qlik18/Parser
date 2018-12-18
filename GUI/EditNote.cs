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
    public partial class EditNote : Form
    {
        private bool _czyZapisac = false;
        public bool CzyZapisac
        {
            get{return _czyZapisac;}
            set { _czyZapisac = value; }
        }
        public string Notatka
        {
            get
            {
                return this.richTextBox1.Text;
            }
            set
            {
                richTextBox1.Text = value;
            }
        }
        public EditNote()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            _czyZapisac = true;
            Close();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            _czyZapisac = false;
            Close();
        }


    }
}
