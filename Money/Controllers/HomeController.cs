using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Money_Transformer.Data;
using Money_Transformer.Models;
using System.Diagnostics;

namespace Money.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment IwebHostEnvironment;


        public HomeController(ILogger<HomeController> logger, AppDbContext context, IWebHostEnvironment iwebHostEnvironment)
        {
            _logger = logger;
            _context = context;
            IwebHostEnvironment = iwebHostEnvironment;
        }

        public IActionResult Index()
        {  ///tuple 
            var home = _context.HomeContents.ToList();
            var about = _context.AboutUsContents.ToList();
            var testimonial =_context.Testimonials.ToList();
            var contact = _context.ContactUs.ToList();
            var model3 = Tuple.Create<IEnumerable<HomeContent>, IEnumerable<AboutUsContent>,
                IEnumerable<Testimonial>,IEnumerable<ContactUs>>(home, about,testimonial,contact);

            return View(model3);  
        }


        public IActionResult User()
        {
            ///tuple 
            var home = _context.HomeContents.ToList();
            var about = _context.AboutUsContents.ToList();
            var testimonial = _context.Testimonials.ToList();
            var contact = _context.ContactUs.ToList();
            var model3 = Tuple.Create<IEnumerable<HomeContent>, IEnumerable<AboutUsContent>,
                IEnumerable<Testimonial>, IEnumerable<ContactUs>>(home, about, testimonial, contact);


            return View(model3);
        }


        public IActionResult Banks()
        {
            var banks = _context.Banks.ToList();
            return View(banks);
        }

        public IActionResult Mywallet()
        {

            ///tuple 
            var bank = _context.Banks.ToList();
            var wallet = _context.Wallets.ToList();
            var model3 = Tuple.Create<IEnumerable<Bank>, IEnumerable<Wallet>>(bank, wallet);


            return View(model3);

        }


      

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}