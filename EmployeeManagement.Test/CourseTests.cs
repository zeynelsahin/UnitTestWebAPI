using EmployeeManagement.DataAccess.Entities;

namespace EmployeeManagement.Test;

public class CourseTests
{
    [Fact]
    public void CourseConstructor_ConstructorCourse_IsNewMustBeTrue()
    {
        var course = new Course("Zeynel");
        Assert.True(course.IsNew);
    }
}