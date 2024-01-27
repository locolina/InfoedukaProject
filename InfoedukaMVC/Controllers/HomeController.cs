using InfoedukaMVC.Models;
using InfoedukaMVC.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace InfoedukaMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly LcolinaDbContext _context;
        private readonly AuthService _authService;

        public HomeController(LcolinaDbContext context, AuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var comments = _context.Comments.Include(c => c.Course).Include(c => c.User);
            await comments.ToListAsync();
            IList<Comment> activeComments = new List<Comment>();
            foreach (var comment in comments)
            {
                if (comment.IsActive == true)
                {
                    activeComments.Add(comment);
                }



            }



            return View(activeComments);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
