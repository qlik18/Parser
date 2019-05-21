using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LogicLayer.Interface;
using Entities;
using Entities.Enums;
using DataLayer.Interface;
using DataLayer.HPDataService;
using DataLayer.Implementation;
using Utility;

namespace LogicLayer.Implementation
{
    class ParserEngineWFS : IParserEngineWFS
    {

        public ParserEngineWFS()
        {
            // wfsDao = ServiceLocator.Instance.Retrieve<IGatewayDaoWFS>(); 
        }


        private User user = null;
        private IGatewayDaoWFS wfsDao = null;

        #region IParserEngineWFS Members

        /// <summary>
        /// Pobiera zalogowanego użytkownika
        /// </summary>
        /// <returns></returns>
        public User getUser()
        {
            return this.user;
        }

        /// <summary>
        /// Loguje użytkownika do systemu
        /// </summary>
        /// <param name="username">login użytkownika</param>
        /// <param name="password">hasło użytkownika</param>
        /// <returns></returns>
        public bool loginToWFSWithUserInfo(string username, string password)
        {
            User user = wfsDao.loginToWFSWithUserInfo(username, password);
            
            if (user != null)
            {
                wfsDao.setLoggedUser(user);
                this.setUser(user);
                return true;

            }
            else
                return false;
        }

        /// <summary>
        /// Przypisuje zalogowanego użytkownika
        /// </summary>
        /// <param name="user"></param>
        private void setUser(User user)
        {
            this.user = user;
        }

        /// <summary>
        /// Przypisuje interfejs warstwy danych
        /// </summary>
        /// <param name="dao"></param>
        public void setWFSDao(IGatewayDaoWFS dao)
        {
            this.wfsDao = dao;
        }
        
        /// <summary>
        /// Pobiera listę komponentow dla zadanego id komponentu-rodzica
        /// </summary>
        /// <param name="id">id komponentu-rodzica</param>
        /// <returns></returns>
        public Dictionary<int, string> getBillingComponents(int id)
        {
            return wfsDao.GetBillingComponents(this.user, id);
        }

        /// <summary>
        /// Tworzy nową sprawę w BPM
        /// </summary>
        /// <param name="issue">zgłoszenie Jira</param>
        /// <param name="WFSIssueId">nowa sprawa BPM</param>
        /// <returns></returns>
        public bool addBillingIssueToWFS(BillingIssueDtoHelios issue, out int WFSIssueId)
        {
            bool result = false;

            try
            {
                int tmp = this.wfsDao.AddBillingIssueToWFS(issue.issueWFS, this.user);
                if (tmp != -1)
                {
                    result = true;
                }
                WFSIssueId = tmp;
            }
            catch (MyCustomException ex)
            {
                throw new MyCustomException("Błąd przy zapisywaniu zgłoszenia!", ex);
            }

            return result;
        }

        /// <summary>
        /// Sprawdzenie czy zgłoszenia zostały już wprowadzone do BPM.
        /// </summary>
        /// <param name="issues">Lista zgłoszeń</param>
        /// <returns></returns>
        public List<BillingIssueDtoHelios> compareBillingWithWFS(List<BillingIssueDtoHelios> issues)
        {
            List<BillingIssueDtoHelios> returnList = new List<BillingIssueDtoHelios>();
            List<string> issuesNumber = issues.Select(x => x.Idnumber).ToList<string>();

            Dictionary<KeyValuePair<int, string>, string> list = wfsDao.CheckIssuesPresenceOnBpm(issues);
            //Dictionary<KeyValuePair<int, string>, string> list = wfsDao.CheckBillingIssuesPresenceOnWFS(issuesNumber.ToArray());

            for (int i = 0; i < issues.Count; i++)
            {
                BillingIssueDtoHelios id = issues[i];
                id.issueWFS = new BillingDTHIssueWFS();
                id.isInWFS = list.Any(x => x.Value.Contains(id.Idnumber) || x.Value.Contains(id.issueHelios.jiraIdentifier));
                if (id.isInWFS)
                {
                    KeyValuePair<KeyValuePair<int, string>, string> para = list.Where(x => x.Value.Contains(id.Idnumber) || x.Value.Contains(id.issueHelios.jiraIdentifier)).First();
                    id.issueWFS.WFSIssueId = para.Key.Key;
                    id.issueWFS.WFSState = para.Key.Value;
                }
                else id.issueWFS.WFSIssueId = 0;
            }
            return issues;
        }

