using System.Security.Claims;
using AutoMapper;
using EmployeeManagement.Business;
using EmployeeManagement.Controllers;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EmployeeManagement.Test;

public class DemoInternalEmployeesControllerTests
{
    [Fact]
    public async Task CreateInternalEmployee_InvalidInput_MustReturnBadRequest()
    {
        var employeeServiceMock = new Mock<IEmployeeService>();
        var mapperMock = new Mock<IMapper>();
        var demoInternalEmployeeController = new DemoInternalEmployeesController(employeeServiceMock.Object, mapperMock.Object);

        var internalEmployeeForCreationDto = new InternalEmployeeForCreationDto();
        
        demoInternalEmployeeController.ModelState.AddModelError("FirstName","Required");
        var result = await demoInternalEmployeeController.CreateInternalEmployee(internalEmployeeForCreationDto);

        var actionResult = Assert.IsType<ActionResult<Models.InternalEmployeeDto>>(result);
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
        Assert.IsType<SerializableError>(badRequestResult.Value);
    }

    [Fact]
    public void GetProtectedInternalEmployees_GetActionForUserInAdminRole_MustRedirectToGetInternalEmployeesOnProtectedInternalEmployees()
    {
        var employeeServiceMock = new Mock<IEmployeeService>();
        var mapperMock = new Mock<IMapper>();
        var demoInternalEmployeesController = new DemoInternalEmployeesController(employeeServiceMock.Object,mapperMock.Object);
        var userClaims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, "Zeynel"),
            new Claim(ClaimTypes.Role, "NotAdmin")
        };

        var claimsIdentity = new ClaimsIdentity(userClaims, "UnitTest");
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        var httpContext = new DefaultHttpContext()
        {
            User = claimsPrincipal
        };
        demoInternalEmployeesController.ControllerContext = new ControllerContext()
        {
            HttpContext = httpContext
        };
        var result = demoInternalEmployeesController.GetProtectedInternalEmployees();
        var actionResult = Assert.IsAssignableFrom<IActionResult>(result);
        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        
        Assert.Equal("GetInternalEmployees",redirectToActionResult.ActionName);
        Assert.Equal("InternalEmployees",redirectToActionResult.ControllerName);
    } 
    [Fact]
    public void GetProtectedInternalEmployees_GetActionForUserInAdminRole_MustRedirectToGetInternalEmployeesOnProtectedInternalEmployees_WithMoq()
    {
        var employeeServiceMock = new Mock<IEmployeeService>();
        var mapperMock = new Mock<IMapper>();
        var demoInternalEmployeesController = new DemoInternalEmployeesController(employeeServiceMock.Object,mapperMock.Object);

        var mockPrincipal = new Mock<ClaimsPrincipal>();
        mockPrincipal.Setup(p => p.IsInRole(It.Is<string>(s => s == "NotAdmin"))).Returns(true);

        var httpContextMock = new Mock<HttpContext>();
        httpContextMock.Setup(p => p.User).Returns(mockPrincipal.Object);
        
        demoInternalEmployeesController.ControllerContext = new ControllerContext()
        {
            HttpContext = httpContextMock.Object
        };
        var result = demoInternalEmployeesController.GetProtectedInternalEmployees();
        var actionResult = Assert.IsAssignableFrom<IActionResult>(result);
        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        
        Assert.Equal("GetInternalEmployees",redirectToActionResult.ActionName);
        Assert.Equal("InternalEmployees",redirectToActionResult.ControllerName);
    }
}