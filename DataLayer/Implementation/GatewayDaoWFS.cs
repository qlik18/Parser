using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataLayer.Interface;
using Entities;
using Entities.Enums;
using System.Data.SqlClient;
using Utility;
using System.Configuration;
using System.Data;
using DataLayer.HPDataService;
using System.ServiceModel;
using Logging;

namespace DataLayer.Implementation
{
    public class GatewayDaoWFS : IGatewayDaoWFS, INotesManager, IUsersHelios, IProcessManagerGateway
    {
        private ServiceManager manager;
        private INotesManager notesManager;
        private IUsersHelios usersManager;
        private IProcessManagerGateway processManager;
        //private DataServiceBpm.DataServiceSoapClient bpmClient;

        private User loggedUser = null;

        public GatewayDaoWFS()
        {
            manager = new ServiceManager(new EventHandler(Channel_Faulted));
            notesManager = DBNotesManager.GetNoteDBManager(manager);
            usersManager = new DBUsersHelios(manager);
            processManager = ProcessManagerGateway.GetProcessManagerGateway(manager);
            //bpmClient = new DataServiceBpm.DataServiceSoapClient();
        }

        /// <summary>
        /// Ustawia pole zalogowanego uytkownika
        /// </summary>
        /// <param name="u">zalogowany użytkownik</param>
        public void setLoggedUser(User u)
        {
            loggedUser = u;
        }

        /// <summary>
        /// Wykonuje akcję na sprawie
        /// </summary>
        /// <param name="idIssue">id sprawy</param>
        /// <param name="eventMoveId">id akcji</param>
        /// <param name="paramz">parametry akcji</param>
        /// <param name="user">zalogowany użytkownik</param>
        /// <returns></returns>
        public int DoActionInIssue(int? IssueId, int EventMove,List<EventParam> paramz, User user)
        {
            //DataServiceSoapClient client = new DataServiceSoapClient("DataServiceSoap");

            //DSEventParamValue[] param = new DSEventParamValue[paramz.Count];
            //for (int i = 0; i < paramz.Count; i++)
            //{

            //    DSEventParamValue par = new DSEventParamValue();

            //    par.IdEventParam = paramz[i].EventParamId;
            //    par.DBValue = paramz[i].DBValue;
            //    par.Value = paramz[i].Value;
            //    par.DockNr = 0;
            //    par.DBExtValue = paramz[i].DBExtValue;

            //    param[i] = par;
            //}
            
            //return manager.HPService.NewInsertIssueWithLoginAndTransaction(IssueId, EventMove, param, user.login, user.password, string.Empty).Param;

            return manager.HPService.DoActionInIssue(IssueId, EventMove, paramz.ToArray(), user);
        }

        /// <summary>
        /// Pobiera dostepne kroki dla danej sprawy BPM
        /// </summary>
        /// <param name="issueId">id sprawy BPM</param>
        /// <param name="userId">id użytkownika</param>
        /// <returns></returns>
        public Dictionary<int, string> GetActionForIssue(int issueId, int userId)
        {
            ResultValue<Dictionary<int,string>> moves = manager.HPService.GetActionForIssue(loggedUser,issueId, userId);
            return moves.GetResult() ;
        }
        
        /// <summary>
        /// Pobiera numery zgłoszeń z Jira dodane do BPM
        /// </summary>
        /// <param name="issues">tablica numerów zgłoszeń</param>
        /// <returns></returns>
        public Dictionary<KeyValuePair<int, string>, string> CheckBillingIssuesPresenceOnWFS(string[] issues)
        {
            int n = issues.Length;
            if (n == 0)
            {
                return new Dictionary<KeyValuePair<int, string>, string>();
            }
            String[] result = new String[n];
            ResultValue<Dictionary<KeyValuePair<int, string>, string>> list = manager.HPService.CheckBillingIssuesPresenceOnWFS(loggedUser, issues);
            return list.GetResult();
        }

