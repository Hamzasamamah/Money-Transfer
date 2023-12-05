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
    public class TransactionsController : Controller
    {
        private readonly AppDbContext _context;

        public TransactionsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Transactions
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Transactions.Include(t => t.ReceiverIbanNavigation).Include(t => t.SenderIbanNavigation);
            return View(await modelContext.ToListAsync());
        }

        // GET: Transactions/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Transactions == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .Include(t => t.ReceiverIbanNavigation)
                .Include(t => t.SenderIbanNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // GET: Transactions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SenderIban,ReceiverIban,Amount,Fees,Date")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                var senderWallet = await _context.Wallets.FirstOrDefaultAsync(w => w.Iban == transaction.SenderIban);
                var receiverWallet = await _context.Wallets.FirstOrDefaultAsync(w => w.Iban == transaction.ReceiverIban);

                if (senderWallet != null && receiverWallet != null)
                {
                    // Check if the transformation is from one bank to another
                    bool isDifferentBanks = senderWallet.BankId != receiverWallet.BankId;

                    // Calculate fees based on the condition
                    decimal fees = isDifferentBanks ? transaction.Amount * 0.005m : 0.0m;

                    decimal totalAmountWithFees = transaction.Amount + fees;

                    // Check if the sender has enough balance
                    if (senderWallet.Balance >= totalAmountWithFees)
                    {
                        senderWallet.Balance -= totalAmountWithFees;
                        receiverWallet.Balance += transaction.Amount;

                        // Set the calculated fees and current date
                        transaction.Fees = fees;
                        transaction.Date = DateTime.Now;

                        _context.Add(transaction);
                        await _context.SaveChangesAsync();
                        // Set a success message in TempData to be displayed on the next request
                        TempData["TransactionMessage"] = "Transaction submitted successfully.";
                        return RedirectToAction("Create");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Your wallet balance not enough for this transaction.");
                    }
                }
            }

            return View(transaction);
        }



        // GET: Transactions/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Transactions == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }
            ViewData["ReceiverIban"] = new SelectList(_context.Wallets, "Iban", "Iban", transaction.ReceiverIban);
            ViewData["SenderIban"] = new SelectList(_context.Wallets, "Iban", "Iban", transaction.SenderIban);
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,SenderIban,ReceiverIban,Amount,Fees,Date")] Transaction transaction)
        {
            if (id != transaction.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transaction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransactionExists(transaction.Id))
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
            ViewData["ReceiverIban"] = new SelectList(_context.Wallets, "Iban", "Iban", transaction.ReceiverIban);
            ViewData["SenderIban"] = new SelectList(_context.Wallets, "Iban", "Iban", transaction.SenderIban);
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Transactions == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .Include(t => t.ReceiverIbanNavigation)
                .Include(t => t.SenderIbanNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Transactions == null)
            {
                return Problem("Entity set 'ModelContext.Transactions'  is null.");
            }
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransactionExists(decimal id)
        {
          return (_context.Transactions?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
