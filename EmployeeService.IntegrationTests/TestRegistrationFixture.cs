using Autofac;
using EmployeeService.Implementation;
using EmployeeService.Implementation.Dtos;
using EmployeeService.Implementation.Repository;
using EmployeeService.IntegrationTests.Fakers;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;

namespace EmployeeService.IntegrationTests
{
    public class TestRegistrationFixture : IDisposable
    {
        public IContainer Container { get; private set; }
        public ILifetimeScope TestScope { get; private set; }
        public Mock<IEmployeeRepository> MockRepository { get; private set; }
        public List<EmployeeDto> FakeActiveEmployees { get; private set; }
        public EmployeeDto FakeTargetActiveEmployee { get; private set; }
        public EmployeeDto FakeTargetInctiveEmployee { get; private set; }
        public int ValidId => this.FakeTargetActiveEmployee.Id;
        public int ValidInactiveId => this.FakeTargetInctiveEmployee.Id;

        public TestRegistrationFixture()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule(new ProgramModule());

            this.FakeActiveEmployees = EmployeeFaker.GenerateList(3);

            this.FakeTargetActiveEmployee = EmployeeFaker.GenerateSingle();
            this.FakeTargetInctiveEmployee = EmployeeFaker.GenerateSingle(false);

            var subEmployee = EmployeeFaker.GenerateSingle();
            subEmployee.ManagerId = this.FakeTargetActiveEmployee.Id;
            subEmployee.Employees = new List<EmployeeDto>();

            this.FakeActiveEmployees.Add(this.FakeTargetActiveEmployee);
            this.FakeActiveEmployees.Add(subEmployee);

            this.MockRepository = new Mock<IEmployeeRepository>();

            this.MockRepository
                .Setup(repo => repo.GetAllActiveEmployeesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(this.FakeActiveEmployees);

            this.MockRepository
                .Setup(repo => repo.UpdateEnableEmployeeAsync(this.ValidId, It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(this.FakeTargetActiveEmployee);

            this.MockRepository
                .Setup(repo => repo.UpdateEnableEmployeeAsync(this.ValidInactiveId, TestConstants.Disable, It.IsAny<CancellationToken>()))
                .ReturnsAsync(this.FakeTargetInctiveEmployee);

            this.MockRepository
                .Setup(repo => repo.UpdateEnableEmployeeAsync(TestConstants.NonExistingId, It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((EmployeeDto)null);

            builder.RegisterInstance(this.MockRepository.Object).As<IEmployeeRepository>();

            this.Container = builder.Build();
            this.TestScope = this.Container.BeginLifetimeScope();
        }

        public T Resolve<T>() => this.TestScope.Resolve<T>();

        public void Dispose()
        {
            this.TestScope?.Dispose();
            this.Container?.Dispose();
        }
    }
}