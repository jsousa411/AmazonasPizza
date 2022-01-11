using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using StarterProject.Context.Contexts.AppContext;
using StarterProject.Crosscutting;
using StarterProject.WebSite.Models;
using System.Security.Claims;

namespace StarterProject.WebSite.Controllers
{
    public class AccountController : BaseController
    {
        public IActionResult Index()
        {
            return RedirectToAction("Login");
        }

        public IActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new LoginModel { ReturnUrl = returnUrl });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel login)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(login.Email) || string.IsNullOrWhiteSpace(login.Password))
                {
                    throw new Exception("Invalid user or password");
                }

                var user = Context.User.FirstOrDefault(c => c.Email == login.Email);
                if (user == null || !Tools.VerifyPassword(login.Password, user.Salt, user.Password))
                {
                    throw new Exception("Invalid user or password");
                }

                await Authenticate(user, login.KeepConnected);

                if (login.ReturnUrl != null)
                {
                    return Redirect(login.ReturnUrl);
                }

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(login);
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }

        private async Task Authenticate(User user, bool isPersistent)
        {
            var claims = new List<Claim>
                {
                    new Claim("sub", user.Id.ToString()),
                    new Claim("user", user.Email),
                    new Claim("name", user.Name),
                    new Claim("email", user.Email)
                };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(new ClaimsIdentity(claims, "Cookies", "user", "role")),
                new AuthenticationProperties
                {
                    IsPersistent = isPersistent
                });
        }
    }
}