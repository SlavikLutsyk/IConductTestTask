using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EmployeeService.Implementation.Dtos
{
    [DataContract]
    public class EmployeeDto
    {
        [DataMember(Order = 1, Name = "ID")]
        public int Id { get; set; }

        [DataMember(Order = 2)]
        public string Name { get; set; }

        [DataMember(Order = 3, Name = "ManagerID", EmitDefaultValue = false)]
        public int? ManagerId { get; set; }

        [DataMember(Order = 4, EmitDefaultValue = false)]
        public bool? Enabled { get; set; }

        [DataMember(Order = 5, EmitDefaultValue = false)]
        public List<EmployeeDto> Employees { get; set; } = new List<EmployeeDto>();
    }
}
