using EmployeeService.Implementation;
using EmployeeService.Implementation.Contracts.Enums;
using EmployeeService.Implementation.Contracts.Requests;
using EmployeeService.Implementation.Contracts.Responses;
using EmployeeService.Implementation.ErrorHandling;
using EmployeeService.Implementation.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace EmployeeService.IntegrationTests
{
    [TestClass]
    public class GetEmployeeTesting
    {
        private TestRegistrationFixture Fixture { get; set; }
        private IRequestHandler RequestHandler { get; set; }

        [TestInitialize]
        public void Setup()
        {
            this.Fixture = new TestRegistrationFixture();
            this.RequestHandler = this.Fixture.Resolve<IRequestHandler>();
        }

        [TestCleanup]
        public void Teardown() => this.Fixture?.Dispose();

        [TestMethod]
        public async Task GetEmployee_Success_ShouldReturnEmployeeWithEmployeeTree()
        {
            var request = new GetEmployeeRequest { Id = this.Fixture.ValidId };
            var response = await this.RequestHandler.HandleAsyncRequest<GetEmployeeRequest, GetEmployeeResponse>(request);

            Assert.IsNotNull(response);
            Assert.AreEqual(EmployeeConstants.SuccessMessage, response.ResponseMessage);
            Assert.IsNotNull(response.Employee);
            Assert.AreEqual(this.Fixture.FakeTargetActiveEmployee.Name, response.Employee.Name);
            Assert.IsTrue(response.Employee.Employees.Count > 0);
        }

        [TestMethod]
        public async Task GetEmployee_ValidationError_WhenIdIsZeroOrNegative_ShouldReturnValidationError()
        {
            var request = new GetEmployeeRequest { Id = TestConstants.InvalidId };
            var response = await this.RequestHandler.HandleAsyncRequest<GetEmployeeRequest, GetEmployeeResponse>(request);

            Assert.IsNotNull(response);
            Assert.AreEqual(ErrorStatus.ValidationRequestError.GetDescription(), response.ResponseMessage);
            Assert.IsNotNull(response.ExtendedResponseMessage);
            Assert.IsNull(response.Employee);
        }

        [TestMethod]
        public async Task GetEmployee_NotFound_WhenIdDoesNotExistInActiveList_ShouldReturnSearchError()
        {
            var request = new GetEmployeeRequest { Id = TestConstants.NonExistingId };
            var response = await this.RequestHandler.HandleAsyncRequest<GetEmployeeRequest, GetEmployeeResponse>(request);

            Assert.IsNotNull(response);
            Assert.IsNull(response.Employee);
            Assert.AreEqual(ErrorStatus.SearchError.GetDescription(), response.ResponseMessage);
        }
    }
}