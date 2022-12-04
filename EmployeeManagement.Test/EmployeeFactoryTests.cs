using EmployeeManagement.Business;
using EmployeeManagement.DataAccess.Entities;

namespace EmployeeManagement.Test;

[Collection("No parallelism")]
public class EmployeeFactoryTests : IDisposable
{
    public EmployeeFactoryTests()
    {
        EmployeeFactory = new EmployeeFactory(); // Her test için tekraradan instanceı oluşturulur
    }

    public void Dispose()
    {
    }

    private EmployeeFactory? EmployeeFactory { get; set; }

    [Fact(Skip = "Skipping this one for demo reasons")]
    [Trait("Category", "EmployeeFactory_CreateEmployee_Salary")]
    public void CreateEmployee_ConstructInternalEmployee_SalaryMustBeBetween2500And3500()
    {
        var employeeFactory = new EmployeeFactory();
        var employee = (InternalEmployee)employeeFactory.CreateEmployee("Zeynel", "Sahin");

        Assert.True(employee.Salary is >= 2500 and <= 3500, "employee.Salary is >= 2500 and <= 3500");
    }

    [Fact]
    [Trait("Category", "EmployeeFactory_CreateEmployee_Salary")]
    public void CreateEmployee_ConstructInternalEmployee_SalaryMustBeBetween2500And3500_Alternative()
    {
        var employee = (InternalEmployee)EmployeeFactory.CreateEmployee("Zeynel", "Sahin");

        Assert.True(employee.Salary is >= 2500);
        Assert.True(employee.Salary <= 3500);
    }

    [Fact]
    [Trait("Category", "EmployeeFactory_CreateEmployee_Salary")]
    public void CreateEmployee_ConstructInternalEmployee_SalaryMustBeBetween2500And3500_AlternativeWithInRange()
    {
        var employee = (InternalEmployee)EmployeeFactory.CreateEmployee("Zeynel", "Sahin");

        Assert.InRange(employee.Salary, 2000, 3400);
    }

    [Fact]
    [Trait("Category", "EmployeeFactory_CreateEmployee_Salary")]
    public void CreateEmployee_ConstructInternalEmployee_SalaryMustBeBetween2500_PrecisionExample()
    {
        var employee = (InternalEmployee)EmployeeFactory.CreateEmployee("Zeynel", "Sahin");
        employee.Salary = 2500.123m;
        Assert.Equal(2500, employee.Salary, 0); //precision virgülden sonra kaç basamağın dikkate alıncağını belirtir default değeri tümü dür
    }

    [Fact]
    [Trait("Category", "EmployeeFactory_CreateEmployee_ReturnType")]
    public void CreateEmployee_IsExternalIsTrue_ReturnTypeMustBExternalEmployee()
    {
        var employee = EmployeeFactory.CreateEmployee("Zeynel", "Sahin", "TheKing", true);
        Assert.IsType<ExternalEmployee>(employee);
        // Assert.IsAssignableFrom<Employee>(employee);
    }

    [Fact]
    public void SlowTest1()
    {
        Thread.Sleep(5000);
        Assert.True(true);
    }
}