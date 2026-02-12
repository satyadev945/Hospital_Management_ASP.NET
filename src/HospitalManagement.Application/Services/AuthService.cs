using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using HospitalManagement.Domain.DTOs;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces.Repositories;
using HospitalManagement.Domain.Interfaces.Services;

namespace HospitalManagement.Application.Services
{
    /// <summary>
    /// Service implementation for authentication and authorization operations
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly ILoginRepository _loginRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            ILoginRepository loginRepository,
            IPatientRepository patientRepository,
            IDoctorRepository doctorRepository,
            IMapper mapper,
            ILogger<AuthService> logger)
        {
            _loginRepository = loginRepository ?? throw new ArgumentNullException(nameof(loginRepository));
            _patientRepository = patientRepository ?? throw new ArgumentNullException(nameof(patientRepository));
            _doctorRepository = doctorRepository ?? throw new ArgumentNullException(nameof(doctorRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            try
            {
                _logger.LogInformation("Attempting login for email: {Email}", loginDto.Email);

                var login = await _loginRepository.ValidateLoginAsync(
                    loginDto.Email, loginDto.Password, CancellationToken.None);

                if (login == null)
                {
                    _logger.LogWarning("Login failed for email: {Email} - Invalid credentials", loginDto.Email);
                    return new AuthResponseDto
                    {
                        Success = false,
                        Message = "Invalid email or password",
                        User = null,
                        Token = null
                    };
                }

                if (!login.IsActive)
                {
                    _logger.LogWarning("Login failed for email: {Email} - Account is inactive", loginDto.Email);
                    return new AuthResponseDto
                    {
                        Success = false,
                        Message = "Account is inactive. Please contact administrator.",
                        User = null,
                        Token = null
                    };
                }

                if (login.IsLockedOut)
                {
                    _logger.LogWarning("Login failed for email: {Email} - Account is locked", loginDto.Email);
                    return new AuthResponseDto
                    {
                        Success = false,
                        Message = $"Account is locked until {login.LockoutEnd?.ToString("yyyy-MM-dd HH:mm:ss")}",
                        User = null,
                        Token = null
                    };
                }

                // Update last login date
                login.LastLoginDate = DateTime.UtcNow;
                login.FailedLoginAttempts = 0;
                await _loginRepository.UpdateAsync(login, CancellationToken.None);

                var userDto = _mapper.Map<UserDto>(login);
                userDto.TypeName = GetUserTypeName(login.Type);

                _logger.LogInformation("Login successful for email: {Email}, Type: {UserType}",
                    loginDto.Email, userDto.TypeName);

                return new AuthResponseDto
                {
                    Success = true,
                    Message = "Login successful",
                    User = userDto,
                    Token = GenerateToken(login) // In production, implement proper JWT token generation
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for email: {Email}", loginDto.Email);
                throw;
            }
        }

        public async Task<AuthResponseDto> RegisterPatientAsync(PatientCreateDto patientCreateDto)
        {
            try
            {
                _logger.LogInformation("Attempting to register patient with email: {Email}", patientCreateDto.Email);

                // Check if email already exists
                var existingLogin = await _loginRepository.GetByEmailAsync(
                    patientCreateDto.Email, CancellationToken.None);

                if (existingLogin != null)
                {
                    _logger.LogWarning("Registration failed - Email {Email} already exists", patientCreateDto.Email);
                    return new AuthResponseDto
                    {
                        Success = false,
                        Message = "Email already exists",
                        User = null,
                        Token = null
                    };
                }

                // Create login record
                var login = new Login
                {
                    Email = patientCreateDto.Email,
                    Password = patientCreateDto.Password, // In production, hash the password
                    Type = 1, // 1 = Patient
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                };

                var createdLogin = await _loginRepository.AddAsync(login, CancellationToken.None);

                // Create patient record
                var patient = _mapper.Map<Patient>(patientCreateDto);
                patient.PatientID = createdLogin.LoginID;
                patient.IsActive = true;
                patient.CreatedDate = DateTime.UtcNow;

                await _patientRepository.AddAsync(patient, CancellationToken.None);

                var userDto = _mapper.Map<UserDto>(createdLogin);
                userDto.TypeName = "Patient";

                _logger.LogInformation("Successfully registered patient with email: {Email}, LoginID: {LoginID}",
                    patientCreateDto.Email, createdLogin.LoginID);

                return new AuthResponseDto
                {
                    Success = true,
                    Message = "Registration successful",
                    User = userDto,
                    Token = GenerateToken(createdLogin)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during patient registration for email: {Email}", patientCreateDto.Email);
                throw;
            }
        }

        public async Task<AuthResponseDto> RegisterDoctorAsync(DoctorCreateDto doctorCreateDto)
        {
            try
            {
                _logger.LogInformation("Attempting to register doctor with email: {Email}", doctorCreateDto.Email);

                // Check if email already exists
                var existingLogin = await _loginRepository.GetByEmailAsync(
                    doctorCreateDto.Email, CancellationToken.None);

                if (existingLogin != null)
                {
                    _logger.LogWarning("Registration failed - Email {Email} already exists", doctorCreateDto.Email);
                    return new AuthResponseDto
                    {
                        Success = false,
                        Message = "Email already exists",
                        User = null,
                        Token = null
                    };
                }

                // Create login record
                var login = new Login
                {
                    Email = doctorCreateDto.Email,
                    Password = doctorCreateDto.Password, // In production, hash the password
                    Type = 2, // 2 = Doctor
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                };

                var createdLogin = await _loginRepository.AddAsync(login, CancellationToken.None);

                // Create doctor record
                var doctor = _mapper.Map<Doctor>(doctorCreateDto);
                doctor.DoctorID = createdLogin.LoginID;
                doctor.IsActive = true;
                doctor.PatientsTreated = 0;
                doctor.ReputeIndex = 0.0m;
                doctor.CreatedDate = DateTime.UtcNow;

                await _doctorRepository.AddAsync(doctor, CancellationToken.None);

                var userDto = _mapper.Map<UserDto>(createdLogin);
                userDto.TypeName = "Doctor";

                _logger.LogInformation("Successfully registered doctor with email: {Email}, LoginID: {LoginID}",
                    doctorCreateDto.Email, createdLogin.LoginID);

                return new AuthResponseDto
                {
                    Success = true,
                    Message = "Doctor registration successful",
                    User = userDto,
                    Token = GenerateToken(createdLogin)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during doctor registration for email: {Email}", doctorCreateDto.Email);
                throw;
            }
        }

        public async Task<bool> ChangePasswordAsync(int loginId, string oldPassword, string newPassword)
        {
            try
            {
                _logger.LogInformation("Attempting to change password for login ID: {LoginId}", loginId);

                // Verify old password
                var isValid = await _loginRepository.VerifyPasswordAsync(loginId, oldPassword, CancellationToken.None);
                if (!isValid)
                {
                    _logger.LogWarning("Password change failed for login ID: {LoginId} - Old password is incorrect", loginId);
                    return false;
                }

                // Update to new password
                var result = await _loginRepository.UpdatePasswordAsync(
                    loginId, newPassword, CancellationToken.None); // In production, hash the password

                if (result)
                {
                    _logger.LogInformation("Successfully changed password for login ID: {LoginId}", loginId);
                }
                else
                {
                    _logger.LogWarning("Failed to change password for login ID: {LoginId}", loginId);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing password for login ID: {LoginId}", loginId);
                throw;
            }
        }

        public async Task<bool> ResetPasswordAsync(string email, string newPassword)
        {
            try
            {
                _logger.LogInformation("Attempting to reset password for email: {Email}", email);

                var result = await _loginRepository.UpdatePasswordByEmailAsync(
                    email, newPassword, CancellationToken.None); // In production, hash the password

                if (result)
                {
                    _logger.LogInformation("Successfully reset password for email: {Email}", email);
                }
                else
                {
                    _logger.LogWarning("Failed to reset password for email: {Email}", email);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error resetting password for email: {Email}", email);
                throw;
            }
        }

        public async Task<UserDto> GetUserByIdAsync(int loginId)
        {
            try
            {
                _logger.LogInformation("Retrieving user with login ID: {LoginId}", loginId);

                var login = await _loginRepository.GetByIdAsync(loginId, CancellationToken.None);

                if (login == null)
                {
                    _logger.LogWarning("User with login ID {LoginId} not found", loginId);
                    return null;
                }

                var userDto = _mapper.Map<UserDto>(login);
                userDto.TypeName = GetUserTypeName(login.Type);

                _logger.LogInformation("Successfully retrieved user with login ID: {LoginId}", loginId);

                return userDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user with login ID: {LoginId}", loginId);
                throw;
            }
        }

        public async Task<UserDto> GetUserByEmailAsync(string email)
        {
            try
            {
                _logger.LogInformation("Retrieving user with email: {Email}", email);

                var login = await _loginRepository.GetByEmailAsync(email, CancellationToken.None);

                if (login == null)
                {
                    _logger.LogWarning("User with email {Email} not found", email);
                    return null;
                }

                var userDto = _mapper.Map<UserDto>(login);
                userDto.TypeName = GetUserTypeName(login.Type);

                _logger.LogInformation("Successfully retrieved user with email: {Email}", email);

                return userDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user with email: {Email}", email);
                throw;
            }
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            try
            {
                _logger.LogInformation("Checking if email exists: {Email}", email);

                var exists = await _loginRepository.EmailExistsAsync(email, CancellationToken.None);

                _logger.LogInformation("Email {Email} exists: {Exists}", email, exists);

                return exists;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if email exists: {Email}", email);
                throw;
            }
        }

        public async Task<bool> ValidateCredentialsAsync(string email, string password)
        {
            try
            {
                _logger.LogInformation("Validating credentials for email: {Email}", email);

                var login = await _loginRepository.ValidateLoginAsync(email, password, CancellationToken.None);
                var isValid = login != null;

                _logger.LogInformation("Credentials validation for email {Email}: {IsValid}", email, isValid);

                return isValid;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating credentials for email: {Email}", email);
                throw;
            }
        }

        public async Task<bool> LogoutAsync(int loginId)
        {
            try
            {
                _logger.LogInformation("Logging out user with login ID: {LoginId}", loginId);

                // In production, you might want to invalidate tokens, clear sessions, etc.
                // For now, we'll just log the logout action

                _logger.LogInformation("Successfully logged out user with login ID: {LoginId}", loginId);

                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error logging out user with login ID: {LoginId}", loginId);
                throw;
            }
        }

        private string GetUserTypeName(int type)
        {
            return type switch
            {
                1 => "Patient",
                2 => "Doctor",
                3 => "Admin",
                4 => "Staff",
                _ => "Unknown"
            };
        }

        private string GenerateToken(Login login)
        {
            // In production, implement proper JWT token generation with signing key
            // For now, return a placeholder token
            var tokenPayload = $"{login.LoginID}:{login.Email}:{login.Type}:{DateTime.UtcNow.Ticks}";
            var token = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(tokenPayload));

            _logger.LogInformation("Generated token for login ID: {LoginId}", login.LoginID);

            return token;
        }
    }
}
