using EmployeeManagement.Business;
using EmployeeManagement.Controllers;
using EmployeeManagement.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Moq;

namespace EmployeeManagement.Test;

public class InternalEmployeeControllerTests
{
    private readonly InternalEmployeesController _internalEmployeesController;

    public InternalEmployeeControllerTests()
    {
        var employeeServiceMock = new Mock<IEmployeeService>();
        employeeServiceMock.Setup(p => p.FetchInternalEmployeesAsync()).ReturnsAsync(new List<InternalEmployee>()
        {
            new InternalEmployee("Zeynel", "Sahin", 2, 3000, false, 2),
            new InternalEmployee("Zeynel", "Sahin", 3, 3400, false, 1),
            new InternalEmployee("Zeynel", "Sahin", 3, 4000, false, 3)
        });

        _internalEmployeesController = new InternalEmployeesController(employeeServiceMock.Object, null);

    }
    [Fact]
    public async Task GetInternalEmployees_GetAction_MustReturnOkObjectResult()
    {
     
        var result = await _internalEmployeesController.GetInternalEmployees();
        var actionResult = Assert.IsType<ActionResult<IEnumerable<Models.InternalEmployeeDto>>>(result);

        Assert.IsType<OkObjectResult>(actionResult.Result);
    }

    [Fact]
    public async Task GetInternalEmployees_GetAction_MustReturnIEnumerableOfInternalEmployeeOfInternalEmployeeDtoAsModelType()
    {
        var result = await _internalEmployeesController.GetInternalEmployees();
        var actionResult = Assert.IsType<ActionResult<IEnumerable<Models.InternalEmployeeDto>>>(result);
        //Assert.IsType<>() interface implementasyonunda çalışmayacaktır <IEnumerable> vb. 
        Assert.IsAssignableFrom<IEnumerable<Models.InternalEmployeeDto>>(((OkObjectResult)actionResult.Result).Value);
    }

    [Fact]
    public async Task GetInternalEmployees_GetAction_MustBeReturnNumberOfInputtedInternalEmployees()
    {
        var resul = await _internalEmployeesController.GetInternalEmployees();

        var actionResult = Assert.IsType<ActionResult<IEnumerable<Models.InternalEmployeeDto>>>(resul);

        Assert.Equal(3,((IEnumerable<Models.InternalEmployeeDto>)
            ((OkObjectResult)actionResult.Result).Value).Count());
    }
}