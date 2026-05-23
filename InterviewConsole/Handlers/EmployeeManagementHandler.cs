using EmployeeService.Implementation.Contracts.Enums;
using EmployeeService.Implementation.Contracts.Requests;
using EmployeeService.Implementation.Contracts.Responses;
using EmployeeService.Implementation.Dtos;
using EmployeeService.Implementation.ErrorHandling.Exceptions;
using EmployeeService.Implementation.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeService.Implementation.Handlers
{
    public class EmployeeManagementHandler : IEmployeeManagementHandler
    {
        private IEmployeeRepository EmployeeRepository { get; }

        public EmployeeManagementHandler(IEmployeeRepository employeeRepository)
        {
            this.EmployeeRepository = employeeRepository;
        }

        public async Task<GetEmployeeResponse> Handle(GetEmployeeRequest request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
            {
                throw new EmployeeServiceException(ErrorStatus.ValidationRequestError, EmployeeConstants.IdIncorrectMessage);
            }

            var allEmployees = await this.EmployeeRepository.GetAllActiveEmployeesAsync(cancellationToken);

            var searchedEmployee = allEmployees.FirstOrDefault(e => e.Id == request.Id);
            if (searchedEmployee == null)
            {
                return new GetEmployeeResponse { Employee = null };
            }

            cancellationToken.ThrowIfCancellationRequested();
            AddEmployeesToManager(searchedEmployee, allEmployees.ToList());

            return new GetEmployeeResponse
            {
                Employee = searchedEmployee
            };
        }

        public async Task<EnableEmployeeResponse> Handle(EnableEmployeeRequest request, CancellationToken cancellationToken)
        {

            if (request.Id <= 0)
            {
                throw new EmployeeServiceException(ErrorStatus.ValidationRequestError, EmployeeConstants.IdIncorrectMessage);
            }

            if (request.Enable != 0 && request.Enable != 1)
            {
                throw new EmployeeServiceException(ErrorStatus.ValidationRequestError, EmployeeConstants.EnableParameterIncorrectMessage);
            }

            var updatedEmployee = await this.EmployeeRepository.UpdateEnableEmployeeAsync(request.Id, request.Enable, cancellationToken);

            return new EnableEmployeeResponse
            {
                Employee = updatedEmployee
            };
        }

        private void AddEmployeesToManager(EmployeeDto currentManager, List<EmployeeDto> allEmployees)
        {
            var employees = allEmployees
                .Where(e => e.ManagerId == currentManager.Id && e.Id != currentManager.Id)
                .ToList();

            foreach (var employee in employees)
            {
                currentManager.Employees.Add(employee);
                AddEmployeesToManager(employee, allEmployees);
            }
        }
    }
}
