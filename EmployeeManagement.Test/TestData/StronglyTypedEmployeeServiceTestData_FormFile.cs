namespace EmployeeManagement.Test.TestData;

public class StronglyTypedEmployeeServiceTestData_FormFile: TheoryData<int,bool>
{
    public StronglyTypedEmployeeServiceTestData_FormFile()
    {
        var testDataLines = File.ReadAllLines("TestData/EmployeeServiceTestData.csv");
        foreach (var line in testDataLines)
        {
            var splitString = line.Split(",");
            if (int.TryParse(splitString[0],out var raise)&& bool.TryParse(splitString[1],out var minimumRaiseGive))
            {
                Add(raise,minimumRaiseGive);
            }
        }
    }
}