using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces.Repositories;
using PatientEntity = HospitalManagement.Domain.Entities.Patient;

namespace HospitalManagement.Web.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly ILoginRepository _loginRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly ILogger<RegisterModel> _logger;

        public RegisterModel(
            ILoginRepository loginRepository,
            IPatientRepository patientRepository,
            ILogger<RegisterModel> logger)
        {
            _loginRepository = loginRepository;
            _patientRepository = patientRepository;
            _logger = logger;
        }

        [BindProperty]
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        public string Phone { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "Confirm password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "Date of birth is required")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [BindProperty]
        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters")]
        public string? Address { get; set; }

        public string? ErrorMessage { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                // Validate age (must be at least 1 year old)
                var age = DateTime.Now.Year - BirthDate.Year;
                if (DateTime.Now.DayOfYear < BirthDate.DayOfYear)
                    age--;

                if (age < 1)
                {
                    ErrorMessage = "Invalid date of birth. Patient must be at least 1 year old.";
                    return Page();
                }

                if (age > 150)
                {
                    ErrorMessage = "Invalid date of birth. Please enter a valid date.";
                    return Page();
                }

                // Check if email already exists
                var emailExists = await _loginRepository.EmailExistsAsync(Email);
                if (emailExists)
                {
                    ErrorMessage = "An account with this email already exists. Please use a different email or login.";
                    _logger.LogWarning("Registration attempt with existing email: {Email}", Email);
                    return Page();
                }

                // Create login record
                var login = new Login
                {
                    Email = Email,
                    Password = Password, // In production, this should be hashed
                    Type = 1, // Patient type
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "System"
                };

                var createdLogin = await _loginRepository.AddAsync(login);

                // Create patient record
                var patient = new PatientEntity
                {
                    PatientID = createdLogin.LoginID,
                    Name = Name,
                    Email = Email,
                    Password = Password, // In production, this should be hashed
                    Phone = Phone,
                    Gender = Gender,
                    BirthDate = BirthDate,
                    Address = Address,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "System"
                };

                await _patientRepository.AddAsync(patient);

                _logger.LogInformation("New patient registered successfully: {Email}", Email);

                // Redirect to login page with success message
                return RedirectToPage("/Login", new { message = "Registration successful! Please login to continue." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during patient registration for email: {Email}", Email);
                ErrorMessage = "An error occurred during registration. Please try again later.";
                return Page();
            }
        }
    }
}
