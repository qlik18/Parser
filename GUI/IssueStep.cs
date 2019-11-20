using Entities;
using Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utility;

namespace GUI
{
    public partial class MainForm
    {

        private void issueStep_RozpocznijDiagnoze(object sender, string issueId)
        {
            this.Invoke((MethodInvoker)delegate
            {
                const int eventMoveId = 614;

                List<BillingIssueDtoHelios> issue = new List<BillingIssueDtoHelios>();
                addIssueToTreeNode(issueId, issue);

                //Pobranie Rozmieszczenia parametrów zdarzenia
                List<EventParamModeler> eventParamForFormByEventMove = gujaczWFS.GetEventParamForFormByEventMove(eventMoveId);
                KeyValuePair<int, string> selectOption = (KeyValuePair<int, string>)sender;
                WFSModelerForm wmfw = new WFSModelerForm(eventParamForFormByEventMove, "Rozpoczęcie diagnozy - fast", selectIssue, gujaczWFS, eventMoveId, new WFSModelerForm.calbackDelegate(ModelerForm_sla_ActionFinishAfterAutoStep), treeView4, true, selectOption);
            });
        }

        private void issueStep_WeryfikacjaRealizacji(object sender, string issueId)
        {
            this.Invoke((MethodInvoker)delegate
            {
                const int eventMoveId = 616;

                List<BillingIssueDtoHelios> issue = new List<BillingIssueDtoHelios>();
                addIssueToTreeNode(issueId, issue);

                //Pobranie Rozmieszczenia parametrów zdarzenia
                List<EventParamModeler> eventParamForFormByEventMove = gujaczWFS.GetEventParamForFormByEventMove(eventMoveId);

                WFSModelerForm wmfw = new WFSModelerForm(eventParamForFormByEventMove, "Weryfikacja Realizacji - fast", selectIssue, gujaczWFS, eventMoveId, new WFSModelerForm.calbackDelegate(ModelerForm_sla_ActionFinishAfterAutoStep), treeView4, true, null);
            });
        }
        private void issueStep_ZamknieciePoRealizacji(object sender, string issueId)
        {
            this.Invoke((MethodInvoker)delegate
            {
                const int eventMoveId = 618;

                List<BillingIssueDtoHelios> issue = new List<BillingIssueDtoHelios>();
                addIssueToTreeNode(issueId, issue);

                //Pobranie Rozmieszczenia parametrów zdarzenia
                List<EventParamModeler> eventParamForFormByEventMove = gujaczWFS.GetEventParamForFormByEventMove(eventMoveId);

                WFSModelerForm wmfw = new WFSModelerForm(eventParamForFormByEventMove, "Zamknięcie zgłoszenia - fast", selectIssue, gujaczWFS, eventMoveId, new WFSModelerForm.calbackDelegate(ModelerForm_sla_ActionFinishAfterAutoStep), treeView4, true, null);
            });
        }
        private void issueStep_OdrzucenieZgloszenia(object sender, string issueId)
        {
            this.Invoke((MethodInvoker)delegate
            {
                const int eventMoveId = 617;

                List<BillingIssueDtoHelios> issue = new List<BillingIssueDtoHelios>();
                addIssueToTreeNode(issueId, issue);

                //Pobranie Rozmieszczenia parametrów zdarzenia
                List<EventParamModeler> eventParamForFormByEventMove = gujaczWFS.GetEventParamForFormByEventMove(eventMoveId);

                KeyValuePair<int, string> selectOption = new KeyValuePair<int, string>(401, "Zgłoszenie odrzucone");

                WFSModelerForm wmfw = new WFSModelerForm(eventParamForFormByEventMove, "Odrzucenie zgłoszenia - fast", selectIssue, gujaczWFS, eventMoveId, new WFSModelerForm.calbackDelegate(ModelerForm_sla_ActionFinishAfterAutoStep), treeView4, true, selectOption);
            });
        }
        private void issueStep_ZmianaKataloguJira(object sender, string issueId)
        {
            this.Invoke((MethodInvoker)delegate
            {
                const int eventMoveId = 617;

                List<BillingIssueDtoHelios> issue = new List<BillingIssueDtoHelios>();
                addIssueToTreeNode(issueId, issue);

                //Pobranie Rozmieszczenia parametrów zdarzenia
                List<EventParamModeler> eventParamForFormByEventMove = gujaczWFS.GetEventParamForFormByEventMove(eventMoveId);

                KeyValuePair<int, string> selectOption = new KeyValuePair<int, string>(673, "Zmiana katalogu w Jira");

                WFSModelerForm wmfw = new WFSModelerForm(eventParamForFormByEventMove, "Zmiana Obszaru - fast", selectIssue, gujaczWFS, eventMoveId, new WFSModelerForm.calbackDelegate(ModelerForm_sla_ActionFinishAfterAutoStep), treeView4, true, selectOption);
            });
        }

