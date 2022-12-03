using EmployeeManagement.Business;
using EmployeeManagement.Controllers;
using EmployeeManagement.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EmployeeManagement.Test;

public class InternalEmployeeControllerTests
{
    [Fact]
    public async Task GetInternalEmployees_GetAction_MustReturnOkObjectResult()
    {
        var employeeServiceMock = new Mock<IEmployeeService>();
        employeeServiceMock.Setup(p => p.FetchInternalEmployeesAsync()).ReturnsAsync(new List<InternalEmployee>()
        {
            new InternalEmployee("Zeynel", "Sahin", 2, 3000, false, 2),
            new InternalEmployee("Zeynel", "Sahin", 3, 3400, false, 1),
            new InternalEmployee("Zeynel", "Sahin", 3, 4000, false, 3)
        });

        var internalEmployeesController = new InternalEmployeesController(employeeServiceMock.Object, null);
        var result = await internalEmployeesController.GetInternalEmployees();
        var actionResult = Assert.IsType<ActionResult<IEnumerable<Models.InternalEmployeeDto>>>(result);

        Assert.IsType<OkObjectResult>(actionResult.Result);
    }
}