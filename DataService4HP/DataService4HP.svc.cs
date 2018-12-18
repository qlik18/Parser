using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Data.SqlClient;
using Entities;
using Entities.Enums;
using System.Data;
using System.Configuration;
using DataService4HP.DataServiceWFS;
using System.IO;

namespace DataService4HP
{
    [ServiceBehaviorAttribute(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Multiple)]//[ServiceBehaviorAttribute(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Multiple, AddressFilterMode = AddressFilterMode.Any)]
    public class DataServiceHP : IDataService4HP
    {
        public readonly static string ADDONS = "ADDONS";
        public readonly static string WFSDB = "WFSDB";
        public readonly static string CPUSERS = "CPUSERS";

        private readonly static int EM_ZAKLADANIE_SPRAWY_BILLING = 769;

        private readonly static DataServiceWFS.DataServiceSoapClient clientWFS = new DataServiceWFS.DataServiceSoapClient("DataServiceSoap");

        private int UserId = -1;

        /// <summary>
        /// Sprawdza czy użytkownik jest obecnie zalogowany
        /// </summary>
        /// <returns></returns>
        private bool CheckUserCredentials()
        {
            return (UserId > 0);
        }

        /// <summary>
        /// Inicjuje połączenie z bazą danych (Support_BPM)
        /// </summary>
        /// <returns></returns>
        private SqlConnection SetConnection()
        {
            SqlConnection thisConnection = null;
            try
            {
                thisConnection = new SqlConnection(ConfigurationManager.ConnectionStrings[WFSDB].ConnectionString);
                thisConnection.Open();
                return thisConnection;
            }
            catch (SqlException sx)
            {
                if (thisConnection != null)
                    thisConnection.Close();
                throw new Exception("Błąd połączenia z bazą danych GatewayDaoWFS.SetConnection()", sx);
            }
        }
        
        /// <summary>
        /// Inicjuje połączenie z bazą danych.
        /// </summary>
        /// <param name="ConnName">nazwa connectionString</param>
        /// <returns></returns>
        private SqlConnection SetConnection(string ConnName)
        {
            SqlConnection thisConnection = null;
            try
            {
                thisConnection = new SqlConnection(ConfigurationManager.ConnectionStrings[ConnName].ConnectionString);
                thisConnection.Open();
                return thisConnection;
            }
            catch (SqlException sx)
            {
                if (thisConnection != null)
                    thisConnection.Close();
                throw new Exception("Błąd połączenia z bazą danych GatewayDaoWFS.SetConnection()", sx);
            }
        }

        /// <summary>
        /// Loguje użytkownika do systemu
        /// </summary>
        /// <param name="login">login użytkownika</param>
        /// <param name="pass">hasło użytkownika</param>
        /// <returns></returns>
        public User LogIn2(string login, string pass)
        {            
            DSUser2 user = clientWFS.LogIn2(login, pass);
            UserId = user.Id;
            return new User(user.Id, login, pass, user.Name, user.Surname);
        }

        /// <summary>
        /// Wykonuje akcję na sprawie
        /// </summary>
        /// <param name="idIssue">id sprawy</param>
        /// <param name="eventMoveId">id akcji</param>
        /// <param name="paramz">parametry akcji</param>
        /// <param name="u">zalogowany użytkownik</param>
        /// <returns></returns>
        public int DoActionInIssue(int? idIssue, int eventMoveId, List<EventParam> paramz, User u)
        {
            if (!CheckUserCredentials())
                return -1;

            DSEventParamValue[] param = new DSEventParamValue[paramz.Count];
            for (int i = 0; i < paramz.Count; i++)
            {

                DSEventParamValue par = new DSEventParamValue()
                {
                    IdEventParam = paramz[i].EventParamId,
                    DBValue = paramz[i].DBValue,
                    Value = paramz[i].Value,
                    DockNr = 0,
                    DBExtValue = paramz[i].DBExtValue,
                };

                param[i] = par;
            }


            return clientWFS.NewInsertIssueWithLogin(idIssue, eventMoveId, param, u.login, u.password).Param;
        }

        /// <summary>
        /// Zakańcza sesję użytkownika
        /// </summary>
        public void LogOut()
        {

        }

