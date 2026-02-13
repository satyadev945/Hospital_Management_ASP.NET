using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Enums;
using ClinicManagement.Domain.Interfaces.Repositories;
using ClinicManagement.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

using PatientEntity = ClinicManagement.Domain.Entities.Patient;

namespace ClinicManagement.Web.Pages.Account;

public class RegisterModel : PageModel
{
    private readonly IUserService _userService;
    private readonly IPatientRepository _patientRepository;
    private readonly ILogger<RegisterModel> _logger;

    public RegisterModel(IUserService userService, IPatientRepository patientRepository, ILogger<RegisterModel> logger)
    {
        _userService = userService;
        _patientRepository = patientRepository;
        _logger = logger;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public class InputModel
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [Required]
        [Phone]
        public string PhoneNo { get; set; } = string.Empty;

        [Required]
        public string Gender { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Address { get; set; } = string.Empty;
    }

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
            var user = new User
            {
                Name = Input.Name,
                Email = Input.Email,
                Password = Input.Password,
                BirthDate = Input.BirthDate,
                PhoneNo = Input.PhoneNo,
                Gender = Input.Gender,
                Address = Input.Address,
                UserType = UserType.Patient,
                CreatedDate = DateTime.UtcNow,
                IsActive = true,
                CreatedBy = "System"
            };

            var createdUser = await _userService.CreateAsync(user);

            var patient = new PatientEntity
            {
                UserId = createdUser.Id,
                CreatedDate = DateTime.UtcNow,
                IsActive = true,
                CreatedBy = "System"
            };

            await _patientRepository.AddAsync(patient);

            HttpContext.Session.SetInt32("UserId", createdUser.Id);
            HttpContext.Session.SetInt32("UserType", (int)UserType.Patient);

            return RedirectToPage("/Patient/Home");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during registration for email: {Email}", Input.Email);
            ModelState.AddModelError(string.Empty, "An error occurred during registration. Email may already exist.");
            return Page();
        }
    }
}