        public Dictionary<KeyValuePair<int, string>, string> CheckIssuesPresenceOnBpm(List<BillingIssueDtoHelios> issues)
        {
            if (issues.Count == 0)
                return new Dictionary<KeyValuePair<int, string>, string>();

            List<BillingDTHIssueWFS> list = new List<BillingDTHIssueWFS>();
            foreach (var item in issues)
            {
                BillingDTHIssueWFS issue = new BillingDTHIssueWFS();
                issue.JiraId = item.issueHelios.jiraIdentifier;
                issue.NumerZgloszenia = item.issueHelios.number;
                list.Add(issue);
            }

            ResultValue<Dictionary<KeyValuePair<int, string>, string>> result = manager.HPService.CheckIssuesPresenceOnBpm(loggedUser, list.ToArray());
                        
            return result.GetResult();
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
            try
            {
                ResultValue<string[][]> result;
                result = manager.HPService.ExecuteStoredProcedure(this.loggedUser, procedureName, parameters, database);

                List<List<string>> list = new List<List<string>>();

                string[][] lines = result.GetResult();

                foreach (var item in lines)
                {
                    List<string> l = item.ToList();
                    list.Add(l);
                }

                return list;
            }
            catch(Exception ex)
            {
                ExceptionManager.LogError(ex, Logger.Instance, true);
                return ExecuteStoredProcedure(procedureName, parameters, database);
            }
        }
        
        /// <summary>
        /// Pobiera listę komponentow dla zadanego id komponentu-rodzica
        /// </summary>
        /// <param name="user">zalogowany użytkownik</param>
        /// <param name="id">id komponentu-rodzica</param>
        /// <returns></returns>
        public Dictionary<int, string> GetBillingComponents(Entities.User user, int id)
        {
            ResultValue<Dictionary<int, string>> result = manager.HPService.GetBillingComponents(user, id);
            return result.GetResult();
        }

        /// <summary>
        /// Tworzy nową sprawę w BPM
        /// </summary>
        /// <param name="issue">zgłoszenie Jira</param>
        /// <param name="login"login użytkownika</param>
        /// <param name="haslo">hasło użytkoniwka</param>
        /// <returns></returns>
        public int AddBillingIssueToWFS(BillingDTHIssueWFS issue, User user)
        {

            Entities.HeliosUser tmpuser = usersManager.SearchUserByEmail(issue.Email);
            if (tmpuser == null)
            {
               
                    tmpuser = new HeliosUser();
                    tmpuser.telefon = "";
                    tmpuser.email = issue.Email;
                    tmpuser.imie = issue.Imie;
                    tmpuser.nazwisko = issue.Nazwisko;
                    usersManager.AddUser(tmpuser);
            }
            return manager.HPService.NewBillingIssue(issue, user);
        }

        #region Obsługa formularzy WFS
        /// <summary>
        /// Pobiera listę parametrów do modelowania formatki akcji
        /// </summary>
        /// <param name="eventMoveId">id akcji</param>
        /// <returns></returns>
        public List<Entities.EventParamModeler> GetEventParamForFormByEventMove(int eventMoveId)
        {
            return manager.HPService.GetEventParamForFormByEventMove(loggedUser,eventMoveId).GetResult().ToList();
        }

        /// <summary>
        /// Pobiera parametry powiązane z danymi parametrami dla sprawy BPM
        /// </summary>
        /// <param name="issueId">id sprawy</param>
        /// <param name="eventParamId">id parametrów</param>
        /// <returns></returns>
        public List<Entities.EventParam> GetBoundEventParamForIssue(int issueId, int[] eventParamId)
        {
            return manager.HPService.GetBoundEventParamForIssue(loggedUser, issueId, eventParamId).GetResult().ToList();
        }

        /// <summary>
        /// Pobiera akutalne parametry sprawy, dla zadanych parametrów związanych
        /// </summary>
        /// <param name="user">zalogowany użytkownik</param>
        /// <param name="issueId">id sprawy</param>
        /// <param name="eventParamId">id parametrów związanych</param>
        /// <returns></returns>
        public List<Entities.EventParam> GetBillingBoundEventParamForIssue(int issueId, int[] eventParamId)
        {
            return manager.HPService.GetBillingBoundEventParamForIssue(loggedUser, issueId, eventParamId).GetResult().ToList();
        }

