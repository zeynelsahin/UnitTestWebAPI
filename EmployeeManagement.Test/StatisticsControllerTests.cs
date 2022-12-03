using AutoMapper;
using EmployeeManagement.Controllers;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit.Sdk;

namespace EmployeeManagement.Test;

public class StatisticsControllerTests
{
    [Fact]
    public void GetStatistics_InputFromHttpConnectionFeature_MustReturnInputtedIps()
    {
        var localIpAddress = System.Net.IPAddress.Parse("111.111.111.111");
        var localPort = 5000;
        var remoteIpAddress = System.Net.IPAddress.Parse("222.222.222.222");
        var remotePort = 8080;

        var featureCollectionMock = new Mock<IFeatureCollection>();

        featureCollectionMock.Setup(p => p.Get<IHttpConnectionFeature>())
            .Returns(new HttpConnectionFeature()
            {
                LocalIpAddress = localIpAddress,
                LocalPort = localPort,
                RemoteIpAddress = remoteIpAddress,
                RemotePort = remotePort
            });

        var httpContextMock = new Mock<HttpContext>();
        httpContextMock.Setup(p => p.Features).Returns(featureCollectionMock.Object);

        var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile<MapperProfiles.StatisticsProfile>());
        var mapper = new Mapper(mapperConfiguration);
        var statisticController = new StatisticsController(mapper)
        {
            ControllerContext = new ControllerContext()
            {
                HttpContext = httpContextMock.Object
            }
        };

        var result = statisticController.GetStatistics();

        var actionResult = Assert.IsType<ActionResult<StatisticsDto>>(result);
        var okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var staticsDto = Assert.IsType<StatisticsDto>(okObjectResult.Value);
        Assert.Equal(localIpAddress.ToString(), staticsDto.LocalIpAddress);
        Assert.Equal(localPort, staticsDto.LocalPort);
        Assert.Equal(remoteIpAddress.ToString(), staticsDto.RemoteIpAddress);
        Assert.Equal(remotePort, staticsDto.RemotePort);
    }

}