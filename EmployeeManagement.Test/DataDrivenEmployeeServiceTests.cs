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

    [Fact]
    public async Task GiveRaise_MoreThanMinimumRaiseGiven_EmployeeMinimumRaiseGivenMustBe()
    {
        var internalEmployee = new InternalEmployee("Zeynel", "Sahin", 4, 3500, false, 1);
        await _employeeServiceFixture.EmployeeService.GiveRaiseAsync(internalEmployee, 200);
        Assert.False(internalEmployee.MinimumRaiseGiven);
    }
}