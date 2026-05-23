using EmployeeService.Implementation.Contracts.Responses;
using MediatR;
using System.Runtime.Serialization;

namespace EmployeeService.Implementation.Contracts.Requests
{
    public class EnableEmployeeRequest : IRequest<EnableEmployeeResponse>
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int Enable { get; set; }
    }
}
