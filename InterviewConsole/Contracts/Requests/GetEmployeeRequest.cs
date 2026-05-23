using EmployeeService.Implementation.Contracts.Responses;
using MediatR;
using System.Runtime.Serialization;

namespace EmployeeService.Implementation.Contracts.Requests
{
    [DataContract]
    public class GetEmployeeRequest : IRequest<GetEmployeeResponse>
    {
        [DataMember]
        public int Id { get; set; }
    }
}
