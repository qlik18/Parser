using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utility;
using Logging;
using System.Windows.Forms;
using System.ComponentModel;
using Entities;
using System.IO;

namespace GUI
{
    public partial class MainForm
    {
        private void doSearchPolsatUsers(object sender, DoWorkEventArgs e)
        {
            try
            {
                this.pb_SetMaxProgressBar(100);
                this.pb_SetVisibilityPanel(true);
                this.pb_UpdateProgressBar("Pobieranie użytkowników z bazy danych");
               // PolsatUsers = usersHelios.GetAllUsers();
                this.pb_UpdateProgressBar("Pobieranie użytkowników z bazy danych");
                foreach (Entities.HeliosUser item in PolsatUsers)
                {
                    ListViewItem it = new ListViewItem(item.email);
                    it.SubItems.Add(item.imie);
                    it.SubItems.Add(item.nazwisko);
                    it.SubItems.Add(item.telefon);
                    lv_Users.Invoke((MethodInvoker)delegate
                    {
                        lv_Users.Items.Add(it);
                    });
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.LogError(ex, Logger.Instance, true);
            }
        }
        
        private void doSearchPolsatUsersCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.pb_SetVisibilityPanel(false);
        }

        private void doGetComponents(object sender, DoWorkEventArgs e)
        {
            try
            {
                Implementation.GeneralStatic.listaKomp = new List<Komponent>();//gujaczWFS.getComponents();
            }
            catch (Exception ex)
            {
                ExceptionManager.LogError(ex, Logger.Instance, true);
            }

        }
        /*
         * Po pobraniu komponentow z bazy do Comboboxów   
         */
        private void doGetComponentsCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            isReadedComponents = true;
        }

        /*
         * Po pobraniu 
         */
        private void doGetMyissuesHeliosCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            /*listaZgloszen.Items.Clear();

            //nie wiadomo czy zadziala takie rzutowanie
            listaZgloszenHelios = (List<IssueHelios>)e.Result;

            foreach (IssueHelios itemlist in listaZgloszenHelios)
            {
                listaZgloszen.CheckOnClick = true;
                //zmienić
                this.listaZgloszen.Items.Add(itemlist.number.ToString() + " | " + itemlist.title, itemlist.isInWFS);
                listaZgloszen.CheckOnClick = false;
            }
            this.toolStripStatusLabel5.Text = listaZgloszenHelios.Count.ToString();*/
            //log("Pobrano zgłoszenia");
            this.setBusy(false);

        }

    }
}
