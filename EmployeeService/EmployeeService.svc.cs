using EmployeeService.Implementation.Contracts.Requests;
using EmployeeService.Implementation.Contracts.Responses;
using EmployeeService.Implementation.ErrorHandling;
using System.ServiceModel;
using System.Threading.Tasks;

namespace EmployeeService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "EmployeeService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select EmployeeService.svc or EmployeeService.svc.cs at the Solution Explorer and start debugging.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class EmployeeWcfService : IEmployeeService
    {
        private IRequestHandler RequestHandler { get; }

        public EmployeeWcfService(IRequestHandler requestHandler)
        {
            this.RequestHandler = requestHandler;
        }

        public Task<EnableEmployeeResponse> EnableEmployee(int id, int enable)
        {
            return this.RequestHandler.HandleAsyncRequest<EnableEmployeeRequest, EnableEmployeeResponse>(new EnableEmployeeRequest { Id = id, Enable = enable });
        }

        public Task<GetEmployeeResponse> GetEmployeeById(int id)
        {
            return this.RequestHandler.HandleAsyncRequest<GetEmployeeRequest, GetEmployeeResponse>(new GetEmployeeRequest { Id = id });
        }
    }
}