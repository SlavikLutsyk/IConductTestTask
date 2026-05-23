using EmployeeService.Implementation.Dtos;
using System.Runtime.Serialization;

namespace EmployeeService.Implementation.Contracts.Responses
{
    [DataContract]
    public class BaseResponse
    {
        [DataMember(Order = 1, EmitDefaultValue = false)]
        public EmployeeDto Employee { get; set; }

        [DataMember(Order = 2)]
        public string ResponseMessage { get; set; }

        [DataMember(Order = 3)]
        public string ExtendedResponseMessage { get; set; }

        public bool IsSuccessResponse() => this.Employee != null;
    }
}