        /// <summary>
        /// Sprawdzenie czy zgłoszenia zostały już wprowadzone do BPM.
        /// </summary>
        /// <param name="issues">Lista zgłoszeń</param>
        /// <returns></returns>
        //public BillingIssueDtoHelios GetIssueFromBPM(string issuesNumber)
        //{


        //    Dictionary<KeyValuePair<int, string>, string> list = wfsDao.CheckIssuesPresenceOnBpm(issues);
        //    //Dictionary<KeyValuePair<int, string>, string> list = wfsDao.CheckBillingIssuesPresenceOnWFS(issuesNumber.ToArray());

        //    for (int i = 0; i < issues.Count; i++)
        //    {
        //        BillingIssueDtoHelios id = issues[i];
        //        id.issueWFS = new BillingDTHIssueWFS();
        //        id.isInWFS = list.Any(x => x.Value.Contains(id.Idnumber) || x.Value.Contains(id.issueHelios.jiraIdentifier));
        //        if (id.isInWFS)
        //        {
        //            KeyValuePair<KeyValuePair<int, string>, string> para = list.Where(x => x.Value.Contains(id.Idnumber) || x.Value.Contains(id.issueHelios.jiraIdentifier)).First();
        //            id.issueWFS.WFSIssueId = para.Key.Key;
        //            id.issueWFS.WFSState = para.Key.Value;
        //        }
        //        else id.issueWFS.WFSIssueId = 0;
        //    }
        //    return issues;
        //}

        /// <summary>
        /// Sprawdzenie czy zgłoszenie zostało już wprowadzone do WFS
        /// </summary>
        /// <param name="issue"></param>
        /// <returns></returns>
        public bool isInWFS(IssueDtoHelios issue)
        {
            bool result = false;
            if (issue.isInWFS)
            {
                result = true;
            }

            return result;

        }//public bool isInWfsGUI

        /// <summary>
        /// Wykonuje akcję na sprawie
        /// </summary>
        /// <param name="idIssue">id sprawy</param>
        /// <param name="eventMoveId">id akcji</param>
        /// <param name="paramz">parametry akcji</param>
        /// <returns></returns>
        public int DoActionInIssue(int? IssueId, int EventMove, List<EventParam> paramz)
        {
            return wfsDao.DoActionInIssue(IssueId, EventMove, paramz, user);
        }
        #endregion

        /// <summary>
        /// Pobiera dostepne kroki dla danej sprawy BPM
        /// </summary>
        /// <param name="issueId">id sprawy BPM</param>
        /// <param name="userId">id użytkownika</param>
        /// <returns></returns>
        public Dictionary<int, string> GetActionForIssue(int issueId, int userId)
        {
            return wfsDao.GetActionForIssue(issueId, userId);
        }

        /// <summary>
        /// Wykonuje procedurę składowaną na bazie.
        /// </summary>
        /// <param name="procedureName">nazwa procedury</param>
        /// <param name="parameters">tablica parametrów</param>
        /// <param name="database">baza danych, na której ma zostać wykonana procedura.</param>
        /// <returns></returns>
        public List<List<string>> ExecuteStoredProcedure(string procedureName, string[] parameters, DatabaseName database = DatabaseName.None)
        {
            return wfsDao.ExecuteStoredProcedure(procedureName, parameters, database);
        }

        /// <summary>
        /// Pobiera parametry powiązane z danymi parametrami dla sprawy BPM
        /// </summary>
        /// <param name="issueId">id sprawy</param>
        /// <param name="eventParamId">id parametrów</param>
        /// <returns></returns>
        public List<Entities.EventParam> GetBoundEventParamForIssue(int issueId, int[] eventsParamId)
        {
            return wfsDao.GetBoundEventParamForIssue(issueId, eventsParamId);
        }

        /// <summary>
        /// Pobiera akutalne parametry sprawy, dla zadanych parametrów związanych
        /// </summary>
        /// <param name="issueId">id sprawy</param>
        /// <param name="eventParamId">id parametrów związanych</param>
        /// <returns></returns>
        public List<Entities.EventParam> GetBillingBoundEventParamForIssue(int issueId, int[] eventsParamId)
        {
            return wfsDao.GetBillingBoundEventParamForIssue(issueId, eventsParamId);
        }

