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
    public class EnableEmployeeTesting
    {
        private TestRegistrationFixture Fixture {  get; set; }
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
        public async Task EnableEmployee_Success_ShouldReturnUpdatedEmployee()
        {
            var request = new EnableEmployeeRequest { Id = this.Fixture.ValidId, Enable = TestConstants.Enable };
            var response = await this.RequestHandler.HandleAsyncRequest<EnableEmployeeRequest, EnableEmployeeResponse>(request);

            Assert.IsNotNull(response);
            Assert.AreEqual(EmployeeConstants.SuccessMessage, response.ResponseMessage);
            Assert.IsNotNull(response.Employee);
            Assert.AreEqual(this.Fixture.FakeTargetActiveEmployee.Id, response.Employee.Id);
        }

        [TestMethod]
        public async Task DisableEmployee_Success_ShouldReturnUpdatedEmployee()
        {
            var request = new EnableEmployeeRequest { Id = this.Fixture.ValidInactiveId, Enable = TestConstants.Disable };
            var response = await this.RequestHandler.HandleAsyncRequest<EnableEmployeeRequest, EnableEmployeeResponse>(request);

            Assert.IsNotNull(response);
            Assert.AreEqual(EmployeeConstants.SuccessMessage, response.ResponseMessage);
            Assert.IsNotNull(response.Employee);
            Assert.AreEqual(this.Fixture.FakeTargetInctiveEmployee.Id, response.Employee.Id);
        }

        [TestMethod]
        public async Task EnableEmployee_ValidationError_WhenIdIsInvalid_ShouldReturnError()
        {
            var request = new EnableEmployeeRequest { Id = TestConstants.InvalidId, Enable = TestConstants.Enable };
            var response = await this.RequestHandler.HandleAsyncRequest<EnableEmployeeRequest, EnableEmployeeResponse>(request);

            Assert.IsNotNull(response);
            Assert.AreEqual(ErrorStatus.ValidationRequestError.GetDescription(), response.ResponseMessage);
            Assert.IsNull(response.Employee);
        }

        [TestMethod]
        public async Task EnableEmployee_ValidationError_WhenEnableParameterIsIncorrect_ShouldReturnError()
        {
            var request = new EnableEmployeeRequest { Id = this.Fixture.ValidId, Enable = TestConstants.InvalidEnable };
            var response = await this.RequestHandler.HandleAsyncRequest<EnableEmployeeRequest, EnableEmployeeResponse>(request);

            Assert.IsNotNull(response);
            Assert.AreEqual(ErrorStatus.ValidationRequestError.GetDescription(), response.ResponseMessage);
            Assert.IsNull(response.Employee);
        }

        [TestMethod]
        public async Task EnableEmployee_NotFoundInDatabase_ShouldReturnSearchError()
        {
            var request = new EnableEmployeeRequest { Id = TestConstants.NonExistingId, Enable = TestConstants.Disable };
            var response = await this.RequestHandler.HandleAsyncRequest<EnableEmployeeRequest, EnableEmployeeResponse>(request);

            Assert.IsNotNull(response);
            Assert.IsNull(response.Employee);
            Assert.AreEqual(ErrorStatus.SearchError.GetDescription(), response.ResponseMessage);
        }
    }
}