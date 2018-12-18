using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;

namespace DataLayer.Interface
{
    public interface IProcessManagerGateway
    {
        

        List<ErrorType> GetErrorTypes(Entities.User user);

        List<Process> GetProcesses(Entities.User user);

        List<Error> GetErrors(Entities.User user, int processId = -1);

        List<Solution> GetSolutions(Entities.User user);

        Process CreateNewProcess(Entities.User user, Process process);

        Error CreateNewError(User user, Error error);

        Solution CreateNewSolution(User user, Solution solution);

        bool BoundErrorWithProcess(User user, Process process, List<Error> errors, bool delete = false);

        bool InsertNewProcessLog(User user, string inXml);
    }
}
