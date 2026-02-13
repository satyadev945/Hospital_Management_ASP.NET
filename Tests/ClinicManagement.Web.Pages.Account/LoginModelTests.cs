using Xunit;
using Moq;
using ClinicManagement.Web.Pages.Account;
using ClinicManagement.Domain.Interfaces.Services;
using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Tests.ClinicManagement.Web.Pages.Account;

public class LoginModelTests
{
    private readonly Mock<IUserService> _mockUserService;
    private readonly Mock<ILogger<LoginModel>> _mockLogger;
    private readonly LoginModel _loginModel;

    public LoginModelTests()
    {
        _mockUserService = new Mock<IUserService>();
        _mockLogger = new Mock<ILogger<LoginModel>>();
        _loginModel = new LoginModel(_mockUserService.Object, _mockLogger.Object);

        var httpContext = new DefaultHttpContext();
        httpContext.Session = new MockSession();
        _loginModel.PageContext.HttpContext = httpContext;
    }

    [Fact]
    public void LoginModel_Constructor_ShouldInitialize()
    {
        // Assert
        Assert.NotNull(_loginModel);
        Assert.IsAssignableFrom<PageModel>(_loginModel);
    }

    [Fact]
    public void LoginModel_OnGet_ShouldExecuteWithoutError()
    {
        // Act
        var exception = Record.Exception(() => _loginModel.OnGet());

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void LoginModel_Input_ShouldInitialize()
    {
        // Assert
        Assert.NotNull(_loginModel.Input);
    }

    private class MockSession : ISession
    {
        private readonly Dictionary<string, byte[]> _sessionStorage = new();

        public string Id => "test-session-id";
        public bool IsAvailable => true;
        public IEnumerable<string> Keys => _sessionStorage.Keys;

        public void Clear() => _sessionStorage.Clear();

        public Task CommitAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

        public Task LoadAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

        public void Remove(string key) => _sessionStorage.Remove(key);

        public void Set(string key, byte[] value) => _sessionStorage[key] = value;

        public bool TryGetValue(string key, out byte[]? value) => _sessionStorage.TryGetValue(key, out value);
    }
}
