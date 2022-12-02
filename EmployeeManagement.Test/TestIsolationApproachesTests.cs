using EmployeeManagement.Business;
using EmployeeManagement.DataAccess.DbContexts;
using EmployeeManagement.DataAccess.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit.Sdk;

namespace EmployeeManagement.Test;

public class TestIsolationApproachesTests
{
    [Fact]
    public async Task AttendCourseAsync_CourseAttended_SuggestedBonusMustCorrectlyBeRecalculated()
    {
        var connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();
        var optionBuilder = new DbContextOptionsBuilder<EmployeeDbContext>().UseSqlite(connection);
        var dbContext = new EmployeeDbContext(optionBuilder.Options);
         await dbContext.Database.MigrateAsync();
        var employeeManagementRepository = new EmployeeManagementRepository(dbContext);
        var employeeService = new EmployeeService(employeeManagementRepository, new EmployeeFactory());
        var courseToAttend = await employeeManagementRepository.GetCourseAsync(Guid.Parse("844e14ce-c055-49e9-9610-855669c9859b"));
        var internalEmployee = await employeeManagementRepository.GetInternalEmployeeAsync(Guid.Parse("72f2f5fe-e50c-4966-8420-d50258aefdcb"));

        if (courseToAttend==null ||internalEmployee==null)
        {
            throw new XunitException("Arranging the test failed");
        }

        var expectedSuggestedBonus = internalEmployee.YearsInService * (internalEmployee.AttendedCourses.Count + 1) * 100;
        await employeeService.AttendCourseAsync(internalEmployee, courseToAttend);
        Assert.Equal(expectedSuggestedBonus,internalEmployee.SuggestedBonus);
    }
}