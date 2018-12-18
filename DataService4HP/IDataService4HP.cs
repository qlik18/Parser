using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Entities;
using Entities.Enums;

namespace DataService4HP
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract(SessionMode=SessionMode.Required)]
    public interface IDataService4HP
    {
        [OperationContract]
        ResultValue<Dictionary<int, string>> GetActionForIssue(User user,int issueId, int userId);

        [OperationContract]
        ResultValue<Dictionary<KeyValuePair<int, string>, string>> CheckBillingIssuesPresenceOnWFS(User user, string[] issues);

        [OperationContract]
        ResultValue<Dictionary<KeyValuePair<int, string>, string>> CheckIssuesPresenceOnBpm(Entities.User user, List<BillingDTHIssueWFS> issues);

        [OperationContract]
        ResultValue<List<List<string>>> ExecuteStoredProcedure(User user, string procedureName, string[] parameters, DatabaseName database = DatabaseName.None);
        
        [OperationContract]
        ResultValue<Dictionary<int, string>> GetBillingComponents(Entities.User user, int id);
        
        [OperationContract]
        ResultValue<List<EventParamModeler>> GetEventParamForFormByEventMove(User user, int eventMoveId);

        [OperationContract]
        ResultValue<List<EventParam>> GetBoundEventParamForIssue(User user, int issueId, int[] eventParamId);
        
        [OperationContract]
        ResultValue<List<EventParam>> GetBillingBoundEventParamForIssue(User user, int issueId, int[] eventParamId);

        [OperationContract]
        int DoActionInIssue(int? idIssue, int eventMoveId, List<EventParam> paramz, User u);

        [OperationContract]
        int NewBillingIssue(BillingDTHIssueWFS issue, User user);

        #region Session context
        [OperationContract(IsInitiating = true, IsTerminating = false)]
        User LogIn2(string login, string pass);

        [OperationContract(IsInitiating=false, IsTerminating=true)]
        void LogOut();
        #endregion

        [OperationContract]
        ResultValue<bool> Notify();

        #region Uzytkownicy Helios
        [OperationContract]
        ResultValue<bool> AddUser(HeliosUser user);

        [OperationContract]
        ResultValue<List<HeliosUser>> GetAllUsers();

        [OperationContract]
        ResultValue<HeliosUser> SearchUserByEmail(string email);

        [OperationContract]
        ResultValue<bool> UpdateUser(HeliosUser user);
        #endregion

        #region Notatki
        [OperationContract]
        ResultValue<bool> AddNote(Note note);

        [OperationContract]
        ResultValue<bool> UpdateNote(Note note);

        [OperationContract]
        ResultValue<Note> SearchIssueNote(string issuenumber);

        [OperationContract]
        ResultValue<List<Note>> SearchIssueNotes(List<string> issueNumbers);
        #endregion

        #region ProcessManager
        [OperationContract]
        ResultValue<List<ErrorType>> GetErrorTypes(Entities.User user);

        [OperationContract]
        ResultValue<List<Process>> GetProcesses(Entities.User user);

        [OperationContract]
        ResultValue<List<Error>> GetErrors(Entities.User user, int processId = -1);

        [OperationContract]
        ResultValue<List<Solution>> GetSolutions(Entities.User user);

        [OperationContract]
        ResultValue<Process> CreateNewProcess(User user, Process process);

        [OperationContract]
        ResultValue<Error> CreateNewError(User user, Error error);

        [OperationContract]
        ResultValue<Solution> CreateNewSolution(User user, Solution solution);

        [OperationContract]
        bool BoundErrorWithProcess(User user, Process process, List<Error> errors, bool delete = false);

        [OperationContract]
        bool InsertNewProcessLog(User user, string inXml);
        #endregion
    }
}
