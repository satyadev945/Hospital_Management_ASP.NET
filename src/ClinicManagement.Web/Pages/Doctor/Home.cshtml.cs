using ClinicManagement.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClinicManagement.Web.Pages.Doctor;

public class HomeModel : PageModel
{
    private readonly IUserService _userService;

    public HomeModel(IUserService userService)
    {
        _userService = userService;
    }

    public string DoctorName { get; set; } = string.Empty;

    public async Task<IActionResult> OnGetAsync()
    {
        var userId = HttpContext.Session.GetInt32("UserId");

        if (userId == null)
        {
            return RedirectToPage("/Account/Login");
        }

        var user = await _userService.GetByIdAsync(userId.Value);

        if (user == null)
        {
            return RedirectToPage("/Account/Login");
        }

        DoctorName = user.Name;

        return Page();
    }
}
