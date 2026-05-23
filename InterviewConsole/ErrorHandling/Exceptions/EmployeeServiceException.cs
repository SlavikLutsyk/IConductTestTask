using EmployeeService.Implementation.Contracts.Enums;
using EmployeeService.Implementation.Extensions;
using System;

namespace EmployeeService.Implementation.ErrorHandling.Exceptions
{
    public class EmployeeServiceException : Exception
    {
        public string ResponseMessage { get; private set; }

        public EmployeeServiceException(ErrorStatus status, string extendedResponseMessage)
            : base(extendedResponseMessage)
        {
            this.ResponseMessage = status.GetDescription();
        }
    }
}
