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
    
    Assert.True(employee.Salary is >= 2500 and <= 3500);
  }
} 