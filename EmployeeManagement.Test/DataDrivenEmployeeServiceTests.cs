using EmployeeManagement.Business.EventArguments;
using EmployeeManagement.Business.Exceptions;
using EmployeeManagement.DataAccess.Entities;
using EmployeeManagement.Test.Fixtures;

namespace EmployeeManagement.Test;

[Collection("EmployeeServiceCollection")]
public class DataDrivenEmployeeServiceTests : IClassFixture<EmployeeServiceFixture>
{
    private readonly EmployeeServiceFixture _employeeServiceFixture;

    public DataDrivenEmployeeServiceTests(EmployeeServiceFixture employeeServiceFixture)
    {
        _employeeServiceFixture = employeeServiceFixture;
    }
    
    [Theory]
    [InlineData("1fd115cf-f44c-4982-86bc-a8fe2e4ff83e")]
    [InlineData("37e03ca7-c730-4351-834c-b66f280cdb01")]
    public void CreateInternalEmployee_InternalEmployeeCreated_MustHaveAttendedSecondObligatoryCourse(Guid courseId)
    {
        var internalEmployee = _employeeServiceFixture.EmployeeService.CreateInternalEmployee("Zeynel", "Sahin");
        Assert.Contains(internalEmployee.AttendedCourses, course => course.Id == courseId);
    }
    [Fact]
    public async Task GiveRaise_MoreThanMinimumRaiseGiven_EmployeeMinimumRaiseGivenMustBe()
    {
        var internalEmployee = new InternalEmployee("Zeynel", "Sahin", 4, 3500, false, 1);
        await _employeeServiceFixture.EmployeeService.GiveRaiseAsync(internalEmployee, 200);
        Assert.False(internalEmployee.MinimumRaiseGiven);
    }
}