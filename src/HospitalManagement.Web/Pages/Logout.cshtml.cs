using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HospitalManagement.Web.Pages
{
    public class LogoutModel : PageModel
    {
        private readonly ILogger<LogoutModel> _logger;

        public LogoutModel(ILogger<LogoutModel> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var userName = User.Identity?.Name ?? "Unknown";

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            _logger.LogInformation("User {UserName} logged out at {Time}", userName, DateTime.UtcNow);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userName = User.Identity?.Name ?? "Unknown";

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            _logger.LogInformation("User {UserName} logged out at {Time}", userName, DateTime.UtcNow);

            return RedirectToPage("/Login");
        }
    }
}
