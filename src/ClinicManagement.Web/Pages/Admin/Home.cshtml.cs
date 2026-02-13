using ClinicManagement.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClinicManagement.Web.Pages.Admin;

public class HomeModel : PageModel
{
    private readonly IUserService _userService;

    public HomeModel(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var userId = HttpContext.Session.GetInt32("UserId");

        if (userId == null)
        {
            return RedirectToPage("/Account/Login");
        }

        return Page();
    }
}
