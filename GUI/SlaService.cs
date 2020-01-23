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
                jira.MaxIssuesPerRequest = 200;
                List<List<string>> lista = null;

                pb_SetVisibilityPanel(true);

                this.Invoke((MethodInvoker)delegate
                {
                    toolStripStatusLabel6.Text = "Trwa odświeżanie raportu SLA...";
                });


                //this.Invoke((MethodInvoker)delegate
                //{
                    lista = gujaczWFS.ExecuteStoredProcedure("cp_sla_raport_v2", new string[] { }, DatabaseName.SupportCP);
                //});

                List<string> numerki = new List<string>();
                zgloszeniaWjira = null;

                for (int i = 0; i < lista.Count(); i++)
                {
                    numerki.Add(lista[i][1].ToString());
                }
                string numerkiJQL = fnGetStringFromList(numerki, ',');
                numerkiJQL = "issue in (" + numerkiJQL + ")";

                this.Invoke((MethodInvoker)delegate
                {
                    if (cb_SLA_JiraSynchro.Checked && doSynchro)
                    {
                        zgloszeniaWjira = jira.GetIssuesFromJql(numerkiJQL, 200);// lista.Count());
                    }
                });

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
                                            //&& (zgloszeniaWjira.FirstOrDefault(y => y.Key == x[1]) != null ? !userBpmJiraList.IsBillUser(zgloszeniaWjira.FirstOrDefault(y => y.Key == x[1]).Assignee) : true ) );
                        }
                        if (cb_SLAWstrzymane.Checked)
                        {
                            lista.RemoveAll(x => x[5] == "0");
                        }

                        ///Synchro z JIRY
                        if (cb_SLA_JiraSynchro.Checked && doSynchro)
                        {

                            dgv_SlaRaport.Columns["dgvAktualnyStan"].Visible = true;
                            dgv_SlaRaport.Columns["dgvAktualniePrzydzielony"].Visible = true;
                            dgv_SlaRaport.Columns["dgvAktualnyPriorytet"].Visible = true;
                            dgv_SlaRaport.Columns["dgvOstatniaAkcja"].Visible = true;
                        }
                        else
                        {
                            dgv_SlaRaport.Columns["dgvAktualnyStan"].Visible = false;
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

                            //if (cb_SLApauza.Checked && row[5] == "0")
                            //{
                            //    dgv_SlaRaport.Rows[0].Visible = true;
                            //}
                            //else if (cb_SLAWstrzymane.Checked && row[5] == "1")
                            //{
                            //    dgv_SlaRaport.Rows[0].Visible = true;
                            //}
                            //else
                            //{
                            //    dgv_SlaRaport.Rows[0].Visible = false;
                            //}

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

                                        string _jiraResolution = string.Empty;// = zgloszeniaWjira.Where(x => x.Key.Value == row[1]).Select(y => y.Resolution.Name).First().ToString();



                                        dgv_SlaRaport["dgvAktualnyStan", 0].Value = _jiraStatusName;
                                        dgv_SlaRaport["dgvAktualniePrzydzielony", 0].Value = (dgv_SlaRaport["dgvJiraNr", 0].Value.ToString() != _jiraKeyValue ? string.Format("{0} -> {1}", _jiraKeyValue, _jiraAssige) : _jiraAssige);
                                        dgv_SlaRaport["dgvAktualniePrzydzielony", 0].Tag = _jiraKeyValue;
                                        dgv_SlaRaport["dgvAktualnyPriorytet", 0].Value = _jiraPriority;
                                        dgv_SlaRaport["dgvOstatniaAkcja", 0].Value = _jiraUpdated;

                                        IssueResolution ir = zgloszeniaWjira.Where(x => x.Key.Value == row[1]).FirstOrDefault().Resolution;
                                        if (!isNullObjectOrEmptyString(ir))
                                        {
                                            _jiraResolution = ir.Name;
                                            dgv_SlaRaport["dgvAktualnyStan", 0].Value += " - " + _jiraResolution;
                                            dgv_SlaRaport.Rows[0].DefaultCellStyle.BackColor = Color.FromArgb(222, 222, 222);
                                            dgv_SlaRaport.Rows[0].Tag = dgv_SlaRaport.Rows[0].DefaultCellStyle.BackColor;
                                            //dgv_SlaRaport.Rows[0].Cells[7].Style.BackColor = Color.FromArgb(222, 222, 222);
                                            //Color.FromArgb(255, 111, 239, 222);
                                        }
                                        //if (_jiraResolution != string.Empty)
                                        //{
                                        //    dgv_SlaRaport.Rows[0].DefaultCellStyle.BackColor = Color.FromArgb(255, 111, 239, 222);
                                        //}

                                    }
                                }
                                catch (Exception ex)
                                {
                                    //doByWorker(new DoWorkEventHandler(slaUpdateRowInfo), _row, null);
                                }





                            }
                            /// czyoncall GetBillingBoundEventParamForIssue
                            /// 
                            var vOncall = gujaczWFS.GetBillingBoundEventParamForIssue(Int32.Parse(row[0]), new int[] {  6816, 6817});
                            dgv_SlaRaport["dgvOnCall", 0].Value = vOncall.OrderByDescending(x => x.EventParamId).FirstOrDefault().Value;


                            int totalTime, timeLeft;
                            string bpmPriority, jiraPriority;

                            if (row[7] != string.Empty && row[8] != string.Empty && isNullObjectOrEmptyString(dgv_SlaRaport.Rows[0].Tag))
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


                                if (vOncall.OrderByDescending(x => x.EventParamId).FirstOrDefault().Value == "True")
                                {
                                    dgv_SlaRaport.Rows[0].DefaultCellStyle.BackColor = Color.Black;
                                    dgv_SlaRaport.Rows[0].DefaultCellStyle.ForeColor = Color.DarkRed;
                                }

                                //tmpRow.DefaultCellStyle.BackColor = Color.FromArgb(192, 192, 192);
                                //tmpRow.Tag = (Color)tmpRow.DefaultCellStyle.BackColor;

                                dgv_SlaRaport.Rows[0].Tag = dgv_SlaRaport.Rows[0].DefaultCellStyle.BackColor;



                            }
                            ////
                            ///
                            //this.Invoke((MethodInvoker)delegate
                            //{
                            //    updateTreeNodeForSLA(dgv_SlaRaport.Rows[0]);
                            //});

                        }
                        dgv_SlaRaport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;



                        dgv_SlaRaport.ReadOnly = true;

                        this.Invoke((MethodInvoker)delegate
                        {
                            toolStripStatusLabel6.Text = string.Empty;
                        });

                        pb_SetVisibilityPanel(false);
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
            const string treeViewName = "treeView4";

            try
            {
                treeView4.Nodes.Clear();
                issues[treeViewName].Clear();
                selectIssueList.Clear();

                if(wfsList.ContainsKey(treeViewName))
                {
                    wfsList[treeViewName].Clear();
                }

                if (!issueMove.ContainsKey(treeViewName))
                {
                    issueMove[treeViewName] = new Dictionary<string, Dictionary<int, string>>();
                }
                else
                {
                    issueMove[treeViewName].Clear();
                }

                //string _jiraStatusName = zgloszeniaWjira.Where(x => x.Key.Value == row[1]).Select(y => y.Status.Name).First().ToString();
                foreach (DataGridViewRow item in dgv_SlaRaport.Rows)
                {
                    if (isNullObjectOrEmptyString(item.Cells[0].Value))
                        return;
                    int issueNumber = Convert.ToInt32(item.Cells[0].Value.ToString());
                    string jiraNumber = item.Cells[1].Value.ToString();
                    string userSLA = item.Cells[3].Value.ToString();
                     
                    var v = GetActionForIssue(issueNumber);
                    Issue issue = zgloszeniaWjira.FirstOrDefault(x => x.Key == jiraNumber);
                    /*
                    List<EventParamModeler> eventParamForFormByEventMove = gujaczWFS.GetEventParamForFormByEventMove(1000000745);
                    List<EventParam> currParam = gujaczWFS.GetBoundEventParamForIssue(issueNumber, eventParamForFormByEventMove.Where(x => x.BoundEventParamId > 0).Select(x => x.BoundEventParamId).ToArray<int>());
                    */
                    
                    if (isNullObjectOrEmptyString(issue))
                    {
                        issue = jira.GetIssue(jiraNumber);

                    }
                    if (isNullObjectOrEmptyString(item.Cells["dgvAktualnyStan"].Value))
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            slaUpdateRowInfo(item);
                            slaAutoAssigneTakenIssue(item);
                        });

                        //doByWorker(new DoWorkEventHandler(slaUpdateRowInfo), item, new RunWorkerCompletedEventHandler(slaAutoAssigneTakenIssue));
                    }
                    //else if (isNullObjectOrEmptyString(item.Cells["dgvOdpowiedzialny"].Value) 
                    //        && userBpmJiraList.IsBillUser(issue.Assignee) )
                    //{

                    //    issueStep_RozpocznijDiagnoze(userBpmJiraList.GetBillUser(issue.Assignee), jiraNumber.ToString());
                    //    //doByWorker(new DoWorkEventHandler(slaAutoAssigneTakenIssue), item, null);
                    //    System.Diagnostics.Debug.WriteLine(string.Format("issueStep_RozpocznijDiagnoze {0} -> {1} ", jiraNumber, issue.Assignee));
                    //}

                    if (!isNullObjectOrEmptyString(item.Cells["dgvOdpowiedzialny"].Value)
                              && ((!isNullObjectOrEmptyString(issue.Resolution) && issue.Resolution.Name == "Odrzucone")
                                 || (issue.Type.Name.Equals("Problem") && issue.Status.Name.Equals("Oczekuje na odrzucenie")) ) 
                              && !isNullObjectOrEmptyString(v.FirstOrDefault(x => x.Key == 617).Value))
                    {
                        issueStep_OdrzucenieZgloszenia(this, jiraNumber.ToString());
                        //issueStep_OdrzucenieZgloszenia(this, issueNumber.ToString());
                    }
                    else
                    {
                      //  issueStep_Aktualizacja(null, jiraNumber.ToString(),issue);

                        bool issueMoveProject = false;
                        bool issueChangeUserBill = false;
                        IEnumerable<IssueChangeLog> cl = issue.GetChangeLogs();
                        foreach (IssueChangeLog changeLog in cl.OrderByDescending(x => x.CreatedDate))
                        {
                            //zmiana katalogu
                            if (!isNullObjectOrEmptyString(changeLog.Items.FirstOrDefault(x => x.FieldName == "Key" && x.FromValue == jiraNumber)))
                            {
                                issueMoveProject = true;
                                break;
                            }

                            var action = gujaczWFS.GetActionForIssue(issueNumber, gujaczWFS.getUser().Id);

                            //user change
                            //param = changeLog.Items.FirstOrDefault(x => x.FieldName == "assignee" && x.FromValue == userSLA && userBpmJiraList.IsBillUser(x.ToId));                            
                            if (userBpmJiraList.IsBillUser(changeLog.Author.Username))
                            {
                                UserBpmJira ubj;
                                IssueChangeLogItem param = changeLog.Items.FirstOrDefault(
                                    x => x.FieldName == "assignee" &&
                                        (userBpmJiraList.IsBillUser(changeLog.Author.Username) && (userBpmJiraList.IsBillUser(x.ToId) || x.FromId == "billennium"))
                                            ||
                                        (userBpmJiraList.IsBillUser(issue.Assignee) && (userBpmJiraList.IsBillUser(x.ToId) && x.FromId == "billennium"))

                                );
                                                                    
                                IssueChangeLogItem status = changeLog.Items.FirstOrDefault(x => x.FieldName == "status" && x.FieldType == "jira");// && (userBpmJiraList.IsBillUser(x.ToId) || x.FromId == "billennium"));

                                //param = changeLog.Items.FirstOrDefault(x => x.FieldName == "assignee" && userBpmJiraList.IsBillUser(issue.Assignee) && (userBpmJiraList.IsBillUser(x.ToId) && x.FromId == "billennium"));
                                if (isNullObjectOrEmptyString(item.Cells["dgvOdpowiedzialny"].Value.ToString().Trim())
                                        && userBpmJiraList.IsBillUser(changeLog.Author.Username))
                                {
                                    if (!isNullObjectOrEmptyString(userBpmJiraList.IsBillUser(changeLog.Author.Username))
                                        && !isNullObjectOrEmptyString(param))

                                    {

                                        if (!isNullObjectOrEmptyString(action.FirstOrDefault(x => x.Value == "Rozpoczęcie diagnozy")))
                                        {
                                            if (param.ToId == issue.Assignee)
                                            {
                                                issueStep_RozpocznijDiagnoze(userBpmJiraList.GetBillUser(issue.Assignee)
                                                                        , jiraNumber.ToString());
                                                System.Diagnostics.Debug.WriteLine(string.Format("issueStep_RozpocznijDiagnoze {0} -> {1} ", jiraNumber, userBpmJiraList.GetBillUser(issue.Assignee)));
                                            }
                                            else //if ()
                                            {
                                                issueStep_RozpocznijDiagnoze(userBpmJiraList.GetBillUser(changeLog.Author.Username)
                                                                            , jiraNumber.ToString());
                                                System.Diagnostics.Debug.WriteLine(string.Format("issueStep_RozpocznijDiagnoze {0} -> {1} ", jiraNumber, userBpmJiraList.GetBillUser(changeLog.Author.Username)));
                                            }
                                        }
                                    }
                                }
                                else if (!isNullObjectOrEmptyString(param)
                                    && userBpmJiraList.TryGetBillUser(issue.Assignee, out ubj)
                                    && userSLA != string.Empty
                                    && ubj.UserJira.FullName != userSLA
                                    && ubj.UserBpm.Id != -1
                                    ) //pominięcie loginu technicznego
                                {


                                    if (!issueMoveProject && !isNullObjectOrEmptyString(action.FirstOrDefault(x => x.Value == "Zmiana wykonawcy")))
                                    {
                                        issueStep_ZmianaWykonawcy(userBpmJiraList.GetBillUser(ubj.UserJira.login), jiraNumber.ToString());
                                        break;
                                    }
                                }
                                if (!isNullObjectOrEmptyString(status))
                                {
                                    if ((status.ToValue == "Zakończone" && issue.Type.Name == "Incydent" && !isNullObjectOrEmptyString(issue.Resolution) && issue.Resolution.Name == "Zakończone")
                                        || (status.ToValue == "W trakcie testów" && issue.Type.Name == "Zlecenie operatorskie" && !userBpmJiraList.IsBillUser(issue.Assignee))
                                        || (status.ToValue == "Weryfikacja naprawy" && issue.Type.Name == "Problem" && !userBpmJiraList.IsBillUser(issue.Assignee))
                                        )
                                    {
                                        if (
                                                
                                                userBpmJiraList.TryGetBillUser(gujaczWFS.getUser().login, out ubj)
                                                && ubj.UserJira.login != changeLog.Author.Username
                                                && userSLA != string.Empty
                                                && ubj.UserJira.FullName != userSLA
                                                && ubj.UserBpm.Id != -1
                                                && !isNullObjectOrEmptyString(action.FirstOrDefault(x => x.Value == "Zmiana wykonawcy"))
                                            ) //pominięcie loginu technicznego
                                        {
                                            
                                            issueStep_ZmianaWykonawcy(userBpmJiraList.GetBillUser(ubj.UserJira.login), jiraNumber.ToString());

                                        }
                                        issueStep_RealizujIZamknij(userBpmJiraList.GetBillUser(changeLog.Author.Username)
                                                                        , jiraNumber.ToString());
                                    }
                                }
                                
                            }
                        }

                        if(issueMoveProject)
                        {



                            Dictionary<int, string> kat = gujaczWFS.getBillingComponents(0);
                            IEnumerable<Project> jiraPorojects =  jira.GetProjects();

                            //foreach (var itemcf in issue.CustomFields)
                            //{
                            //    System.Diagnostics.Debug.WriteLine(string.Format("{0}: {1} - {2}", itemcf.Id, itemcf.Values[0], itemcf.Name));
                            //}

                            if (kat.ContainsValue(jiraPorojects.FirstOrDefault(x=> x.Key == issue.Project).Name))
                            {
                                issueStep_Aktualizacja(this, jiraNumber.ToString(), issue);
                            }
                            else
                            {
                                issueStep_ZmianaKataloguJira(this, jiraNumber.ToString());
                            }
          


                        }

                    }
                    /*else if (!isNullObjectOrEmptyString(item.Cells["dgvAktualnyStan"].Value)
                            && (item.Cells["dgvAktualnyStan"].Value.ToString() == "Odrzucone" ||
                                item.Cells["dgvAktualnyStan"].Value.ToString() == "Odrzucony" ||
                                item.Cells["dgvAktualnyStan"].Value.ToString() == "Zadanie do wycofania"))
                    {
                        doByWorker(new DoWorkEventHandler(slaAutoAssigneTakenIssue), item, null);

                    }*/
                    /*else 
                    {
                        doByWorker(new DoWorkEventHandler(slaAutoAssigneTakenIssue), item, null);
                    }*/
                }

            }
            catch (Exception ex)
            {

            }
        }

        int _count;

        private void slaUpdateRowInfo(DataGridViewRow tmpRow)//(DataGridViewRow row)
        {
            //Thread thrJ = new Thread((ThreadStart)delegate
            //{
            try
            {
                //dgv_SlaRaport["dgvJiraNr", i].

                Issue tmpIssue; //= jira.GetIssue(tmpRow.Cells[1].Value.ToString());  //dgvJiraNr
                if (isNullObjectOrEmptyString(tmpRow.Cells["dgvAktualnyStan"].Value))
                {
                    if (isNullObjectOrEmptyString(tmpRow.Cells[1].Value))
                        return;

                    tmpIssue = zgloszeniaWjira.FirstOrDefault(x => x.Key == tmpRow.Cells[1].ToString());
                    if (isNullObjectOrEmptyString(tmpIssue))
                    {
                        Issue tmpIssueVar = null;
                        this.Invoke((MethodInvoker)delegate
                        {
                            tmpIssueVar = jira.RestClient.GetIssueAsync(tmpRow.Cells[1].Value.ToString(), CancellationToken.None).Result;
                        });
                        tmpIssue = tmpIssueVar;
                    }


                    tmpRow.Cells["dgvAktualnyStan"].Value = tmpIssue.Status.Name;
                    tmpRow.Cells["dgvAktualnyPriorytet"].Value = tmpIssue.Priority.Name;
                    tmpRow.Cells["dgvOstatniaAkcja"].Value = tmpIssue.Updated;

                    tmpRow.DefaultCellStyle.BackColor = Color.FromArgb(192, 192, 192);
                    tmpRow.Tag = (Color)tmpRow.DefaultCellStyle.BackColor;

                    tmpRow.Cells["dgvAktualniePrzydzielony"].Value = (!tmpRow.Cells[1].Value.ToString().Equals(tmpIssue.Key.Value) ? string.Format("{0} -> {1}", tmpIssue.Key, tmpIssue.Assignee) : tmpIssue.Assignee);

                    tmpRow.Cells["dgvOstatniaAkcja"].Value = tmpIssue.Updated.Value.ToLocalTime().ToString();//dgvOstatniaAkcja
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.LogError(ex, Logger.Instance, false);
            }

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

        }

        //[MTAThread]
        [STAThread]
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
                    if (isNullObjectOrEmptyString(tmpRow.Cells[1].Value))
                        return;

                    tmpIssue = zgloszeniaWjira.FirstOrDefault(x => x.Key == tmpRow.Cells[1].ToString());
                    if(isNullObjectOrEmptyString(tmpIssue))
                    {
                        Issue tmpIssueVar = null;
                        this.Invoke((MethodInvoker)delegate
                        {
                            tmpIssueVar = jira.RestClient.GetIssueAsync(tmpRow.Cells[1].Value.ToString(), CancellationToken.None).Result;
                        });
                        tmpIssue = tmpIssueVar;
                    }


                    tmpRow.Cells["dgvAktualnyStan"].Value = tmpIssue.Status.Name;
                    tmpRow.Cells["dgvAktualnyPriorytet"].Value = tmpIssue.Priority.Name;
                    tmpRow.Cells["dgvOstatniaAkcja"].Value = tmpIssue.Updated;

                    tmpRow.DefaultCellStyle.BackColor = Color.FromArgb(192, 192, 192);
                    tmpRow.Tag = (Color)tmpRow.DefaultCellStyle.BackColor;

                    tmpRow.Cells["dgvAktualniePrzydzielony"].Value = (!tmpRow.Cells[1].Value.ToString().Equals(tmpIssue.Key.Value) ? string.Format("{0} -> {1}", tmpIssue.Key, tmpIssue.Assignee) : tmpIssue.Assignee);

                    tmpRow.Cells["dgvOstatniaAkcja"].Value = tmpIssue.Updated.Value.ToLocalTime().ToString();//dgvOstatniaAkcja
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.LogError(ex, Logger.Instance, false);
            }

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

            e.Result = tmpRow;
        }

        [Obsolete("aktualnie nie używane")]
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

        void slaAutoStep(DataGridViewRow tmpRow, string issueNumber)
        {
            try
            {
                if (!isNullObjectOrEmptyString(issueNumber))
                {

                    Issue itmp = zgloszeniaWjira.FirstOrDefault(x => x.Key == issueNumber);
                    int issueId = Convert.ToInt32(tmpRow.Cells["dgvIssueId"].Value);

                    if (isNullObjectOrEmptyString(itmp))
                    {

                        this.Invoke((MethodInvoker)delegate
                        {
                            itmp = jira.RestClient.GetIssueAsync(tmpRow.Cells[1].Value.ToString(), CancellationToken.None).Result;
                        });

                    }

                    if (isNullObjectOrEmptyString(itmp)
                        //|| !isNullObjectOrEmptyString(tmpRow.Cells["dgvOdpowiedzialny"].Value)
                        || isNullObjectOrEmptyString(tmpRow.Cells["dgvAkcjaBPM"].Value)
                       )
                    {
                        return;
                    }

                    List<BillingIssueDtoHelios> issue = new List<BillingIssueDtoHelios>();
                    //addIssueToTreeNode(issueNumber, issue); ///autostem sla
                    addIssueToTreeNodeForAutoStep(issueNumber, issueId); ///autostem sla

                    string _bpmLastAction = tmpRow.Cells["dgvAkcjaBPM"].Value.ToString();

                    
                    if (
                        checkBillUser(itmp.Assignee, LoginParamType.Login) &&
                        _bpmLastAction.Contains("Utworzenie zgłoszenia") &&
                         isNullObjectOrEmptyString(tmpRow.Cells["dgvOdpowiedzialny"].Value))
                    {
                        /*
                        BillingIssueDto bid;

                        if (!selectIssueList.TryGetValue(issueNumber, out bid))
                        {
                            ExceptionManager.LogWarning(string.Format("selectIssueList nie zawiera {0}", issueNumber), Logger.Instance);
                            return;
                        }


                        tmpRow.Selected = true;

                        this.Tag = (bool)true;


                        UserBpmJira ubj = getUserBpmJira(itmp.Assignee);
                        if (true)
                        {
                            this.Invoke((MethodInvoker)delegate
                            {
                                List<EventParamModeler> eventParamForFormByEventMove = gujaczWFS.GetEventParamForFormByEventMove(614);

                                WFSModelerForm wmfw = new WFSModelerForm(eventParamForFormByEventMove, "Rozpoczęcie diagnozy", bid, gujaczWFS, 614, new WFSModelerForm.calbackDelegate(ModelerForm_sla_ActionFinish), treeView4, true, new KeyValuePair<int, string>(ubj.UserBpm.Id, ubj.UserBpm.FullName));

                            });
                        }

                        tmpRow.Selected = false;
                        this.Tag = null;
                        */
                        UserBpmJira ubjt = getUserBpmJira(itmp.Assignee);
                        goActionForAutomat("Rozpoczęcie diagnozy", issueNumber, 614, new KeyValuePair<int, string>(ubjt.UserBpm.Id, ubjt.UserBpm.FullName), null);
                        //break;
                    }

                    else if ((_bpmLastAction.Equals("Rozpoczęcie diagnozy") ||
                               _bpmLastAction.Equals("Zmiana wykonawcy"))
                            && (itmp.Status.Name == "Odrzucone" ||
                                itmp.Status.Name == "Odrzucony" ||
                                itmp.Status.Name == "Zadanie do wycofania"))
                    {
                        UserBpmJira ubj = getUserBpmJira(itmp.Assignee);

                        goActionForAutomat("Zamknięcie zgłoszenia", issueNumber, 617, ubj, ubj.GetType());
                        /*
                        BillingIssueDto bid;

                        if (!selectIssueList.TryGetValue(issueNumber, out bid))
                        {
                            ExceptionManager.LogWarning(string.Format("selectIssueList nie zawiera {0}", issueNumber), Logger.Instance);
                            return;
                        }


                        tmpRow.Selected = true;

                        this.Tag = (bool)true;


                        UserBpmJira ubj = getUserBpmJira(itmp.Assignee);
                        if (true)
                        {
                            this.Invoke((MethodInvoker)delegate
                            {
                                List<EventParamModeler> eventParamForFormByEventMove = gujaczWFS.GetEventParamForFormByEventMove(617);

                                WFSModelerForm wmfw = new WFSModelerForm(eventParamForFormByEventMove, "Zamknięcie zgłoszenia", bid, gujaczWFS, 617, new WFSModelerForm.calbackDelegate(ModelerForm_sla_ActionFinish), treeView4, true, new KeyValuePair<int, string>(401,"Zgłoszenie odrzucone"));

                            });
                        }

                        tmpRow.Selected = false;
                        this.Tag = null;--*/


                    }
                    else
                    {
                        foreach (var issueHistory in itmp.GetChangeLogs())
                        {
                            UserBpmJira ubj = getUserBpmJira(issueHistory.Author.Username.ToString());
                            if (!isNullObjectOrEmptyString(ubj))
                            {
                                foreach (var item in issueHistory.Items)
                                {
                                    if (
                                        (
                                            (item.FieldName == "assigne" && item.ToId == ubj.UserJira.login)
                                            || 
                                            ( issueHistory.Author.Username.ToString() == ubj.UserJira.login)
                                        )
                                        && _bpmLastAction.Contains("Utworzenie zgłoszenia") 
                                        && isNullObjectOrEmptyString(tmpRow.Cells["dgvOdpowiedzialny"].Value)
                                    )
                                    {
                                        System.Diagnostics.Debug.Write("historia assigne");

                                        /*BillingIssueDto bid;

                                        if (!selectIssueList.TryGetValue(issueNumber, out bid))
                                        {
                                            ExceptionManager.LogWarning(string.Format("selectIssueList nie zawiera {0}", issueNumber), Logger.Instance);
                                            return;
                                        }
                                        tmpRow.Selected = true;

                                        this.Tag = (bool)true;


                                        if (true)
                                        {
                                            this.Invoke((MethodInvoker)delegate
                                            {
                                                List<EventParamModeler> eventParamForFormByEventMove = gujaczWFS.GetEventParamForFormByEventMove(614);

                                                WFSModelerForm wmfw = new WFSModelerForm(eventParamForFormByEventMove, "Rozpoczęcie diagnozy", bid, gujaczWFS, 614, new WFSModelerForm.calbackDelegate(ModelerForm_sla_ActionFinish), treeView4, true, new KeyValuePair<int, string>(ubj.UserBpm.Id, ubj.UserBpm.FullName));

                                            });
                                        }

                                        tmpRow.Selected = false;
                                        this.Tag = null;
                                        */

                                        goActionForAutomat("Rozpoczęcie diagnozy", issueNumber, 614, new KeyValuePair<int, string>(ubj.UserBpm.Id, ubj.UserBpm.FullName), null);
                                        break;
                                    }
                                    else if (item.FieldName == "assigne"
                                        && item.ToId == ubj.UserJira.login
                                        && _bpmLastAction.Contains("Utworzenie zgłoszenia")
                                        && isNullObjectOrEmptyString(tmpRow.Cells["dgvOdpowiedzialny"].Value)
                                    )
                                    {

                                    }
                                }
                               
                            }
                        }   
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.LogError(ex, Logger.Instance, false);

            }
        }
        /// <summary>
        /// tu du
        /// </summary>
        /// <param name="stepName"></param>
        /// <param name="issueNumber"></param>
        /// <param name="EventMoveId"></param>
        /// <param name="param"></param>
        public void goActionForAutomat(string stepName, string issueNumber, int EventMoveId, object param, Type type)
        {
            BillingIssueDto bid;

            if (!selectIssueList.TryGetValue(issueNumber, out bid))
            {
                ExceptionManager.LogWarning(string.Format("selectIssueList nie zawiera {0}", issueNumber), Logger.Instance);
                return;
            }
 
            this.Tag = (bool)true;

            this.Invoke((MethodInvoker)delegate
            {
                List<EventParamModeler> eventParamForFormByEventMove = gujaczWFS.GetEventParamForFormByEventMove(EventMoveId);

                WFSModelerForm wmfw = new WFSModelerForm(eventParamForFormByEventMove, stepName, bid, gujaczWFS, EventMoveId, new WFSModelerForm.calbackDelegate(ModelerForm_sla_ActionFinish), treeView4, true, param);

            });
        }

        [STAThread]
        private void slaAutoAssigneTakenIssue(object sender, DoWorkEventArgs e)
        {
            string issueNumber = null;
            DataGridViewRow tmpRow = e.Argument as DataGridViewRow;
            if (isNullObjectOrEmptyString(tmpRow))
                return;

            issueNumber = tmpRow.Cells["dgvJiraNr"].Value == null ? string.Empty : tmpRow.Cells["dgvJiraNr"].Value.ToString();

            slaAutoStep(tmpRow, issueNumber);


        }

        [STAThread]
        private void slaAutoAssigneTakenIssue(object sender, RunWorkerCompletedEventArgs e)
        { 
            string issueNumber = null;
            DataGridViewRow tmpRow = e.Result as DataGridViewRow;
            if (isNullObjectOrEmptyString(tmpRow))
                return;

            issueNumber = tmpRow.Cells["dgvJiraNr"].Value == null ? string.Empty : tmpRow.Cells["dgvJiraNr"].Value.ToString();

            slaAutoStep(tmpRow, issueNumber);


        }

        private void slaAutoAssigneTakenIssue(DataGridViewRow tmpRow)
        {
            string issueNumber = null;
            if (isNullObjectOrEmptyString(tmpRow))
                return;

            issueNumber = tmpRow.Cells["dgvJiraNr"].Value == null ? string.Empty : tmpRow.Cells["dgvJiraNr"].Value.ToString();

            slaAutoStep(tmpRow, issueNumber);


        }

        private void dgv_SlaRaportCellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 1)
                {

                    numerZgl.Text = dgv_SlaRaport.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    System.Diagnostics.Process.Start("https://jira.polsatc/browse/" + numerZgl.Text);
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


        [STAThread]
        private void cb_SLApauza_CheckedChanged(object sender, EventArgs e)
        {
            //foreach (DataGridViewRow row in dgv_SlaRaport.Rows)
            //{
            //    if(row.Index < dgv_SlaRaport.RowCount-1)
            //     row.Visible = false;
            //    //else
            //    //    MessageBox.Show("Test");
            //}
                CheckBox tmp = (CheckBox)sender;
            try
            {
                if (tmp.Name.Equals("cb_SLApauza") && tmp.Checked)
                {
                    cb_SLAWstrzymane.CheckState = CheckState.Unchecked;
                    //foreach (DataGridViewRow row in dgv_SlaRaport.Rows)
                    //{

                    //    if (row.Cells["dgvPauza"].ToString().Contains("0"))
                    //    {
                    //        row.Visible = true;
                    //    }
                    //    //else
                    //    //{
                    //    //    row.Visible = false;
                    //    //}
                        
                    //}

                }
                else if (tmp.Name.Equals("cb_SLAWstrzymane") && tmp.Checked)
                {
                    cb_SLApauza.CheckState = CheckState.Unchecked;

                    //foreach (DataGridViewRow row in dgv_SlaRaport.Rows)
                    //{
                    //    if (row.Cells["dgvPauza"].ToString().Contains("1"))
                    //    {
                    //        row.Visible = true;
                    //    }
                    //    //else
                    //    //{
                    //    //    row.Visible = false;
                    //    //}
                    //}
                }
            }
            catch (Exception ex)
            { }
            //dgv_SlaRaport.Refresh();

            doByWorker(new DoWorkEventHandler(btn_slaRaport_Load_Click), null, new RunWorkerCompletedEventHandler(SlaReportCompleted));
            //if (!WorkingSla)
            //    btn_slaRaport_Load_Click(this, null);
        }


        /// <summary>
        /// Dodanie menu kontekstowego do raportu SLA
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 

        //[STAThread]
        private void dgv_SlaRaport_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            const string treeViewName = "treeView4";
            if (e.Button == MouseButtons.Right && e.RowIndex>-1)
            {
                issues[treeViewName].Clear();
                for (int i = 0; i < dgv_SlaRaport.RowCount - 1; i++)
                {
                    dgv_SlaRaport.Rows[i].Selected = false;
                    dgv_SlaRaport.Rows[i].DefaultCellStyle.BackColor = (Color)dgv_SlaRaport.Rows[i].Tag;
                }

                dgv_SlaRaport.Rows[e.RowIndex].Selected = true;

                string issueNumber = dgv_SlaRaport.Rows[e.RowIndex].Cells["dgvJiraNr"].Value.ToString();
                cms_IssuePopup.Items.Clear();


                List<BillingIssueDtoHelios> issue = new List<BillingIssueDtoHelios>();
                addIssueToTreeNode(issueNumber, issue, treeViewName);


                if (!wfsList.ContainsKey(treeViewName))
                {
                    wfsList[treeViewName] = new List<BillingIssueDtoHelios>();
                }

                wfsList[treeViewName] = gujaczWFS.compareBillingWithWFS(issue);

                BillingIssueDtoHelios item = wfsList[treeViewName][0];


                BillingIssueDtoHelios updatedIssue = gujaczWFS.UpdateIssue(item);

                IssueState state = (item.isInWFS) ? IssueState.INWFS : IssueState.NEW;

                //issues["treeView4"].Add(updatedIssue, state);


                KeyValuePair<BillingIssueDto, IssueState> tmp = issues[treeViewName].Where(x =>
                {
                    if (x.Key.JiraKey == item.JiraKey)
                        return true;
                    else
                        return false;
                }).FirstOrDefault();


                selectIssue = tmp.Key;
                if (!selectIssueList.Any(x => x.Key == issueNumber))
                    selectIssueList.Add(issueNumber, tmp.Key);
                //Issue jiraIssue = jira.GetIssue(issueNumber);
                //BillingIssueDtoHelios issueDtoHelio = issue[0];
                ////jiraIssue.Add(jira.GetIssue(issueNumber));

                //KeyValuePair<BillingIssueDto, IssueState> tmp = issues["treeView4"].Where(x =>
                //{
                //    if (x.Key.JiraKey == issueNumber)
                //        return true;
                //    else
                //        return false;
                //}).FirstOrDefault();


                ////selectIssue = tmp.Key;

                //Logic.Implementation.JiraIssues jIssues = new Logic.Implementation.JiraIssues(this.jiraUser.Login, this.jiraUser.Password, "https://jira.polsatc");

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

        private void dgv_SlaRaport_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (isNullObjectOrEmptyString(e.Value))
                    return;
                if (isNullObjectOrEmptyString(zgloszeniaWjira))
                    return;
                if (e.ColumnIndex == 1 && e.RowIndex > -1)
                {
                    Issue i = zgloszeniaWjira.FirstOrDefault(x => x.Key == e.Value.ToString());
                    if (isNullObjectOrEmptyString(i))
                        return;
                    dgv_SlaRaport[1, e.RowIndex].ToolTipText = i.Summary;

                }
                else if (dgv_SlaRaport.Columns[e.ColumnIndex].Name == "dgvPozostaloMin" && e.RowIndex > -1) // pozostałe sla
                {
                    int minute = Convert.ToInt32(dgv_SlaRaport["dgvPozostaloMin", e.RowIndex].Value);
                    if (minute > 0)
                    {
                        DateTime ees = estimateEndSla(DateTime.Now, minute);
                        dgv_SlaRaport["dgvPozostaloMin", e.RowIndex].ToolTipText = string.Format("{0}h {1}min\n{2}", minute / 60, minute % 60,ees.ToString());
                    }
                }
            }
            catch(Exception ex)
            {

            }
            
        }

        TimeSpan time_16 = TimeSpan.Parse("16:00:00");
        TimeSpan time_08 = TimeSpan.Parse("08:00:00");

        private DateTime estimateEndSla(DateTime dt, int minute)
        {
            bool calculate = true;
            DateTime date = dt;
            while (calculate)
            {
                int addDays = minute / 480;//ile dni dodatkowo
                int addMin = minute % 480;//ile min dodatkowo

                date = date.AddDays(addDays).AddMinutes(addMin);

                switch (date.DayOfWeek)
                {
                    case DayOfWeek.Sunday:
                        date = date.AddDays(1);
                        break;
                    case DayOfWeek.Saturday:
                        date = date.AddDays(2);
                        break;
                    default:
                        break;
                }

                if (date.Hour >= 16)
                {
                    minute = (int)(date.TimeOfDay - time_16).TotalMinutes;
                    date = date.AddDays(1);
                    DateTime tmpDateTime = new DateTime(
                                                          date.Year
                                                        , date.Month
                                                        , date.Day
                                                        , 8
                                                        , 0
                                                        , 0);



                    date = tmpDateTime;
                }
                else if (date.Hour <= 8)
                {
                    minute = (int)(time_08 - date.TimeOfDay).TotalMinutes;
                    DateTime tmpDateTime = new DateTime(
                                                          date.Year
                                                        , date.Month
                                                        , date.Day
                                                        , 8
                                                        , 0
                                                        , 0);

                    date = tmpDateTime;
                    date = date.AddMinutes(minute);
                }
                else
                    calculate = false;
                if (minute < 1) break;

            }
            //int weeks =
             int weeks = (int)(date - dt).TotalDays/5;

            date = date.AddDays(weeks * 2);

            switch (date.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    date = date.AddDays(1);
                    break;
                case DayOfWeek.Monday:
                    break;
                case DayOfWeek.Tuesday:
                    break;
                case DayOfWeek.Wednesday:
                    break;
                case DayOfWeek.Thursday:
                    break;
                case DayOfWeek.Friday:
                    break;
                case DayOfWeek.Saturday:
                    date = date.AddDays(2);
                    break;
                default:
                    break;
            }

            return date;
        }

        private DateTime estimateEndSla2(DateTime dt, int minute)
        {
            int addDays = minute / 480;//ile dni dodatkowo
            int addMin = minute % 480;//ile dni dodatkowo

            DateTime date = dt.AddMinutes(addMin);
            if(date.Hour >= 16 )
            {
                addDays += 1;
                addMin += (int)(date.TimeOfDay - time_16).TotalMinutes;
            }


            

            return dt.AddMinutes(addMin).AddDays(addDays);
        }

        private string fnGetStringFromList(List<string> list, char pos)
        {
            string returnValue = string.Empty;

            foreach (string item in list)
            {
                returnValue += item + pos;
            }
            
            return returnValue.Remove(returnValue.Length - 1); ;
        }

        private bool updateTreeNodeForSLA(DataGridViewRow row)
        {
           string nrJira = row.Cells["dgvJiraNr"].Value.ToString();
            string status = row.Cells["dgvAkcjaBPM"].Value.ToString();
            string pozostalyCzas = row.Cells["dgvPozostaloMin"].Value.ToString();
            bool returnValue = false;

            foreach (TreeNode item in treeView1.Nodes)
            {
                if (item.Text.Split(' ')[0] == nrJira)
                {
                    if (!item.Text.Contains("]"))
                    {
                        item.Text += string.Concat(" [", pozostalyCzas, ']');
                        return true;
                    }

                    break;
                }
            }



            if (!returnValue)
            {
                foreach (TreeNode item in treeView2.Nodes)
                {
                    string s = item.Text.Split(' ')[0];
                    if (item.Text.Split(' ')[0] == nrJira)
                    {
                        if (!item.Text.Contains("]"))
                        {
                            item.Text += string.Concat(" [", pozostalyCzas, ']');
                            return true;
                        }
                    }
                    break;
                }
            }
            nrJira = null;
            return returnValue;
        }

        private List<string> getActualSLAforIssueId(int issueId)
        {
            return gujaczWFS.ExecuteStoredProcedure("cp_sla_raport_v2", new string[] { issueId.ToString() }, DatabaseName.SupportCP)[0];
        }

        private void setActualSLAforIssueId(int issueId, bool isFastStep)
        {
            List<string> slaParam = getActualSLAforIssueId(issueId);
            foreach (DataGridViewRow item in dgv_SlaRaport.Rows)
            {
                if(!isNullObjectOrEmptyString(item.Cells["dgvIssueId"].Value) && item.Cells["dgvIssueId"].Value.ToString() == issueId.ToString())
                {
                    //IssueId, nrJira, Odpowiedzialny, TypZgloszenia, Priorytet, Pauza, AktCzasRealizacji, CzasRozwiazania, PozostaloMin, OstatniaAkcja

                    /*
                            dgvSlaTmp.Rows[0].Cells["dgvIssueId"].Value = row[0];  //dgvIssueId
                            dgvSlaTmp.Rows[0].Cells["dgvJiraNr"].Value = row[1];  //dgvJiraNr
                            dgvSlaTmp.Rows[0].Cells["dgvOdpowiedzialny"].Value = row[2];  //dgvOdpowiedzialny
                            dgvSlaTmp.Rows[0].Cells["dgvTypZgloszenia"].Value = row[3];  //dgvTypZgloszenia
                            dgvSlaTmp.Rows[0].Cells["dgvPriorytet"].Value = row[4];  //dgvPriorytet
                            dgvSlaTmp.Rows[0].Cells["dgvPauza"].Value = row[5];  //dgvPauza
                            dgvSlaTmp.Rows[0].Cells["dgvAktCzasRealizacji"].Value = row[6];  //dgvAktCzasRealizacji
                            dgvSlaTmp.Rows[0].Cells["dgvCzasRozwiazania"].Value = row[7];  //dgvCzasRozwiazania 
                            dgvSlaTmp.Rows[0].Cells["dgvPozostaloMin"].Value = row[8];  //dgvPozostaloMin  

                            dgvSlaTmp.Rows[0].Cells["dgvAkcjaBPM"].Value = row[9];  //dgvOstatniaAkcja  
                     */

                    item.Cells["dgvOdpowiedzialny"].Value = slaParam[2];
                    item.Cells["dgvTypZgloszenia"].Value = slaParam[3];
                    item.Cells["dgvPriorytet"].Value = slaParam[4];

                    item.Cells["dgvPauza"].Value = slaParam[5]; 
                    item.Cells["dgvAktCzasRealizacji"].Value = slaParam[6];  
                    item.Cells["dgvCzasRozwiazania"].Value = slaParam[7]; 

                    item.Cells["dgvPozostaloMin"].Value = slaParam[8];
                    item.Cells["dgvAkcjaBPM"].Value = string.Concat(slaParam[9], isFastStep?" (AutoStep)":"");

                    break;
                }
            }

        }

    }
}
