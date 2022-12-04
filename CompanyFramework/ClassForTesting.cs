using System.Threading;

namespace CompanyFramework;

public class ClassForTesting
{
    public bool MethodForTesting()
    {
        Thread.Sleep(5000);
        return true;
    }
}