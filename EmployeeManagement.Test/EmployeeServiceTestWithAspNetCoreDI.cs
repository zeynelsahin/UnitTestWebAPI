using EmployeeManagement.Test.Fixtures;

namespace EmployeeManagement.Test;

public class EmployeeServiceTestWithAspNetCoreDI: IClassFixture<EmployeeServiceWithAspNetCoreDIFixture>
{
    private readonly EmployeeServiceWithAspNetCoreDIFixture _employeeServiceWithAspNetCoreDiFixture;

    public EmployeeServiceTestWithAspNetCoreDI(EmployeeServiceWithAspNetCoreDIFixture employeeServiceWithAspNetCoreDiFixture)
    {
        _employeeServiceWithAspNetCoreDiFixture = employeeServiceWithAspNetCoreDiFixture;
    }

    [Fact]
    public void CreateInternalEmployee_InternalEmployeeCreated_MustHaveAttendedFirstObligatory()
    {
        var obligatoryCourse= _employeeServiceWithAspNetCoreDiFixture.EmployeeManagementRepository.GetCourse(Guid.Parse("37e03ca7-c730-4351-834c-b66f280cdb01"));
        
        var internalEmployee = _employeeServiceWithAspNetCoreDiFixture.EmployeeService.CreateInternalEmployee("Zeynel", "Sahin");

        Assert.Contains(obligatoryCourse, internalEmployee.AttendedCourses);
    }
}