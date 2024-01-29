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

namespace InfoedukaMVC.Controllers
{
    public class AppUsersController : Controller
    {
        private readonly LcolinaDbContext _context;
        private readonly AuthService _authService;

        public AppUsersController(LcolinaDbContext context, AuthService authService)
        {
            _context = context;
            _authService = authService;
            
        }


        // GET: AppUsers
        public async Task<IActionResult> Index()
        {
            var lcolinaDbContext = _context.AppUsers.Include(a => a.UserType);
            return View(await lcolinaDbContext.ToListAsync());
        }

        // GET: AppUsers/Create
        public IActionResult Create()
        {
            ViewData["UserTypeId"] = new SelectList(_context.UserTypes, "UserTypeId", "UserTypeName");
            return View();
        }

        // POST: AppUsers/Create
        [HttpPost]
        public async Task<IActionResult> Create([Bind("UserId,FirstName,LastName,UserName,Pass,UserTypeId,IsActive")] AppUserDTO appUserDto)
        {
            var user = AppUserMapper.MapToDAL(appUserDto);
            var type = _context.UserTypes.FirstOrDefault(x => x.UserTypeId == user.UserTypeId);
            user.UserTypeId = type.UserTypeId;
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: AppUsers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.AppUsers == null)
            {
                return NotFound();
            }

            var appUser = await _context.AppUsers.FindAsync(id);
            if (appUser == null)
            {
                return NotFound();
            }
            ViewData["UserTypeId"] = new SelectList(_context.UserTypes, "UserTypeId", "UserTypeName", appUser.UserTypeId);
            return View(appUser);
        }

        // POST: AppUsers/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,FirstName,LastName,UserName,Pass,UserTypeId,IsActive")] AppUserDTO appUser)
        {
            var dbAppUser = AppUserMapper.MapToDAL(appUser);
            if (id != appUser.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dbAppUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppUserExists(dbAppUser.UserId))
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
            ViewData["UserTypeId"] = new SelectList(_context.UserTypes, "UserTypeId", "UserTypeId", appUser.UserTypeId);
            var appUserDto = AppUserMapper.MapToDTO(dbAppUser);
            return View();
        }

        // GET: AppUsers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.AppUsers == null)
            {
                return NotFound();
            }

            var appUser = await _context.AppUsers
                .Include(a => a.UserType)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (appUser == null)
            {
                return NotFound();
            }

            return View(appUser);
        }

        // POST: AppUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.AppUsers == null)
            {
                return Problem("Entity set 'LcolinaDbContext.AppUsers'  is null.");
            }
            var appUser = await _context.AppUsers.FirstOrDefaultAsync(x=> x.UserId==id);
            var userCourse = await _context.UserCourseMappings.FirstOrDefaultAsync(x => x.UserId == id);


            if (userCourse !=null)
            {
                _context.UserCourseMappings.Remove(userCourse);
            }
            if (appUser != null) { 
                _context.AppUsers.Remove(appUser);
            
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppUserExists(int id)
        {
          return (_context.AppUsers?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
    }
}
