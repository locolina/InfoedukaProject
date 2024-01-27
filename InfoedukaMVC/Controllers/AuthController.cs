using InfoedukaMVC.Models;
using InfoedukaMVC.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        // POST: AuthController/Create
        [HttpPost]
        public ActionResult Login(string username, string pass)
        {
            var user = _context.AppUsers.FirstOrDefault(x => x.UserName == username && x.Pass == pass);

            if (user != null)
            {
                _authService.SignIn(user.UserId, user.UserName, user.UserTypeId);
                ViewBag.AuthData = _authService.IsUserAuthenticated().ToString();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Message = "Invalid username or password";
                return View();
            }   
        }


    }
}
