using Xunit;
using Moq;
using ClinicManagement.Web.Pages.Account;
using ClinicManagement.Domain.Interfaces.Services;
using ClinicManagement.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Tests.ClinicManagement.Web.Pages.Account;

public class RegisterModelTests
{
    private readonly Mock<IUserService> _mockUserService;
    private readonly Mock<IPatientRepository> _mockPatientRepository;
    private readonly Mock<ILogger<RegisterModel>> _mockLogger;
    private readonly RegisterModel _registerModel;

    public RegisterModelTests()
    {
        _mockUserService = new Mock<IUserService>();
        _mockPatientRepository = new Mock<IPatientRepository>();
        _mockLogger = new Mock<ILogger<RegisterModel>>();
        _registerModel = new RegisterModel(
            _mockUserService.Object,
            _mockPatientRepository.Object,
            _mockLogger.Object);
    }

    [Fact]
    public void RegisterModel_Constructor_ShouldInitialize()
    {
        // Assert
        Assert.NotNull(_registerModel);
        Assert.IsAssignableFrom<PageModel>(_registerModel);
    }

    [Fact]
    public void RegisterModel_OnGet_ShouldExecuteWithoutError()
    {
        // Act
        var exception = Record.Exception(() => _registerModel.OnGet());

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void RegisterModel_Input_ShouldInitialize()
    {
        // Assert
        Assert.NotNull(_registerModel.Input);
    }
}
