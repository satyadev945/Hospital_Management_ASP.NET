using Xunit;
using Moq;
using ClinicManagement.Web.Pages.Doctor;
using ClinicManagement.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Tests.ClinicManagement.Web.Pages.Doctor;

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
    public void HomeModel_DoctorName_ShouldInitializeEmpty()
    {
        // Assert
        Assert.Equal(string.Empty, _homeModel.DoctorName);
    }

    [Fact]
    public void HomeModel_DoctorName_ShouldBeSettable()
    {
        // Arrange
        var name = "Dr. Smith";

        // Act
        _homeModel.DoctorName = name;

        // Assert
        Assert.Equal(name, _homeModel.DoctorName);
    }
}