        /// <summary>
        /// Zakańcza sesję użytkownika
        /// </summary>
        public void LogOutFromHPService()
        {
            manager.HPService.LogOut();
        }

        /// <summary>
        /// Powiadamia webService, że sesja powinna zostać podtrzymana, zapobiegając zakończeniu sesji po czasie.
        /// </summary>
        /// <returns></returns>
        public void notifyWebService()
        {
            try
            {
                manager.HPService.Notify();
            }
            catch (Exception Ex)
            {

                throw new Exception("Usługa HPWEBService zerwała połączenie. Spróbj odświeżyć aplikację za chwilę."); ;
            }
        }
        #endregion

        /// <summary>
        /// Loguje użytkownika do systemu
        /// </summary>
        /// <param name="user">login użytkownika</param>
        /// <param name="password">hasło użytkownika</param>
        /// <returns></returns>
        public User loginToWFSWithUserInfo(string user, string password)
        {
            return manager.HPService.LogIn2(user, password);
        }

        private void Channel_Faulted(object obj, EventArgs arg)
        {
            int UserId = -1;
            if (loggedUser != null)
            {
                UserId = this.loginToWFSWithUserInfo(loggedUser.login, loggedUser.password).Id;
            }
            if (UserId <= 0)
                throw new Exception("Usługa HPWEBService zerwała połączenie. Nie ma możliwości powtórnego zalogowania się. Zrestartuj aplikację");
        }
        
        /// <summary>
        /// Zapisuje nowe zgłoszenie Jira w BPM
        /// </summary>
        /// <param name="issue">zgłoszenie Jira</param>
        /// <returns>id nowej sprawy BPM</returns>
        public int CreateNewIssue(BillingDTHIssueWFS issue)
        {
            

            throw new NotImplementedException();

            //return bpmClient.NewInsertIssueWithLoginAndTransaction(null, 614, paramz, this.loggedUser.login, this.loggedUser.password, string.Empty).Param;

            // Nowy proces - odkomentować po wdrożeniu - NEWPROC
            //param = new DSEventParamValue();
            //param.IdEventParam = 3702;
            //param.Value = issue.NumerZgloszenia;
            //param.DockNr = 0;
            //paramz[0] = param;
            //param = new DSEventParamValue();
            //param.DockNr = 0;
            //param.IdEventParam = 3703;
            //param.Value = issue.TytulZgloszenia;
            //paramz[1] = param;
            //param = new DSEventParamValue();
            //param.IdEventParam = 3704;
            //param.Value = issue.Imie;
            //param.DockNr = 0;
            //paramz[2] = param;
            //param = new DSEventParamValue();
            //param.DockNr = 0;
            //param.IdEventParam = 3705;
            //param.Value = issue.Nazwisko;
            //paramz[3] = param;
            //param = new DSEventParamValue();
            //param.DockNr = 0;
            //param.IdEventParam = 3706;
            //param.Value = issue.Email;
            //paramz[4] = param;
            //param = new DSEventParamValue();
            //param.DockNr = 0;
            //param.IdEventParam = 3707;
            //param.Value = issue.DataWystapieniaBledu;
            //paramz[5] = param;
            //param = new DSEventParamValue();
            //param.DockNr = 0;
            //param.IdEventParam = 3708;
            //param.Value = issue.DataIGodzinaUtworzeniaZgloszenia;
            //paramz[6] = param;
            //param = new DSEventParamValue();
            //param.DockNr = 0;
            //param.IdEventParam = 3709;
            //param.Value = issue.DataIGodzinaOstatniegoKomentarza;
            //paramz[7] = param;
            //param = new DSEventParamValue();
            //param.DockNr = 0;
            //param.IdEventParam = 3710;
            //param.Value = issue.System.Text;
            //param.DBValue = issue.System.Value;
            //paramz[8] = param;
            //param = new DSEventParamValue();
            //param.DockNr = 0;
            //param.IdEventParam = 3711;
            //param.Value = issue.Kategoria.Text;
            //param.DBValue = issue.Kategoria.Value;
            //paramz[9] = param;
            //param = new DSEventParamValue();
            //param.DockNr = 0;
            //param.IdEventParam = 3712;
            //param.Value = issue.Rodzaj.Text;
            //param.DBValue = issue.Rodzaj.Value;
            //paramz[10] = param;
            //param = new DSEventParamValue();
            //param.DockNr = 0;
            //param.IdEventParam = 3713;
            //param.Value = issue.Typ.Text;
            //param.DBValue = issue.Typ.Value;
            //paramz[11] = param;
            //param = new DSEventParamValue();
            //param.DockNr = 0;
            //param.IdEventParam = 3714;
            //param.Value = issue.TrescZgloszenia;
            //paramz[12] = param;
            //param = new DSEventParamValue();
            //param.DockNr = 0;
            //param.IdEventParam = 3715;
            //param.Value = issue.IdKontraktu;
            //paramz[13] = param;
            //param = new DSEventParamValue();
            //param.DockNr = 0;
            //param.IdEventParam = 3717;
            //param.Value = issue.IdZamowienia;
            //paramz[14] = param;

            //List<List<string>> priorytety = this.ExecStoredProcedure(user, "Billing_GetListOfPriorities", new string[] { }, "Support_ADDONS_PREPROD").GetResult();

            //param = new DSEventParamValue();
            //param.DockNr = 0;
            //param.IdEventParam = 3718;
            //param.Value = priorytety.Where(x => x[0].Equals(issue.Priorytet)).FirstOrDefault()[1];
            //param.DBValue = Convert.ToInt32(issue.Priorytet);
            //paramz[15] = param;

            //return bpmClient.NewInsertIssueWithLoginAndTransaction(null, 769, paramz, this.loggedUser.login, this.loggedUser.password, string.Empty).Param;
        }

