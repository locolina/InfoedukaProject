using InfoedukaMVC.Models;
using InfoedukaMVC.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace InfoedukaMVC.Controllers
{
    public class AssignController : Controller
    {
        private readonly LcolinaDbContext _context;
        private readonly AuthService _authService;

        public AssignController(LcolinaDbContext context, AuthService authService)
        {
            _context = context;
            _authService = authService;
        }
       
        public async Task<IActionResult> AssignAsync()
        {
            if (User.HasClaim(c => c.Value == "Admin"))
            {
                // Admin can see all courses
                var teachers = _context.AppUsers.Where(u => u.UserTypeId == 2).ToList();
                var teacherList = new SelectList(teachers, "UserId", "UserName");

                var courses = await _context.Courses.ToListAsync(); 
                var courseList = new SelectList(courses, "CourseId", "CourseName");

                var viewModel = new AssignCourseViewModel
                {
                    TeacherList = teacherList,
                    CourseList = courseList
                };

                return View("Assign", viewModel);
            }



            return View();
        }
        // POST: Courses/Assign
        [HttpPost]
        public async Task<IActionResult> Assign(AssignCourseViewModel viewModel)
        {
            // Get the current user
            var currentUser = await GetCurrentUserAsync();

            // Check if the user is authenticated and is an admin
            if (User.Identity.IsAuthenticated && User.HasClaim(c => c.Value == "Admin"))
            {
                // Check if the provided professorId is valid
                var teacher = await _context.AppUsers.FindAsync(viewModel.TeacherId);
                if (teacher == null || teacher.UserTypeId != 2)
                {
                    return NotFound();
                }

                // Check if the provided courseId is valid and available for assignment
                var course = await _context.Courses
                    .FirstOrDefaultAsync(c => c.CourseId == viewModel.CourseId);

                if (course == null)
                {
                    return NotFound();
                }

                // Check if the current user is already assigned to the course
                var userCourseMapping = await _context.UserCourseMappings
                    .FirstOrDefaultAsync(uc => uc.UserId == teacher.UserId && uc.CourseId == viewModel.CourseId);

                if (userCourseMapping != null)
                {
                    // The teacher is already assigned to the course, handle accordingly
                    TempData["ErrorMessage"] = "The teacher is already assigned to this course.";
                    return RedirectToAction(nameof(Assign));
                }
              

                // Assign the course to the teacher
                var newUserCourseMapping = new UserCourseMapping
                {
                    UserId = teacher.UserId,
                    CourseId = viewModel.CourseId
                };

                _context.UserCourseMappings.Add(newUserCourseMapping);
                await _context.SaveChangesAsync();
                
            }

            TempData["SuccessMessage"] = "The teacher is assigned to this course.";
            return RedirectToAction(nameof(Assign));

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
