using System.Threading.Tasks;
using HospitalManagement.Domain.DTOs;

namespace HospitalManagement.Domain.Interfaces.Services
{
    public interface IAuthService
    {
        /// <summary>
        /// Authenticates a user with email and password
        /// </summary>
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);

        /// <summary>
        /// Registers a new patient
        /// </summary>
        Task<AuthResponseDto> RegisterPatientAsync(PatientCreateDto patientCreateDto);

        /// <summary>
        /// Registers a new doctor (Admin only)
        /// </summary>
        Task<AuthResponseDto> RegisterDoctorAsync(DoctorCreateDto doctorCreateDto);

        /// <summary>
        /// Changes user password
        /// </summary>
        Task<bool> ChangePasswordAsync(int loginId, string oldPassword, string newPassword);

        /// <summary>
        /// Resets user password
        /// </summary>
        Task<bool> ResetPasswordAsync(string email, string newPassword);

        /// <summary>
        /// Gets user by ID
        /// </summary>
        Task<UserDto> GetUserByIdAsync(int loginId);

        /// <summary>
        /// Gets user by email
        /// </summary>
        Task<UserDto> GetUserByEmailAsync(string email);

        /// <summary>
        /// Checks if email exists
        /// </summary>
        Task<bool> EmailExistsAsync(string email);

        /// <summary>
        /// Validates user credentials
        /// </summary>
        Task<bool> ValidateCredentialsAsync(string email, string password);

        /// <summary>
        /// Logs out a user
        /// </summary>
        Task<bool> LogoutAsync(int loginId);
    }
}
