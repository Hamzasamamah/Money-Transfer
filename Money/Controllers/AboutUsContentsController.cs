using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Money_Transformer.Data;
using Money_Transformer.Models;

namespace Money.Controllers
{
    public class AboutUsContentsController : Controller
    {
        private readonly AppDbContext _context;

        public AboutUsContentsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: AboutUsContents
        public async Task<IActionResult> Index()
        {
              return _context.AboutUsContents != null ? 
                          View(await _context.AboutUsContents.ToListAsync()) :
                          Problem("Entity set 'ModelContext.AboutUsContents'  is null.");
        }

        // GET: AboutUsContents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.AboutUsContents == null)
            {
                return NotFound();
            }

            var aboutUsContent = await _context.AboutUsContents
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aboutUsContent == null)
            {
                return NotFound();
            }

            return View(aboutUsContent);
        }

        // GET: AboutUsContents/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AboutUsContents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Content")] AboutUsContent aboutUsContent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(aboutUsContent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(aboutUsContent);
        }

        // GET: AboutUsContents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.AboutUsContents == null)
            {
                return NotFound();
            }

            var aboutUsContent = await _context.AboutUsContents.FindAsync(id);
            if (aboutUsContent == null)
            {
                return NotFound();
            }
            return View(aboutUsContent);
        }

        // POST: AboutUsContents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Content")] AboutUsContent aboutUsContent)
        {
            if (id != aboutUsContent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(aboutUsContent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AboutUsContentExists(aboutUsContent.Id))
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
            return View(aboutUsContent);
        }

        // GET: AboutUsContents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.AboutUsContents == null)
            {
                return NotFound();
            }

            var aboutUsContent = await _context.AboutUsContents
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aboutUsContent == null)
            {
                return NotFound();
            }

            return View(aboutUsContent);
        }

        // POST: AboutUsContents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.AboutUsContents == null)
            {
                return Problem("Entity set 'ModelContext.AboutUsContents'  is null.");
            }
            var aboutUsContent = await _context.AboutUsContents.FindAsync(id);
            if (aboutUsContent != null)
            {
                _context.AboutUsContents.Remove(aboutUsContent);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AboutUsContentExists(int id)
        {
          return (_context.AboutUsContents?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
