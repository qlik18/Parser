﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataLayer.HPDataService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="HPDataService.IDataService4HP", SessionMode=System.ServiceModel.SessionMode.Required)]
    public interface IDataService4HP {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService4HP/GetActionForIssue", ReplyAction="http://tempuri.org/IDataService4HP/GetActionForIssueResponse")]
        Entities.ResultValue<System.Collections.Generic.Dictionary<int, string>> GetActionForIssue(Entities.User user, int issueId, int userId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService4HP/CheckBillingIssuesPresenceOnWFS", ReplyAction="http://tempuri.org/IDataService4HP/CheckBillingIssuesPresenceOnWFSResponse")]
        Entities.ResultValue<System.Collections.Generic.Dictionary<System.Collections.Generic.KeyValuePair<int, string>, string>> CheckBillingIssuesPresenceOnWFS(Entities.User user, string[] issues);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService4HP/CheckIssuesPresenceOnBpm", ReplyAction="http://tempuri.org/IDataService4HP/CheckIssuesPresenceOnBpmResponse")]
        Entities.ResultValue<System.Collections.Generic.Dictionary<System.Collections.Generic.KeyValuePair<int, string>, string>> CheckIssuesPresenceOnBpm(Entities.User user, Entities.BillingDTHIssueWFS[] issues);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService4HP/ExecuteStoredProcedure", ReplyAction="http://tempuri.org/IDataService4HP/ExecuteStoredProcedureResponse")]
        Entities.ResultValue<string[][]> ExecuteStoredProcedure(Entities.User user, string procedureName, string[] parameters, Entities.Enums.DatabaseName database);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService4HP/GetBillingComponents", ReplyAction="http://tempuri.org/IDataService4HP/GetBillingComponentsResponse")]
        Entities.ResultValue<System.Collections.Generic.Dictionary<int, string>> GetBillingComponents(Entities.User user, int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService4HP/GetEventParamForFormByEventMove", ReplyAction="http://tempuri.org/IDataService4HP/GetEventParamForFormByEventMoveResponse")]
        Entities.ResultValue<Entities.EventParamModeler[]> GetEventParamForFormByEventMove(Entities.User user, int eventMoveId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService4HP/GetBoundEventParamForIssue", ReplyAction="http://tempuri.org/IDataService4HP/GetBoundEventParamForIssueResponse")]
        Entities.ResultValue<Entities.EventParam[]> GetBoundEventParamForIssue(Entities.User user, int issueId, int[] eventParamId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService4HP/GetBillingBoundEventParamForIssue", ReplyAction="http://tempuri.org/IDataService4HP/GetBillingBoundEventParamForIssueResponse")]
        Entities.ResultValue<Entities.EventParam[]> GetBillingBoundEventParamForIssue(Entities.User user, int issueId, int[] eventParamId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService4HP/DoActionInIssue", ReplyAction="http://tempuri.org/IDataService4HP/DoActionInIssueResponse")]
        int DoActionInIssue(System.Nullable<int> idIssue, int eventMoveId, Entities.EventParam[] paramz, Entities.User u);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService4HP/NewBillingIssue", ReplyAction="http://tempuri.org/IDataService4HP/NewBillingIssueResponse")]
        int NewBillingIssue(Entities.BillingDTHIssueWFS issue, Entities.User user);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService4HP/LogIn2", ReplyAction="http://tempuri.org/IDataService4HP/LogIn2Response")]
        Entities.User LogIn2(string login, string pass);
        
        [System.ServiceModel.OperationContractAttribute(IsTerminating=true, IsInitiating=false, Action="http://tempuri.org/IDataService4HP/LogOut", ReplyAction="http://tempuri.org/IDataService4HP/LogOutResponse")]
        void LogOut();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService4HP/Notify", ReplyAction="http://tempuri.org/IDataService4HP/NotifyResponse")]
        Entities.ResultValue<bool> Notify();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService4HP/AddUser", ReplyAction="http://tempuri.org/IDataService4HP/AddUserResponse")]
        Entities.ResultValue<bool> AddUser(Entities.HeliosUser user);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService4HP/GetAllUsers", ReplyAction="http://tempuri.org/IDataService4HP/GetAllUsersResponse")]
        Entities.ResultValue<Entities.HeliosUser[]> GetAllUsers();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService4HP/SearchUserByEmail", ReplyAction="http://tempuri.org/IDataService4HP/SearchUserByEmailResponse")]
        Entities.ResultValue<Entities.HeliosUser> SearchUserByEmail(string email);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService4HP/UpdateUser", ReplyAction="http://tempuri.org/IDataService4HP/UpdateUserResponse")]
        Entities.ResultValue<bool> UpdateUser(Entities.HeliosUser user);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService4HP/AddNote", ReplyAction="http://tempuri.org/IDataService4HP/AddNoteResponse")]
        Entities.ResultValue<bool> AddNote(Entities.Note note);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService4HP/UpdateNote", ReplyAction="http://tempuri.org/IDataService4HP/UpdateNoteResponse")]
        Entities.ResultValue<bool> UpdateNote(Entities.Note note);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService4HP/SearchIssueNote", ReplyAction="http://tempuri.org/IDataService4HP/SearchIssueNoteResponse")]
        Entities.ResultValue<Entities.Note> SearchIssueNote(string issuenumber);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService4HP/SearchIssueNotes", ReplyAction="http://tempuri.org/IDataService4HP/SearchIssueNotesResponse")]
        Entities.ResultValue<Entities.Note[]> SearchIssueNotes(string[] issueNumbers);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService4HP/GetErrorTypes", ReplyAction="http://tempuri.org/IDataService4HP/GetErrorTypesResponse")]
        Entities.ResultValue<Entities.ErrorType[]> GetErrorTypes(Entities.User user);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService4HP/GetProcesses", ReplyAction="http://tempuri.org/IDataService4HP/GetProcessesResponse")]
        Entities.ResultValue<Entities.Process[]> GetProcesses(Entities.User user);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService4HP/GetErrors", ReplyAction="http://tempuri.org/IDataService4HP/GetErrorsResponse")]
        Entities.ResultValue<Entities.Error[]> GetErrors(Entities.User user, int processId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService4HP/GetSolutions", ReplyAction="http://tempuri.org/IDataService4HP/GetSolutionsResponse")]
        Entities.ResultValue<Entities.Solution[]> GetSolutions(Entities.User user);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService4HP/CreateNewProcess", ReplyAction="http://tempuri.org/IDataService4HP/CreateNewProcessResponse")]
        Entities.ResultValue<Entities.Process> CreateNewProcess(Entities.User user, Entities.Process process);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService4HP/CreateNewError", ReplyAction="http://tempuri.org/IDataService4HP/CreateNewErrorResponse")]
        Entities.ResultValue<Entities.Error> CreateNewError(Entities.User user, Entities.Error error);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService4HP/CreateNewSolution", ReplyAction="http://tempuri.org/IDataService4HP/CreateNewSolutionResponse")]
        Entities.ResultValue<Entities.Solution> CreateNewSolution(Entities.User user, Entities.Solution solution);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService4HP/BoundErrorWithProcess", ReplyAction="http://tempuri.org/IDataService4HP/BoundErrorWithProcessResponse")]
        bool BoundErrorWithProcess(Entities.User user, Entities.Process process, Entities.Error[] errors, bool delete);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService4HP/InsertNewProcessLog", ReplyAction="http://tempuri.org/IDataService4HP/InsertNewProcessLogResponse")]
        bool InsertNewProcessLog(Entities.User user, string inXml);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IDataService4HPChannel : DataLayer.HPDataService.IDataService4HP, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class DataService4HPClient : System.ServiceModel.ClientBase<DataLayer.HPDataService.IDataService4HP>, DataLayer.HPDataService.IDataService4HP {
        
        public DataService4HPClient() {
        }
        
        public DataService4HPClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public DataService4HPClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public DataService4HPClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public DataService4HPClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public Entities.ResultValue<System.Collections.Generic.Dictionary<int, string>> GetActionForIssue(Entities.User user, int issueId, int userId) {
            return base.Channel.GetActionForIssue(user, issueId, userId);
        }
        
        public Entities.ResultValue<System.Collections.Generic.Dictionary<System.Collections.Generic.KeyValuePair<int, string>, string>> CheckBillingIssuesPresenceOnWFS(Entities.User user, string[] issues) {
            return base.Channel.CheckBillingIssuesPresenceOnWFS(user, issues);
        }
        
        public Entities.ResultValue<System.Collections.Generic.Dictionary<System.Collections.Generic.KeyValuePair<int, string>, string>> CheckIssuesPresenceOnBpm(Entities.User user, Entities.BillingDTHIssueWFS[] issues) {
            return base.Channel.CheckIssuesPresenceOnBpm(user, issues);
        }
        
        public Entities.ResultValue<string[][]> ExecuteStoredProcedure(Entities.User user, string procedureName, string[] parameters, Entities.Enums.DatabaseName database) {
            return base.Channel.ExecuteStoredProcedure(user, procedureName, parameters, database);
        }
        
        public Entities.ResultValue<System.Collections.Generic.Dictionary<int, string>> GetBillingComponents(Entities.User user, int id) {
            return base.Channel.GetBillingComponents(user, id);
        }
        
        public Entities.ResultValue<Entities.EventParamModeler[]> GetEventParamForFormByEventMove(Entities.User user, int eventMoveId) {
            return base.Channel.GetEventParamForFormByEventMove(user, eventMoveId);
        }
        
        public Entities.ResultValue<Entities.EventParam[]> GetBoundEventParamForIssue(Entities.User user, int issueId, int[] eventParamId) {
            return base.Channel.GetBoundEventParamForIssue(user, issueId, eventParamId);
        }
        
        public Entities.ResultValue<Entities.EventParam[]> GetBillingBoundEventParamForIssue(Entities.User user, int issueId, int[] eventParamId) {
            return base.Channel.GetBillingBoundEventParamForIssue(user, issueId, eventParamId);
        }
        
        public int DoActionInIssue(System.Nullable<int> idIssue, int eventMoveId, Entities.EventParam[] paramz, Entities.User u) {
            return base.Channel.DoActionInIssue(idIssue, eventMoveId, paramz, u);
        }
        
        public int NewBillingIssue(Entities.BillingDTHIssueWFS issue, Entities.User user) {
            return base.Channel.NewBillingIssue(issue, user);
        }
        
        public Entities.User LogIn2(string login, string pass) {
            return base.Channel.LogIn2(login, pass);
        }
        
        public void LogOut() {
            base.Channel.LogOut();
        }
        
        public Entities.ResultValue<bool> Notify() {
            return base.Channel.Notify();
        }
        
        public Entities.ResultValue<bool> AddUser(Entities.HeliosUser user) {
            return base.Channel.AddUser(user);
        }
        
        public Entities.ResultValue<Entities.HeliosUser[]> GetAllUsers() {
            return base.Channel.GetAllUsers();
        }
        
        public Entities.ResultValue<Entities.HeliosUser> SearchUserByEmail(string email) {
            return base.Channel.SearchUserByEmail(email);
        }
        
        public Entities.ResultValue<bool> UpdateUser(Entities.HeliosUser user) {
            return base.Channel.UpdateUser(user);
        }
        
        public Entities.ResultValue<bool> AddNote(Entities.Note note) {
            return base.Channel.AddNote(note);
        }
        
        public Entities.ResultValue<bool> UpdateNote(Entities.Note note) {
            return base.Channel.UpdateNote(note);
        }
        
        public Entities.ResultValue<Entities.Note> SearchIssueNote(string issuenumber) {
            return base.Channel.SearchIssueNote(issuenumber);
        }
        
        public Entities.ResultValue<Entities.Note[]> SearchIssueNotes(string[] issueNumbers) {
            return base.Channel.SearchIssueNotes(issueNumbers);
        }
        
        public Entities.ResultValue<Entities.ErrorType[]> GetErrorTypes(Entities.User user) {
            return base.Channel.GetErrorTypes(user);
        }
        
        public Entities.ResultValue<Entities.Process[]> GetProcesses(Entities.User user) {
            return base.Channel.GetProcesses(user);
        }
        
        public Entities.ResultValue<Entities.Error[]> GetErrors(Entities.User user, int processId) {
            return base.Channel.GetErrors(user, processId);
        }
        
        public Entities.ResultValue<Entities.Solution[]> GetSolutions(Entities.User user) {
            return base.Channel.GetSolutions(user);
        }
        
        public Entities.ResultValue<Entities.Process> CreateNewProcess(Entities.User user, Entities.Process process) {
            return base.Channel.CreateNewProcess(user, process);
        }
        
        public Entities.ResultValue<Entities.Error> CreateNewError(Entities.User user, Entities.Error error) {
            return base.Channel.CreateNewError(user, error);
        }
        
        public Entities.ResultValue<Entities.Solution> CreateNewSolution(Entities.User user, Entities.Solution solution) {
            return base.Channel.CreateNewSolution(user, solution);
        }
        
        public bool BoundErrorWithProcess(Entities.User user, Entities.Process process, Entities.Error[] errors, bool delete) {
            return base.Channel.BoundErrorWithProcess(user, process, errors, delete);
        }
        
        public bool InsertNewProcessLog(Entities.User user, string inXml) {
            return base.Channel.InsertNewProcessLog(user, inXml);
        }
    }
}
