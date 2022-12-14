using AutoMapper.Configuration.Annotations;
using EmployeeManagement.Business;
using EmployeeManagement.DataAccess.Entities;
using EmployeeManagement.DataAccess.Services;
using EmployeeManagement.Services.Test;
using Moq;

namespace EmployeeManagement.Test;

public class MoqTests
{
    [Fact]
    public void FetchInternalEmployee_EmployeeFetched_SuggestedBonusMustBeCalculated()
    {
        var employeeManagementTestDataRepository = new EmployeeManagementTestDataRepository();
        // var employeeFactory = new EmployeeFactory();
        var employeeFactoryMock = new Mock<EmployeeFactory>();
        var employeeService = new EmployeeService(employeeManagementTestDataRepository, employeeFactoryMock.Object);
        var employee = employeeService.FetchInternalEmployee(Guid.Parse("72f2f5fe-e50c-4966-8420-d50258aefdcb"));
        Assert.Equal(400, employee.SuggestedBonus);
    }

    [Fact]
    public void CreateInternalEmployee_InternalEmployeeCreated_SuggestedBonusMustBe()
    {
        var employeeManagementTestDataRepository = new EmployeeManagementTestDataRepository();
        var employeeFactoryMock = new Mock<EmployeeFactory>();
        employeeFactoryMock.Setup(p => p.CreateEmployee("Zeynel", It.IsAny<string>(), null, false))
            .Returns(new InternalEmployee("Zeynel", "Sahin", 5, 2700, false, 1));
        employeeFactoryMock.Setup(p => p.CreateEmployee(It.Is<string>(value => value.Contains('z')), It.IsAny<string>(), null, false))
            .Returns(new InternalEmployee("Zeynel", "Sahin", 5, 2700, false, 1));
        employeeFactoryMock.Setup(p => p.CreateEmployee("Yavuz", It.IsAny<string>(), null, false))
            .Returns(new InternalEmployee("Zeynel", "Sahin", 5, 2700, false, 1));

        var employeeService = new EmployeeService(employeeManagementTestDataRepository, employeeFactoryMock.Object);

        const decimal suggestedBonus = 1000;
        var employee = employeeService.CreateInternalEmployee("Zeynel", "Sahin");
        Assert.Equal(suggestedBonus, employee.SuggestedBonus);
    }

    [Fact]
    public void FetchInternalEmployee_EmployeeFetched_SuggestedBonusMustBeCalculated_MoqInterface()
    {
        var employeeManagementTestDataRepositoryMock = new Mock<IEmployeeManagementRepository>();

        employeeManagementTestDataRepositoryMock.Setup(p => p.GetInternalEmployee(It.IsAny<Guid>()))
            .Returns(new InternalEmployee("Zeynel", "Sahin", 2, 2500, false, 2)
            {
                AttendedCourses = new List<Course>()
                {
                    new Course("A course"), new Course("Another course")
                }
            });

        var employeeFactoryMock = new Mock<EmployeeFactory>();
        var employeeService = new EmployeeService(employeeManagementTestDataRepositoryMock.Object, employeeFactoryMock.Object);
        var employee = employeeService.FetchInternalEmployee(Guid.Empty);
        Assert.Equal(400, employee.SuggestedBonus);
    }
    public async Task FetchInternalEmployee_EmployeeFetched_SuggestedBonusMustBeCalculated_MoqInterface_Async()
    {
        var employeeManagementTestDataRepositoryMock = new Mock<IEmployeeManagementRepository>();

        employeeManagementTestDataRepositoryMock.Setup(p => p.GetInternalEmployeeAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new InternalEmployee("Zeynel", "Sahin", 2, 2500, false, 2)
            {
                AttendedCourses = new List<Course>()
                {
                    new Course("A course"), new Course("Another course")
                }
            });

        var employeeFactoryMock = new Mock<EmployeeFactory>();
        var employeeService = new EmployeeService(employeeManagementTestDataRepositoryMock.Object, employeeFactoryMock.Object);
        var employee = await employeeService.FetchInternalEmployeeAsync(Guid.Empty);
        Assert.Equal(400, employee.SuggestedBonus);
    }
}