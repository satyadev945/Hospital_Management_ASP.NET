using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HospitalManagement.Web.Pages
{
    public class AccessDeniedModel : PageModel
    {
        private readonly ILogger<AccessDeniedModel> _logger;

        public AccessDeniedModel(ILogger<AccessDeniedModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            var userName = User.Identity?.Name ?? "Anonymous";
            var requestedPath = HttpContext.Request.Path;

            _logger.LogWarning(
                "Access denied for user {UserName} attempting to access {Path}",
                userName,
                requestedPath);
        }
    }
}
