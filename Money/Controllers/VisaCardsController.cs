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
    public class VisaCardsController : Controller
    {
        private readonly AppDbContext _context;

        public VisaCardsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: VisaCards
        public async Task<IActionResult> Index()
        {
              return _context.VisaCards != null ? 
                          View(await _context.VisaCards.ToListAsync()) :
                          Problem("Entity set 'ModelContext.VisaCards'  is null.");
        }

        // GET: VisaCards/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.VisaCards == null)
            {
                return NotFound();
            }

            var visaCard = await _context.VisaCards
                .FirstOrDefaultAsync(m => m.Id == id);
            if (visaCard == null)
            {
                return NotFound();
            }

            return View(visaCard);
        }

        // GET: VisaCards/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: VisaCards/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CardNum,ExDate,HolderName,Cvv,Balance")] VisaCard visaCard)
        {
            if (ModelState.IsValid)
            {
                _context.Add(visaCard);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(visaCard);
        }

        // GET: VisaCards/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.VisaCards == null)
            {
                return NotFound();
            }

            var visaCard = await _context.VisaCards.FindAsync(id);
            if (visaCard == null)
            {
                return NotFound();
            }
            return View(visaCard);
        }

        // POST: VisaCards/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CardNum,ExDate,HolderName,Cvv,Balance")] VisaCard visaCard)
        {
            if (id != visaCard.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(visaCard);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VisaCardExists(visaCard.Id))
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
            return View(visaCard);
        }

        // GET: VisaCards/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.VisaCards == null)
            {
                return NotFound();
            }

            var visaCard = await _context.VisaCards
                .FirstOrDefaultAsync(m => m.Id == id);
            if (visaCard == null)
            {
                return NotFound();
            }

            return View(visaCard);
        }

        // POST: VisaCards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.VisaCards == null)
            {
                return Problem("Entity set 'ModelContext.VisaCards'  is null.");
            }
            var visaCard = await _context.VisaCards.FindAsync(id);
            if (visaCard != null)
            {
                _context.VisaCards.Remove(visaCard);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VisaCardExists(int id)
        {
          return (_context.VisaCards?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
