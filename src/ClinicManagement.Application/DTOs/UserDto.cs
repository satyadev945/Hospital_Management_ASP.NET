using ClinicManagement.Domain.Enums;

namespace ClinicManagement.Application.DTOs;

/// <summary>
/// Data transfer object for User
/// </summary>
public class UserDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public string PhoneNo { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public UserType UserType { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
}

/// <summary>
/// Data transfer object for creating a User
/// </summary>
public class UserCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public string PhoneNo { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public UserType UserType { get; set; }
}

/// <summary>
/// Data transfer object for updating a User
/// </summary>
public class UserUpdateDto
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public string PhoneNo { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
}

/// <summary>
/// Data transfer object for login
/// </summary>
public class LoginDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

/// <summary>
/// Data transfer object for login response
/// </summary>
public class LoginResponseDto
{
    public bool Success { get; set; }
    public int UserId { get; set; }
    public UserType UserType { get; set; }
    public string Message { get; set; } = string.Empty;
}