        /// <summary>
        /// Zakańcza sesję użytkownika
        /// </summary>
        public void LogOutFromHPService()
        {
            wfsDao.LogOutFromHPService();
        }

        /// <summary>
        /// Powiadamia webService, że sesja powinna zostać podtrzymana, zapobiegając zakończeniu sesji po czasie.
        /// </summary>
        /// <returns></returns>
        public void notifyWebService()
        {
            wfsDao.notifyWebService();
        }

        /// <summary>
        /// Pobiera listę parametrów do modelowania formatki akcji
        /// </summary>
        /// <param name="eventMoveId">id akcji</param>
        /// <returns></returns>
        public List<EventParamModeler> GetEventParamForFormByEventMove(int eventMoveId)
        {
            return wfsDao.GetEventParamForFormByEventMove(eventMoveId);
        }

        /// <summary>
        /// Sprawdza czy dane zgłoszenie jest już archiwizowane
        /// </summary>
        /// <param name="issueId">id sprawdzanego zgłoszenia</param>
        /// <returns></returns>
        public bool IsIssueInArchive(int issueId)
        {
            //List<List<string>> result = this.ExecuteStoredProcedure("BillingDTH_IsIssueInArchive", new string[] { issueId.ToString() }, DatabaseName.SupportADDONS);

            List<List<string>> result = this.ExecuteStoredProcedure("sp_IsIssueInArchive", new string[] { issueId.ToString() }, DatabaseName.SupportCP);

            return result.Count > 0 ? true : false;
        }

        /// <summary>
        /// Pobiera zgłoszenia z BPM (z pominięciem Jira), przypisane do danego użytkownika.
        /// Używane do zgłoszeń wewnętrznych
        /// </summary>
        /// <param name="userId">id użytkownika</param>
        /// <returns></returns>
        public List<BillingIssueDtoHelios> GetIssuesFromBPM(int userId)
        {
            List<List<string>> result = this.ExecuteStoredProcedure("CP_New_Assigned_Issue", new string[] { userId.ToString() }, DatabaseName.SupportADDONS);

            List<BillingIssueDtoHelios> issues = new List<BillingIssueDtoHelios>();

            foreach (var item in result)
            {
                BillingIssueDtoHelios issue = new BillingIssueDtoHelios();

                issue.issueWFS = new BillingDTHIssueWFS();
                issue.issueHelios = new IssueHelios();
                issue.issueHelios.number = item[1];
                issue.issueWFS.WFSIssueId = Convert.ToInt32(item[0]);
                issue.issueHelios.date = item[2];
                issue.issueHelios.updated = item[2];
                issue.issueHelios.email = item[3];
                issue.issueHelios.firstName = item[4];
                issue.issueHelios.lastName = item[5];
                issue.issueHelios.content = item[6];
                issue.issueHelios.title = item[7];

                if (string.IsNullOrEmpty(item[8]))
                    issue.issueHelios.severity = "4";
                else
                    issue.issueHelios.severity = item[8];

                issues.Add(issue);
            }

            return issues;
        }

        /// <summary>
        /// Pobiera szczegóły zgłoszenia z BPM (z pominięciem Jira).
        /// Używane do zgłoszeń wewnętrznych
        /// </summary>
        /// <param name="issueId">id Issue</param>
        /// <returns>BillingIssueDtoHelios</returns>
        public BillingIssueDtoHelios GetIssue(int issueId)
        {
            List<List<string>> result = this.ExecuteStoredProcedure("CP_Get_Issue_Param", new string[] { issueId.ToString() }, DatabaseName.SupportCP);

            List<string> row = result[0];


            BillingIssueDtoHelios issue = new BillingIssueDtoHelios();

            issue.issueWFS = new BillingDTHIssueWFS();
            issue.issueHelios = new IssueHelios();
            issue.issueHelios.number = row[1];
            issue.issueWFS.WFSIssueId = Convert.ToInt32(row[0]);
            issue.issueHelios.date = row[2];
            issue.issueHelios.updated = row[2];
            issue.issueHelios.email = row[3];
            issue.issueHelios.firstName = row[4];
            issue.issueHelios.lastName = row[5];
            issue.issueHelios.content = row[6];
            issue.issueHelios.title = row[7];
            issue.issueHelios.severity = row[8];

            //issue.issueHelios. = row[8];


            return issue;
        }


