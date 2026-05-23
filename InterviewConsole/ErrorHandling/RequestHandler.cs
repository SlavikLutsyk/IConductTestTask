using AutoMapper;
using EmployeeService.Implementation.Contracts.Enums;
using EmployeeService.Implementation.Contracts.Responses;
using EmployeeService.Implementation.ErrorHandling.Exceptions;
using EmployeeService.Implementation.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace EmployeeService.Implementation.ErrorHandling
{
    public class RequestHandler : IRequestHandler
    {
        private IMediator Mediator { get; }

        private IMapper Mapper { get; }

        private ILogger Log { get; }
        public RequestHandler(
            IMediator mediator,
            IMapper mapper,
            ILogger log)
        {
            this.Mediator = mediator;
            this.Mapper = mapper;
            this.Log = log;
        }
        public async Task<TResponse> HandleAsyncRequest<TRequest, TResponse>(TRequest request) where TResponse : BaseResponse, new()
        {
            TResponse response = new TResponse();

            try
            {
                object obj = await this.Mediator.Send((object)request, HttpContext.Current?.Response.ClientDisconnectedToken ?? CancellationToken.None);
                response = this.Mapper.Map<TResponse>(obj);
                if (response.IsSuccessResponse())
                {
                    response.AsSuccess();
                }
                else
                {
                    response.AsError(ErrorStatus.SearchError.GetDescription(), EmployeeConstants.SearchFailedMessage);
                }
            }catch(Exception exception)
            {
                this.HandleError(exception, response);
            }

            return response;
        }

        private void HandleError<TResponse>(Exception exception, TResponse response) where TResponse : BaseResponse
        {
            EmployeeServiceException employeeServiceException = exception as EmployeeServiceException;
            response.AsError(employeeServiceException?.ResponseMessage ?? ErrorStatus.UndefinedTechnicalError.GetDescription(), exception.Message);
            this.Log.LogError(exception.ToString());
        }
    }
}
