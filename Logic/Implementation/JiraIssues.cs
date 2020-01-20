using System;
using System.Collections.Generic;
using System.Linq;
using Atlassian.Jira;

namespace Logic.Implementation
{
    public class JiraIssues
    {
        string login;
        string password;
        string uri;

        Jira jira = null;
        Issue _issue = null;

        System.Threading.Tasks.Task<JiraUser> taskUser;
        public List<Entities.JiraUser> userslist = new List<Entities.JiraUser>();
        

        public JiraIssues()
        {
        }

        public JiraIssues(string login, string password, string uri)
        {
            this.login = login;
            this.password = password;
            this.uri = uri;
        }

        private bool LoginJira()
        {
            try
            {
                jira = Jira.CreateRestClient("https://jira", login, password);
                //new Jira(uri, login, password);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public bool GetJiraIssueDetail(string jiraNr)
        {
            if (!LoginJira())
                return false;
            try
            {
                _issue = jira.GetIssue(jiraNr);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public List<string> GetJiraIssuesTypes()
        {
            if (!LoginJira())
                return null;

            var types = jira.GetIssueTypes();
            List<string> result = new List<string>();

            foreach (IssueType t in types)
                result.Add(t.Name);

            return result;
        }

        public List<string> GetJiraIssuesStatuses()
        {
            if (!LoginJira())
                return null;

            var statuses = jira.GetIssueStatuses();
            List<string> result = new List<string>();

            foreach (IssueStatus s in statuses)
                result.Add(s.Name);

            return result;
        }

        public List<string> GetJiraProjects()
        {
            if (!LoginJira())
                return null;

            var projects = jira.GetProjects();
            List<string> result = new List<string>();

            foreach (Project p in projects)
                result.Add(p.Name);

            return result;
        }

        private void GetCustomFields()
        {
            throw new NotImplementedException();
        }


        public bool ChangeDataModel(IEnumerable<Issue> issues, out List<Entities.BillingIssueDtoHelios> newModel,ref List<Entities.JiraUser> jiraUsers)
        {
            if (!LoginJira())
            {
                newModel = null;
                jiraUsers = null; 
                return false;
            }


            List<Entities.BillingIssueDtoHelios> newModelIssues = new List<Entities.BillingIssueDtoHelios>();
            List<Entities.JiraUser> newjiraUsers = jiraUsers;


            //List<List<string>> userslist = new List<List<string>>();
            
            foreach (var iss in issues)
            {

                //GetJiraIssueDetail(iss.Key.Value);

                string firstname = "";
                string lastname = "";
                string email = "";

                foreach (var userSave in newjiraUsers)
                {
                    if (userSave.Login == iss.Reporter)
                    {
                        firstname = userSave.FirstName;
                        lastname = userSave.LastName;
                        email = userSave.Email;

                        //System.Diagnostics.Debug.WriteLine(string.Format("Pobrałem użytkownika z listy {0} - {1}", firstname + ' ', lastname));

                        break;
                    }
                }

                if (lastname == "" && firstname == "" && email == "")
                {

                    if (iss.Reporter != null)
                    {
                        taskUser = jira.GetUserAsync(iss.Reporter);

                        if (taskUser != null)
                        {

                            //System.Diagnostics.Debug.WriteLine(string.Format("Uzytkownik {0}", taskUser.Result.DisplayName));
                            firstname = taskUser.Result.DisplayName.Split(' ').First();
                            lastname = (taskUser.Result.DisplayName.Contains(' ') ? taskUser.Result.DisplayName.Substring(firstname.Length + 1).Split(' ').First() : " ");
                            email = taskUser.Result.Email.ToString();

                            Entities.JiraUser user = new Entities.JiraUser();
                            user.FirstName = firstname;
                            user.LastName = lastname;
                            user.Email = email;
                            user.Reporter = taskUser.Result.DisplayName;
                            user.Login = iss.Reporter;

                            //System.Diagnostics.Debug.WriteLine(string.Format("Dodałem użytkownika do listy {0} - {1}", firstname + ' ', lastname));

                            newjiraUsers.Add(user);
                        }
                        else
                        {
                            firstname = "Użytkownik";
                            lastname = "Nieokreślony";
                            email = "---";
                        }
                    }
                    else
                    {
                        firstname = "Użytkownik";
                        lastname = "Nieaktywny";
                        email = "---";
                    }
                }


                //System.Diagnostics.Debug.WriteLine(string.Format("{0} - {1} - {2}", iss.Key, iss.JiraIdentifier, firstname + ' ' + lastname));

                List<string> kontrakt = new List<string>();
                List<string> konto = new List<string>();
                List<string> rblad = new List<string>();
                List<string> zamowienie = new List<string>();
                List<string> oryginalId = new List<string>();
                List<string> opisRozszerzony = new List<string>();
                List<string> srodowiskoProblemuCF = new List<string>();


                List<string> opcjeCF = new List<string>();

                for (int i = 0; i < iss.CustomFields.Count; i++)
                {
                    var a = iss.CustomFields[i];

                    try
                    {
                        if (a.Id == "customfield_14354") //Id Kontraktu
                        {
                            kontrakt.Add(a.Values[0]);
                        }
                        else if (a.Id == "customfield_14350") //Rodzaj błędu
                        {
                            rblad.Add(a.Values[0]);
                        }
                        else if (a.Id == "customfield_21150") // Rodzaj błędu ZO
                        {
                            rblad.Add(a.Values[0]);
                        }
                        else if (a.Id == "customfield_19351") // Rodzaj błędu Problem
                        {
                            rblad.Add(a.Values[0]);
                        }

                        else if (a.Id == "customfield_14650") // oryginalne id
                        {
                            oryginalId.Add(a.Values[0]);
                        }
                        else if (a.Id == "customfield_11450") // Opis rozszerzony 
                        {
                            opisRozszerzony.Add(a.Values[0]);
                        }
                        else if (a.Id == "customfield_19350") // Środowisko problemu 
                        {
                            srodowiskoProblemuCF.Add(a.Values[0]);
                        }
                        else if (a.Id == "customfield_20350") // Środowisko problemu 
                        {
                            foreach (var item in a.Values)
                            {
                                if (item == "Wdrożenie priorytetowe")
                                {
                                    zamowienie.Add(item);
                                }
                                else if (item == "Wstrzymuje testy")
                                {
                                    kontrakt.Add(item);
                                }
                            }

                        }
                    }
                    catch
                    {
                    }
                }


                //var typeList = jira.GetIssueTypes(iss.Project);
                
                DateTime now = DateTime.Now;
                TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
                int addHours = tzi.IsDaylightSavingTime(now) ? 2 : 1;
                Entities.BillingIssueDtoHelios issueHelios = new Entities.BillingIssueDtoHelios()
                {
                    issueHelios = new Entities.IssueHelios()
                    {
                        assigned_to = iss.Assignee,
                        content = (iss.Description != null) ? iss.Description : opisRozszerzony[0],
                        date = iss.Created.Value.ToString(),
                        updated = iss.Updated.Value.ToString(),
                        email = email,
                        firstName = firstname,
                        lastName = lastname,
                        number = iss.Key.Value,
                        title = iss.Summary,
                        severity = iss.Priority.ToString(),//(opcjeCF.Count == 0) ? iss.Priority.ToString(): opcjeCF[0],
                        status = iss.Status.Name,
                        //rodzaj_zgloszenia = iss.Type.Name,
                        rodzaj_zgloszenia = iss.Type.Name,// typeList.FirstOrDefault(x => x.Id == iss.Type.Id).Name,
                        idKontraktu = (kontrakt.Count > 0) ? kontrakt[0] : "0",
                        idKonta = (konto.Count > 0) ? konto[0] : null,
                        idZamowienia = (zamowienie.Count > 0) ? zamowienie[0] : "0",
                        oryginalneId = (oryginalId.Count > 0) ? oryginalId[0] : iss.Key.Value,
                        rodzaj_bledu = (rblad.Count > 0) ? rblad[0] : null,
                        projekt = iss.Project,
                        jiraIdentifier = iss.JiraIdentifier,
                        czyOnCall = "False",
                        srodowiskoProblemu = (srodowiskoProblemuCF.Count > 0) ? srodowiskoProblemuCF[0] : null
                    }
                };
                newModelIssues.Add(issueHelios);
            }

            newModel = newModelIssues;
            jiraUsers = newjiraUsers;

            return true;
        }



        public bool ChangeDataModel(Issue issue, out Entities.BillingIssueDtoHelios newModel, ref List<Entities.JiraUser> jiraUsers)
        {
            if (!LoginJira())
            {
                newModel = null;
                jiraUsers = null;
                return false;
            }


            Entities.BillingIssueDtoHelios newModelIssues = new Entities.BillingIssueDtoHelios();
            List<Entities.JiraUser> newjiraUsers = jiraUsers;


            string firstname = "";
            string lastname = "";
            string email = "";

            foreach (var userSave in newjiraUsers)
            {
                if (userSave.Login == issue.Reporter)
                {
                    firstname = userSave.FirstName;
                    lastname = userSave.LastName;
                    email = userSave.Email;

                    System.Diagnostics.Debug.WriteLine(string.Format("Pobrałem użytkownika z listy {0} - {1}", firstname + ' ', lastname));

                    break;
                }
            }

            if (lastname == "" && firstname == "" && email == "")
            {

                if (issue.Reporter != null)
                {
                    taskUser = jira.GetUserAsync(issue.Reporter);

                    if (taskUser != null)
                    {

                        System.Diagnostics.Debug.WriteLine(string.Format("Uzytkownik {0}", taskUser.Result.DisplayName));
                        firstname = taskUser.Result.DisplayName.Split(' ').First();
                        lastname = (taskUser.Result.DisplayName.Contains(' ') ? taskUser.Result.DisplayName.Substring(firstname.Length + 1).Split(' ').First() : " ");
                        email = taskUser.Result.Email.ToString();

                        Entities.JiraUser user = new Entities.JiraUser();
                        user.FirstName = firstname;
                        user.LastName = lastname;
                        user.Email = email;
                        user.Reporter = taskUser.Result.DisplayName;
                        user.Login = issue.Reporter;

                        System.Diagnostics.Debug.WriteLine(string.Format("Dodałem użytkownika do listy {0} - {1}", firstname + ' ', lastname));

                        newjiraUsers.Add(user);
                    }
                    else
                    {
                        firstname = "Użytkownik";
                        lastname = "Nieokreślony";
                        email = "---";
                    }
                }
                else
                {
                    firstname = "Użytkownik";
                    lastname = "Nieaktywny";
                    email = "---";
                }
            }


            System.Diagnostics.Debug.WriteLine(string.Format("{0} - {1} - {2}", issue.Key, issue.JiraIdentifier, firstname + ' ' + lastname));

            List<string> kontrakt = new List<string>();
            List<string> konto = new List<string>();
            List<string> rblad = new List<string>();
            List<string> zamowienie = new List<string>();
            List<string> oryginalId = new List<string>();
            List<string> opisRozszerzony = new List<string>();
            List<string> srodowiskoProblemuCF = new List<string>();


            List<string> opcjeCF = new List<string>();

            for (int i = 0; i < issue.CustomFields.Count; i++)
            {
                var a = issue.CustomFields[i];

                try
                {
                    if (a.Id == "customfield_14354") //Id Kontraktu
                    {
                        kontrakt.Add(a.Values[0]);
                    }
                    else if (a.Id == "customfield_14350") //Rodzaj błędu
                    {
                        rblad.Add(a.Values[0]);
                    }
                    else if (a.Id == "customfield_21150") // Rodzaj błędu ZO
                    {
                        rblad.Add(a.Values[0]);
                    }
                    else if (a.Id == "customfield_19351") // Rodzaj błędu Problem
                    {
                        rblad.Add(a.Values[0]);
                    }

                    else if (a.Id == "customfield_14650") // oryginalne id
                    {
                        oryginalId.Add(a.Values[0]);
                    }
                    else if (a.Id == "customfield_11450") // Opis rozszerzony 
                    {
                        opisRozszerzony.Add(a.Values[0]);
                    }
                    else if (a.Id == "customfield_19350") // Środowisko problemu 
                    {
                        srodowiskoProblemuCF.Add(a.Values[0]);
                    }
                    else if (a.Id == "customfield_20350") // Środowisko problemu 
                    {
                        foreach (var item in a.Values)
                        {
                            if (item == "Wdrożenie priorytetowe")
                            {
                                zamowienie.Add(item);
                            }
                            else if (item == "Wstrzymuje testy")
                            {
                                kontrakt.Add(item);
                            }
                        }

                    }
                }
                catch
                {
                }
            }


            DateTime now = DateTime.Now;
            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
            int addHours = tzi.IsDaylightSavingTime(now) ? 2 : 1;
            Entities.BillingIssueDtoHelios issueHelios = new Entities.BillingIssueDtoHelios()
            {
                issueHelios = new Entities.IssueHelios()
                {
                    assigned_to = issue.Assignee,
                    content = (issue.Description != null) ? issue.Description : opisRozszerzony[0],
                    date = issue.Created.Value.ToString(),
                    updated = issue.Updated.Value.ToString(),
                    email = email,
                    firstName = firstname,
                    lastName = lastname,
                    number = issue.Key.Value,
                    title = issue.Summary,
                    severity = issue.Priority.ToString(),//(opcjeCF.Count == 0) ? issue.Priority.ToString(): opcjeCF[0],
                    status = issue.Status.Name,
                    //rodzaj_zgloszenia = issue.Type.Name,
                    rodzaj_zgloszenia = issue.Type.Name,// typeList.FirstOrDefault(x => x.Id == issue.Type.Id).Name,
                    idKontraktu = (kontrakt.Count > 0) ? kontrakt[0] : "0",
                    idKonta = (konto.Count > 0) ? konto[0] : null,
                    idZamowienia = (zamowienie.Count > 0) ? zamowienie[0] : "0",
                    oryginalneId = (oryginalId.Count > 0) ? oryginalId[0] : issue.Key.Value,
                    rodzaj_bledu = (rblad.Count > 0) ? rblad[0] : null,
                    projekt = issue.Project,
                    jiraIdentifier = issue.JiraIdentifier,
                    czyOnCall = "False",
                    srodowiskoProblemu = (srodowiskoProblemuCF.Count > 0) ? srodowiskoProblemuCF[0] : null
                }
            };


            newModel = issueHelios;
            jiraUsers = newjiraUsers;

            return true;
        }

        public IEnumerable<Issue> GetIssuesByNumberAsync(string issueId)
        {
            if (!LoginJira())
                return null;

            string[] keys = issueId.Split(',');
            List<Issue> issues = new List<Issue>();

            try
            {
                foreach (var item in keys)
                {
                    Issue issue = jira.GetIssue(item);

                    issues.Add(issue);
                }
            }
            catch (Exception ex)
            {
            }

            return issues as IEnumerable<Issue>;
        }

        //[Obsolete("NIE UŻYWAJ! MOŻE NIE DZIAŁAC!", true)]
        //public Dictionary<Issue, bool> GetAllIssues(JiraProject project, string assignedFilterName = "", string unassignedFilterName = "")
        //{
        //    //if (!LoginJira())
        //    //    return null;

        //    //jira.MaxIssuesPerRequest = 50;

        //    //IEnumerable<Issue> przydzielone = null;
        //    //IEnumerable<Issue> nieprzydzielone = null;

        //    //bool assignedLoaded = false;
        //    //bool unassignedLoaded = false;

        //    //if (!string.IsNullOrEmpty(assignedFilterName))
        //    //{
        //    //    System.Diagnostics.Debug.WriteLine(assignedFilterName);

        //    //    try
        //    //    {
        //    //        przydzielone = await jira.Issues.GetIssuesFromFilter(assignedFilterName);
        //    //        assignedLoaded = true;
        //    //    }
        //    //    catch (InvalidOperationException ex)
        //    //    {
        //    //        List<Issue> emptyResult = new List<Issue>();
        //    //        przydzielone = emptyResult as IEnumerable<Issue>;
        //    //        assignedLoaded = true;
        //    //    }
        //    //    catch (Exception ex)
        //    //    {
        //    //        throw new InvalidOperationException("Błąd pobierania zgłoszeń", ex);
        //    //    }
        //    //}

        //    //if (!string.IsNullOrEmpty(unassignedFilterName))
        //    //{
        //    //    System.Diagnostics.Debug.WriteLine(unassignedFilterName);

        //    //    try
        //    //    {
        //    //        nieprzydzielone = await jira.Issues.GetIssuesFromFilter(unassignedFilterName);
        //    //        unassignedLoaded = true;
        //    //    }
        //    //    catch (InvalidOperationException ex)
        //    //    {
        //    //        List<Issue> emptyResult = new List<Issue>();
        //    //        nieprzydzielone = emptyResult as IEnumerable<Issue>;
        //    //        unassignedLoaded = true;
        //    //    }
        //    //    catch (Exception ex)
        //    //    {
        //    //        throw new InvalidOperationException("Błąd pobierania zgłoszeń", ex);
        //    //    }
        //    //}

        //    //switch (project)
        //    //{
        //    //    case JiraProject.CRM:
        //    //        {
        //    //            if (!assignedLoaded)
        //    //            {
        //    //                przydzielone = jira.GetIssuesFromJql("(status IN (Nowe, \"Oczekuje na weryfikację IT\", \"Ponownie otwarte\", \"W trakcie realizacji\", \"W trakcie testów\") AND issuetype in (\"Zlecenie operatorskie\", \"Błąd - system produkcyjny\", \"Błąd masowy - system produkcyjny\") AND assignee = CurrentUser()) OR (project = CRM AND status IN (Nowe, \"Oczekuje na weryfikację IT\", \"Ponownie otwarte\", \"W trakcie realizacji\", \"W trakcie testów\") AND issuetype IN (\"Błąd - system produkcyjny\", \"Błąd masowy - system produkcyjny\") AND assignee IS EMPTY)");
        //    //            }

        //    //            if (!unassignedLoaded)
        //    //            {
        //    //                if (this.login.Equals("billennium", StringComparison.CurrentCultureIgnoreCase))
        //    //                {
        //    //                    System.Linq.IOrderedQueryable<Atlassian.Jira.Issue> result = from i in jira.Issues
        //    //                                                                                 where (((i.Project == "CRM" && (i.Type == "Błąd - system produkcyjny" || i.Type == "Błąd masowy - system produkcyjny" || i.Type == "Zlecenie operatorskie"))
        //    //                                                                                 && i.Created > new DateTime(2012, 11, 30, 0, 0, 0, 0)
        //    //                                                                                 && i.Resolution == "unresolved"
        //    //                                                                                     //((i.Project == "INFOLINIA") && (i.Type == "Błąd - system produkcyjny" || i.Type == "Błąd masowy - system produkcyjny")))
        //    //                                                                                 && i.Assignee == ""))
        //    //                                                                                 orderby i.Created
        //    //                                                                                 select i;
        //    //                    nieprzydzielone = result as IEnumerable<Issue>;
        //    //                }
        //    //                else
        //    //                    nieprzydzielone = await jira.Issues.GetIssuesFromFilter("nieprzydzielone");
        //    //            }
        //    //            break;
        //    //        }
        //    //    case JiraProject.Billing:
        //    //        {
        //    //            if (!assignedLoaded)
        //    //            {
        //    //                System.Linq.IOrderedQueryable<Atlassian.Jira.Issue> result = from i in jira.Issues
        //    //                                                                             where i.Resolution == "unresolved"
        //    //                                                                             && i.Assignee == this.login
        //    //                                                                             orderby i.Created
        //    //                                                                             select i;
        //    //                przydzielone = result as IEnumerable<Issue>;
        //    //            }
        //    //            //System.Linq.IOrderedQueryable<Atlassian.Jira.Issue> result1 = from i in jira.Issues
        //    //            //                                                              where (((i.Project == "CP" || i.Project == "NDTH") && (i.Type == "Błąd - system produkcyjny" || i.Type == "Błąd masowy - system produkcyjny" || i.Type == "Zlecenie operatorskie"))
        //    //            //                                                              || ((i.Project == "SR") && (i.Type == "Błąd - system produkcyjny" || i.Type == "Błąd masowy - system produkcyjny")))
        //    //            //                                                              && i.Assignee == ""
        //    //            //                                                              orderby i.Created
        //    //            //                                                              select i;
        //    //            if (!unassignedLoaded)
        //    //            {
        //    //                IEnumerable<Atlassian.Jira.Issue> result2 = jira.GetIssuesFromJql("((project in (\"Naliczenia DTH\", \"Cross Promocje\", \"Konfigurator DTH\", \"Naliczenia DTH - Wyrocznia\") AND type IN (\"Błąd - system produkcyjny\", \"Zlecenie operatorskie\")) OR (project = Rozliczenia AND type = \"Błąd - system produkcyjny\")) AND assignee IS EMPTY");

        //    //                nieprzydzielone = result2;
        //    //            }
        //    //            break;
        //    //        }
        //    //    case JiraProject.Zamowienia:
        //    //        {
        //    //            if (!assignedLoaded)
        //    //            {
        //    //                System.Linq.IOrderedQueryable<Atlassian.Jira.Issue> result = from i in jira.Issues
        //    //                                                                             where i.Resolution == "unresolved"
        //    //                                                                             && i.Assignee == this.login
        //    //                                                                             orderby i.Created
        //    //                                                                             select i;
        //    //                przydzielone = result as IEnumerable<Issue>;
        //    //            }
        //    //            if (!unassignedLoaded)
        //    //            {
        //    //                nieprzydzielone = jira.GetIssuesFromJql("(project in (PLK, Szablonajzer, SOS) AND resolution = Unresolved AND status in (Nowe, \"W trakcie realizacji\", \"Ponownie otwarte\") AND (\"Data realizacji\" is EMPTY OR \"Data realizacji\" < -2w) AND type in (\"Błąd - system produkcyjny\") AND (labels not in (Weryfikacja_Billennium) OR labels is EMPTY)) OR (project in (ZCC) AND resolution = Unresolved AND assignee is empty AND type IN (\"Zlecenie operatorskie\", \"Błąd - system produkcyjny\", \"Błąd masowy - system produkcyjny\")) ORDER BY  type ASC, priority DESC, createdDate ASC, key DESC", 50);
        //    //            }
        //    //            break;
        //    //        }
        //    //}

        //    //Dictionary<Issue, bool> issues = new Dictionary<Issue, bool>();

        //    //foreach (var item in przydzielone)
        //    //{
        //    //    issues.Add(item, true);
        //    //}
        //    //foreach (var item in nieprzydzielone)
        //    //{
        //    //    issues.Add(item, false);
        //    //}

        //    throw new NotImplementedException();
        //}

        public IEnumerable<Issue> GetIssues(bool przydzielone, string filterName = "")
        {
            if (!LoginJira())
                return null;

            jira.MaxIssuesPerRequest = 200;

            List<Issue> emptyResult = new List<Issue>();
            //return emptyResult as IEnumerable<Issue>;

            //if (przydzielone)
            //{

            if (!string.IsNullOrEmpty(filterName))
            {
                System.Diagnostics.Debug.WriteLine(filterName);

                try
                {
                    return jira.GetIssuesFromFilter(filterName);
                }
                catch (InvalidOperationException ex)
                {
                    return emptyResult as IEnumerable<Issue>;
                }
                catch (Exception ex)
                {
                    return emptyResult as IEnumerable<Issue>;
                    throw new InvalidOperationException("Błąd pobierania zgłoszeń", ex);
                }
            }

                /*switch (project)
                {
                    case JiraProject.CRM:
                        if (this.login.Equals("billennium", StringComparison.CurrentCultureIgnoreCase))
                        {
                            IEnumerable<Issue> issues = jira.GetIssuesFromJql("assignee = billennium AND project in (CRM, INFOLINIA, \"Zamówienia CC\", \"CRM Windykacja\") AND resolution = Unresolved AND type in (\"Błąd - system produkcyjny\", \"Błąd masowy - system produkcyjny\", \"Zlecenie operatorskie\", Incydent)");
                            //jira.GetIssuesFromJql("(status IN (Nowe, \"Oczekuje na weryfikację IT\", \"Ponownie otwarte\", \"W trakcie realizacji\", \"W trakcie testów\") AND issuetype in (\"Zlecenie operatorskie\", \"Błąd - system produkcyjny\", \"Błąd masowy - system produkcyjny\") AND assignee = CurrentUser()) OR (project = CRM AND status IN (Nowe, \"Oczekuje na weryfikację IT\", \"Ponownie otwarte\", \"W trakcie realizacji\", \"W trakcie testów\") AND issuetype IN (\"Błąd - system produkcyjny\", \"Błąd masowy - system produkcyjny\") AND assignee IS EMPTY)");

                            return issues;
                        }
                        else
                        {
                            System.Linq.IOrderedQueryable<Atlassian.Jira.Issue> issues = from i in jira.Issues
                                                                                         where i.Resolution == "unresolved"
                                                                                         && i.Assignee == this.login
                                                                                         orderby i.Created
                                                                                         select i;
                            return issues as IEnumerable<Issue>;
                        }

                    default:
                        System.Linq.IOrderedQueryable<Atlassian.Jira.Issue> result = from i in jira.Issues
                                                                                     where i.Resolution == "unresolved"
                                                                                     && i.Assignee == this.login
                                                                                     orderby i.Created
                                                                                     select i;

                        return result as IEnumerable<Issue>;
                }*/
            //}
            //else
            //{
            //    if (!string.IsNullOrEmpty(filterName))
            //    {
            //        System.Diagnostics.Debug.WriteLine(filterName);

            //        try
            //        {
            //            return jira.GetIssuesFromFilter(filterName);
            //        }
            //        catch (InvalidOperationException ex)
            //        {
            //            return emptyResult as IEnumerable<Issue>;
            //        }
            //        catch (Exception ex)
            //        {
            //            return emptyResult as IEnumerable<Issue>;
            //            throw new InvalidOperationException("Błąd pobierania zgłoszeń", ex);
            //        }
            //    }

            //    /*switch (project)
            //    {
            //        case JiraProject.CRM:
            //            {
            //                IEnumerable<Issue> issues = null;

            //                if (this.login.Equals("billennium", StringComparison.CurrentCultureIgnoreCase))
            //                {
            //                    System.Linq.IOrderedQueryable<Atlassian.Jira.Issue> result = from i in jira.Issues
            //                                                                                 where (((i.Project == "CRM" && (i.Type == "Błąd - system produkcyjny" || i.Type == "Błąd masowy - system produkcyjny" || i.Type == "Zlecenie operatorskie"))
            //                                                                                 && i.Created > new DateTime(2012, 11, 30, 0, 0, 0, 0)
            //                                                                                 && i.Resolution == "unresolved"
            //                                                                                 //((i.Project == "INFOLINIA") && (i.Type == "Błąd - system produkcyjny" || i.Type == "Błąd masowy - system produkcyjny")))
            //                                                                                 && i.Assignee == ""))
            //                                                                                 orderby i.Created
            //                                                                                 select i;
            //                    issues = result as IEnumerable<Issue>;
            //                }
            //                else
            //                    issues = jira.GetIssuesFromFilter("nieprzydzielone");


            //                return issues;
            //            }
            //        case JiraProject.Billing:
            //            {
            //                //System.Linq.IOrderedQueryable<Atlassian.Jira.Issue> result = from i in jira.Issues
            //                //                                                             where (((i.Project == "CP" || i.Project == "NDTH") && (i.Type == "Błąd - system produkcyjny" || i.Type == "Błąd masowy - system produkcyjny" || i.Type == "Zlecenie operatorskie"))
            //                //                                                             || ((i.Project == "SR") && (i.Type == "Błąd - system produkcyjny" || i.Type == "Błąd masowy - system produkcyjny")))
            //                //                                                             && i.Assignee == ""
            //                //                                                             orderby i.Created
            //                //                                                             select i;


            //                IEnumerable<Atlassian.Jira.Issue> result = jira.GetIssuesFromJql("((project in (\"Naliczenia DTH\", \"Cross Promocje\", \"Konfigurator DTH\", \"Naliczenia DTH - Wyrocznia\") AND type IN (\"Błąd - system produkcyjny\", \"Zlecenie operatorskie\")) OR (project = Rozliczenia AND type = \"Błąd - system produkcyjny\")) AND assignee IS EMPTY");
            //                return result as IEnumerable<Issue>;
            //            }
            //        case JiraProject.Zamowienia:
            //            {
            //                IEnumerable<Issue> issues = jira.GetIssuesFromJql("(project in (PLK, Szablonajzer, SOS) AND resolution = Unresolved AND status in (Nowe, \"W trakcie realizacji\", \"Ponownie otwarte\") AND (\"Data realizacji\" is EMPTY OR \"Data realizacji\" < -2w) AND type in (\"Błąd - system produkcyjny\") AND (labels not in (Weryfikacja_Billennium) OR labels is EMPTY)) OR (project in (ZCC) AND resolution = Unresolved AND assignee is empty AND type IN (\"Zlecenie operatorskie\", \"Błąd - system produkcyjny\", \"Błąd masowy - system produkcyjny\")) ORDER BY  type ASC, priority DESC, createdDate ASC, key DESC", 50);

            //                return issues;
            //            }
            //        default: throw new ArgumentException("Brak możliwości pobrania zgłoszeń dla podanego projektu");
            //    }*/
            //}
            return emptyResult as IEnumerable<Issue>;
            //throw new NotImplementedException();
        }

        public int GetIssuesCountFromFilterAsync(string filterName)
        {
            if (!LoginJira())
                return -1;

            var issues = jira.GetIssuesFromFilter(filterName);//, 0, 500);

            return issues.Count();
        }

        public IEnumerable<Issue> GetIssuesFromFilterAsync(string filterName)
        {
            if (!LoginJira())
                return null;

            var issues = jira.GetIssuesFromFilter(filterName);//, 0, 500);

            return issues;
        }

        public List<string> GetFiltersAsync()
        {
            if (!LoginJira())
                return null;

            //IEnumerable<JiraNamedEntity> filters = jira.GetFilters();
            var filters = jira.GetFilters();

            List<string> filterNames = new List<string>();

            foreach (var item in filters)
            {
                filterNames.Add(item.Name);
            }

            return filterNames;
        }
    }

    public enum JiraProject
    {
        CRM = 0,
        Billing,
        Zamowienia
    }
}
