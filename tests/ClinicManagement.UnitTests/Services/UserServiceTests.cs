using ClinicManagement.Application.Services;
using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using FluentAssertions;

namespace ClinicManagement.UnitTests.Services;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<ILogger<UserService>> _mockLogger;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockLogger = new Mock<ILogger<UserService>>();
        _userService = new UserService(_mockUserRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllUsers()
    {
        var users = new List<User>
        {
            new User { Id = 1, Name = "User 1", Email = "user1@test.com" },
            new User { Id = 2, Name = "User 2", Email = "user2@test.com" }
        };

        _mockUserRepository.Setup(x => x.GetAllAsync(default)).ReturnsAsync(users);

        var result = await _userService.GetAllAsync();

        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(users);
    }

    [Fact]
    public async Task GetByIdAsync_WithValidId_ReturnsUser()
    {
        var user = new User { Id = 1, Name = "Test User", Email = "test@test.com" };
        _mockUserRepository.Setup(x => x.GetByIdAsync(1, default)).ReturnsAsync(user);

        var result = await _userService.GetByIdAsync(1);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(user);
    }
}
