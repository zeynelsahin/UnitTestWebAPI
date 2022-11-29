using EmployeeManagement.DataAccess.Entities;

namespace EmployeeManagement.Test;

public class EmployeeTests
{
    [Fact]
    public void EmployeeFullNamePropertyGetter_InputFirstNameAndLastName_FullNameIsConcatenation()
    {
        var employee = new InternalEmployee("Zeynel", "Şahin", 0, 2500, false, 1);

        employee.FirstName = "ZeynelAbidin";
        employee.LastName = "Deneme";
        Assert.Equal("ZeynelAbidin Deneme",employee.FullName,ignoreCase:true);
    }
    [Fact]
    public void EmployeeFullNamePropertyGetter_InputFirstNameAndLastName_FullNameStartWithFirstName()
    {
        var employee = new InternalEmployee("Zeynel", "Şahin", 0, 2500, false, 1);

        employee.FirstName = "ZeynelAbidin";
        employee.LastName = "Deneme";
        Assert.StartsWith(employee.FirstName,employee.FullName);
    }
    [Fact]
    public void EmployeeFullNamePropertyGetter_InputFirstNameAndLastName_FullNameEndsWithLastName()
    {
        var employee = new InternalEmployee("Zeynel", "Şahin", 0, 2500, false, 1);

        employee.FirstName = "ZeynelAbidin";
        employee.LastName = "Deneme";
        Assert.EndsWith(employee.LastName,employee.FullName);
    }
    [Fact]
    public void EmployeeFullNamePropertyGetter_InputFirstNameAndLastName_FullNameContainsPartOfConcatenation()
    {
        var employee = new InternalEmployee("Zeynel", "Şahin", 0, 2500, false, 1);

        employee.FirstName = "ZeynelAbidin";
        employee.LastName = "Deneme";
        Assert.Contains("in De",employee.FullName);
    }
    [Fact]
    public void EmployeeFullNamePropertyGetter_InputFirstNameAndLastName_FullNameSoundsLikeConcatenation()
    {
        var employee = new InternalEmployee("Zeynel", "Şahin", 0, 2500, false, 1);

        employee.FirstName = "ZeynelAbidin";
        employee.LastName = "Deneme";
        Assert.Matches("ZeynelA(b|d|s)idin De(m|n|k)eme",employee.FullName);
    }
}