using EmployeeService.Implementation.Contracts.Responses;
using System.Threading.Tasks;

namespace EmployeeService.Implementation.ErrorHandling
{
    public interface IRequestHandler
    {
        Task<TResponse> HandleAsyncRequest<TRequest, TResponse>(TRequest request) where TResponse : BaseResponse, new();
    }
}
