using AutoMapper;
using EmployeeManagement.Business;
using EmployeeManagement.Controllers;
using EmployeeManagement.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit.Abstractions;

namespace EmployeeManagement.Test;

public class InternalEmployeeControllerTests
{
    private readonly InternalEmployeesController _internalEmployeesController;
    private readonly InternalEmployee _firstEmployee;
    private readonly ITestOutputHelper _testOutputHelper;

    public InternalEmployeeControllerTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;

        _firstEmployee = new InternalEmployee("Zeynel", "Sahin", 2, 3000, false, 2);
        var employeeServiceMock = new Mock<IEmployeeService>();
        employeeServiceMock.Setup(p => p.FetchInternalEmployeesAsync()).ReturnsAsync(new List<InternalEmployee>()
        {
            _firstEmployee,
            new InternalEmployee("Zeynel", "Sahin", 3, 3400, false, 1),
            new InternalEmployee("Zeynel", "Sahin", 3, 4000, false, 3)
        });
        // var mapperMock = new Mock<IMapper>();
        // mapperMock.Setup(m => m.Map<InternalEmployee, Models.InternalEmployeeDto>(It.IsAny<InternalEmployee>())).Returns(new Models.InternalEmployeeDto());

        var mapperConfiguration = new MapperConfiguration(configure=>configure.AddProfile<MapperProfiles.EmployeeProfile>());
        var mapper = new Mapper(mapperConfiguration);
        _internalEmployeesController = new InternalEmployeesController(employeeServiceMock.Object, mapper);
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

        Assert.Equal(3, ((IEnumerable<Models.InternalEmployeeDto>)
            ((OkObjectResult)actionResult.Result).Value).Count());
    }

    [Fact]
    public async Task GetInternalEmployees_GetAction_ReturnsOkObjectResultWithCorrectAmountOfInternalEmployees()
    {
        var result = await _internalEmployeesController.GetInternalEmployees();
        var actionResult = Assert.IsType<ActionResult<IEnumerable<Models.InternalEmployeeDto>>>(result);
        var okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var dtoList = Assert.IsAssignableFrom<IEnumerable<Models.InternalEmployeeDto>>(okObjectResult.Value);

        Assert.Equal(3, dtoList.Count());
        _testOutputHelper.WriteLine($"Dto List Count = {dtoList.Count()}");
    }
}