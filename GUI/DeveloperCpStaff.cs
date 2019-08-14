using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    public partial class MainForm
    {
        private void rb_devCp_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            if (rb.Name == rb_devCp_add.Name && rb.Checked)
            {
                bt_devCP.Text = "Dodaj Dewelopera";
                return;
            }

            if (rb.Name == rb_devCp_del.Name && rb.Checked)
            {
                bt_devCP.Text = "Dezaktywuj Dewelopera";
                return;
            }
        }

        private void bt_devCP_Click(object sender, EventArgs e)
        {
            if (isNullObjectOrEmptyString(tb_devCp_name.Text) || isNullObjectOrEmptyString(tb_devCp_surname.Text))
            {
                MessageBox.Show("Imię i nazwisko musi być uzupełnione!!!", "UWAGA GAMONIU!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!rb_devCp_add.Checked && !rb_devCp_del.Checked)
            {
                MessageBox.Show("Wybierz operację!!!", "UWAGA GAMONIU!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //else if()
            else if (rb_devCp_add.Checked)
            {

                var v = gujaczWFS.ExecuteStoredProcedure("[CP_dodaj_dewelopera]", new string[]
                                                                                { tb_devCp_name.Text.ToString().Trim(), tb_devCp_surname.Text.ToString().Trim() }
                                                                                , DatabaseName.SupportCP);
                MessageBox.Show(v[0][0], "UWAGA GAMONIU!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else if (rb_devCp_del.Checked)
            {

                var v = gujaczWFS.ExecuteStoredProcedure("[CP_dezaktywuj_dewelopera]", new string[]
                                                                                { tb_devCp_name.Text.ToString(), tb_devCp_surname.Text.ToString() }
                                                                               , DatabaseName.SupportCP);
                MessageBox.Show(v[0][0], "UWAGA GAMONIU!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            bt_devCP.Text = "Wybierz operację";
            rb_devCp_add.Checked = false;
            rb_devCp_del.Checked = false;
            tb_devCp_name.Text = string.Empty;
            tb_devCp_surname.Text = string.Empty;
        }
    }
}
