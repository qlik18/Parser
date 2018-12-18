using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections.Specialized;
using System.Collections;
using Entities;
using Utility;
using LogicLayer;
using LogicLayer.Interface;

namespace GUI
{
    public partial class AutoCompleteForm : Form
    {
        IParserEngineWFS gujacz2;

        public int idSystemu = -1;
        public int idKategorii = -1;
        public int idRodzaju = -1;
        public int idTypu = -1;
        public string idKontraktu = "";

        public AutoCompleteForm()
        {
            gujacz2 = ServiceLocator.Instance.Retrieve<IParserEngineWFS>();

            InitializeComponent();

            Dictionary<int, string> sys = gujacz2.getBillingComponents(-1);
            Dictionary<int, string> kat = gujacz2.getBillingComponents(0);
            Dictionary<int, string> rodz = gujacz2.getBillingComponents(kat.Keys.ToArray()[0]);
            Dictionary<int, string> typy = gujacz2.getBillingComponents(rodz.Keys.ToArray()[0]);

            foreach (KeyValuePair<int, string> s in sys)
            {
                tbSystem.Items.Add(new Entities.BillingDthLBItem() { Text = s.Value, Value = s.Key });
            }
            foreach (KeyValuePair<int, string> s in kat)
            {
                tbKategoria.Items.Add(new Entities.BillingDthLBItem() { Text = s.Value, Value = s.Key });
            }
            foreach (KeyValuePair<int, string> s in rodz)
            {
                tbRodzaj.Items.Add(new Entities.BillingDthLBItem() { Text = s.Value, Value = s.Key });
            }
            foreach (KeyValuePair<int, string> s in typy)
            {
                tbTyp.Items.Add(new Entities.BillingDthLBItem() { Text = s.Value, Value = s.Key });
            }

            tbSystem.SelectedIndex = 0;
            tbKategoria.SelectedIndex = 0;
            tbRodzaj.SelectedIndex = 0;
            tbTyp.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            idSystemu = ((BillingDthLBItem)tbSystem.SelectedItem).Value;
            idKategorii = ((BillingDthLBItem)tbKategoria.SelectedItem).Value;
            idRodzaju = ((BillingDthLBItem)tbRodzaj.SelectedItem).Value;
            idTypu = ((BillingDthLBItem)tbTyp.SelectedItem).Value;

            try
            {
                if (idKontraktuTB.Text.Length > 0)
                {
                    idKontraktu = Convert.ToInt32(idKontraktuTB.Text).ToString();
                    this.Visible = false;
                }
                else
                {
                    if (MessageBox.Show("Brak id kontraktu! Kontynuować?", "Błąd!", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                    {
                        idKontraktu = "";
                        this.Visible = false;
                    }
                    else
                    {
                        return;
                    }
                }
            }
            catch
            {
                MessageBox.Show("Nieprawidłowe id kontraktu!", "Błąd!", MessageBoxButtons.OK);
            }
        }

        private void tbKategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox kat = (ComboBox)sender;

            Dictionary<int, string> rodz = gujacz2.getBillingComponents(((Entities.BillingDthLBItem)kat.SelectedItem).Value);
            Dictionary<int, string> typy = gujacz2.getBillingComponents(rodz.Keys.ToArray()[0]);

            tbRodzaj.Items.Clear();
            foreach (KeyValuePair<int, string> s in rodz)
            {
                tbRodzaj.Items.Add(new Entities.BillingDthLBItem() { Text = s.Value, Value = s.Key });
            }
            tbTyp.Items.Clear();
            foreach (KeyValuePair<int, string> s in typy)
            {
                tbTyp.Items.Add(new Entities.BillingDthLBItem() { Text = s.Value, Value = s.Key });
            }

            if (tbRodzaj.Items.Count > 0)
            {
                tbRodzaj.SelectedIndex = 0;
            }
            if (tbTyp.Items.Count > 0)
            {
                tbTyp.SelectedIndex = 0;
            }
        }

        private void tbRodzaj_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox rodz = (ComboBox)sender;

            Dictionary<int, string> typy = gujacz2.getBillingComponents(((Entities.BillingDthLBItem)rodz.SelectedItem).Value);

            tbTyp.Items.Clear();
            foreach (KeyValuePair<int, string> s in typy)
            {
                tbTyp.Items.Add(new Entities.BillingDthLBItem() { Text = s.Value, Value = s.Key });
            }

            if (tbTyp.Items.Count > 0)
            {
                tbTyp.SelectedIndex = 0;
            }
        }
    }
}
