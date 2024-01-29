using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InfoedukaMVC.Models;
using InfoedukaMVC.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace InfoedukaMVC.Controllers
{
    public class CoursesController : Controller
    {
        
        private readonly LcolinaDbContext _context;
        private readonly AuthService _authService;

        public CoursesController(LcolinaDbContext context, AuthService authService)
        {
            _context = context;
            _authService = authService;
            
        }

        // GET: Courses
        public async Task<IActionResult> Index()
        {
            var currentUser = await GetCurrentUserAsync();

            if (User.HasClaim(c => c.Type == ClaimTypes.Role && c.Value == "RegularUser"))
            {
                // Regular user (teacher) can see only assigned courses
                var assignedCourses = _context.UserCourseMappings
                .Where(ucm => ucm.UserId == currentUser.UserId)
                .Select(ucm => ucm.Course)
                .ToList();


                return View(assignedCourses);
            }
            else
            {
                var courses = await _context.Courses.ToListAsync();
                return View(courses);
            }



        }

        // GET: Courses/Create
        public IActionResult Create()
        {

            return View();
        }

        // POST: Courses/Create
        [HttpPost]
        public async Task<IActionResult> Create([Bind("CourseId,CourseName,IsActive")] Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        // POST: Courses/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("CourseId,CourseName,IsActive")] Course course)
        {
            if (id != course.CourseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.CourseId))
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
            return View(course);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(x=>x.CourseId==id);
            var userCourse = await _context.UserCourseMappings.FirstOrDefaultAsync(x => x.CourseId==id);
            var comment= await _context.Comments.FirstOrDefaultAsync(x => x.CourseId == id);

            if(userCourse != null )
            {
                _context.UserCourseMappings.Remove(userCourse);
            }
            if (comment != null)
            {
                _context.Comments.Remove(comment);

            }
            if (course!=null)
            {

                _context.Courses.Remove(course);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
            return (_context.Courses?.Any(e => e.CourseId == id)).GetValueOrDefault();
        }


        private async Task<AppUser> GetCurrentUserAsync()
        {
            var username = User.FindFirstValue(ClaimTypes.Name);

            if (!string.IsNullOrEmpty(username))
            {
                var currentUser = await _context.AppUsers.FirstOrDefaultAsync(u => u.UserName == username);
                return currentUser;
            }

            return null;
        }
    }


}


