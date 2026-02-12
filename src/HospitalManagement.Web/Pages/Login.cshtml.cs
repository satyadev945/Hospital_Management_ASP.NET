using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using HospitalManagement.Domain.Interfaces.Repositories;

namespace HospitalManagement.Web.Pages
{
    public class LoginModel : PageModel
    {
        private readonly ILoginRepository _loginRepository;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(ILoginRepository loginRepository, ILogger<LoginModel> logger)
        {
            _loginRepository = loginRepository;
            _logger = logger;
        }

        [BindProperty]
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [BindProperty]
        public int UserType { get; set; } = 1; // Default: Patient

        [BindProperty]
        public bool RememberMe { get; set; }

        public string? ErrorMessage { get; set; }
        public string? SuccessMessage { get; set; }

        public void OnGet(string? returnUrl = null, string? message = null)
        {
            if (!string.IsNullOrEmpty(message))
            {
                SuccessMessage = message;
            }

            ViewData["ReturnUrl"] = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                // Validate login credentials
                var login = await _loginRepository.ValidateLoginAsync(Email, Password);

                if (login == null)
                {
                    ErrorMessage = "Invalid email or password. Please try again.";
                    _logger.LogWarning("Failed login attempt for email: {Email}", Email);
                    return Page();
                }

                // Check if user type matches
                if (login.Type != UserType)
                {
                    ErrorMessage = $"This account is not registered as a {GetUserTypeName(UserType)}. Please select the correct user type.";
                    _logger.LogWarning("User type mismatch for email: {Email}. Expected: {Expected}, Actual: {Actual}",
                        Email, UserType, login.Type);
                    return Page();
                }

                // Check if account is locked
                if (login.IsLockedOut)
                {
                    ErrorMessage = "Your account has been locked due to multiple failed login attempts. Please try again later.";
                    _logger.LogWarning("Login attempt for locked account: {Email}", Email);
                    return Page();
                }

                // Create claims
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, login.LoginID.ToString()),
                    new Claim(ClaimTypes.Name, Email),
                    new Claim(ClaimTypes.Email, Email),
                    new Claim(ClaimTypes.Role, GetUserTypeName(login.Type)),
                    new Claim("UserType", login.Type.ToString())
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = RememberMe,
                    ExpiresUtc = RememberMe ? DateTimeOffset.UtcNow.AddDays(30) : DateTimeOffset.UtcNow.AddHours(8),
                    AllowRefresh = true
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    claimsPrincipal,
                    authProperties);

                _logger.LogInformation("User {Email} logged in successfully as {Role}", Email, GetUserTypeName(login.Type));

                // Redirect based on user type
                return login.Type switch
                {
                    1 => RedirectToPage("/Patient/Dashboard"),
                    2 => RedirectToPage("/Doctor/Dashboard"),
                    3 => RedirectToPage("/Admin/Index"),
                    4 => RedirectToPage("/Staff/Dashboard"),
                    _ => LocalRedirect(returnUrl)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for email: {Email}", Email);
                ErrorMessage = "An error occurred during login. Please try again later.";
                return Page();
            }
        }

        private static string GetUserTypeName(int userType)
        {
            return userType switch
            {
                1 => "Patient",
                2 => "Doctor",
                3 => "Admin",
                4 => "Staff",
                _ => "Unknown"
            };
        }
    }
}
