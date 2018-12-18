using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataLayer.Interface;

namespace DataLayer.Implementation
{
    class ProcessManagerGateway : IProcessManagerGateway
    {
        private ServiceManager manager;

        private ProcessManagerGateway(ServiceManager manager)
        {
            this.manager = manager;
        }

        public static ProcessManagerGateway GetProcessManagerGateway(ServiceManager manager)
        {
            return new ProcessManagerGateway(manager);
        }

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

        public Entities.Process CreateNewProcess(Entities.User user, Entities.Process process)
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
    }
}
