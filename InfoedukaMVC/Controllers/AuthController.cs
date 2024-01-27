using InfoedukaMVC.Models;
using InfoedukaMVC.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InfoedukaMVC.Controllers
{
    public class AuthController : Controller
    {
        private readonly LcolinaDbContext _context;
        private readonly AuthService _authService;

        public AuthController(LcolinaDbContext context, AuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        // GET: AuthController/Create
        public ActionResult Login()
        {
            return View();
        }

        //// POST: AuthController/Create
        //[HttpPost]
        //public ActionResult Login(string username, string pass)
        //{
        //    var user = _context.AppUsers.FirstOrDefault(x => x.UserName == username && x.Pass == pass);

        //    if (user != null)
        //    {
        //        _authService.SignIn(user.UserId, user.UserName, user.UserTypeId);
        //        ViewBag.AuthData = _authService.IsUserAuthenticated().ToString();
        //        return RedirectToAction("Index", "Home");
        //    }
        //    else
        //    {
        //        ViewBag.Message = "Invalid username or password";
        //        return View();
        //    }   
        //}
        [HttpPost]
        public IActionResult Login(string username, string pass)
        {
            var user = _context.AppUsers.FirstOrDefault(x => x.UserName == username && x.Pass == pass);
            // Validate credentials (this is a simplified example, do not use in production)
            if (user != null)
            {
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, username),
            // Add other shared claims
        };

                if (user.UserTypeId == 1)
                {
                    claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                    // Add other admin-specific claims
                }
                else if (user.UserTypeId == 2)
                {
                    claims.Add(new Claim(ClaimTypes.Role, "RegularUser"));
                    // Add other regular user-specific claims
                }

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    // Customize authentication properties if needed
                };

                HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties).Wait();

                return RedirectToAction("Index", "Home"); // Redirect to the home page after successful login
            }

            // Handle invalid credentials
            return View();
        }
        public IActionResult Signout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).Wait();
            return RedirectToAction("Index", "Home"); // Redirect to the home page after logout
        }

    }
}
