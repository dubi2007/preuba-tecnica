using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRUEBAWEBLOGIN.Data;
using System.Security.Claims;

namespace PRUEBAWEBLOGIN.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public IActionResult Welcome()
        {
            return View();
        }

        public async Task<IActionResult> Profile()
        {
            var username = User.Identities.FirstOrDefault()?.Claims.FirstOrDefault(c => c.Type == "Username")?.Value;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            
            if (user == null) return RedirectToAction("Login", "Account");

            return View(user);
        }
    }
}