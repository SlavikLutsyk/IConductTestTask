using EmployeeService.Implementation.Contracts.Responses;

namespace EmployeeService.Implementation.Extensions
{
    public static class EmployeeExtensions
    {
        public static TResponse AsSuccess<TResponse>(this TResponse response) where TResponse : BaseResponse
        {
            if (response == null)
            {
                return null;
            }

            response.ResponseMessage = EmployeeConstants.SuccessMessage;
            response.ExtendedResponseMessage = EmployeeConstants.SuccessMessage;
            return response;
        }

        public static TResponse AsError<TResponse>(this TResponse response, string responseMessage, string extendedResponseMessage) where TResponse : BaseResponse
        {
            if (response == null)
            {
                return null;
            }

            response.ResponseMessage = responseMessage;
            response.ExtendedResponseMessage = extendedResponseMessage;
            return response;
        }
    }
}
