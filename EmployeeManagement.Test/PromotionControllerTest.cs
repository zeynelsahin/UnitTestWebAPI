using System.Net;
using System.Text;
using System.Text.Json;
using EmployeeManagement.Business;
using EmployeeManagement.Controllers;
using EmployeeManagement.DataAccess.Entities;
using EmployeeManagement.Models;
using EmployeeManagement.Services.Test;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace EmployeeManagement.Test;

public class PromotionControllerTest
{
    [Fact]
    public async Task CreatePromotion_RequestPromotionForEligibleEmployee_MustPromoteEmployee()
    {
        var expectedEmployeeId = Guid.NewGuid();
        var currentJobLevel = 1;
        var promotionForCreationDto = new PromotionForCreationDto()
        {
            EmployeeId = expectedEmployeeId
        };
        var employeeServiceMock = new Mock<IEmployeeService>();

        employeeServiceMock.Setup(m => m.FetchInternalEmployeeAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new InternalEmployee("Zeynel", "Sahin", 3, 3500, true, currentJobLevel)
            {
                Id = expectedEmployeeId,
                SuggestedBonus = 500
            });

        var eligibleFroPromotionHandlerMock = new Mock<HttpMessageHandler>();
        eligibleFroPromotionHandlerMock.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(new PromotionEligibility() { EligibleForPromotion = true }, new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                }), Encoding.ASCII, "application/json")
            });
        var httpClient = new HttpClient(eligibleFroPromotionHandlerMock.Object);
        var promotionService = new PromotionService(httpClient, new EmployeeManagementTestDataRepository());

        var promotionController = new PromotionsController(employeeServiceMock.Object, promotionService);

        var result = await promotionController.CreatePromotion(promotionForCreationDto);
        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        var promotionResultDto = Assert.IsType<PromotionResultDto>(okObjectResult.Value);

        Assert.Equal(expectedEmployeeId, promotionResultDto.EmployeeId);
        Assert.Equal(++currentJobLevel, promotionResultDto.JobLevel);
    }
}