using EmployeeManagement.Business;
using EmployeeManagement.Business.EventArguments;
using EmployeeManagement.Business.Exceptions;
using EmployeeManagement.DataAccess.Entities;
using EmployeeManagement.Services.Test;
using EmployeeManagement.Test.Fixtures;
using Xunit;
using Xunit.Abstractions;

namespace EmployeeManagement.Test;

[Collection("EmployeeServiceCollection")]
public class EmployeeServiceTest //: IClassFixture<EmployeeServiceFixture>
{
    private readonly EmployeeServiceFixture _employeeServiceFixture;
    private readonly ITestOutputHelper _testOutputHelper;

    public EmployeeServiceTest(EmployeeServiceFixture employeeServiceFixture, ITestOutputHelper testOutputHelper)
    {
        _employeeServiceFixture = employeeServiceFixture;
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void CreateInternalEmployee_InternalEmployeeCreated_MustHaveAttendedFirstObligatoryCourseWithObject()
    {
        var obligatoryCourse = _employeeServiceFixture.EmployeeManagementTestDataRepository.GetCourse(Guid.Parse("37e03ca7-c730-4351-834c-b66f280cdb01"));
        var internalEmployee = _employeeServiceFixture.EmployeeService.CreateInternalEmployee("Zeynel", "Sahin");
        Assert.Contains(obligatoryCourse, internalEmployee.AttendedCourses);
    }

    [Fact]
    public void CreateInternalEmployee_InternalEmployeeCreated_MustHaveAttendedFirstObligatoryCourseWithPredicate()
    {
        var internalEmployee = _employeeServiceFixture.EmployeeService.CreateInternalEmployee("Zeynel", "Sahin");
        Assert.Contains(internalEmployee.AttendedCourses, course => course.Id == Guid.Parse("37e03ca7-c730-4351-834c-b66f280cdb01"));
    }

    [Fact]
    public void CreateInternalEmployee_InternalEmployeeCreated_AttendedCoursesMustMatchObligatoryCourses()
    {
        var obligatoryCourse = _employeeServiceFixture.EmployeeManagementTestDataRepository.GetCourses(
            Guid.Parse("37e03ca7-c730-4351-834c-b66f280cdb01"),
            Guid.Parse("1fd115cf-f44c-4982-86bc-a8fe2e4ff83e"));
        var internalEmployee = _employeeServiceFixture.EmployeeService.CreateInternalEmployee("Zeynel", "Sahin");
        _testOutputHelper.WriteLine($"Employee after Act: {internalEmployee.FirstName} {internalEmployee.LastName}");
        internalEmployee.AttendedCourses.ForEach(c=>_testOutputHelper.WriteLine($"Attended course: {c.Id} {c.Title}"));
        Assert.Equal(obligatoryCourse, internalEmployee.AttendedCourses);
    }

    [Fact]
    public void CreateInternalEmployee_InternalEmployeeCreated_AttendedCoursesMustBeNew()
    {
        var internalEmployee = _employeeServiceFixture.EmployeeService.CreateInternalEmployee("Zeynel", "Sahin");
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
        var obligatoryCourse = await _employeeServiceFixture.EmployeeManagementTestDataRepository.GetCoursesAsync(
            Guid.Parse("37e03ca7-c730-4351-834c-b66f280cdb01"),
            Guid.Parse("1fd115cf-f44c-4982-86bc-a8fe2e4ff83e"));
        var internalEmployee = await _employeeServiceFixture.EmployeeService.CreateInternalEmployeeAsync("Zeynel", "Sahin");
        Assert.Equal(obligatoryCourse, internalEmployee.AttendedCourses);
    }

    [Fact]
    public async Task GiveRaise_RaiseBelowMinimumGiven_EmployeeInvalidRaiseExceptionMustBeThrown()
    {
        var internalEmployee = new InternalEmployee("Zeynel", "Sahin", 4, 3500, false, 1);
        await Assert.ThrowsAsync<EmployeeInvalidRaiseException>(async () => await _employeeServiceFixture.EmployeeService.GiveRaiseAsync(internalEmployee, 50));
    }

    [Fact]
    public void NotifyOfAbsence_EmployeeIsAbsent_OnEmployeeIsAbsentMustBeTriggered()
    {
        var internalEmployee = new InternalEmployee("Zeynel", "Sahin", 4, 3500, false, 1);
        Assert.Raises<EmployeeIsAbsentEventArgs>(handler => _employeeServiceFixture.EmployeeService.EmployeeIsAbsent += handler,
            handler => _employeeServiceFixture.EmployeeService.EmployeeIsAbsent -= handler,
            () => _employeeServiceFixture.EmployeeService.NotifyOfAbsence(internalEmployee));
    }
}