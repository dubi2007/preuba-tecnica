using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRUEBAWEBLOGIN.Data;

namespace PRUEBAWEBLOGIN.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        // nmuestra formulario de login
        [HttpGet]
        public IActionResult Login(bool expired = false)
        {
            if (User.Identity!.IsAuthenticated) return RedirectToAction("Profile", "Home");
            if (expired)
            {
                ViewBag.SessionExpired = true;
            }
            return View();
        }

        // valida credenciales del usuario
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

            // valida si el usuario existe
            if (user == null)
            {
                ViewBag.UsernameError = "Usuario incorrecto";
                return View();
            }

            // verifica si la cuenta está bloqueada
            if (user.LockoutEnd.HasValue && user.LockoutEnd.Value > DateTime.UtcNow)
            {
                return RedirectToAction("Lockout");
            }

            // vlidae contraseña y control de intentos fallidos
            if (user.Password != password)
            {
                user.AccessFailedCount++;
                if (user.AccessFailedCount >= 5)
                {
                    // bloquear cuenta por 1 minuto   
                    user.LockoutEnd = DateTime.UtcNow.AddMinutes(1);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Lockout");
                }
                
                await _context.SaveChangesAsync();
                ViewBag.UsernameValue = username;
                ViewBag.PasswordError = "Contraseña incorrecta";
                return View();
            }

            // reiniciar contador de intentos fallidos al iniciar sesión correctamente
            user.AccessFailedCount = 0;
            user.LockoutEnd = null;
            await _context.SaveChangesAsync();

            // Crear claims de autenticación
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("Username", user.Username)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            
            // Configurar propiedades de autenticación  tiempo limite se sesion 
            var authProperties = new AuthenticationProperties 
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddSeconds(60)
            };
            
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            return RedirectToAction("Profile", "Home");
        }

        // Página de cuenta bloqueada
        [HttpGet]
        public IActionResult Lockout()
        {
            return View();
        }

        // Extender sesión (renueva la cookie de autenticación)
        [HttpPost]
        [Route("Account/ExtendSession")]
        public IActionResult ExtendSession()
        {
            return Ok();
        }

        // Cerrar sesión
        [HttpPost]
        [HttpGet]
        public async Task<IActionResult> Logout(bool expired = false)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (expired)
            {
                return RedirectToAction("Login", new { expired = true });
            }
            return RedirectToAction("Login");
        }
    }
}