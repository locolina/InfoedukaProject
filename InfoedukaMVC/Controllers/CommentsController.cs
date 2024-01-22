using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InfoedukaMVC.Models;

namespace InfoedukaMVC.Controllers
{
    public class CommentsController : Controller
    {
        private readonly LcolinaDbContext _context;

        public CommentsController(LcolinaDbContext context)
        {
            _context = context;
        }

        //TODO: KOD GETOVA TREBA VRACATI SAMO OBJAVE OD TRENUTNO PRIJAVLJENOG KORISNIKA, OSIM ZA ADMINA ONDA VRACAMO SVE
        
        // GET: Comments
        public async Task<IActionResult> Index()
        {
            var lcolinaDbContext = _context.Comments.Include(c => c.Class).Include(c => c.User);
            return View(await lcolinaDbContext.ToListAsync());
        }

        // GET: Comments/Details/5
        //TODO: PROVJERITI MISLIM DA JE ZA OVAJ APP NEPOTREBAN KOMAD KODA
        // public async Task<IActionResult> Details(int? id)
        // {
        //     if (id == null || _context.Comments == null)
        //     {
        //         return NotFound();
        //     }
        //
        //     var comment = await _context.Comments
        //         .Include(c => c.Class)
        //         .Include(c => c.User)
        //         .FirstOrDefaultAsync(m => m.CommentId == id);
        //     if (comment == null)
        //     {
        //         return NotFound();
        //     }
        //
        //     return View(comment);
        // }

        // GET: Comments/Create
        public IActionResult Create()
        {
            ViewData["ClassId"] = new SelectList(_context.Classes, "ClassId", "ClassName");
            //TODO: KORISNIKA TREBA IZVUCI IS SESSIONA A NE GA RUCNO KUCATI KADA SE KREIRA NOVI KOMENTAR
            ViewData["UserId"] = new SelectList(_context.AppUsers, "UserId", "UserName"); 
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CommentId,Title,Content,DatePosted,DateExpires,IsActive,UserId,ClassId")] Comment comment)
        {
            ModelState.Remove("User");
            ModelState.Remove("Class");

            if (ModelState.IsValid)
            {
                _context.Add(comment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClassId"] = new SelectList(_context.Classes, "ClassId", "ClassId", comment.ClassId);
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
            ViewData["ClassId"] = new SelectList(_context.Classes, "ClassId", "ClassId", comment.ClassId);
            ViewData["UserId"] = new SelectList(_context.AppUsers, "UserId", "UserId", comment.UserId);
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CommentId,Title,Content,DatePosted,DateExpires,IsActive,UserId,ClassId")] BLComment comment)
        {
            var dbComment = MappingComment.MapToDAL(comment);
            if (id != comment.CommentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(comment.CommentId))
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
            ViewData["ClassId"] = new SelectList(_context.Classes, "ClassId", "ClassId", comment.ClassId);
            ViewData["UserId"] = new SelectList(_context.AppUsers, "UserId", "UserId", comment.UserId);
            var blComment = MappingComment.MapToBL(dbComment);
            return View(blComment);
        }

        // GET: Comments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Comments == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments
                .Include(c => c.Class)
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