        /// <summary>
        /// Tworzy nową sprawę w BPM
        /// </summary>
        /// <param name="issue">zgłoszenie Jira</param>
        /// <param name="user">zalogowany użytkownik</param>
        /// <returns></returns>
        public int NewBillingIssue(BillingDTHIssueWFS issue, User user)
        {
            List<List<string>> priorytety = this.ExecuteStoredProcedure(user, "Billing_GetListOfPriorities", new string[] { }, DatabaseName.SupportADDONS).GetResult();
            DSEventParamValue[] paramz = new DSEventParamValue[19];

            paramz[0] = new DSEventParamValue() { IdEventParam = 3705, Value = issue.NumerZgloszenia, DockNr = 0 };
            paramz[1] = new DSEventParamValue() { IdEventParam = 3706, Value = issue.TytulZgloszenia, DockNr = 0 };
            paramz[2] = new DSEventParamValue() { IdEventParam = 3707, Value = issue.Imie, DockNr = 0 };
            paramz[3] = new DSEventParamValue() { IdEventParam = 3708, Value = issue.Nazwisko, DockNr = 0 };
            paramz[4] = new DSEventParamValue() { IdEventParam = 3709, Value = issue.Email, DockNr = 0 }; ;
            paramz[5] = new DSEventParamValue() { IdEventParam = 3710, Value = issue.DataWystapieniaBledu, DockNr = 0 }; ;
            paramz[6] = new DSEventParamValue() { IdEventParam = 3711, Value = issue.DataIGodzinaUtworzeniaZgloszenia, DockNr = 0 }; ;
            paramz[7] = new DSEventParamValue() { IdEventParam = 3712, Value = issue.DataIGodzinaOstatniegoKomentarza, DockNr = 0 }; ;
            paramz[8] = new DSEventParamValue() { IdEventParam = 3713, Value = issue.System.Text, DBValue = issue.System.Value, DockNr = 0 }; ;
            paramz[9] = new DSEventParamValue() { IdEventParam = 3714, Value = issue.Kategoria.Text, DBValue = issue.System.Value, DockNr = 0 }; ;
            paramz[10] = new DSEventParamValue() { IdEventParam = 3715, Value = issue.Rodzaj.Text, DBValue = issue.Rodzaj.Value, DockNr = 0 }; ;
            paramz[11] = new DSEventParamValue() { IdEventParam = 3716, Value = issue.Typ.Text, DBValue = issue.Typ.Value, DockNr = 0 }; ;
            paramz[12] = new DSEventParamValue() { IdEventParam = 3717, Value = issue.TrescZgloszenia, DockNr = 0 }; ;
            paramz[13] = new DSEventParamValue() { IdEventParam = 3718, Value = issue.IdKontraktu, DockNr = 0 }; ;
            paramz[14] = new DSEventParamValue() { IdEventParam = 3720, Value = issue.IdZamowienia, DockNr = 0 }; ;
            paramz[15] = new DSEventParamValue() { IdEventParam = 3721,
                Value = priorytety.Where(x => x[0].Equals(issue.Priorytet)).FirstOrDefault()[1],
                DBValue = Convert.ToInt32(issue.Priorytet),
                DockNr = 0 };
            paramz[16] = new DSEventParamValue() { IdEventParam = 3766, Value = issue.JiraId, DockNr = 0 }; ;
            paramz[17] = new DSEventParamValue() { IdEventParam = 6816, Value = issue.CzyOnCall, DockNr = 0 }; ;
            paramz[18] = new DSEventParamValue() { IdEventParam = 6828, Value = issue.SrodowiskoProblemu, DockNr = 0 }; ;

            ReturnedValueOfInt32 result = clientWFS.NewInsertIssueWithLogin(null, EM_ZAKLADANIE_SPRAWY_BILLING, paramz, user.login, user.password);

            return result.Param;
        }