        private void issueStep_ZmianaWykonawcy(object sender, string issueId)
        {
            this.Invoke((MethodInvoker)delegate
            {
                const int eventMoveId = 621;

                List<BillingIssueDtoHelios> issue = new List<BillingIssueDtoHelios>();
                addIssueToTreeNode(issueId, issue);

                //Pobranie Rozmieszczenia parametrów zdarzenia
                List<EventParamModeler> eventParamForFormByEventMove = gujaczWFS.GetEventParamForFormByEventMove(eventMoveId);

                KeyValuePair<int, string> selectOption = (KeyValuePair<int, string>)sender;


                WFSModelerForm wmfw = new WFSModelerForm(eventParamForFormByEventMove, "Zmiana wykonawcy - fast", selectIssue, gujaczWFS, eventMoveId, new WFSModelerForm.calbackDelegate(ModelerForm_sla_ActionFinishAfterAutoStep), treeView4, true, selectOption);
            });
        }
        private void btn_issueStep_RealizujIZamknij_Click(object sender, EventArgs e)
        {
            const int eventMoveId = 612;
            //List<BillingIssueDtoHelios> issue = new List<BillingIssueDtoHelios>();
            //addIssueToTreeNode(issueId, issue);


            ToolStripMenuItem tmp = (ToolStripMenuItem)sender; //losowy komponet do przechowywania Tag

            // BillingIssueDto selectIssue = null;
            KeyValuePair<int, string> item = new KeyValuePair<int, string>();
            KeyValuePair<int, string> selectOption = new KeyValuePair<int, string>();

            List<object> tagTmp = new List<object>();

            if (tmp.Tag != null && tmp.Tag.GetType() == typeof(List<object>))
            {
                tagTmp = (List<object>)tmp.Tag;

                selectIssue = (BillingIssueDto)tagTmp[0];
                item = (KeyValuePair<int, string>)tagTmp[1];
                selectOption = (KeyValuePair<int, string>)tagTmp[2];
            }

            if (tmp.Tag != null && tmp.Tag.GetType() == typeof(IssueFinalParameters))
            {
                IssueFinalParameters par = ((IssueFinalParameters)tmp.Tag);
                // tagTmp = (List<object>)tmp.Tag;
                selectIssue = par.issueNumber;
                item = par.item;
                selectOption = par.selectOption;
            }
            try
            {
                //1. BillingIssueDto            selectIssue
                //2. Dictionary<int, string>    item
                //3. KeyValuePair<int, string>  s

                this.Invoke((MethodInvoker)delegate
                {
                    if (item.Key == 610)
                    {
                        string[] paramString = (string[])tagTmp[3];
                        new WFSModelerForm(gujaczWFS.GetEventParamForFormByEventMove(item.Key), item.Value, selectIssue, gujaczWFS, item.Key, new WFSModelerForm.calbackDelegate(ModelerForm_sla_ActionFinish), treeView4, true, paramString);
                    }
                    else
                    {
                        List<EventParamModeler> eventParamForFormByEventMove = gujaczWFS.GetEventParamForFormByEventMove(eventMoveId);
                        WFSModelerForm wmfw = new WFSModelerForm(eventParamForFormByEventMove, "Przyjęcie do realizacji - fast", selectIssue, gujaczWFS, eventMoveId, new WFSModelerForm.calbackDelegate(ModelerForm_sla_ActionFinishAfterAutoStep), treeView4, false, selectOption);
                        wmfw.ShowDialog();
                    }
                });


                this.Invoke((MethodInvoker)delegate
                {
                    issueStep_WeryfikacjaRealizacji(this, selectIssue.JiraKey);
                });

                this.Invoke((MethodInvoker)delegate
                {
                    issueStep_ZamknieciePoRealizacji(this, selectIssue.JiraKey);
                });

            }
            catch (Exception ex)
            {
                ExceptionManager.LogWarning(ex.Message, Logger.Instance);
            }
        }

    }
}
