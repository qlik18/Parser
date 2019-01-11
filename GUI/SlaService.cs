using Atlassian.Jira;
using Entities;
using Entities.Enums;
using Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utility;

namespace GUI
{
    public partial class MainForm
    {
        private void slaReportLoad(object sender, DoWorkEventArgs e)
        {
            try
            {
                bool doSynchro = tryLogginToJira(jiraUser.Login, jiraUser.Password);

                pb_SetVisibilityPanel(true);
                toolStripStatusLabel6.Text = "Trwa odświeżanie raportu SLA...";

                List<List<string>> lista = gujaczWFS.ExecuteStoredProcedure("cp_sla_raport_v2", new string[] { }, DatabaseName.SupportCP);

                this.Invoke((MethodInvoker)delegate
                {
                    if (lista.Count > 0)
                    {
                        Issue tmpIssue2 = null;
                        tb_sla_ileWstrzymane.Text = lista.Count(x => x[5] == "1").ToString();
                        tb_sla_ileRealizacja.Text = lista.Count(x => x[5] == "0").ToString();

                        lista.RemoveAll(x => x[8] == string.Empty);
                        if (cb_SLApauza.Checked)
                        {
                            lista.RemoveAll(x => x[5] == "1");
                        }
                        if (cb_SLAWstrzymane.Checked)
                        {
                            lista.RemoveAll(x => x[5] == "0");
                        }

                        ///Synchro z JIRY
                        IEnumerable<Issue> zgloszeniaWjira = null;
                        if (cb_SLA_JiraSynchro.Checked && doSynchro)
                        {
                            //Jira jiraSlaSynch;
                            //jiraSlaSynch = Jira.CreateRestClient("http://jira", jiraUser.Login, jiraUser.Password);
                            string numerki = "";

                            for (int i = 0; i < lista.Count(); i++)
                            {
                                numerki += (lista[i][1].ToString() == null ||
                                            lista[i][1].ToString() == ""
                                            ? "" : lista[i][1].ToString() + ",");
                                //numerki += dgv_SlaRaport.Rows[i].Cells[1].Value.ToString() +"," ; 
                            }

                            numerki = "issue in (" + numerki.Remove(numerki.Length - 1) + ")";
                            zgloszeniaWjira = jira.GetIssuesFromJql(numerki, lista.Count());

                            dgv_SlaRaport.Columns["dgvAktualnyStan"].Visible = true;
                            dgv_SlaRaport.Columns["dgvAktualniePrzydzielony"].Visible = true;
                            dgv_SlaRaport.Columns["dgvAktualniePrzydzielony"].Visible = true;
                            dgv_SlaRaport.Columns["dgvAktualnyPriorytet"].Visible = true;
                            dgv_SlaRaport.Columns["dgvOstatniaAkcja"].Visible = true;
                        }
                        else
                        {
                            dgv_SlaRaport.Columns["dgvAktualnyStan"].Visible = false;
                            dgv_SlaRaport.Columns["dgvAktualniePrzydzielony"].Visible = false;
                            dgv_SlaRaport.Columns["dgvAktualniePrzydzielony"].Visible = false;
                            dgv_SlaRaport.Columns["dgvAktualnyPriorytet"].Visible = false;
                            dgv_SlaRaport.Columns["dgvOstatniaAkcja"].Visible = false;
                        }
                        ///

                        //dgv_SlaRaport.Columns.Clear();
                        dgv_SlaRaport.Rows.Clear();

                        List<string> wartosciUpdatowane = new List<string>();

                        lista = lista.OrderByDescending(x => Int32.Parse(x[8])).ToList();
                        foreach (var row in lista)
                        {
                            dgv_SlaRaport.Rows.Insert(0, new DataGridViewRow());
                            dgv_SlaRaport.Rows[0].Cells["dgvIssueId"].Value = row[0];  //dgvIssueId
                            dgv_SlaRaport.Rows[0].Cells["dgvJiraNr"].Value = row[1];  //dgvJiraNr
                            dgv_SlaRaport.Rows[0].Cells["dgvOdpowiedzialny"].Value = row[2];  //dgvOdpowiedzialny
                            dgv_SlaRaport.Rows[0].Cells["dgvTypZgloszenia"].Value = row[3];  //dgvTypZgloszenia
                            dgv_SlaRaport.Rows[0].Cells["dgvPriorytet"].Value = row[4];  //dgvPriorytet
                            dgv_SlaRaport.Rows[0].Cells["dgvPauza"].Value = row[5];  //dgvPauza
                            dgv_SlaRaport.Rows[0].Cells["dgvAktCzasRealizacji"].Value = row[6];  //dgvAktCzasRealizacji
                            dgv_SlaRaport.Rows[0].Cells["dgvCzasRozwiazania"].Value = row[7];  //dgvCzasRozwiazania 
                            dgv_SlaRaport.Rows[0].Cells["dgvPozostaloMin"].Value = row[8];  //dgvPozostaloMin  

                            dgv_SlaRaport.Rows[0].Cells["dgvAkcjaBPM"].Value = row[9];  //dgvOstatniaAkcja  

                            DataGridViewRow _row = dgv_SlaRaport.Rows[0];
                            if (cb_SLA_JiraSynchro.Checked && doSynchro)
                            {
                                try
                                {
                                    if (!isNullObjectOrEmptyString(zgloszeniaWjira.FirstOrDefault(x => x.Key.Value == row[1])))
                                    {
                                        string _jiraStatusName = zgloszeniaWjira.Where(x => x.Key.Value == row[1]).Select(y => y.Status.Name).First().ToString();
                                        string _jiraKeyValue = zgloszeniaWjira.Where(x => x.Key.Value == row[1]).Select(y => y.Key.Value).First().ToString();
                                        string _jiraAssige = zgloszeniaWjira.Where(x => x.Key.Value == row[1]).Select(y => y.Assignee).First().ToString();
                                        string _jiraPriority = zgloszeniaWjira.Where(x => x.Key.Value == row[1]).Select(y => y.Priority.Name).First().ToString();
                                        string _jiraUpdated = zgloszeniaWjira.Where(x => x.Key.Value == row[1]).Select(y => y.Updated).First().ToString();

                                        dgv_SlaRaport["dgvAktualnyStan", 0].Value = _jiraStatusName;
                                        dgv_SlaRaport["dgvAktualniePrzydzielony", 0].Value = (dgv_SlaRaport["dgvJiraNr", 0].Value.ToString() != _jiraKeyValue ? string.Format("{0} -> {1}", _jiraKeyValue, _jiraAssige) : _jiraAssige);
                                        dgv_SlaRaport["dgvAktualniePrzydzielony", 0].Tag = _jiraKeyValue;
                                        dgv_SlaRaport["dgvAktualnyPriorytet", 0].Value = _jiraPriority;
                                        dgv_SlaRaport["dgvOstatniaAkcja", 0].Value = _jiraUpdated;
                                    }
                                }
                                catch
                                {
                                    //doByWorker(new DoWorkEventHandler(slaUpdateRowInfo), _row, null);
                                }

                                //Issue tmpIssue = jira.GetIssue(row[1].ToString());  //dgvJiraNr

                                //slaUpdateRowInfo(ref _row);

                                //doByWorker(new DoWorkEventHandler(slaUpdateRowInfo), _row, null);

                                //if (isNullObjectOrEmptyString(dgv_SlaRaport.Rows[i].Cells["dgvAktualnyStan"].Value))
                                //dgv_SlaRaport.Rows[i].Cells["dgvAktualnyStan"].Value == string.Empty
                                //|| dgv_SlaRaport.Rows[i].Cells["dgvAktualnyStan"].Value == null)
                                //{

                                //}





                                //dgv_SlaRaport.Rows[0].Cells["dgvAktualnyStan"].Value = tmpIssue.Status;//dgvAktualnyStan

                                //dgv_SlaRaport.Rows[0].Cells["dgvAktualniePrzydzielony"].Value = (item.Assignee == null ? " " : item.Assignee.ToString());//dgvAktualniePrzydzielony   

                                //switch (item.Priority.Id.ToString())
                                //{
                                //    case "1":
                                //        dgv_SlaRaport.Rows[0].Cells["dgvAktualnyPriorytet"].Value = "Blokujacy";
                                //        break;
                                //    case "2":
                                //        dgv_SlaRaport.Rows[0].Cells["dgvAktualnyPriorytet"].Value = "Krytyczny";
                                //        break;
                                //    case "3":
                                //        dgv_SlaRaport.Rows[0].Cells["dgvAktualnyPriorytet"].Value = "Wazny";
                                //        break;
                                //    case "4":
                                //        dgv_SlaRaport.Rows[0].Cells["dgvAktualnyPriorytet"].Value = "Sredni";
                                //        break;
                                //    case "5":
                                //        dgv_SlaRaport.Rows[0].Cells["dgvAktualnyPriorytet"].Value = "Niski";
                                //        break;
                                //    default:
                                //        break;
                                //}



                                /**/

                                //foreach (var item in zgloszeniaWjira)
                                //{
                                //    if (row[1] == item.Key.Value)
                                //    {

                                //        dgv_SlaRaport.Rows[0].Cells["dgvAktualnyStan"].Value = item.Status.Name.ToString();//dgvAktualnyStan

                                //        dgv_SlaRaport.Rows[0].Cells["dgvAktualniePrzydzielony"].Value = (item.Assignee == null ? " " : item.Assignee.ToString());//dgvAktualniePrzydzielony   

                                //        switch (item.Priority.Id.ToString())
                                //        {
                                //            case "1":
                                //                dgv_SlaRaport.Rows[0].Cells["dgvAktualnyPriorytet"].Value = "Blokujacy";
                                //                break;
                                //            case "2":
                                //                dgv_SlaRaport.Rows[0].Cells["dgvAktualnyPriorytet"].Value = "Krytyczny";
                                //                break;
                                //            case "3":
                                //                dgv_SlaRaport.Rows[0].Cells["dgvAktualnyPriorytet"].Value = "Wazny";
                                //                break;
                                //            case "4":
                                //                dgv_SlaRaport.Rows[0].Cells["dgvAktualnyPriorytet"].Value = "Sredni";
                                //                break;
                                //            case "5":
                                //                dgv_SlaRaport.Rows[0].Cells["dgvAktualnyPriorytet"].Value = "Niski";
                                //                break;
                                //            default:
                                //                break;
                                //        }

                                //        foreach (var customFild in item.CustomFields)
                                //        {
                                //            if (customFild.Id == "customfield_20350")
                                //            {
                                //                foreach (var opcje in customFild.Values)
                                //                {
                                //                    string str = " (" + opcje + ")";
                                //                    dgv_SlaRaport.Rows[0].Cells["dgvAktualnyPriorytet"].Value += str;
                                //                    /**/
                                //                    List<List<string>> cf_opcje = gujaczWFS.ExecuteStoredProcedure("sp_aktualizujOpcje", new string[] { row[0], opcje }, DatabaseName.SupportCP);
                                //                    if (cf_opcje[0][0] == "UPDATE")
                                //                    {
                                //                        wartosciUpdatowane.Add("Aktualizowano " + row[1] + " (" + row[0] + ") na wartość: " + opcje);
                                //                    }
                                //                }
                                //            }
                                //        }
                                //        dgv_SlaRaport.Rows[0].Cells["dgvOstatniaAkcja"].Value = item.Updated.Value.ToLocalTime().ToString();//dgvOstatniaAkcja

                                //    }



                            }


                            int totalTime, timeLeft;
                            string bpmPriority, jiraPriority;

                            if (row[7] != string.Empty && row[8] != string.Empty)
                            {
                                totalTime = Int32.Parse(row[7]);
                                timeLeft = Int32.Parse(row[8]);

                                if (timeLeft <= totalTime / 4)
                                    dgv_SlaRaport.Rows[0].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 98, 118);
                                else if (timeLeft <= totalTime / 2)
                                    dgv_SlaRaport.Rows[0].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 222, 89);
                                //else if (!isNullObjectOrEmptyString(tmpIssue)
                                //        && row[1] != tmpIssue.Key.Value)
                                //    dgv_SlaRaport.Rows[0].DefaultCellStyle.BackColor = Color.FromArgb(192, 192, 192);
                                else
                                    dgv_SlaRaport.Rows[0].DefaultCellStyle.BackColor = Color.FromArgb(255, 198, 239, 206);

                                dgv_SlaRaport.Rows[0].Tag = dgv_SlaRaport.Rows[0].DefaultCellStyle.BackColor;


                            }

                        }
                        dgv_SlaRaport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
                        //if (cb_SLA_JiraSynchro.Checked && doSynchro)
                        //{
                        //    for (int i = 0; i < dgv_SlaRaport.RowCount - 1; i++)
                        //    {
                        //        if (dgv_SlaRaport.Rows[i].Cells["dgvAktualnyPriorytet"].Value != null && dgv_SlaRaport.Rows[i].Cells["dgvPriorytet"].Value.ToString() != dgv_SlaRaport.Rows[i].Cells["dgvAktualnyPriorytet"].Value.ToString())
                        //        {
                        //            dgv_SlaRaport.Rows[i].DefaultCellStyle.BackColor = Color.Cyan;
                        //            dgv_SlaRaport.Rows[i].Tag = dgv_SlaRaport.Rows[i].DefaultCellStyle.BackColor;
                        //        }

                        //        if(isNullObjectOrEmptyString(dgv_SlaRaport.Rows[i].Cells["dgvAktualnyStan"].Value))
                        //        //dgv_SlaRaport.Rows[i].Cells["dgvAktualnyStan"].Value == string.Empty
                        //        //|| dgv_SlaRaport.Rows[i].Cells["dgvAktualnyStan"].Value == null)
                        //        {
                        //            Issue tmpIssue = jira.GetIssue(dgv_SlaRaport.Rows[i].Cells["dgvJiraNr"].Value.ToString());

                        //            dgv_SlaRaport.Rows[i].Cells["dgvAktualnyStan"].Value = tmpIssue.Status.Name;
                        //            dgv_SlaRaport.Rows[i].Cells["dgvAktualniePrzydzielony"].Value = string.Format("{0} -> {1}",tmpIssue.Key, tmpIssue.Assignee);
                        //            dgv_SlaRaport.Rows[i].Cells["dgvAktualnyPriorytet"].Value = tmpIssue.Priority.Name;
                        //            dgv_SlaRaport.Rows[i].Cells["dgvOstatniaAkcja"].Value = tmpIssue.Updated;

                        //            dgv_SlaRaport.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(192, 192, 192);

                        //        }
                        //    }
                        //}

                        dgv_SlaRaport.ReadOnly = true;

                        dgv_SlaRaport.Columns["dgvCzasRozwiazania"].Visible = false;

                        ////
                        //if (wartosciUpdatowane.Count > 0)
                        //{
                        //    string komunikat = "";
                        //    foreach (var item in wartosciUpdatowane)
                        //    {
                        //        komunikat += item + "\n";

                        //    }
                        //    MessageBox.Show(komunikat, "Automatycznie poprawiono dane!");

                        //}

                        toolStripStatusLabel6.Text = string.Empty;
                        pb_SetVisibilityPanel(false);

                        //WorkingSla = false;
                        //if (cb_SLA_JiraSynchro.Checked && doSynchro)
                        //{
                        //    slaAutoAssigneTakenIssue(lista, zgloszeniaWjira);
                        //}

                    }

                });

            }
            catch (Exception ex)
            {
                ExceptionManager.LogError(ex, Logger.Instance, true);
                NoticeForm.ShowNotice(ex.Message);
            }
            WorkingSla = false;

        }

        [STAThreadAttribute]
        private void btn_slaRaport_Load_Click(object sender, EventArgs e)
        {
            try
            {
                if (!WorkingSla)
                {


                    WorkingSla = true;
                    doByWorker(new DoWorkEventHandler(slaReportLoad), null, new RunWorkerCompletedEventHandler(slaReportLoadJiraInfo));
                }
                //slaReportLoad();


                //thr.IsBackground = true;
                //thr.Start();

                //while (thr.IsAlive)// ThreadState != System.Threading.ThreadState.Running)
                //{
                //    //Thread.Sleep(1000);
                //    //SpinWait sw = new SpinWait();
                //    //sw.
                //}
                //if (cb_SLA_JiraSynchro.Checked && doSynchro)
                //{
                //    foreach (DataGridViewRow item in dgv_SlaRaport.Rows)
                //    {

                //        DataGridViewRow _row = item;
                //        slaUpdateRowInfo(ref _row);
                //    }


                //}
            }
            catch (Exception ex)
            {
                ExceptionManager.LogError(ex, Logger.Instance, true);
                NoticeForm.ShowNotice(ex.Message);
            }
        }




        private void slaReportLoadJiraInfo(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {

                //string _jiraStatusName = zgloszeniaWjira.Where(x => x.Key.Value == row[1]).Select(y => y.Status.Name).First().ToString();
                foreach (DataGridViewRow item in dgv_SlaRaport.Rows)
                {

                    doByWorker(new DoWorkEventHandler(slaUpdateRowInfo), item, new RunWorkerCompletedEventHandler(slaAutoAssigneTakenIssue));
                }

            }
            catch (Exception ex)
            {

            }
        }

        int _count;


        private void slaUpdateRowInfo(object sender, DoWorkEventArgs e)//(DataGridViewRow row)
        {

            DataGridViewRow tmpRow = e.Argument as DataGridViewRow;

            //Thread thrJ = new Thread((ThreadStart)delegate
            //{
            try
            {
                //dgv_SlaRaport["dgvJiraNr", i].

                Issue tmpIssue; //= jira.GetIssue(tmpRow.Cells[1].Value.ToString());  //dgvJiraNr
                if (isNullObjectOrEmptyString(tmpRow.Cells["dgvAktualnyStan"].Value))
                {
                    tmpIssue = jira.RestClient.GetIssueAsync(tmpRow.Cells[1].Value.ToString(), CancellationToken.None).Result;
                    //tmpIssue = jira.GetIssue(tmpRow.Cells[1].Value.ToString());
                    //DataGridViewRow tmpRow = dgv_SlaRaport.Rows[0];

                    tmpRow.Cells["dgvAktualnyStan"].Value = tmpIssue.Status.Name;
                    tmpRow.Cells["dgvAktualnyPriorytet"].Value = tmpIssue.Priority.Name;
                    tmpRow.Cells["dgvOstatniaAkcja"].Value = tmpIssue.Updated;

                    tmpRow.DefaultCellStyle.BackColor = Color.FromArgb(192, 192, 192);
                    tmpRow.Tag = (Color)tmpRow.DefaultCellStyle.BackColor;

                    tmpRow.Cells["dgvAktualniePrzydzielony"].Value = (tmpRow.Cells[1].Value.ToString() != tmpIssue.Key.Value ? string.Format("{0} -> {1}", tmpIssue.Key, tmpIssue.Assignee) : tmpIssue.Assignee);

                    //foreach (var customFild in tmpIssue.CustomFields)
                    //{
                    //    if (customFild.Id == "customfield_20350")
                    //    {
                    //        foreach (var opcje in customFild.Values)
                    //        {
                    //            string str = " (" + opcje + ")";
                    //            dgv_SlaRaport.Rows[0].Cells["dgvAktualnyPriorytet"].Value += str;
                    //            /**/
                    //            List<List<string>> cf_opcje = gujaczWFS.ExecuteStoredProcedure("sp_aktualizujOpcje", new string[] { row[0], opcje }, DatabaseName.SupportCP);
                    //            if (cf_opcje[0][0] == "UPDATE")
                    //            {
                    //                wartosciUpdatowane.Add("Aktualizowano " + row[1] + " (" + row[0] + ") na wartość: " + opcje);
                    //            }
                    //            break;
                    //        }
                    //    }
                    //}

                    tmpRow.Cells["dgvOstatniaAkcja"].Value = tmpIssue.Updated.Value.ToLocalTime().ToString();//dgvOstatniaAkcja
                }
            }
            catch (Exception ex)
            { }

            if (!isNullObjectOrEmptyString(tmpRow.Cells["dgvAktualnyStan"].Value))
                dgv_SlaRaport["dgvAktualnyStan", tmpRow.Index].Value = tmpRow.Cells["dgvAktualnyStan"].Value.ToString();// tmpIssue.Status.Name;

            if (!isNullObjectOrEmptyString(tmpRow.Cells["dgvAktualniePrzydzielony"].Value))
                dgv_SlaRaport["dgvAktualniePrzydzielony", tmpRow.Index].Value = tmpRow.Cells["dgvAktualniePrzydzielony"].Value.ToString();//(tmpRow.Cells[1].Value.ToString() != tmpIssue.Key.Value ? string.Format("{0} -> {1}", tmpIssue.Key, tmpIssue.Assignee) : tmpIssue.Assignee);

            if (!isNullObjectOrEmptyString(tmpRow.Cells["dgvAktualniePrzydzielony"].Tag))
                dgv_SlaRaport["dgvAktualniePrzydzielony", tmpRow.Index].Tag = tmpRow.Cells["dgvAktualniePrzydzielony"].Tag.ToString();// tmpIssue.Key.Value.ToString();

            if (!isNullObjectOrEmptyString(tmpRow.Cells["dgvAktualnyPriorytet"].Value))
                dgv_SlaRaport["dgvAktualnyPriorytet", tmpRow.Index].Value = tmpRow.Cells["dgvAktualnyPriorytet"].Value.ToString();// tmpIssue.Priority.Name;

            if (!isNullObjectOrEmptyString(tmpRow.Cells["dgvOstatniaAkcja"].Value))
                dgv_SlaRaport["dgvOstatniaAkcja", tmpRow.Index].Value = tmpRow.Cells["dgvOstatniaAkcja"].Value.ToString();// tmpIssue.Updated;

            //zgloszeniaWjira.
            //});

            //thrJ.IsBackground = true;
            //thrJ.Start();
            e.Result = tmpRow;
        }


        private bool slaAutoAssigneTakenIssue(List<List<string>> IssueBPM, IEnumerable<Issue> IssueJira)
        {


            //foreach (var iJira in IssueJira)
            //{
            if (!isNullObjectOrEmptyString(IssueJira))
            {
                for (int i = 0; i < dgv_SlaRaport.Rows.Count; i++)
                {
                    string s = dgv_SlaRaport["dgvOdpowiedzialny", i].Value == null ? string.Empty : dgv_SlaRaport["dgvOdpowiedzialny", i].Value.ToString();
                    string a = dgv_SlaRaport["dgvAkcjaBPM", i].Value == null ? string.Empty : dgv_SlaRaport["dgvAkcjaBPM", i].Value.ToString();

                    string issueNumber = dgv_SlaRaport["dgvJiraNr", i].Value == null ? string.Empty : dgv_SlaRaport["dgvJiraNr", i].Value.ToString();
                    Issue itmp = IssueJira.FirstOrDefault(x => x.Key == issueNumber);


                    if (
                        //iJira.Key == issueNumber &&
                        itmp != null &&
                        s.Contains(string.Empty) &&
                        checkBillUser(itmp.Assignee, LoginParamType.Login) &&
                        a.Contains("Utworzenie zgłoszenia"))
                    {
                        dgv_SlaRaport.Rows[i].Selected = true;

                        MouseEventArgs mea = new MouseEventArgs(MouseButtons.Right, 1, 0, 0, 0);
                        DataGridViewCellMouseEventArgs e = new DataGridViewCellMouseEventArgs(1, i, 0, 0, mea);

                        cms_IssuePopup.Items.Clear();

                        this.Tag = (bool)true;

                        List<BillingIssueDtoHelios> issue = new List<BillingIssueDtoHelios>();
                        addIssueToTreeNode(issueNumber, issue);
                        getActionToIssue(issue, issueNumber, "treeView4", getUserBpmJira(itmp.Assignee));


                        dgv_SlaRaport.Rows[i].Selected = false;
                        this.Tag = null;
                    }
                    //break;
                }
            }


            return true;
        }

        private void slaAutoAssigneTakenIssue(object sender, RunWorkerCompletedEventArgs e)
        {
            DataGridViewRow tmpRow = e.Result as DataGridViewRow;

            string issueNumber = tmpRow.Cells["dgvJiraNr"].Value == null ? string.Empty : tmpRow.Cells["dgvJiraNr"].Value.ToString();
            try
            {
                if (!isNullObjectOrEmptyString(issueNumber))
                {
                    //Issue itmp = jira.RestClient..GetIssue(issueNumber);
                    Issue itmp = jira.RestClient.GetIssueAsync(issueNumber, CancellationToken.None).Result;

                    string _bpmAssigne = isNullObjectOrEmptyString(tmpRow.Cells["dgvOdpowiedzialny"].Value) ? string.Empty : tmpRow.Cells["dgvOdpowiedzialny"].Value.ToString();
                    string _bpmLastAction = isNullObjectOrEmptyString(tmpRow.Cells["dgvAkcjaBPM"].Value) ? string.Empty : tmpRow.Cells["dgvAkcjaBPM"].Value.ToString();


                    //Issue itmp = IssueJira.FirstOrDefault(x => x.Key == issueNumber);

                    if (
                        //iJira.Key == issueNumber &&
                        itmp != null &&
                        _bpmAssigne.Contains(string.Empty) &&
                        checkBillUser(itmp.Assignee, LoginParamType.Login) &&
                        _bpmLastAction.Contains("Utworzenie zgłoszenia"))
                    {
                        tmpRow.Selected = true;
                        //dgv_SlaRaport.Rows[i].Selected = true;

                        MouseEventArgs mea = new MouseEventArgs(MouseButtons.Right, 1, 0, 0, 0);
                        DataGridViewCellMouseEventArgs em = new DataGridViewCellMouseEventArgs(1, tmpRow.Index, 0, 0, mea);

                        cms_IssuePopup.Items.Clear();

                        this.Tag = (bool)true;

                        List<BillingIssueDtoHelios> issue = new List<BillingIssueDtoHelios>();
                        addIssueToTreeNode(issueNumber, issue);
                        getActionToIssue(issue, issueNumber, "treeView4", getUserBpmJira(itmp.Assignee));


                        tmpRow.Selected = false;
                        this.Tag = null;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void dgv_SlaRaportCellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 1)
                {

                    numerZgl.Text = dgv_SlaRaport.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    System.Diagnostics.Process.Start("https://jira/browse/" + numerZgl.Text);
                }
                else if (e.ColumnIndex == 0)
                {

                    numerZgl.Text = dgv_SlaRaport.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    // System.Diagnostics.Process.Start("https://support.billennium.pl/Bpm/UserPanel/IssueDetails.aspx?issueId=" + numerZgl.Text);
                    System.Diagnostics.Process.Start("https://support.billennium.pl/Bpm/UserPanel/IssueHistory.aspx?issueId=" + numerZgl.Text);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.LogError(ex, Logger.Instance, true);
                NoticeForm.ShowNotice(ex.Message);
            }
        }
        private void dgv_SlaRaport_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            dgv_SlaRaport.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(51, 153, 255);

        }
        private void dgv_SlaRaport_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (dgv_SlaRaport.Rows[e.RowIndex].Tag != null)
            {
                dgv_SlaRaport.Rows[e.RowIndex].DefaultCellStyle.BackColor = (Color)dgv_SlaRaport.Rows[e.RowIndex].Tag;
            }
        }


        
        private void cb_SLApauza_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox tmp = (CheckBox)sender;
            if (tmp.Name.Equals("cb_SLApauza") && tmp.Checked)
            {
                cb_SLAWstrzymane.CheckState = CheckState.Unchecked;
            }
            else if (tmp.Name.Equals("cb_SLAWstrzymane") && tmp.Checked)
            {
                cb_SLApauza.CheckState = CheckState.Unchecked;
            }

            if (!WorkingSla)
                btn_slaRaport_Load_Click(this, null);
        }


        /// <summary>
        /// Dodanie menu kontekstowego do raportu SLA
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgv_SlaRaport_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                issues["treeView4"].Clear();
                for (int i = 0; i < dgv_SlaRaport.RowCount - 1; i++)
                {
                    dgv_SlaRaport.Rows[i].Selected = false;
                    dgv_SlaRaport.Rows[i].DefaultCellStyle.BackColor = (Color)dgv_SlaRaport.Rows[i].Tag;
                }

                dgv_SlaRaport.Rows[e.RowIndex].Selected = true;

                string issueNumber = dgv_SlaRaport.Rows[e.RowIndex].Cells["dgvJiraNr"].Value.ToString();
                cms_IssuePopup.Items.Clear();




                List<BillingIssueDtoHelios> issue = new List<BillingIssueDtoHelios>();
                addIssueToTreeNode(issueNumber, issue, "treeView4");


                if (!wfsList.ContainsKey("treeView4"))
                {
                    wfsList["treeView4"] = new List<BillingIssueDtoHelios>();
                }

                wfsList["treeView4"] = gujaczWFS.compareBillingWithWFS(issue);

                BillingIssueDtoHelios item = wfsList["treeView4"][0];


                BillingIssueDtoHelios updatedIssue = gujaczWFS.UpdateIssue(item);

                IssueState state = (item.isInWFS) ? IssueState.INWFS : IssueState.NEW;

                //issues["treeView4"].Add(updatedIssue, state);


                KeyValuePair<BillingIssueDto, IssueState> tmp = issues["treeView4"].Where(x =>
                {
                    if (x.Key.Idnumber == item.Idnumber)
                        return true;
                    else
                        return false;
                }).FirstOrDefault();


                selectIssue = tmp.Key;

                //Issue jiraIssue = jira.GetIssue(issueNumber);
                //BillingIssueDtoHelios issueDtoHelio = issue[0];
                ////jiraIssue.Add(jira.GetIssue(issueNumber));

                //KeyValuePair<BillingIssueDto, IssueState> tmp = issues["treeView4"].Where(x =>
                //{
                //    if (x.Key.Idnumber == issueNumber)
                //        return true;
                //    else
                //        return false;
                //}).FirstOrDefault();


                ////selectIssue = tmp.Key;

                //Logic.Implementation.JiraIssues jIssues = new Logic.Implementation.JiraIssues(this.jiraUser.Login, this.jiraUser.Password, "http://jira");

                //jIssues.ChangeDataModel(jiraIssue, out issueDtoHelio, ref JiraUsers);

                ////gujaczWFS.UpdateIssue(issueDtoHelio);
                //selectIssue = issueDtoHelio;


                getActionToIssue(issue, issueNumber);

            }
        }


        private void bt_SLA_synchronizacja_Click(object sender, EventArgs e)
        {
            if (dgv_SlaRaport.Rows.Count > 0)
            {
                for (int i = 0; i < dgv_SlaRaport.Rows.Count - 1; i++)
                {
                    dgv_SlaRaport.Rows[i].Visible = false;

                    //obsługa różnego priorytetu  
                    if (dgv_SlaRaport.Rows[i].Cells["dgvAktualnyPriorytet"].Value != null
                        && dgv_SlaRaport.Rows[i].Cells["dgvPriorytet"].Value.ToString() != dgv_SlaRaport.Rows[i].Cells["dgvAktualnyPriorytet"].Value.ToString()
                        )
                    {
                        dgv_SlaRaport.Rows[i].Visible = true;
                    }

                    //Spradzenie Konsultacji  
                    if (dgv_SlaRaport.Rows[i].Cells["dgvAkcjaBPM"].Value != null
                        && dgv_SlaRaport.Rows[i].Cells["dgvAkcjaBPM"].Value.ToString() == ""
                        )
                    {
                        dgv_SlaRaport.Rows[i].Visible = true;
                    }

                }
            }
        }

    }
}
