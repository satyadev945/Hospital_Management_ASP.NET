using Xunit;
using Moq;
using ClinicManagement.Web.Pages.Patient;
using ClinicManagement.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Tests.ClinicManagement.Web.Pages.Patient;

public class HomeModelTests
{
    private readonly Mock<IUserService> _mockUserService;
    private readonly HomeModel _homeModel;

    public HomeModelTests()
    {
        _mockUserService = new Mock<IUserService>();
        _homeModel = new HomeModel(_mockUserService.Object);
    }

    [Fact]
    public void HomeModel_Constructor_ShouldInitialize()
    {
        // Assert
        Assert.NotNull(_homeModel);
        Assert.IsAssignableFrom<PageModel>(_homeModel);
    }

    [Fact]
    public void HomeModel_PatientName_ShouldInitializeEmpty()
    {
        // Assert
        Assert.Equal(string.Empty, _homeModel.PatientName);
    }

    [Fact]
    public void HomeModel_PatientName_ShouldBeSettable()
    {
        // Arrange
        var name = "John Doe";

        // Act
        _homeModel.PatientName = name;

        // Assert
        Assert.Equal(name, _homeModel.PatientName);
    }
}
