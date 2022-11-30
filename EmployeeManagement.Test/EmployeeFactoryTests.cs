using EmployeeManagement.Business;
using EmployeeManagement.DataAccess.Entities;

namespace EmployeeManagement.Test;

public class EmployeeFactoryTests
{
  [Fact]
  public void CreateEmployee_ConstructInternalEmployee_SalaryMustBeBetween2500And3500()
  {
    var employeeFactory = new EmployeeFactory();
    var employee = (InternalEmployee)employeeFactory.CreateEmployee("Zeynel", "Sahin");
    
    Assert.True(employee.Salary is >= 2500 and <= 3500,"employee.Salary is >= 2500 and <= 3500");
  }
  [Fact]
  public void CreateEmployee_ConstructInternalEmployee_SalaryMustBeBetween2500And3500_Alternative()
  {
    var employeeFactory = new EmployeeFactory();
    var employee = (InternalEmployee)employeeFactory.CreateEmployee("Zeynel", "Sahin");

    Assert.True(employee.Salary is >= 2500);
    Assert.True(employee.Salary <= 3500);
  }
  [Fact]
  public void CreateEmployee_ConstructInternalEmployee_SalaryMustBeBetween2500And3500_AlternativeWithInRange()
  {
    var employeeFactory = new EmployeeFactory();
    var employee = (InternalEmployee)employeeFactory.CreateEmployee("Zeynel", "Sahin");

    Assert.InRange(employee.Salary, 2000, 3400);
  } 
  [Fact]
  public void CreateEmployee_ConstructInternalEmployee_SalaryMustBeBetween2500_PrecisionExample()
  {
    var employeeFactory = new EmployeeFactory();
    var employee = (InternalEmployee)employeeFactory.CreateEmployee("Zeynel", "Sahin");
    employee.Salary = 2500.123m;
    Assert.Equal(2500,employee.Salary,0);//precision virgülden sonra kaç basamağın dikkate alıncağını belirtir default değeri tümü dür
  }

  [Fact]
  public void CreateEmployee_IsExternalIsTrue_ReturnTypeMustBExternalEmployee()
  {
    var employeeFactory = new EmployeeFactory();
    var employee = employeeFactory.CreateEmployee("Zeynel", "Sahin", "TheKing", true);
    Assert.IsType<ExternalEmployee>(employee);
    // Assert.IsAssignableFrom<Employee>(employee);
  }
} 