        /// <summary>
        /// Aktualizuje dane dotyczące zgłoszenia (pobiera z issueHelios, przypisuje do issueWFS)
        /// </summary>
        /// <param name="issue">zgłoszenie do aktualizacji</param>
        /// <returns></returns>
        public BillingIssueDtoHelios UpdateIssue(BillingIssueDtoHelios issue)
        {
            issue.issueWFS.DataIGodzinaUtworzeniaZgloszenia = issue.issueHelios.updated;
            issue.issueWFS.DataWystapieniaBledu = issue.issueHelios.updated;
            issue.issueWFS.DataIGodzinaOstatniegoKomentarza = issue.issueHelios.date;
            issue.issueWFS.Email = issue.issueHelios.email;
            issue.issueWFS.Imie = issue.issueHelios.firstName;
            issue.issueWFS.Nazwisko = issue.issueHelios.lastName;
            issue.issueWFS.TrescZgloszenia = issue.issueHelios.content;
            issue.issueWFS.TytulZgloszenia = issue.issueHelios.title;
            issue.issueWFS.NumerZgloszenia = issue.issueWFS.WFSIssueId.ToString();
            issue.issueWFS.IdKontraktu = issue.issueHelios.idKontraktu;
            issue.issueWFS.IdZamowienia = issue.issueHelios.idZamowienia;
            issue.issueWFS.Priorytet = issue.issueHelios.severity;
            issue.issueWFS.JiraId = issue.issueHelios.jiraIdentifier;

            issue.issueWFS.CzyOnCall = (issue.issueHelios.czyOnCall == "True" ? "True" : "False");
            issue.issueWFS.SrodowiskoProblemu = issue.issueHelios.srodowiskoProblemu;

            if (issue.issueWFS.NumerZgloszenia != "0")
            {
                List<EventParam> evep;
                if(issue.issueWFS.WFSState == "Czeka na diagnoze")
                    evep = this.GetBillingBoundEventParamForIssue(issue.issueWFS.WFSIssueId, new int[] { 3706, 3707, 3705, 3708, 3709, 3710, 3711, 3712, 3713, 3714, 3715, 3716, 3722, 3717, 3718, 3719, 3720, 3721 /*,6816, 6828*/ });
                else
                    evep = this.GetBillingBoundEventParamForIssue(issue.issueWFS.WFSIssueId, new int[] { 2833, 2834, 2835, 2836, 2837, 2838, 2839, 2840, 2841, 2842, 2843, 2830, 2831, 2832, 2867, 3197, 3525, 3684, 6817, 6829 });


                foreach (EventParam ep in evep)
                {
                    if (ep.EventParamId == 2839 || ep.EventParamId == 3713)
                    {
                        issue.issueWFS.System = new Entities.Component();
                        issue.issueWFS.System.Text = ep.Value;
                        issue.issueWFS.System.Value = ((ep.DBValue.HasValue) ? ep.DBValue.Value : -1);
                    }
                    else if (ep.EventParamId == 2840 || ep.EventParamId == 3714)
                    {
                        issue.issueWFS.Kategoria = new Entities.Component();
                        issue.issueWFS.Kategoria.Text = ep.Value;
                        issue.issueWFS.Kategoria.Value = ((ep.DBValue.HasValue) ? ep.DBValue.Value : -1);
                    }
                    else if (ep.EventParamId == 2841 || ep.EventParamId == 3715)
                    {
                        issue.issueWFS.Rodzaj = new Entities.Component();
                        issue.issueWFS.Rodzaj.Text = ep.Value;
                        issue.issueWFS.Rodzaj.Value = ((ep.DBValue.HasValue) ? ep.DBValue.Value : -1);
                    }
                    else if (ep.EventParamId == 2842 || ep.EventParamId == 3716)
                    {
                        issue.issueWFS.Typ = new Entities.Component();
                        issue.issueWFS.Typ.Text = ep.Value;
                        issue.issueWFS.Typ.Value = ((ep.DBValue.HasValue) ? ep.DBValue.Value : -1);
                    }
                    else if (ep.EventParamId == 2867 || ep.EventParamId == 3718)
                    {
                        issue.issueWFS.IdKontraktu = issue.issueHelios.idKontraktu;
                    }
                    else if (ep.EventParamId == 3525 || ep.EventParamId == 3720)
                    {
                        issue.issueWFS.IdZamowienia = issue.issueHelios.idZamowienia;
                    }
                    else if (ep.EventParamId == 3684 || ep.EventParamId == 3721)
                    {
                        issue.issueWFS.Priorytet = ((ep.DBValue.HasValue) ? ep.DBValue.Value : -1).ToString();
                    }
                    else if (ep.EventParamId == 2831 || ep.EventParamId == 3705)
                    {
                        issue.issueWFS.NumerZgloszenia = issue.issueHelios.number;
                    }
                    else if (ep.EventParamId == 6816 || ep.EventParamId == 6817)
                    {
                        issue.issueWFS.CzyOnCall = (issue.issueHelios.czyOnCall == "True" ? "True" : "False");
                    }
                    else if (ep.EventParamId == 6829 || ep.EventParamId == 6828)
                    {
                        issue.issueWFS.SrodowiskoProblemu = (issue.issueHelios.srodowiskoProblemu != null ? issue.issueHelios.srodowiskoProblemu : "");
                    }
                }
            }

            return issue;
        }

