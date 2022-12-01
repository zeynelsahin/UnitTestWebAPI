using EmployeeManagement.Business.EventArguments;
using EmployeeManagement.Business.Exceptions;
using EmployeeManagement.DataAccess.Entities;
using EmployeeManagement.Test.Fixtures;
using EmployeeManagement.Test.TestData;

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
    public async Task GiveRaise_MinimumRaiseGiven_EmployeeMinimumRaiseGivenMustBeTrue()
    {
        var internalEmployee = new InternalEmployee(
            "Zeynel", "Sahin", 5, 3000, false, 1);
        await _employeeServiceFixture
            .EmployeeService.GiveRaiseAsync(internalEmployee, 100);
        Assert.True(internalEmployee.MinimumRaiseGiven);
    }


    [Fact]
    public async Task GiveRaise_MoreThanMinimumRaiseGiven_EmployeeMinimumRaiseGivenMustBeFalse()
    {
        var internalEmployee = new InternalEmployee(
            "Zeynel", "Sahin", 5, 3000, false, 1);
        await _employeeServiceFixture.EmployeeService
            .GiveRaiseAsync(internalEmployee, 200);
        Assert.False(internalEmployee.MinimumRaiseGiven);
    }

    public static IEnumerable<object[]> ExampleTestDataForGiveRaiseWithProperty
    {
        get
        {
            return new List<object[]>
            {
                new object[] { 100, true },
                new object[] { 200, false },
            };
        }
    }
    public static TheoryData<int,bool> StronglyTypedExampleForGiveRaiseWithProperty =>
        new TheoryData<int, bool>
        {
            { 100, true },
            { 200, false }
        };

    public static IEnumerable<object[]> ExampleTestDataForGiveRaiseWithMethod(int testDataInstancesToProvide)
    {
        var testData=new List<object[]>
        {
            new object[] { 100, true },
            new object[] { 200, false },
        };
        return testData.Take(testDataInstancesToProvide);
    }

    [Theory]
    // [MemberData(nameof(ExampleTestDataForGiveRaiseWithMethod),1,MemberType = typeof(DataDrivenEmployeeServiceTests)),]
    // [ClassData(typeof(EmployeeServiceTestData))]
     // [ClassData(typeof(StronglyTypedEmployeeServiceTestData))]
    // [MemberData(nameof(StronglyTypedExampleForGiveRaiseWithProperty))]
     [ClassData(typeof(StronglyTypedEmployeeServiceTestData_FormFile))]
    public async Task GiveRaise_RaiseGive_EmployeeMinimumRaiseGivenMatchesValue(int raiseGiven, bool exceptedValueForMinimumRaiseGiven)
    {
        var internalEmployee = new InternalEmployee(
            "Zeynel", "Sahin", 5, 3000, false, 1);
        await _employeeServiceFixture.EmployeeService.GiveRaiseAsync(internalEmployee, raiseGiven);
        Assert.Equal(exceptedValueForMinimumRaiseGiven, internalEmployee.MinimumRaiseGiven);
    }
}