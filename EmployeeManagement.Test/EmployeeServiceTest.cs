using EmployeeManagement.Business;
using EmployeeManagement.Business.EventArguments;
using EmployeeManagement.Business.Exceptions;
using EmployeeManagement.DataAccess.Entities;
using EmployeeManagement.Services.Test;

namespace EmployeeManagement.Test;

public class EmployeeServiceTest
{
    [Fact]
    public void CreateInternalEmployee_InternalEmployeeCreated_MustHaveAttendedFirstObligatoryCourseWithObject()
    {
        var employeeManagementTestDataRepository = new EmployeeManagementTestDataRepository();
        var employeeService = new EmployeeService(employeeManagementTestDataRepository, new EmployeeFactory());
        var obligatoryCourse = employeeManagementTestDataRepository.GetCourse(Guid.Parse("37e03ca7-c730-4351-834c-b66f280cdb01"));
        var internalEmployee = employeeService.CreateInternalEmployee("Zeynel", "Sahin");
        Assert.Contains(obligatoryCourse, internalEmployee.AttendedCourses);
    }

    [Fact]
    public void CreateInternalEmployee_InternalEmployeeCreated_MustHaveAttendedFirstObligatoryCourseWithPredicate()
    {
        var employeeManagementTestDataRepository = new EmployeeManagementTestDataRepository();
        var employeeService = new EmployeeService(employeeManagementTestDataRepository, new EmployeeFactory());
        var internalEmployee = employeeService.CreateInternalEmployee("Zeynel", "Sahin");
        ;
        Assert.Contains(internalEmployee.AttendedCourses, course => course.Id == Guid.Parse("37e03ca7-c730-4351-834c-b66f280cdb01"));
    }

    [Fact]
    public void CreateInternalEmployee_InternalEmployeeCreated_AttendedCoursesMustMatchObligatoryCourses()
    {
        var employeeManagementTestDataRepository = new EmployeeManagementTestDataRepository();
        var employeeService = new EmployeeService(employeeManagementTestDataRepository, new EmployeeFactory());
        var obligatoryCourse = employeeManagementTestDataRepository.GetCourses(
            Guid.Parse("37e03ca7-c730-4351-834c-b66f280cdb01"),
            Guid.Parse("1fd115cf-f44c-4982-86bc-a8fe2e4ff83e"));
        var internalEmployee = employeeService.CreateInternalEmployee("Zeynel", "Sahin");
        Assert.Equal(obligatoryCourse, internalEmployee.AttendedCourses);
    }

    [Fact]
    public void CreateInternalEmployee_InternalEmployeeCreated_AttendedCoursesMustBeNew()
    {
        var employeeManagementTestDataRepository = new EmployeeManagementTestDataRepository();
        var employeeService = new EmployeeService(employeeManagementTestDataRepository, new EmployeeFactory());
        var internalEmployee = employeeService.CreateInternalEmployee("Zeynel", "Sahin");
        // internalEmployee.AttendedCourses[0].IsNew = true;
        // foreach (var course in internalEmployee.AttendedCourses)
        // {
        //     Assert.False(course.IsNew);
        // }
        Assert.All(internalEmployee.AttendedCourses, p => Assert.False(p.IsNew));
    }

    [Fact]
    public async Task CreateInternalEmployee_InternalEmployeeCreated_AttendedCoursesMustMatchObligatoryCoursesAsync()
    {
        var employeeManagementTestDataRepository = new EmployeeManagementTestDataRepository();
        var employeeService = new EmployeeService(employeeManagementTestDataRepository, new EmployeeFactory());
        var obligatoryCourse = await employeeManagementTestDataRepository.GetCoursesAsync(
            Guid.Parse("37e03ca7-c730-4351-834c-b66f280cdb01"),
            Guid.Parse("1fd115cf-f44c-4982-86bc-a8fe2e4ff83e"));
        var internalEmployee = await employeeService.CreateInternalEmployeeAsync("Zeynel", "Sahin");
        Assert.Equal(obligatoryCourse, internalEmployee.AttendedCourses);
    }

    [Fact]
    public async Task GiveRaise_RaiseBelowMinimumGiven_EmployeeInvalidRaiseExceptionMustBeThrown()
    {
        var employeeService = new EmployeeService(new EmployeeManagementTestDataRepository(), new EmployeeFactory());
        var internalEmployee = new InternalEmployee("Zeynel", "Sahin", 4, 3500, false, 1);

        await Assert.ThrowsAsync<EmployeeInvalidRaiseException>(async () => await employeeService.GiveRaiseAsync(internalEmployee, 50));
    }

    [Fact]
    public void NotifyOfAbsence_EmployeeIsAbsent_OnEmployeeIsAbsentMustBeTriggered()
    {
        var employeeService = new EmployeeService(new EmployeeManagementTestDataRepository(), new EmployeeFactory());
        var internalEmployee = new InternalEmployee("Zeynel", "Sahin", 4, 3500, false, 1);
        Assert.Raises<EmployeeIsAbsentEventArgs>(handler => employeeService.EmployeeIsAbsent += handler,
            handler => employeeService.EmployeeIsAbsent -= handler,
            () => employeeService.NotifyOfAbsence(internalEmployee));
    }
}