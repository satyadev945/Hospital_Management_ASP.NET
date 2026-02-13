using Xunit;
using Moq;
using ClinicManagement.Web.Pages.Admin;
using ClinicManagement.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Tests.ClinicManagement.Web.Pages.Admin;

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
}
