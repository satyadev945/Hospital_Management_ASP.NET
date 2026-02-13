using Xunit;
using ClinicManagement.Web.Pages;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Tests.ClinicManagement.Web.Pages;

public class IndexModelTests
{
    [Fact]
    public void IndexModel_Constructor_ShouldInitialize()
    {
        // Arrange & Act
        var model = new IndexModel();

        // Assert
        Assert.NotNull(model);
        Assert.IsAssignableFrom<PageModel>(model);
    }

    [Fact]
    public void IndexModel_OnGet_ShouldExecuteWithoutError()
    {
        // Arrange
        var model = new IndexModel();

        // Act
        var exception = Record.Exception(() => model.OnGet());

        // Assert
        Assert.Null(exception);
    }
}
