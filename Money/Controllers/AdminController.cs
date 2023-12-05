using Microsoft.AspNetCore.Mvc;
using Money_Transformer.Data;

namespace Money.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        public AdminController(AppDbContext context)
        {
            _context = context;
        }



        public IActionResult Index()
        {
            ViewBag.numberOfUsers = _context.Users.Count();
            ViewBag.numberOfBanks = _context.Banks.Count();
            ViewBag.numberOfWallets = _context.Wallets.Count();
            ViewBag.numberOfTransactions = _context.Transactions.Count();
            var banks = _context.Banks.ToList(); // Get the list of all banks
            var walletCountsByBank = new Dictionary<string, int>();

            foreach (var bank in banks)
            {
                int walletCount = _context.Wallets.Count(e => e.BankId == bank.Id);
                walletCountsByBank.Add(bank.Name, walletCount);
            }

            ViewBag.WalletCountsByBank = walletCountsByBank;

            return View();
        }

        //[HttpGet]
        //public IActionResult Report()
        //{
        //    var transaction = _context.Transactions.ToList();
        //    var procustomers =_context.ProductCustomers.ToList();
        //    var products = _context.Products.ToList();
        //    var categoreys = _context.Categories.ToList();
        //    var multiTable = from c in customer
        //                     join pc in procustomers on c.Id equals pc.CustomerId
        //                     join p in products on pc.ProductId equals p.Id
        //                     join cat in categoreys on p.CategoryId equals cat.Id
        //                     select new JoinTables
        //                     {
        //                         Product = p,
        //                         Customer = c,
        //                         ProductCustomer = pc,
        //                         Category = cat
        //                     };
        //     var modelContext =_context.ProductCustomers.Include(p =>p.Customer).Include(p => p.Product).ToList();
        //     ViewBag.TotalQuantity = modelContext.Sum(x =>x.Quantity);
        //     ViewBag.TotalPrice = modelContext.Sum(x =>x.Product.Price* x.Quantity);
        //     var model3 = Tuple.Create<IEnumerable<JoinTables>,
        //     IEnumerable<ProductCustomer>>(multiTable,modelContext);
        //     return View(model3);

        //}

        public IActionResult Report()
        { 
        return View();
        }

            [HttpPost]
        public async Task<IActionResult> Report(DateTime? stratDate, DateTime? endDate)
        {
            var transactions = _context.Transactions.ToList();
          
            if (stratDate == null && endDate == null)
            {
                
                var model3 = _context.Transactions.ToList();
                return View(model3);
            }
            else if (stratDate == null && endDate != null)
            {
                var model3 = _context.Transactions.Where(x => x.Date <= endDate).ToList();
                return View(model3);
            }
            else if (stratDate != null && endDate == null)
            {
                var model3 = _context.Transactions.Where(x => x.Date >= stratDate).ToList();
                return View(model3);
            }
            else
            {
                var model3 = _context.Transactions.Where(x => x.Date >= stratDate && x.Date<= endDate).ToList();
                return View(model3);
            }


        }
    }
}