        /// <summary>
        /// Aktualizuje dane dotyczące zgłoszenia (pobiera z issueWFS, przypisuje do issueHelios)
        /// </summary>
        /// <param name="issue">Zgłoszenie do aktualizacji</param>
        /// <returns></returns>
        public BillingIssueDtoHelios UpdateJiraInfo(BillingIssueDtoHelios issue)
        {
            //issue.issueWFS.DataWystapieniaBledu = issue.issueHelios.oryginalneId == issue.issueHelios.number ? issue.issueHelios.date : issue.issueHelios.updated;
            //issue.issueWFS.Email = issue.issueHelios.email;
            //issue.issueWFS.Imie = issue.issueHelios.firstName;
            //issue.issueWFS.Nazwisko = issue.issueHelios.lastName;
            //issue.issueWFS.TrescZgloszenia = issue.issueHelios.content;
            //issue.issueWFS.TytulZgloszenia = issue.issueHelios.title;
            //issue.issueWFS.NumerZgloszenia = issue.issueWFS.WFSIssueId.ToString();
            //issue.issueWFS.IdKontraktu = issue.issueHelios.idKontraktu;
            //issue.issueWFS.IdZamowienia = issue.issueHelios.idZamowienia;
            //issue.issueWFS.Priorytet = issue.issueHelios.severity;

            BillingIssueDtoHelios result = issue;

            result.issueHelios.date = result.issueHelios.updated = issue.issueWFS.DataWystapieniaBledu;
            result.issueHelios.email = issue.issueWFS.Email;
            result.issueHelios.firstName = issue.issueWFS.Imie;
            result.issueHelios.lastName = issue.issueWFS.Nazwisko;
            result.issueHelios.content = issue.issueWFS.TrescZgloszenia;
            result.issueHelios.title = issue.issueWFS.TytulZgloszenia;
            result.issueHelios.severity = issue.issueWFS.Priorytet;
            result.issueHelios.rodzaj_zgloszenia = issue.issueWFS.System != null ? issue.issueWFS.System.Text : "";
            result.issueHelios.rodzaj_bledu = issue.issueWFS.Rodzaj != null ? issue.issueWFS.Rodzaj.Text : "";
            result.issueHelios.srodowiskoProblemu = issue.issueWFS.SrodowiskoProblemu != null ? issue.issueWFS.SrodowiskoProblemu : "";

            return result;
        }


