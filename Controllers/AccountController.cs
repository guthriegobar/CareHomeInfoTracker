using CareHomeInfoTracker.Data;
using CareHomeInfoTracker.Models;
using CareHomeInfoTracker.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CareHomeInfoTracker.Controllers
{
    public class AccountController(CareHomeInfoContext dataContext) : Controller
    {
        private readonly CareHomeInfoContext _dataContext = dataContext;

        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel user, string returnUrl = null)
        {
            if (!ModelState.IsValid) return View();
            var _user = await _dataContext.SystemUsers.FirstOrDefaultAsync(s => s.Id == user.Id);
            if (_user == null || _user.Password != user.Password)
            {
                ViewBag.InvalidUserPassword = true;
                return View(user);
            
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, _user.FirstName),
                new Claim("Role", _user.Role)
            };
            var identity = new ClaimsIdentity(claims, "CookieAuth");
            var claimsPrincipal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync("CookieAuth", claimsPrincipal);
            if(!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieAuth");
            return RedirectToAction("Index", "Home");
        }
    }
}
