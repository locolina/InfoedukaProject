using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InfoedukaMVC.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InfoedukaMVC.Models;
using InfoedukaMVC.Models.DTO;
using InfoedukaMVC.Services;
using System.ComponentModel.Design;
using System.Security.Claims;

namespace InfoedukaMVC.Controllers
{
    public class CommentsController : Controller
    {
        private readonly LcolinaDbContext _context;
        private readonly AuthService _authService;

        public CommentsController(LcolinaDbContext context, AuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        // GET: Comments
        public async Task<IActionResult> Index()
        {

            var lcolinaDbContext = _context.Comments.Include(c => c.Course).Include(c => c.User);
            return View(await lcolinaDbContext.ToListAsync());
        }

        // GET: Comments/Create
        public IActionResult Create()
        {

            var currentUser = _context.AppUsers
            .Include(u => u.UserCourseMappings)
            .ThenInclude(ucm => ucm.Course)
            .FirstOrDefault(u => u.UserName == User.Identity.Name);

            if (currentUser == null)
            {
                return NotFound(); 
            }

            
            var userCourses = new SelectList(currentUser.UserCourseMappings
                .Select(ucm => ucm.Course), "CourseId", "CourseName");

            ViewData["CourseId"] = userCourses;
            if (User.HasClaim(c => c.Type == ClaimTypes.Role && c.Value == "Admin"))
            {
                ViewData["UserId"] = new SelectList(_context.AppUsers, "UserId", "UserName");

               
                ViewData["AllCourses"] = new SelectList(_context.Courses, "CourseId", "CourseName");

            }
            else
            {
                ViewData["UserId"] = new SelectList(_context.AppUsers
                    .Where(u => u.UserName == User.Identity.Name), "UserId", "UserName");
            }

            return View();
        }

        // POST: Comments/Create
        [HttpPost]
        public async Task<IActionResult> Create([Bind("CommentId,Title,Content,DatePosted,DateExpires,IsActive,UserId,CourseId")] Comment comment)
        {
            ModelState.Remove("User");
            ModelState.Remove("Course");

            if (ModelState.IsValid)
            {
                _context.Add(comment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "CourseId", "CourseId", comment.CourseId);
            ViewData["UserId"] = new SelectList(_context.AppUsers, "UserId", "UserId", comment.UserId);
            return View(comment);
        }

        // GET: Comments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Comments == null)
            {
                return NotFound();
            }
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "CourseId", "CourseName", comment.CourseId);
            ViewData["UserId"] = new SelectList(_context.AppUsers, "UserId", "UserName", comment.UserId);
            return View(comment);
        }

        // POST: Comments/Edit/5
        //[Bind("CommentId,Title,Content,DatePosted,DateExpires,IsActive,UserId,CourseId")]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("CommentId,Title,Content,DatePosted,DateExpires,IsActive,UserId,CourseId")] CommentsDTO comment)
        {
           
            

            var dbComment = CommentMapper.MapToDAL(comment);
            if (id != comment.CommentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dbComment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(dbComment.CommentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "CourseId", "CourseId", comment.CourseId);
            ViewData["UserId"] = new SelectList(_context.AppUsers, "UserId", "UserId", comment.UserId);
            return View(comment);
        }

        // GET: Comments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Comments == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments
                .Include(c => c.Course)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.CommentId == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Comments == null)
            {
                return Problem("Entity set 'LcolinaDbContext.Comments'  is null.");
            }
            var comment = await _context.Comments.FindAsync(id);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommentExists(int id)
        {
            return (_context.Comments?.Any(e => e.CommentId == id)).GetValueOrDefault();
        }
    }
}
