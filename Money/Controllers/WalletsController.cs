using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Money_Transformer.Models;
using Microsoft.AspNetCore.Http;
using System.Text;
using Money_Transformer.Data;

namespace Money_Transformer.Controllers
{
    public class WalletsController : Controller
    {
        private readonly AppDbContext _context;

        public WalletsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Wallets
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Wallets.Include(w => w.Bank).Include(w => w.User);
            return View(await modelContext.ToListAsync());
        }

        // GET: Wallets/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Wallets == null)
            {
                return NotFound();
            }

            var wallet = await _context.Wallets
                .Include(w => w.Bank)
                .Include(w => w.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (wallet == null)
            {
                return NotFound();
            }

            return View(wallet);
        }

        // GET: Wallets/Create
        public IActionResult Create()
        {
           
            return View();
        }

        // POST: Wallets/Create
        // Generates a random 10-digit IBAN
        private string GenerateRandomIban()
        {
            Random random = new Random();
            StringBuilder ibanBuilder = new StringBuilder();

            for (int i = 0; i < 10; i++)
            {
                ibanBuilder.Append(random.Next(0, 10));
            }

            return ibanBuilder.ToString();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Iban,UserId,BankId,Balance,Active")] Wallet wallet, int Id)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var customerIdNullable = HttpContext.Session.GetInt32("CustomerId");

        //        if (customerIdNullable.HasValue)
        //        {
        //            wallet.UserId = customerIdNullable.Value;

        //        }
        //        else
        //        {
        //            throw new InvalidOperationException("Customer ID session value is null.");
        //        }
        //        // Generate a random 10-digit IBAN
        //        string iban = GenerateRandomIban();
        //        wallet.Iban = iban;

        //        // Assign the BankId from the clicked bank
        //        wallet.BankId = Id;

        //        _context.Add(wallet);
        //        await _context.SaveChangesAsync();
        //        TempData["Message"] = "Wallet created successfully.";
        //        return RedirectToAction("Banks", "Home");
        //    }
        //    return View(wallet);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Iban,UserId,BankId,Balance,Active")] Wallet wallet, int Id, string cardNum, DateTime exDate, string holderName, string cvv)
        {
            if (ModelState.IsValid)
            {
                var customerIdNullable = HttpContext.Session.GetInt32("CustomerId");

                if (customerIdNullable.HasValue)
                {
                    wallet.UserId = customerIdNullable.Value;
                }
                else
                {
                    throw new InvalidOperationException("Customer ID session value is null.");
                }

                // Generate a random 10-digit IBAN
                string iban = GenerateRandomIban();
                wallet.Iban = iban;

                // Assign the BankId from the clicked bank
                wallet.BankId = Id;

                // Check if Visa card exists in the VisaCard table
                var visaCard = _context.VisaCards.FirstOrDefault(c => c.CardNum == cardNum && c.ExDate == exDate && c.HolderName == holderName && c.Cvv == cvv);

                if (visaCard != null)
                {
                    // Check if the Visa card balance is greater than or equal to the wallet balance
                    var visaCardBalance = visaCard.Balance; // Get value or 0 if null
                    var walletBalance = wallet.Balance;

                    if (visaCardBalance >= walletBalance)
                    {
                        if (walletBalance>0)
                        {
                            wallet.Active = true;
                        }
                        // Transfer the balance from the Visa card to the wallet
                        visaCardBalance -= (decimal)walletBalance;
                        wallet.Balance = walletBalance;
                        
                        // Update the visa card balance
                        visaCard.Balance = visaCardBalance;

                        _context.Update(visaCard);
                    }
                    else
                    {
                        // Handle the case when there's insufficient balance in the Visa card
                        ModelState.AddModelError(string.Empty, "Insufficient balance in the Visa card.");
                        return View(wallet);
                    }
                }

                _context.Add(wallet);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Wallet created successfully.";
                return RedirectToAction("Banks", "Home");
            }

            return View(wallet);
        }





        // GET: Wallets/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Wallets == null)
            {
                return NotFound();
            }

            var wallet = await _context.Wallets.FindAsync(id);
            if (wallet == null)
            {
                return NotFound();
            }
            ViewData["BankId"] = new SelectList(_context.Banks, "Id", "Id", wallet.BankId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", wallet.UserId);
            return View(wallet);
        }

        // POST: Wallets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,Iban,UserId,BankId,Balance,Active")] Wallet wallet)
        {
            if (id != wallet.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(wallet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WalletExists(wallet.Id))
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
            ViewData["BankId"] = new SelectList(_context.Banks, "Id", "Id", wallet.BankId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", wallet.UserId);
            return View(wallet);
        }

        // GET: Wallets/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Wallets == null)
            {
                return NotFound();
            }

            var wallet = await _context.Wallets
                .Include(w => w.Bank)
                .Include(w => w.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (wallet == null)
            {
                return NotFound();
            }

            return View(wallet);
        }

        // POST: Wallets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Wallets == null)
            {
                return Problem("Entity set 'ModelContext.Wallets'  is null.");
            }
            var wallet = await _context.Wallets.FindAsync(id);
            if (wallet != null)
            {
                _context.Wallets.Remove(wallet);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WalletExists(decimal id)
        {
          return (_context.Wallets?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public IActionResult Recharge()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Recharge(Wallet wallet,int id, string cardNum, DateTime exDate, string holderName, string cvv,decimal balance)
        {
            var WR = _context.Wallets.FirstOrDefault(c => c.Id == id );
            var visaCard = _context.VisaCards.FirstOrDefault(c => c.CardNum == cardNum && c.ExDate == exDate && c.HolderName == holderName && c.Cvv == cvv);

            if (visaCard != null)
            {
                // Check if the Visa card balance is greater than or equal to the wallet balance
                var visaCardBalance = visaCard.Balance; // Get value or 0 if null
               

                if (visaCardBalance >= balance)
                {
                    
                    // Transfer the balance from the Visa card to the wallet
                    visaCardBalance -= (decimal)balance;
                    WR.Balance += balance;
                    WR.BankId=WR.BankId;
                    // Update the visa card balance
                    visaCard.Balance = visaCardBalance;

                    _context.Update(visaCard);
                   
                }
                else
                {
                    // Handle the case when there's insufficient balance in the Visa card
                    ModelState.AddModelError(string.Empty, "Insufficient balance in the Visa card.");
                    return View(wallet);
                }
            }
            _context.Update(WR);
            await _context.SaveChangesAsync();
            return RedirectToAction("Mywallet","Home");
        }


    }
}
