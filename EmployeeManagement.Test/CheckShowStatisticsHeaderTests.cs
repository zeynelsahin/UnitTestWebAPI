using EmployeeManagement.ActionFilters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EmployeeManagement.Test;

public class CheckShowStatisticsHeaderTests
{
     [Fact]
     public void OnActionExecuting_InvokeWithoutShowStatisticsHeader_ReturnBadRequest()
     {
          var checkShowStatisticsHeaderActionFilter = new CheckShowStatisticsHeader();
          var httpContext = new DefaultHttpContext();
          var actionContext = new ActionContext(httpContext, new(), new(), new());
          var actionExecutingContext = new ActionExecutingContext(actionContext, new List<IFilterMetadata>(), new Dictionary<string, object?>(), controller: null);
          
          checkShowStatisticsHeaderActionFilter.OnActionExecuting(actionExecutingContext);

          Assert.IsType<BadRequestResult>(actionExecutingContext.Result);
     }
}