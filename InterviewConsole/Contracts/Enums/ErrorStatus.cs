using System.ComponentModel;

namespace EmployeeService.Implementation.Contracts.Enums
{
    public enum ErrorStatus
    {
        [Description("Undefined technical error")]
        UndefinedTechnicalError,

        [Description("Validation request error")]
        ValidationRequestError,

        [Description("Search failed")]
        SearchError
    }
}