        public BillingIssueDtoHelios UpdateJiraInfo(BillingIssueDtoHelios issue, Atlassian.Jira.Issue issueJira)
        {
            //issue.issueWFS.DataWystapieniaBledu = issue.issueHelios.oryginalneId == issue.issueHelios.number ? issue.issueHelios.date : issue.issueHelios.updated;
            //issue.issueWFS.Email = issue.issueHelios.email;
            //issue.issueWFS.Imie = issue.issueHelios.firstName;
            //issue.issueWFS.Nazwisko = issue.issueHelios.lastName;
            //issue.issueWFS.TrescZgloszenia = issue.issueHelios.content;
            //issue.issueWFS.TytulZgloszenia = issue.issueHelios.title;
            //issue.issueWFS.NumerZgloszenia = issue.issueWFS.WFSIssueId.ToString();
            //issue.issueWFS.IdKontraktu = issue.issueHelios.idKontraktu;
            //issue.issueWFS.IdZamowienia = issue.issueHelios.idZamowienia;
            //issue.issueWFS.Priorytet = issue.issueHelios.severity;

            BillingIssueDtoHelios result = issue;

            result.issueHelios.date = result.issueHelios.updated = issue.issueWFS.DataWystapieniaBledu;
            result.issueHelios.email = issue.issueWFS.Email;
            result.issueHelios.firstName = issue.issueWFS.Imie;
            result.issueHelios.lastName = issue.issueWFS.Nazwisko;
            result.issueHelios.content = issue.issueWFS.TrescZgloszenia;
            result.issueHelios.title = issue.issueWFS.TytulZgloszenia;
            result.issueHelios.severity = issue.issueWFS.Priorytet;
            result.issueHelios.rodzaj_zgloszenia = issue.issueWFS.System != null ? issue.issueWFS.System.Text : "";
            result.issueHelios.rodzaj_bledu = issue.issueWFS.Rodzaj != null ? issue.issueWFS.Rodzaj.Text : "";
            result.issueHelios.srodowiskoProblemu = issue.issueWFS.SrodowiskoProblemu != null ? issue.issueWFS.SrodowiskoProblemu : "";

            if (issueJira != null)
            {
                //result.issueHelios.date = result.issueHelios.updated = issue.issueWFS.DataWystapieniaBledu;
                //result.issueHelios.email = issue.issueWFS.Email;
                result.issueHelios.firstName = issueJira.Assignee.Split(' ').First();
                result.issueHelios.lastName = issueJira.Assignee.Split(' ').Last();
                //result.issueHelios.content = issue.issueWFS.TrescZgloszenia;
                //result.issueHelios.title = issue.issueWFS.TytulZgloszenia;
                result.issueHelios.severity = issueJira.Priority.Name;
                //result.issueHelios.rodzaj_zgloszenia = issue.issueWFS.System != null ? issue.issueWFS.System.Text : "";
                //result.issueHelios.rodzaj_bledu = issue.issueWFS.Rodzaj != null ? issue.issueWFS.Rodzaj.Text : "";
                //result.issueHelios.srodowiskoProblemu = issue.issueWFS.SrodowiskoProblemu != null ? issue.issueWFS.SrodowiskoProblemu : "";
            }


            return result;
        }

        //public BillingIssueDtoHelios GetBilingIssueDetails(BillingIssueDtoHelios issue, Atlassian.Jira.Issue issueJira)
        //{
        //    //issue.issueWFS.DataWystapieniaBledu = issue.issueHelios.oryginalneId == issue.issueHelios.number ? issue.issueHelios.date : issue.issueHelios.updated;
        //    //issue.issueWFS.Email = issue.issueHelios.email;
        //    //issue.issueWFS.Imie = issue.issueHelios.firstName;
        //    //issue.issueWFS.Nazwisko = issue.issueHelios.lastName;
        //    //issue.issueWFS.TrescZgloszenia = issue.issueHelios.content;
        //    //issue.issueWFS.TytulZgloszenia = issue.issueHelios.title;
        //    //issue.issueWFS.NumerZgloszenia = issue.issueWFS.WFSIssueId.ToString();
        //    //issue.issueWFS.IdKontraktu = issue.issueHelios.idKontraktu;
        //    //issue.issueWFS.IdZamowienia = issue.issueHelios.idZamowienia;
        //    //issue.issueWFS.Priorytet = issue.issueHelios.severity;

        //    BillingIssueDtoHelios result = issue;

        //    result.issueHelios.date = result.issueHelios.updated = issue.issueWFS.DataWystapieniaBledu;
        //    result.issueHelios.email = issue.issueWFS.Email;
        //    result.issueHelios.firstName = issue.issueWFS.Imie;
        //    result.issueHelios.lastName = issue.issueWFS.Nazwisko;
        //    result.issueHelios.content = issue.issueWFS.TrescZgloszenia;
        //    result.issueHelios.title = issue.issueWFS.TytulZgloszenia;
        //    result.issueHelios.severity = issue.issueWFS.Priorytet;
        //    result.issueHelios.rodzaj_zgloszenia = issue.issueWFS.System != null ? issue.issueWFS.System.Text : "";
        //    result.issueHelios.rodzaj_bledu = issue.issueWFS.Rodzaj != null ? issue.issueWFS.Rodzaj.Text : "";
        //    result.issueHelios.srodowiskoProblemu = issue.issueWFS.SrodowiskoProblemu != null ? issue.issueWFS.SrodowiskoProblemu : "";

