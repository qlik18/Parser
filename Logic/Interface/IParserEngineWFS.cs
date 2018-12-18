using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;
using Entities.Enums;
using DataLayer.Interface;
using DataLayer.HPDataService;
using Atlassian.Jira;

namespace LogicLayer.Interface
{

    public interface IParserEngineWFS
    {
        /// <summary>
        /// Pobiera aktualnie zalogowanego użytkownika z systemu Billenium BPM
        /// </summary>
        /// <returns>User</returns>
        User getUser();

        /// <summary>
        /// Loguje użytkownika do systemu
        /// </summary>
        /// <param name="username">login użytkownika</param>
        /// <param name="password">hasło użytkownika</param>
        /// <returns></returns>
        bool loginToWFSWithUserInfo(string username, string password);

        /// <summary>
        /// Wstrzykuje implementację warstwy dostępu do danych. Używana przez kontener unity
        /// </summary>
        /// <param name="dao"></param>
        void setWFSDao(IGatewayDaoWFS dao);

        /// <summary>
        /// Pobiera listę komponentow dla zadanego id komponentu-rodzica
        /// </summary>
        /// <param name="id">id komponentu-rodzica</param>
        /// <returns></returns>
        Dictionary<int, string> getBillingComponents(int id);

        /// <summary>
        /// Tworzy nową sprawę w BPM
        /// </summary>
        /// <param name="issue">zgłoszenie Jira</param>
        /// <param name="WFSIssueId">nowa sprawa BPM</param>
        /// <returns></returns>
        bool addBillingIssueToWFS(BillingIssueDtoHelios issue, out int WFSIssueId);

        /// <summary>
        /// Sprawdzenie czy zgłoszenia zostały już wprowadzone do BPM.
        /// </summary>
        /// <param name="issues">Lista zgłoszeń</param>
        /// <returns></returns>
        List<BillingIssueDtoHelios> compareBillingWithWFS(List<BillingIssueDtoHelios> issues);

        /// <summary>
        /// Sprawdza czy zgłoszenie jest już zapisane do BPM
        /// </summary>
        /// <param name="issue">zgłoszenie do sprawdzenia</param>
        /// <returns></returns>
        bool isInWFS(IssueDtoHelios issue);

        /// <summary>
        /// Pobiera dostepne kroki dla danej sprawy BPM
        /// </summary>
        /// <param name="issueId">id sprawy BPM</param>
        /// <param name="userId">id użytkownika</param>
        /// <returns></returns>
        Dictionary<int, string> GetActionForIssue(int issueId, int userId);

        /// <summary>
        /// Wykonuje akcję na sprawie
        /// </summary>
        /// <param name="idIssue">id sprawy</param>
        /// <param name="eventMoveId">id akcji</param>
        /// <param name="paramz">parametry akcji</param>
        /// <returns></returns>
        int DoActionInIssue(int? IssueId, int EventMove, List<EventParam> paramz);

        /// <summary>
        /// Wykonuje procedurę składowaną na bazie.
        /// </summary>
        /// <param name="procedureName">nazwa procedury</param>
        /// <param name="parameters">tablica parametrów</param>
        /// <param name="database">baza danych, na której ma zostać wykonana procedura.</param>
        /// <returns></returns>
        List<List<string>> ExecuteStoredProcedure(string procedureName, string[] parameters, DatabaseName database = DatabaseName.None);

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
        /// Zakańcza sesję użytkownika
        /// </summary>
        void LogOutFromHPService();

        /// <summary>
        /// Powiadamia webService, że sesja powinna zostać podtrzymana, zapobiegając zakończeniu sesji po czasie.
        /// </summary>
        /// <returns></returns>
        void notifyWebService();

        /// <summary>
        /// Pobiera listę parametrów do modelowania formatki akcji
        /// </summary>
        /// <param name="eventMoveId">id akcji</param>
        /// <returns></returns>
        List<EventParamModeler> GetEventParamForFormByEventMove(int eventMoveId);

        /// <summary>
        /// Sprawdza czy dane zgłoszenie jest już archiwizowane
        /// </summary>
        /// <param name="issueId">id sprawdzanego zgłoszenia</param>
        /// <returns></returns>
        bool IsIssueInArchive(int issueId);

        /// <summary>
        /// Pobiera zgłoszenia z BPM (z pominięciem Jira), przypisane do danego użytkownika.
        /// Używane do zgłoszeń wewnętrznych
        /// </summary>
        /// <param name="userId">id użytkownika</param>
        /// <returns></returns>
        List<BillingIssueDtoHelios> GetIssuesFromBPM(int userId);

        /// <summary>
        /// Aktualizuje dane dotyczące zgłoszenia (pobiera z issueWFS, przypisuje do issueHelios)
        /// </summary>
        /// <param name="issue">Zgłoszenie do aktualizacji</param>
        /// <returns></returns>
        BillingIssueDtoHelios UpdateJiraInfo(BillingIssueDtoHelios issue);

        /// <summary>
        /// Aktualizuje dane dotyczące zgłoszenia (pobiera z issueHelios, przypisuje do issueWFS)
        /// </summary>
        /// <param name="issue">zgłoszenie do aktualizacji</param>
        /// <returns></returns>
        BillingIssueDtoHelios UpdateIssue(BillingIssueDtoHelios issue);
        
        #region ProcessManager Engine
        /// <summary>
        /// Pobiera typy błędów procesów do Process Managera
        /// </summary>
        /// <returns>Lista typów błedów</returns>
        List<ErrorType> GetErrorTypes();

        /// <summary>
        /// Pobiera rodzaje procesów
        /// </summary>
        /// <returns></returns>
        List<Process> GetProcesses();

        /// <summary>
        /// Pobiera rodzaje błędów
        /// </summary>
        /// <param name="processId">opcjonalnie idProcesu, dla którego błedów szukamy</param>
        /// <returns></returns>
        List<Error> GetErrors(int processId = -1);

        /// <summary>
        /// Pobiera rozwiązania
        /// </summary>
        /// <returns></returns>
        List<Solution> GetSolutions();

        /// <summary>
        /// Tworzy nowy rodzaj procesu
        /// </summary>
        /// <param name="process">nowy proces</param>
        /// <returns></returns>
        Process CreateNewProcess(Process process);

        /// <summary>
        /// Tworzy nowy rodzaj błędu
        /// </summary>
        /// <param name="error">nowy błąd</param>
        /// <returns></returns>
        Error CreateNewError(Error error);

        /// <summary>
        /// Tworzy nowe rozwiązanie
        /// </summary>
        /// <param name="solution">nowe rozwiązanie</param>
        /// <returns></returns>
        Solution CreateNewSolution(Solution solution);

        /// <summary>
        /// Powiązuje rodzaj procesu z błędemi
        /// </summary>
        /// <param name="process">proces do którego przypisany ma zostac błąd</param>
        /// <param name="errors">lista błędow do powiązania</param>
        /// <param name="delete">czy usunąć powiązanie? Tak jeśli <c>true</c>, nie w przeciwnym wypadku</param>
        /// <returns></returns>
        bool BoundErrorWithProcess(Entities.Process process, List<Entities.Error> errors, bool delete = false);

        /// <summary>
        /// Zapisuje nowy rekord naprawy procesów
        /// </summary>
        /// <param name="inXml">xml wejściowy</param>
        /// <returns></returns>
        bool InsertNewProcessLog(StringBuilder inXml);
        BillingIssueDtoHelios UpdateJiraInfo(BillingIssueDtoHelios updatedIssue, Issue issue);
        #endregion
    }
}