        #region INotesManager Members

        public void AddNote(Note note)
        {
            notesManager.AddNote(note);
        }

        public void UpdateNote(Note note)
        {
            notesManager.UpdateNote(note);
        }

        public Note SearchIssueNote(string issuenumber)
        {
            return notesManager.SearchIssueNote(issuenumber);
        }

        public List<Note> SearchNotes(List<string> issuenumbers)
        {
            return notesManager.SearchNotes(issuenumbers);
        }

        #endregion

        #region IUsersHelios Members

        public void AddUser(HeliosUser user)
        {
            usersManager.AddUser(user);
        }

        public void UpdateUser(HeliosUser user)
        {
            usersManager.UpdateUser(user);
        }

        public HeliosUser SearchUserByEmail(string email)
        {
            return usersManager.SearchUserByEmail(email);
        }

        public List<HeliosUser> GetAllUsers()
        {
            return usersManager.GetAllUsers();
        }

        #endregion

        #region IProcessManagerGateway
        public List<Entities.ErrorType> GetErrorTypes(Entities.User user)
        {
            return manager.HPService.GetErrorTypes(user).GetResult().ToList();
        }

        public List<Entities.Process> GetProcesses(Entities.User user)
        {
            return manager.HPService.GetProcesses(user).GetResult().ToList();
        }

        public List<Entities.Error> GetErrors(Entities.User user, int processId = -1)
        {
            return manager.HPService.GetErrors(user, processId).GetResult().ToList();
        }

        public List<Entities.Solution> GetSolutions(Entities.User user)
        {
            return manager.HPService.GetSolutions(user).GetResult().ToList();
        }

        public Process CreateNewProcess(User user, Process process)
        {
            return manager.HPService.CreateNewProcess(user, process).GetResult();
        }

        public Entities.Error CreateNewError(Entities.User user, Entities.Error error)
        {
            return manager.HPService.CreateNewError(user, error).GetResult();
        }

        public Entities.Solution CreateNewSolution(Entities.User user, Entities.Solution solution)
        {
            return manager.HPService.CreateNewSolution(user, solution).GetResult();
        }

        public bool BoundErrorWithProcess(Entities.User user, Entities.Process process, List<Entities.Error> errors, bool delete = false)
        {
            return manager.HPService.BoundErrorWithProcess(user, process, errors.ToArray(), delete);
        }

        public bool InsertNewProcessLog(Entities.User user, string inXml)
        {
            return manager.HPService.InsertNewProcessLog(user, inXml);
        }
        #endregion
    }
}