        //    if (issueJira != null)
        //    {
        //        //result.issueHelios.date = result.issueHelios.updated = issue.issueWFS.DataWystapieniaBledu;
        //        //result.issueHelios.email = issue.issueWFS.Email;
        //        result.issueHelios.firstName = issueJira.Assignee.Split(' ').First();
        //        result.issueHelios.lastName = issueJira.Assignee.Split(' ').Last();
        //        //result.issueHelios.content = issue.issueWFS.TrescZgloszenia;
        //        //result.issueHelios.title = issue.issueWFS.TytulZgloszenia;
        //        result.issueHelios.severity = issueJira.Priority.Name;
        //        //result.issueHelios.rodzaj_zgloszenia = issue.issueWFS.System != null ? issue.issueWFS.System.Text : "";
        //        //result.issueHelios.rodzaj_bledu = issue.issueWFS.Rodzaj != null ? issue.issueWFS.Rodzaj.Text : "";
        //        //result.issueHelios.srodowiskoProblemu = issue.issueWFS.SrodowiskoProblemu != null ? issue.issueWFS.SrodowiskoProblemu : "";
        //    }


        //    return result;
        //}

        #region ProcessManager Engine
        /// <summary>
        /// Pobiera typy błędów procesów do Process Managera
        /// </summary>
        /// <returns>Lista typów błedów</returns>
        public List<ErrorType> GetErrorTypes()
        {
            return wfsDao.GetErrorTypes(this.getUser());
        }

        /// <summary>
        /// Pobiera rodzaje procesów
        /// </summary>
        /// <returns></returns>
        public List<Process> GetProcesses()
        {
            return wfsDao.GetProcesses(this.getUser());
        }

        /// <summary>
        /// Pobiera rodzaje błędów
        /// </summary>
        /// <param name="processId">opcjonalnie idProcesu, dla którego błedów szukamy</param>
        /// <returns></returns>
        public List<Error> GetErrors(int processId = -1)
        {
            return wfsDao.GetErrors(this.getUser(), processId);
        }

        /// <summary>
        /// Pobiera rozwiązania
        /// </summary>
        /// <returns></returns>
        public List<Solution> GetSolutions()
        {
            return wfsDao.GetSolutions(this.getUser());
        }

        /// <summary>
        /// Tworzy nowy rodzaj procesu
        /// </summary>
        /// <param name="process">nowy proces</param>
        /// <returns></returns>
        public Process CreateNewProcess(Process process)
        {
            return wfsDao.CreateNewProcess(this.getUser(), process);
        }

        /// <summary>
        /// Tworzy nowy rodzaj błędu
        /// </summary>
        /// <param name="error">nowy błąd</param>
        /// <returns></returns>
        public Error CreateNewError(Error error)
        {
            return wfsDao.CreateNewError(this.getUser(), error);
        }

        /// <summary>
        /// Tworzy nowe rozwiązanie
        /// </summary>
        /// <param name="solution">nowe rozwiązanie</param>
        /// <returns></returns>
        public Solution CreateNewSolution(Solution solution)
        {
            return wfsDao.CreateNewSolution(this.getUser(), solution);
        }

        /// <summary>
        /// Powiązuje rodzaj procesu z błędemi
        /// </summary>
        /// <param name="process">proces do którego przypisany ma zostac błąd</param>
        /// <param name="errors">lista błędow do powiązania</param>
        /// <param name="delete">czy usunąć powiązanie? Tak jeśli <c>true</c>, nie w przeciwnym wypadku</param>
        /// <returns></returns>
        public bool BoundErrorWithProcess(Entities.Process process, List<Entities.Error> errors, bool delete = false)
        {
            return wfsDao.BoundErrorWithProcess(this.getUser(), process, errors, delete);
        }

        /// <summary>
        /// Zapisuje nowy rekord naprawy procesów
        /// </summary>
        /// <param name="inXml">xml wejściowy</param>
        /// <returns></returns>
        public bool InsertNewProcessLog(StringBuilder inXml)
        {
            return wfsDao.InsertNewProcessLog(this.getUser(), inXml.ToString());
        }
        #endregion
    }
}
