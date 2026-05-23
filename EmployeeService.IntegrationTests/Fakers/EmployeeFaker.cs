using Bogus;
using EmployeeService.Implementation.Dtos;
using System.Collections.Generic;

namespace EmployeeService.IntegrationTests.Fakers
{
    public static class EmployeeFaker
    {
        private static readonly Faker<EmployeeDto> DtoFaker = new Faker<EmployeeDto>()
            .RuleFor(e => e.Id, f => f.Random.Int(1, 1000))
            .RuleFor(e => e.Name, f => f.Name.FirstName())
            .RuleFor(e => e.ManagerId, f => f.Random.Int(1, 100))
            .RuleFor(e => e.Enabled, f => true)
            .RuleFor(e => e.Employees, f => new List<EmployeeDto>());

        public static EmployeeDto GenerateSingle(bool? setEnabled = null)
        {
            var employee = DtoFaker.Generate();
            if (setEnabled.HasValue)
            {
                employee.Enabled = setEnabled.Value;
            }
            return employee;
        }

        public static List<EmployeeDto> GenerateList(int count)
        {
            return DtoFaker.Generate(count);
        }
    }
}
