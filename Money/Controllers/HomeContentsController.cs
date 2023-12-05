using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Money_Transformer.Data;
using Money_Transformer.Models;

namespace Money_Transformer.Controllers
{
    public class HomeContentsController : Controller
    {
        private readonly AppDbContext _context;

        public HomeContentsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: HomeContents
        public async Task<IActionResult> Index()
        {
              return _context.HomeContents != null ? 
                          View(await _context.HomeContents.ToListAsync()) :
                          Problem("Entity set 'ModelContext.HomeContents'  is null.");
        }

        // GET: HomeContents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.HomeContents == null)
            {
                return NotFound();
            }

            var homeContent = await _context.HomeContents
                .FirstOrDefaultAsync(m => m.Id == id);
            if (homeContent == null)
            {
                return NotFound();
            }

            return View(homeContent);
        }

        // GET: HomeContents/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: HomeContents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Content")] HomeContent homeContent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(homeContent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(homeContent);
        }

        // GET: HomeContents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.HomeContents == null)
            {
                return NotFound();
            }

            var homeContent = await _context.HomeContents.FindAsync(id);
            if (homeContent == null)
            {
                return NotFound();
            }
            return View(homeContent);
        }

        // POST: HomeContents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Content")] HomeContent homeContent)
        {
            if (id != homeContent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(homeContent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HomeContentExists(homeContent.Id))
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
            return View(homeContent);
        }

        // GET: HomeContents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.HomeContents == null)
            {
                return NotFound();
            }

            var homeContent = await _context.HomeContents
                .FirstOrDefaultAsync(m => m.Id == id);
            if (homeContent == null)
            {
                return NotFound();
            }

            return View(homeContent);
        }

        // POST: HomeContents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.HomeContents == null)
            {
                return Problem("Entity set 'ModelContext.HomeContents'  is null.");
            }
            var homeContent = await _context.HomeContents.FindAsync(id);
            if (homeContent != null)
            {
                _context.HomeContents.Remove(homeContent);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HomeContentExists(int id)
        {
          return (_context.HomeContents?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
