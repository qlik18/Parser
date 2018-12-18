using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Logging;
using System.Threading;
using GUI;

namespace GUI.AutoAdd
{
    partial class AutoAddClass
    {
        //public void newIssueAutoAddToBPM(object sender, EventArgs e)
        //{
        //    UzupelnijDane(treeView1, true);
        //    ZapiszZgloszenia(treeView1, true);

        //    UzupelnijDane(treeView2, true);
        //    ZapiszZgloszenia(treeView2, true);
        //}

        //public void newIssueAutoAddToBPM()
        //{
        //    newIssueAutoAddToBPMUzupelnijDane();
        //    newIssueAutoAddToBPMZapiszZgloszenia();
        //    Logger.Instance.LogInformation(string.Format("Automatyczne sprawdzanie zgłoszeń zakończono o: {0} \n\n", DateTime.Now));


        //}

        //public void newIssueAutoAddToBPMUzupelnijDane()
        //{
        //    Thread t = new Thread((ThreadStart)delegate ()
        //    {
        //        try
        //        {
        //            UzupelnijDane(treeView1, true);
        //            UzupelnijDane(treeView2, true);
        //        }
        //        catch (Exception ex)
        //        {
        //            ExceptionManager.LogError(ex, Logger.Instance);
        //        }
        //    });


        //    t.Priority = ThreadPriority.AboveNormal;
        //    t.IsBackground = true;
        //    t.Start();

        //}
    }
}