        /// <summary>
        /// Pobiera dostepne kroki dla danej sprawy BPM
        /// </summary>
        /// <param name="user">zalogowany użytkownik</param>
        /// <param name="issueId">id sprawy BPM</param>
        /// <param name="userId">id użytkownika</param>
        /// <returns></returns>
        public ResultValue<Dictionary<int, string>> GetActionForIssue(Entities.User user, int issueId, int userId)
        {
            if (!CheckUserCredentials())
                return null;

            Dictionary<int, string> moves = new Dictionary<int, string>();
            SqlDataReader rdr = null;
            
            try
            {
                using (SqlConnection conn = SetConnection())
                {
                    SqlCommand command = new SqlCommand(
                    "GetActionsForIssue", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(
                            new SqlParameter("@issueId", issueId));
                    command.Parameters.Add(
                            new SqlParameter("@userId", userId));
                    rdr = command.ExecuteReader();

                    while (rdr.Read())
                    {
                        moves.Add(rdr.GetInt32(0), rdr.GetString(2));
                    }
                }
            }
            catch (Exception e)
            {
                return new ResultValue<Dictionary<int, string>>(moves, e);
            }

            return new ResultValue<Dictionary<int, string>>(moves);
        }

        public ResultValue<Dictionary<KeyValuePair<int, string>, string>> CheckIssuesPresenceOnBpm(Entities.User user, List<BillingDTHIssueWFS> issues)
        {
            if (!CheckUserCredentials())
                return new ResultValue<Dictionary<KeyValuePair<int, string>, string>>(null, false);

            Dictionary<KeyValuePair<int, string>, string> list = new Dictionary<KeyValuePair<int, string>, string>();

            if (issues.Count == 0)
                return new ResultValue<Dictionary<KeyValuePair<int, string>, string>>(new Dictionary<KeyValuePair<int, string>, string>());

            string jiraIds = string.Empty;
            string jiraNumbers = string.Empty;

            for (int i = 0; i < issues.Count; i++)
            {
                jiraIds += issues[i].JiraId;
                jiraNumbers += issues[i].NumerZgloszenia;

                if (i < issues.Count - 1)
                {
                    jiraIds += ',';
                    jiraNumbers += ',';
                }
            }

            if (jiraIds != string.Empty)
            {
                List<List<string>> getByJiraId = this.ExecuteStoredProcedure(user, "spCheckJiraId", new string[] { jiraIds }, DatabaseName.SupportCP).GetResult();

                foreach (var item in getByJiraId)
                {
                    list.Add(new KeyValuePair<int, string>(Int32.Parse(item[1]), item[2]), item[0]);
                }
            }

            
            if (jiraNumbers != string.Empty)
            {
                List<List<string>> getByJiraNumber = this.ExecuteStoredProcedure(user, "Polsat_GetBillingMatchingHeliosNumbersWithIssueId", new string[] { jiraNumbers }, DatabaseName.SupportADDONS).GetResult();

                foreach (var item in getByJiraNumber)
                {
                    if (list.Where(x => x.Key.Key.Equals(Int32.Parse(item[1]))).Count() == 0)
                    {
                        list.Add(new KeyValuePair<int, string>(Int32.Parse(item[1]), item[2]), item[0]);
                    }
                }
            }

            return new ResultValue<Dictionary<KeyValuePair<int, string>, string>>(list);
        }

        /// <summary>
        /// Pobiera numery zgłoszeń z Jira dodane do BPM
        /// </summary>
        /// <param name="issues">tablica numerów zgłoszeń</param>
        /// <returns></returns>
        public ResultValue<Dictionary<KeyValuePair<int, string>, string>> CheckBillingIssuesPresenceOnWFS(Entities.User user, string[] issues)
        {
            if (!CheckUserCredentials())
                return new ResultValue<Dictionary<KeyValuePair<int, string>, string>>(null, false);
            Dictionary<KeyValuePair<int, string>, string> list = new Dictionary<KeyValuePair<int, string>, string>();
            int n = issues.Length;
            if (n == 0)
            {
                return new ResultValue<Dictionary<KeyValuePair<int, string>, string>>(new Dictionary<KeyValuePair<int, string>, string>());
            }
            String[] result = new String[n];

            try
            {
                using (SqlConnection thisConnection = SetConnection(ADDONS))
                {
                    SqlDataReader thisReader = null;
                    try
                    {
                        List<string> matchingIssues = new List<string>();
                        SqlCommand thisCommand = thisConnection.CreateCommand();
                        thisCommand.CommandText = "exec Polsat_GetBillingMatchingHeliosNumbersWithIssueId '" + string.Join(",", issues) + "'";
                        thisReader = thisCommand.ExecuteReader();
                        int i = 0;
                        while (thisReader.Read() && i < n)
                        {
                            //matchingIssues.Add(thisReader["HeliiosNR"].ToString());
                            list.Add(new KeyValuePair<int, string>(thisReader.GetInt32(1), thisReader.GetString(2)), thisReader.GetString(0));
                        }
                        thisReader.Close();

                        thisConnection.Close();
                        return new ResultValue<Dictionary<KeyValuePair<int, string>, string>>(list);
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        if (thisReader != null)
                            thisReader.Close();
                        if (thisConnection != null)
                            thisConnection.Close();
                    }
                }
            }
            catch (Exception e)
            {
                return new ResultValue<Dictionary<KeyValuePair<int, string>, string>>(e);
            }
        }
        
        /// <summary>
        /// Wykonuje procedure składowaną na bazie zwracając Liste wierszy w postaci listy stringow, nie ma ograniczeń co do ilości kolumn
        /// </summary>
        /// <param name="user"></param>
        /// <param name="procedureName">Nazwa procedury</param>
        /// <param name="parameters">lista parametrów</param>
        /// <param name="database">Baza danych</param>
        /// <returns></returns>
        private ResultValue<List<List<string>>> ExecStoredProcedure(Entities.User user, string procedureName, string[] parameters, string database = "")
        {
            if (!CheckUserCredentials())
                return new ResultValue<List<List<string>>>(null, false);
            List<List<string>> result = new List<List<string>>();
            try
            {
                using (SqlConnection thisConnection = SetConnection())
                {
                    SqlDataReader thisReader = null;
                    try
                    {
                        SqlCommand thisCommand = thisConnection.CreateCommand();
                        thisCommand.CommandText = ((database == "") ? "" : "USE " + database) + " exec " + procedureName;
                        if (parameters != null)
                        {
                            thisCommand.CommandText += " ";
                            for (int i = 0; i < parameters.Length; i++)
                            {
                                thisCommand.CommandText += "'" + parameters[i] + "'";
                                if (i < parameters.Length - 1)
                                {
                                    thisCommand.CommandText += ",";
                                }
                                else break;
                            }
                        }
                        thisReader = thisCommand.ExecuteReader();

                        List<string> pom;// = new List<string>();
                        while (thisReader.Read())
                        {
                            pom = new List<string>();
                            for (int i = 0; i < thisReader.FieldCount; i++)
                            {
                                pom.Add(thisReader.GetValue(i).ToString());
                            }
                            result.Add(pom);
                        }
                        thisReader.Close();

                        thisConnection.Close();
                        return new ResultValue<List<List<string>>>(result);
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        if (thisReader != null)
                            thisReader.Close();
                        if (thisConnection != null)
                            thisConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResultValue<List<List<string>>>(ex);
            }
        }

        /// <summary>
        /// Wykonuje procedurę składowaną na bazie.
        /// </summary>
        /// <param name="user">zalogowany użytkoniwk</param>
        /// <param name="procedureName">nazwa procedury</param>
        /// <param name="parameters">tablica parametrów</param>
        /// <param name="database">baza danych, na której ma zostać wykonana procedura.</param>
        /// <returns></returns>
        public ResultValue<List<List<string>>> ExecuteStoredProcedure(User user, string procedureName, string[] parameters, DatabaseName database = DatabaseName.None)
        {
            string databaseName = string.Empty;
            System.Diagnostics.Debug.WriteLine(ConfigurationManager.AppSettings["WFSDBDatabase"]);
            switch(database)
            {
                case Entities.Enums.DatabaseName.SupportADDONS: databaseName = ConfigurationManager.AppSettings["SupportADDONS"]; break;
                case Entities.Enums.DatabaseName.SupportBPM: databaseName = ConfigurationManager.AppSettings["WFSDBDatabase"]; break;
                case Entities.Enums.DatabaseName.SupportCP: databaseName = ConfigurationManager.AppSettings["SupportCP"]; break;
                case Entities.Enums.DatabaseName.SupportDSDB: databaseName = ConfigurationManager.AppSettings["DSDBDatabase"]; break;
                default: databaseName = string.Empty; break;
            }

            return this.ExecStoredProcedure(user, procedureName, parameters, databaseName);
        }
        
        /// <summary>
        /// Pobiera listę komponentow dla zadanego id komponentu-rodzica
        /// </summary>
        /// <param name="user">zalogowany użytkownik</param>
        /// <param name="id">id komponentu-rodzica</param>
        /// <returns></returns>
        public ResultValue<Dictionary<int, string>> GetBillingComponents(Entities.User user, int id)
        {
            Dictionary<int, string> data = new Dictionary<int, string>();

            try
            {
                using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ADDONS"].ConnectionString))
                {
                    string sql =
                    "USE " + ConfigurationManager.AppSettings["SupportADDONS"] + " " +
                    "exec BillingDTH_GetErrorsDictionary N'" + id + "'";

                    con.Open();

                    using (SqlCommand com = new SqlCommand(sql, con))
                    {
                        com.CommandType = System.Data.CommandType.Text;

                        SqlDataReader dr = com.ExecuteReader();

                        while (dr.Read())
                        {
                            if (!data.ContainsKey((int)dr["ID"]))
                            {
                                data[(int)dr["ID"]] = dr["Name"].ToString();
                            }
                        }
                    }
                }

                return new ResultValue<Dictionary<int, string>>(data);
            }
            catch (Exception ex)
            {
                return new ResultValue<Dictionary<int, string>>(ex);
            }
        }
        
        /// <summary>
        /// Pobiera listę parametrów do modelowania formatki akcji
        /// </summary>
        /// <param name="user">zalogowany użytkownik</param>
        /// <param name="eventMoveId">id akcji</param>
        /// <returns></returns>
        public ResultValue<List<EventParamModeler>> GetEventParamForFormByEventMove(Entities.User user, int eventMoveId)
        {
            if (!CheckUserCredentials())
                return new ResultValue<List<EventParamModeler>>(null, false);
            string WFSDB_Database = ConfigurationManager.AppSettings["WFSDBDatabase"];
            string DSDB_Database = ConfigurationManager.AppSettings["DSDBDatabase"];
            List<Entities.EventParamModeler> eventParamList = new List<EventParamModeler>();
            SqlDataReader rdr = null;
            try
            {
                using (SqlConnection conn = SetConnection())
                {
                    string query = Queries.EventParamForFormByEventMove(WFSDB_Database, DSDB_Database);
                    SqlCommand command = new SqlCommand(query, conn);

                    command.Parameters.Add(
                            new SqlParameter("@EventMoveId", eventMoveId));
                    rdr = command.ExecuteReader();

                    while (rdr.Read())
                    {
                        Entities.EventParamModeler epm = new EventParamModeler()
                        {
                            EventId = rdr.GetInt32(0),
                            ParamGroupId = rdr.GetInt32(1),
                            EventParamId = rdr.GetInt32(2),
                            Name = rdr.GetString(3),
                            Details = rdr.GetString(4),
                            Width = rdr.GetInt32(5),
                            Height = rdr.GetInt32(6),
                            Top = rdr.GetInt32(7),
                            Left = rdr.GetInt32(8),
                            LabelTop = rdr.GetInt32(9),
                            LabelLeft = rdr.GetInt32(10),
                            TechName = rdr.GetString(11),
                            FriendlyName = rdr.GetString(12),
                            SoudceId = rdr.GetInt32(13),
                            DSName = rdr.GetString(14),
                            Content = rdr.GetString(15),
                            Description = rdr.GetString(16),
                            DBTypeName = rdr.GetString(17),
                            isRequired = rdr.GetBoolean(18),
                            BoundEventParamId = rdr.GetInt32(19)
                        };
                        eventParamList.Add(epm);
                    }
                }
            }
            catch (Exception e)
            {
                return new ResultValue<List<EventParamModeler>>(e);
            }
            return new ResultValue<List<EventParamModeler>>(eventParamList);
        }
        
        /// <summary>
        /// Pobiera akutalne parametry sprawy, dla zadanych parametrów związanych
        /// </summary>
        /// <param name="user">zalogowany użytkownik</param>
        /// <param name="issueId">id sprawy</param>
        /// <param name="eventParamId">id parametrów związanych</param>
        /// <returns></returns>
        public ResultValue<List<EventParam>> GetBillingBoundEventParamForIssue(Entities.User user, int issueId, int[] eventParamId)
        {
            if (!CheckUserCredentials())
                return new ResultValue<List<EventParam>>(null, false);
            List<EventParam> boundEventParams = new List<EventParam>();
            if (eventParamId != null && eventParamId.Length > 0)
            {
                SqlDataReader rdr = null;
                try
                {
                    using (SqlConnection conn = SetConnection())
                    {
                        string tmpString = "";
                        for (int i = 0; i < eventParamId.Length; i++)
                        {
                            tmpString += eventParamId[i];
                            if (i < eventParamId.Length - 1) tmpString += ",";
                        }
                        string query = Queries.BillingBoundEventParamForIssue(tmpString);
                        SqlCommand command = new SqlCommand(query, conn);

                        command.Parameters.Add(
                                new SqlParameter("@IssueId", issueId));
                        try
                        {
                            rdr = command.ExecuteReader();
                            while (rdr.Read())
                            {
								Entities.EventParam tmp = new Entities.EventParam()
								{
									EventParamId = (int)(rdr[0] ?? -1),
									Value = (rdr[1] ?? string.Empty).ToString(),
									DBValue = (int)(rdr[2] ?? -1),
									DBExtValue = (rdr[3] ?? string.Empty).ToString()
								};
                                boundEventParams.Add(tmp);
                            }
                        }
                        catch
                        {
                            throw;
                        }
                        finally
                        {
                            if (rdr != null && !rdr.IsClosed)
                            {
                                rdr.Close();
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    return new ResultValue<List<EventParam>>(e);
                }
            }
            return new ResultValue<List<EventParam>>(boundEventParams);
        }

        /// <summary>
        /// Pobiera parametry powiązane z danymi parametrami dla sprawy BPM
        /// </summary>
        /// <param name="user">zalogowany użytkownik</param>
        /// <param name="issueId">id sprawy</param>
        /// <param name="eventParamId">id parametrów</param>
        /// <returns></returns>
        public ResultValue<List<EventParam>> GetBoundEventParamForIssue(Entities.User user, int issueId, int[] eventParamId)
        {
            if (!CheckUserCredentials())
                return new ResultValue<List<EventParam>>(null, false);
            List<EventParam> boundEventParams = new List<EventParam>();
            if (eventParamId != null && eventParamId.Length > 0)
            {
                SqlDataReader rdr = null;
                try
                {
                    using (SqlConnection conn = SetConnection())
                    {
                        string tmpString = "";
                        for (int i = 0; i < eventParamId.Length; i++)
                        {
                            tmpString += eventParamId[i];
                            if (i < eventParamId.Length - 1) tmpString += ",";
                        }
                        string query = Queries.BoundEventParamForIssue(tmpString);
                        SqlCommand command = new SqlCommand(query, conn);

                        command.Parameters.Add(
                                new SqlParameter("@IssueId", issueId));
                        try
                        {
                            rdr = command.ExecuteReader();
                            while (rdr.Read())
                            {
                                Entities.EventParam tmp = new Entities.EventParam()
                                {
                                    EventParamId = rdr.GetInt32(0),
                                    Value = rdr.GetString(1),
                                    DBValue = rdr.GetInt32(2),
                                    DBExtValue = rdr.GetString(3)
                                };
                                boundEventParams.Add(tmp);
                            }
                        }
                        catch
                        {
                            throw;
                        }
                        finally
                        {
                            if (rdr != null && !rdr.IsClosed)
                            {
                                rdr.Close();
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    return new ResultValue<List<EventParam>>(e);
                }
            }
            return new ResultValue<List<EventParam>>(boundEventParams);
        }

        /// <summary>
        /// Powiadamia webService, że sesja powinna zostać podtrzymana, zapobiegając zakończeniu sesji po czasie.
        /// </summary>
        /// <returns></returns>
        public ResultValue<bool> Notify()
        {
            Console.WriteLine("notify");
            return new ResultValue<bool>(true);
        }

        #region Uzytkownicy Helios
        /// <summary>
        /// Dodaje nowego użytkownika Jira do bazy danych
        /// </summary>
        /// <param name="user">użytkownik do dodania</param>
        /// <returns></returns>
        public ResultValue<bool> AddUser(HeliosUser user)
        {
            SqlConnection thisConnection = null;
            try
            {
                using (thisConnection = SetConnection(CPUSERS))
                {

                    user.telefon = user.telefon.Length != 0 ? user.telefon : "";
                    SqlCommand insertCommand = new SqlCommand(
                    "INSERT INTO [Users]([email] ,[firstname],[surname],[phone])VALUES" +
                        "('" + user.email +
                        "','" + user.imie +
                        "','" + user.nazwisko +
                        "','" + user.telefon + "')",
                        thisConnection);
                    insertCommand.ExecuteNonQuery();

                }
                thisConnection.Close();
                return new ResultValue<bool>(true);
            }
            catch (Exception ex)
            {
                return new ResultValue<bool>(ex);
            }
        }

        /// <summary>
        /// Pobiera wszystkich użytkowników Jira
        /// </summary>
        /// <returns></returns>
        public ResultValue<List<HeliosUser>> GetAllUsers()
        {
            SqlConnection thisConnection = null;
            try
            {
                List<HeliosUser> users = new List<HeliosUser>();
                using (thisConnection = SetConnection(CPUSERS))
                {
                    SqlCommand writeCommand = new SqlCommand(Queries.ALL_USERS, thisConnection);
                    SqlDataReader rd = null;

                    rd = writeCommand.ExecuteReader();
                    while (rd.Read())
                    {
                        HeliosUser user = new HeliosUser();
                        user.email = rd.GetString(1);
                        user.imie = rd.GetString(2);
                        user.nazwisko = rd.GetString(3);
                        user.telefon = rd.GetString(4);
                        users.Add(user);
                    }
                    rd.Close();
                }
                thisConnection.Close();
                return new ResultValue<List<HeliosUser>>(users);
            }
            catch (Exception ex)
            {
                return new ResultValue<List<HeliosUser>>(ex);
            }
        }

        /// <summary>
        /// Wyszukuje użytkownika Jira 
        /// </summary>
        /// <param name="email">email wyszukiwanego użytkownika</param>
        /// <returns></returns>
        public ResultValue<HeliosUser> SearchUserByEmail(string email)
        {
            SqlConnection thisConnection = null;
            try
            {
                HeliosUser user = null;
                using (thisConnection = SetConnection(CPUSERS))
                {

                    SqlCommand writeCommand = new SqlCommand("Select * from Users where email like '%" + email + "%'", thisConnection);
                    SqlDataReader rd = null;

                    rd = writeCommand.ExecuteReader();
                    while (rd.Read())
                    {
                        user = new HeliosUser();
                        user.email = rd.GetString(1);
                        user.imie = rd.GetString(2);
                        user.nazwisko = rd.GetString(3);
                        user.telefon = rd.GetString(4);
                    }

                }
                thisConnection.Close();
                return new ResultValue<HeliosUser>(user);
            }
            catch (Exception ex)
            {
                return new ResultValue<HeliosUser>(ex);

            }
        }

        /// <summary>
        /// Nadpisuje dane użytkownika Jira
        /// </summary>
        /// <param name="user">użytkownik do nadpisania</param>
        /// <returns></returns>
        public ResultValue<bool> UpdateUser(HeliosUser user)
        {
            using (SqlConnection thisConnection = SetConnection(CPUSERS))
            {
                try
                {
                    SqlCommand insertCommand = new SqlCommand(
                    "UPDATE " + ConfigurationManager.AppSettings["SupportCP"] + ".[dbo].[Users]" +
                    "SET [phone] = '" + user.telefon +
                    "',[firstname] = '" + user.imie +
                    "',[surname] = '" + user.nazwisko + "'" +
                    "WHERE [email]='" + user.email + "'",
                        thisConnection);
                    insertCommand.ExecuteNonQuery();
                    return new ResultValue<bool>(true);
                }
                catch (Exception ex)
                {
                    return new ResultValue<bool>(false, ex);
                }
                finally
                {
                    thisConnection.Close();
                }

            }
        }
        #endregion

        #region notatki DB
        /// <summary>
        /// Dodaje nową notatkę do bazy danych
        /// </summary>
        /// <param name="note">notatka</param>
        /// <returns></returns>
        public ResultValue<bool> AddNote(Note note)
        {
            SqlConnection thisConnection = null;
            try
            {
                using (thisConnection = SetConnection(CPUSERS))
                {

                    SqlCommand insertCommand = new SqlCommand(
                    "INSERT INTO [Notes]([issueNr],[author],[date],[content]) VALUES " +
                        "('" + note.issueNumber +
                        "','" + note.author +
                        "','" + note.date +
                        "','" + note.content + "')",
                        thisConnection);
                    insertCommand.ExecuteNonQuery();


                }
                thisConnection.Close();
                return new ResultValue<bool>(true);
            }
            catch (Exception ex)
            {
                if (thisConnection != null)
                    thisConnection.Close();
                return new ResultValue<bool>(false, ex);
            }
        }

        /// <summary>
        /// Nadpisuje notatkę
        /// </summary>
        /// <param name="note">notatka</param>
        /// <returns></returns>
        public ResultValue<bool> UpdateNote(Note note)
        {
            SqlConnection thisConnection = null;
            try
            {
                using (thisConnection = SetConnection())
                {

                    SqlCommand insertCommand = new SqlCommand(
                    "UPDATE " + ConfigurationManager.AppSettings["SupportCP"] + ".[dbo].[Notes]" +
                    "SET [date] = '" + note.date +
                    "',[content] = '" + note.content + "'" +
                    "WHERE [issueNr]='" + note.issueNumber + "'",
                        thisConnection);
                    insertCommand.ExecuteNonQuery();
                }
                thisConnection.Close();
                return new ResultValue<bool>(true);
            }
            catch (Exception ex)
            {
                if (thisConnection != null)
                    thisConnection.Close();
                return new ResultValue<bool>(false, ex);
            }
        }

        /// <summary>
        /// Wyszukuje notatki dla zgłoszenia
        /// </summary>
        /// <param name="issuenumber">numer zgłoszenia Jira</param>
        /// <returns></returns>
        public ResultValue<Note> SearchIssueNote(string issuenumber)
        {

            SqlConnection thisConnection = null;
            Note note = null;
            try
            {
                using (thisConnection = SetConnection(CPUSERS))
                {

                    SqlCommand writeCommand = new SqlCommand("Select * from Notes where issueNr = '" + issuenumber + "'", thisConnection);
                    SqlDataReader rd = null;

                    rd = writeCommand.ExecuteReader();
                    while (rd.Read())
                    {
                        note = new Note();
                        note.issueNumber = rd.GetString(0);
                        note.author = rd.GetString(1);
                        note.date = rd.GetDateTime(2).ToString();
                        note.content = rd.GetString(3);
                    }
                }
                thisConnection.Close();
            }
            catch (Exception ex)
            {
                if (thisConnection != null)
                    thisConnection.Close();
                return new ResultValue<Note>(ex);
            }
            return new ResultValue<Note>(note);
        }

        /// <summary>
        /// Wyszukuje notatki dla wielu zgłoszeń
        /// </summary>
        /// <param name="issueNumbers">numery zgłoszeń Jira</param>
        /// <returns></returns>
        public ResultValue<List<Note>> SearchIssueNotes(List<string> issueNumbers)
        {
            SqlConnection thisConnection = null;
            List<Note> notes = new List<Note>(issueNumbers.Count);
            string numbers = "'";
            foreach (string item in issueNumbers)
            {
                numbers += item + "','";
            }
            if (numbers.Length > 2)
            {
                numbers = numbers.Remove(numbers.Length - 2);
                try
                {
                    using (thisConnection = SetConnection(CPUSERS))
                    {

                        SqlCommand writeCommand = new SqlCommand("Select * from Notes where issueNr IN (" + numbers + ")", thisConnection);
                        SqlDataReader rd = null;

                        rd = writeCommand.ExecuteReader();
                        while (rd.Read())
                        {
                            Note note = new Note();
                            note.issueNumber = rd.GetString(0);
                            note.author = rd.GetString(1);
                            note.date = rd.GetDateTime(2).ToString();
                            note.content = rd.GetString(3);
                            notes.Add(note);
                        }
                    }
                    thisConnection.Close();
                }
                catch (Exception ex)
                {
                    if (thisConnection != null)
                        thisConnection.Close();
                    return new ResultValue<List<Note>>(ex);
                }
            }
            return new ResultValue<List<Note>>(notes);
        }
        #endregion

        #region ProcessManager
        /// <summary>
        /// Pobiera typy błędów procesów
        /// </summary>
        /// <param name="user">zalogowany użytkownik</param>
        /// <returns></returns>
        public ResultValue<List<ErrorType>> GetErrorTypes(Entities.User user)
        {
            if (!CheckUserCredentials())
                return new ResultValue<List<ErrorType>>(null, false);

            List<ErrorType> errorTypes = new List<ErrorType>();

            try
            {
                List<List<string>> list = this.ExecuteStoredProcedure(user, "spGetErrorTypes", new string[] { }, DatabaseName.SupportCP).GetResult();

                foreach (var item in list)
                {
                    ErrorType et = new ErrorType()
                    {
                        Id = int.Parse(item[0]),
                        Name = item[1]
                    };

                    errorTypes.Add(et);
                }
            }
            catch(Exception ex)
            {
                return new ResultValue<List<ErrorType>>(ex);
            }

            return new ResultValue<List<ErrorType>>(errorTypes);
        }

        /// <summary>
        /// Pobiera rodzaje procesów
        /// </summary>
        /// <param name="user">zalogowany użytkownik</param>
        /// <returns></returns>
        public ResultValue<List<Process>> GetProcesses(Entities.User user)
        {
            if (!CheckUserCredentials())
                return new ResultValue<List<Process>>(null, false);

            List<Process> processes = new List<Process>();

            try
            {
                List<List<string>> list = this.ExecuteStoredProcedure(user, "spGetProcesses", new string[] { }, DatabaseName.SupportCP).GetResult();

                foreach (var item in list)
                {
                    Process p = new Process()
                    {
                        Id = int.Parse(item[0]),
                        Name = item[1],
                        PokazWGui = bool.Parse(item[2])
                    };

                    processes.Add(p);
                }
            }
            catch(Exception ex)
            {
                return new ResultValue<List<Process>>(ex);
            }

            return new ResultValue<List<Process>>(processes);
        }

        /// <summary>
        /// Pobiera rodzaje błędów
        /// </summary>
        /// <param name="user">zalogowany użytkownik</param>
        /// <param name="processId">id procesu - parametr opcjonalny</param>
        /// <returns></returns>
        public ResultValue<List<Error>> GetErrors(Entities.User user, int processId = -1)
        {
            if (!CheckUserCredentials())
                return new ResultValue<List<Error>>(null, false);

            List<Error> errors = new List<Error>();

            try
            {
                List<List<string>> list;

                if (processId == -1)
                    list = this.ExecuteStoredProcedure(user, "spNewGetErrors", new string[] { }, DatabaseName.SupportCP).GetResult();
                else
                    list = this.ExecuteStoredProcedure(user, "spNewGetErrors", new string[] { processId.ToString() }, DatabaseName.SupportCP).GetResult();

                foreach (var item in list)
                {
                    Error e = new Error()
                    {
                        Id = int.Parse(item[0]),
                        ErrorTypeId = int.Parse(item[1]),
                        Description = item[2],
                        DescriptionFull = item[3]
                    };

                    errors.Add(e);
                }
            }
            catch(Exception ex)
            {
                return new ResultValue<List<Error>>(ex);
            }

            return new ResultValue<List<Error>>(errors);
        }

        /// <summary>
        /// Pobiera rodzaje rozwiązań
        /// </summary>
        /// <param name="user">zalogowany użytkownik</param>
        /// <returns></returns>
        public ResultValue<List<Solution>> GetSolutions(Entities.User user)
        {
            if (!CheckUserCredentials())
                return new ResultValue<List<Solution>>(null, false);

            List<Solution> solutions = new List<Solution>();

            try
            {
                List<List<string>> list = this.ExecuteStoredProcedure(user, "spGetSolutions", new string[] { }, DatabaseName.SupportCP).GetResult();

                foreach (var item in list)
                {
                    Solution s = new Solution()
                    {
                        Id = int.Parse(item[0]),
                        NameCP = item[1],
                        NameBussiness = item[2]
                    };

                    solutions.Add(s);
                }
            }
            catch(Exception ex)
            {
                return new ResultValue<List<Solution>>(ex);
            }

            return new ResultValue<List<Solution>>(solutions);
        }

        /// <summary>
        /// Dodaje do bazy danych nowy rodzaj procesu
        /// </summary>
        /// <param name="user">zalogowany użytkownik</param>
        /// <param name="process">nowy proces</param>
        /// <returns></returns>
        public ResultValue<Process> CreateNewProcess(User user, Process process)
        {
            if (!CheckUserCredentials())
                return new ResultValue<Process>(null, false);

            Process p = process;

            try
            {
                List<List<string>> result = this.ExecuteStoredProcedure(user, "spCreateNewProcess", new string[] { p.Name }, DatabaseName.SupportCP).GetResult();

                if (result.Count < 1)
                    throw new InvalidOperationException("Błąd tworzenia procesu.");
                if (result[0].Count < 2)
                    throw new InvalidOperationException("Błąd tworzenia procesu.");

                foreach (var item in result)
                {
                    int id = int.Parse(item[0]);

                    p.Id = id;
                    p.PokazWGui = true;
                }

                return new ResultValue<Process>(p);
            }
            catch(Exception ex)
            {
                return new ResultValue<Process>(ex);
            }
        }

        /// <summary>
        /// Dodaje nowy rodzaj błędu do bazy danych
        /// </summary>
        /// <param name="user">zalogowany użytkownik</param>
        /// <param name="error">nowy błąd</param>
        /// <returns></returns>
        public ResultValue<Error> CreateNewError(User user, Error error)
        {
            if (!CheckUserCredentials())
                return new ResultValue<Error>(null, false);

            Error e = error;

            try
            {
                List<List<string>> result = this.ExecuteStoredProcedure(user, "spCreateNewError", 
                    new string[] { error.ErrorTypeId.ToString(), error.Description, error.DescriptionFull }, DatabaseName.SupportCP).GetResult();

                if (result.Count < 1)
                    throw new InvalidOperationException("Błąd tworzenia nowego błędu.");
                if (result[0].Count < 5)
                    throw new InvalidOperationException("Błąd tworzenia nowego błędu.");

                foreach (var item in result)
                {
                    int id = int.Parse(item[0]);

                    e.Id = id;
                }

                return new ResultValue<Error>(e);
            }
            catch(Exception ex)
            {
                return new ResultValue<Error>(ex);
            }
        }

        /// <summary>
        /// Dodaje nowy rodzaj rozwiązania do bazy danych
        /// </summary>
        /// <param name="user">zalogowany użytkownik</param>
        /// <param name="solution">nowe rozwiązanie</param>
        /// <returns></returns>
        public ResultValue<Solution> CreateNewSolution(User user, Solution solution)
        {
            if (!CheckUserCredentials())
                return new ResultValue<Solution>(null, false);

            Solution s = solution;

            try
            {
                List<List<string>> result = this.ExecuteStoredProcedure(user, "spCreateNewSolution", new string[] { s.NameCP, s.NameBussiness }, DatabaseName.SupportCP).GetResult();


                if (result.Count < 1)
                    throw new InvalidOperationException("Błąd tworzenia nowego rozwiązania.");
                if (result[0].Count < 3)
                    throw new InvalidOperationException("Błąd tworzenia nowego rozwiązania.");

                foreach (var item in result)
                {
                    int id = int.Parse(item[0]);

                    s.Id = id;
                }

                return new ResultValue<Solution>(s);
            }
            catch(Exception ex)
            {
                return new ResultValue<Solution>(ex);
            }
        }

        /// <summary>
        /// Tworzy powiązanie pomiędzy procesem a błędami wskazanymi na liście
        /// </summary>
        /// <param name="user">zalogowany użytkownik</param>
        /// <param name="process">proces</param>
        /// <param name="errors">lista błędów</param>
        /// <param name="delete">czy usunąć (tak, jeśli true)</param>
        public bool BoundErrorWithProcess(User user, Process process, List<Error> errors, bool delete = false)
        {
            if (!CheckUserCredentials())
                return false;
 
            try
            {
                string errorsList = string.Empty;

                for (int i = 0; i < errors.Count; i++)
                {
                    errorsList += errors[i].Id.ToString();

                    if (i < errors.Count - 1)
                        errorsList += ",";
                }

                this.ExecuteStoredProcedure(user, "spBoundErrorWithProcess", new string[] { process.Id.ToString(), errorsList, delete ? "1" : "0" }, DatabaseName.SupportCP);

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Dodaje nowe wpisy z naprawy procesów do bazy danych
        /// </summary>
        /// <param name="user">zalogowany użytkownik</param>
        /// <param name="inXml">xml wejściowy</param>
        /// <returns></returns>
        public bool InsertNewProcessLog(User user, string inXml)
        {
            if (!CheckUserCredentials())
                return false;

            try
            {
                this.ExecuteStoredProcedure(user, "spInsertNewProcessLog", new string[] { inXml }, DatabaseName.SupportCP);

                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}
