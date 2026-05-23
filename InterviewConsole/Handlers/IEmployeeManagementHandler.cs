using EmployeeService.Implementation.Contracts.Requests;
using EmployeeService.Implementation.Contracts.Responses;
using MediatR;

namespace EmployeeService.Implementation.Handlers
{
    public interface IEmployeeManagementHandler :
        IRequestHandler<GetEmployeeRequest, GetEmployeeResponse>,
        IRequestHandler<EnableEmployeeRequest, EnableEmployeeResponse>
    {
    }
}
