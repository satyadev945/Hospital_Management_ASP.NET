using ClinicManagement.Domain.Enums;

namespace ClinicManagement.Domain.Entities;

/// <summary>
/// Represents a user in the clinic management system
/// </summary>
public class User
{
    /// <summary>
    /// User unique identifier
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// User full name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// User email address (used for login)
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// User password (hashed)
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// User birth date
    /// </summary>
    public DateTime BirthDate { get; set; }

    /// <summary>
    /// User phone number
    /// </summary>
    public string PhoneNo { get; set; } = string.Empty;

    /// <summary>
    /// User gender
    /// </summary>
    public string Gender { get; set; } = string.Empty;

    /// <summary>
    /// User address
    /// </summary>
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// User type (Patient, Doctor, Admin)
    /// </summary>
    public UserType UserType { get; set; }

    /// <summary>
    /// Date when the user was created
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Date when the user was last modified
    /// </summary>
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// Indicates if the user is active
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Who created the user
    /// </summary>
    public string CreatedBy { get; set; } = "System";

    /// <summary>
    /// Who modified the user
    /// </summary>
    public string? ModifiedBy { get; set; }
}
