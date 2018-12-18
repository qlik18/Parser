using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;
using Entities.Enums;
using DataLayer.HPDataService;

namespace DataLayer.Interface
{
    public interface IGatewayDaoWFS : INotesManager, IUsersHelios, IProcessManagerGateway
    {
        /// <summary>
        /// Pobiera numery zgłoszeń z Jira dodane do BPM
        /// </summary>
        /// <param name="issues">tablica numerów zgłoszeń</param>
        /// <returns></returns>
        Dictionary<KeyValuePair<int, string>, string> CheckBillingIssuesPresenceOnWFS(string[] issues);

        Dictionary<KeyValuePair<int, string>, string> CheckIssuesPresenceOnBpm(List<BillingIssueDtoHelios> issues);

        /// <summary>
        /// Pobiera listę komponentow dla zadanego id komponentu-rodzica
        /// </summary>
        /// <param name="user">zalogowany użytkownik</param>
        /// <param name="id">id komponentu-rodzica</param>
        /// <returns></returns>
        Dictionary<int, string> GetBillingComponents(Entities.User user, int id);

        /// <summary>
        /// Tworzy nową sprawę w BPM
        /// </summary>
        /// <param name="issue">zgłoszenie Jira</param>
        /// <param name="login"login użytkownika</param>
        /// <param name="haslo">hasło użytkoniwka</param>
        /// <returns></returns>
        int AddBillingIssueToWFS(BillingDTHIssueWFS issue, User user);
        
        /// <summary>
        /// Loguje użytkownika do systemu
        /// </summary>
        /// <param name="user">login użytkownika</param>
        /// <param name="password">hasło użytkownika</param>
        /// <returns></returns>
        User loginToWFSWithUserInfo(string user, string password);

        /// <summary>
        /// Pobiera dostepne kroki dla danej sprawy BPM
        /// </summary>
        /// <param name="issueId">id sprawy BPM</param>
        /// <param name="userId">id użytkownika</param>
        /// <returns></returns>
        Dictionary<int, string> GetActionForIssue(int issueId, int userId);

        /// <summary>
        /// Pobiera listę parametrów do modelowania formatki akcji
        /// </summary>
        /// <param name="eventMoveId">id akcji</param>
        /// <returns></returns>
        List<Entities.EventParamModeler> GetEventParamForFormByEventMove(int eventMoveId);

        /// <summary>
        /// Pobiera parametry powiązane z danymi parametrami dla sprawy BPM
        /// </summary>
        /// <param name="issueId">id sprawy</param>
        /// <param name="eventParamId">id parametrów</param>
        /// <returns></returns>
        List<Entities.EventParam> GetBoundEventParamForIssue(int issueId, int[] eventsParamId);

        /// <summary>
        /// Pobiera akutalne parametry sprawy, dla zadanych parametrów związanych
        /// </summary>
        /// <param name="issueId">id sprawy</param>
        /// <param name="eventParamId">id parametrów związanych</param>
        /// <returns></returns>
        List<Entities.EventParam> GetBillingBoundEventParamForIssue(int issueId, int[] eventsParamId);

        /// <summary>
        /// Wykonuje akcję na sprawie
        /// </summary>
        /// <param name="idIssue">id sprawy</param>
        /// <param name="eventMoveId">id akcji</param>
        /// <param name="paramz">parametry akcji</param>
        /// <param name="user">zalogowany użytkownik</param>
        /// <returns></returns>
        int DoActionInIssue(int? IssueId, int EventMove, List<EventParam> paramz, User user);

        /// <summary>
        /// Wykonuje procedurę składowaną na bazie.
        /// </summary>
        /// <param name="procedureName">nazwa procedury</param>
        /// <param name="parameters">tablica parametrów</param>
        /// <param name="database">baza danych, na której ma zostać wykonana procedura.</param>
        /// <returns></returns>
        List<List<string>> ExecuteStoredProcedure(string procedureName, string[] parameters, DatabaseName database = DatabaseName.None);

        /// <summary>
        /// Ustawia pole zalogowanego uytkownika
        /// </summary>
        /// <param name="u">zalogowany użytkownik</param>
        void setLoggedUser(User u);

        /// <summary>
        /// Zakańcza sesję użytkownika
        /// </summary>
        void LogOutFromHPService();

        /// <summary>
        /// Powiadamia webService, że sesja powinna zostać podtrzymana, zapobiegając zakończeniu sesji po czasie.
        /// </summary>
        /// <returns></returns>
        void notifyWebService();

        /// <summary>
        /// Zapisuje nowe zgłoszenie Jira w BPM
        /// </summary>
        /// <param name="issue">zgłoszenie Jira</param>
        /// <returns>id nowej sprawy BPM</returns>
        int CreateNewIssue(BillingDTHIssueWFS issue);
    }
}
