namespace CompanyFramework.Test;

public class ClassForTestingTests
{
    [Fact]
    public void MethodForTesting_Execute_ReturnTrue()
    {
        var classForTesting = new ClassForTesting();
        var result = classForTesting.MethodForTesting();
        Assert.True(result);
    }
}