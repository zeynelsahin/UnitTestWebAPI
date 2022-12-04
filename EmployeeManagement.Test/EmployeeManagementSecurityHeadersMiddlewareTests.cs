using EmployeeManagement.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic;

namespace EmployeeManagement.Test;

public class EmployeeManagementSecurityHeadersMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_Invoke_SetsExpectedResponseHeaders()
    {
        var httpContext = new DefaultHttpContext();
        RequestDelegate next = (HttpContext httpContext) => Task.CompletedTask;

        var middleware = new EmployeeManagementSecurityHeadersMiddleware(next);

        await middleware.InvokeAsync(httpContext);

        var cspHeader = httpContext.Response.Headers["Content-Security-Policy"].ToString();
        var xContentTypeOptionHeader = httpContext.Response.Headers["X-Content-Type-Options"].ToString();
        
        Assert.Equal("default-src 'self';frame-ancestors 'none';",cspHeader);
        Assert.Equal("nosniff", xContentTypeOptionHeader);
    }
}