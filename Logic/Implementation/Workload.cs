using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;
using LogicLayer.Implementation;
using LogicLayer.Interface;
using Entities.Enums;

namespace Logic.Implementation
{
    public enum WorkloadStatus
    {
        Start = 0,
        Pause,
        Stop,
        OtherUser
    }

    public class Workload
    {
        private static readonly Workload instance = new Workload();
        private IParserEngineWFS gujacz;
        private TimeSpan loggedTime;
        private TimeSpan totalTime;
        private int issueId;
        private DateTime startTime;
        private TimeSpan lastTime;
        private WorkloadStatus status;

        public static Workload Instance
        {
            get { return instance; }
        }

        private BillingIssueDto issue;
        private bool isLogging;

        public BillingIssueDto Issue
        {
            get { return issue; }
            set
            {
                issueId = -1;

                if (value.issueWFS.WFSIssueId == 0)
                {
                    issue = null;
                }
                else
                {
                    issue = value;
                    GetLoggingTime();
                }
            }
        }

        public bool IsLogging
        {
            get { return isLogging; }
            private set { isLogging = value; }
        }

        public TimeSpan LoggedTime
        {
            get { return loggedTime; }
        }

        public DateTime StartTime
        {
            get { return startTime; }
        }

        public TimeSpan TotalTime
        {
            get { return totalTime; }
        }

        public TimeSpan LastTime
        {
            get { return lastTime; }
        }

        public WorkloadStatus Status
        {
            get { return status; }
        }

        public Workload()
        {
            issueId = -1;
        }

        public Workload(IParserEngineWFS gujacz)
            : this()
        {
            this.gujacz = gujacz;
            CheckIfUserLoggingRunning();
        }

        private void GetLoggingTime()
        {
            if (gujacz == null)
                return;

            if (issue == null)
                return;

            if (issue.issueWFS.WFSIssueId != 0)
            {
                List<List<string>> result = gujacz.ExecuteStoredProcedure("CP_WLGetLoggedTimeForIssue", new string[] { issue.issueWFS.WFSIssueId.ToString() }, DatabaseName.SupportCP);

                int seconds = 0;

                if (result.Count > 0)
                {
                    string akcja = result[0][1];
                    switch (akcja)
                    {
                        case "Start": this.status = WorkloadStatus.Start; break;
                        case "Pauza": this.status = WorkloadStatus.Pause; break;
                        case "Stop": this.status = WorkloadStatus.Stop; break;
                    }


                    if (akcja.Equals("start", StringComparison.CurrentCultureIgnoreCase) && string.IsNullOrEmpty(result[0][2]))
                    {
                        this.status = WorkloadStatus.OtherUser;
                        this.lastTime = TimeSpan.FromSeconds(0);
                        this.loggedTime = TimeSpan.FromSeconds(0);
                        this.totalTime = TimeSpan.FromSeconds(0);
                        return;
                    }

                    if (!akcja.Equals("stop", StringComparison.CurrentCultureIgnoreCase))
                    {
                        seconds = Convert.ToInt32(result[0][2]);
                    }


                    lastTime = TimeSpan.FromSeconds(Convert.ToInt32(result[0][2]));
                }
                else
                    this.status = WorkloadStatus.Stop;

                loggedTime = TimeSpan.FromSeconds(seconds);

                List<List<string>> total = gujacz.ExecuteStoredProcedure("CP_WLGetTotalTimeForIssue", new string[] { issue.issueWFS.WFSIssueId.ToString() }, DatabaseName.SupportCP);

                seconds = 0;

                if (total.Count > 0)
                {
                    seconds = Convert.ToInt32(total[0][1]);
                }

                totalTime = TimeSpan.FromSeconds(seconds);
            }
            else
                loggedTime = TimeSpan.FromSeconds(0);
        }

        public void StartLogging()
        {
            if (IsLogging)
                throw new InvalidOperationException("Aktualnie trwa rozpoczęte logowanie czasu dla zgłoszenia " + issue.Idnumber + "!");
            if (issue == null)
                throw new ArgumentNullException("Nie wybrano zgłoszenia!");

            // Wywołujemy prockę
            gujacz.ExecuteStoredProcedure("CP_WLNewAction", new string[] { issue.issueWFS.WFSIssueId.ToString(), gujacz.getUser().Id.ToString(), "1" }, DatabaseName.SupportCP);

            this.startTime = DateTime.Now;
            this.IsLogging = true;
        }

        public void PauseLogging()
        {
            if (!IsLogging)
                throw new InvalidOperationException("Aktualnie nie ma rozpoczętego żadnego logowania czasu!");

            int id = -1;

            if (this.issue != null)
                id = issue.issueWFS.WFSIssueId;
            else
                id = issueId;

            if (id == -1)
                throw new ArgumentNullException("Nie wybrano zgłoszenia!");

            // Wywołujemy prockę
            gujacz.ExecuteStoredProcedure("CP_WLNewAction", new string[] { id.ToString(), gujacz.getUser().Id.ToString(), "3" }, DatabaseName.SupportCP);
            this.IsLogging = false;
            GetLoggingTime();
        }

        public void StopLogging()
        {
            if (!IsLogging)
                throw new InvalidOperationException("Aktualnie nie ma rozpoczętego żadnego logowania czasu!");

            int id = -1;

            if (this.issue != null)
                id = issue.issueWFS.WFSIssueId;
            else
                id = issueId;

            if (id == -1)
                throw new ArgumentNullException("Nie wybrano zgłoszenia!");

            // Wywołujemy prockę
            gujacz.ExecuteStoredProcedure("CP_WLNewAction", new string[] { id.ToString(), gujacz.getUser().Id.ToString(), "2" }, DatabaseName.SupportCP);
            this.IsLogging = false;
            GetLoggingTime();
        }
        
        private void CheckIfUserLoggingRunning()
        {
            List<List<string>> result = gujacz.ExecuteStoredProcedure("CP_WLGetCurrentLoggingIssueForUser", new string[] { gujacz.getUser().Id.ToString() }, DatabaseName.SupportCP);
            if (result.Count > 0)
            {
                this.isLogging = true;

                BillingIssueDtoHelios iss = new BillingIssueDtoHelios();
                iss.issueWFS = new BillingDTHIssueWFS();
                iss.issueHelios = new IssueHelios();
                iss.issueWFS.WFSIssueId = Convert.ToInt32(result[0][0]);
                iss.issueHelios.number = result[0][1];

                this.issue = iss;

                GetLoggingTime();
            }
            else
                this.isLogging = false;
        }

        public List<List<string>> GetOpenIssues()
        {
            return gujacz.ExecuteStoredProcedure("CP_WLGetOpenIssues", new string[] { }, DatabaseName.SupportCP);
        }

        public List<List<string>> GetPausedIssues()
        {
            return gujacz.ExecuteStoredProcedure("CP_WLGetPausedIssuesForUser", new string[] { gujacz.getUser().Id.ToString() }, DatabaseName.SupportCP);
        }
    }
}
