using Microsoft.AspNetCore.Mvc;
using Money.Models;
using Money_Transformer.Data;
using Money_Transformer.Models;

namespace Money.Controllers
{
    public class SearchController : Controller
    {
        private readonly AppDbContext _context;

        public SearchController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Search
            (string searchTerm)
        {


            var banksResults = _context.Banks.Where(b => b.Name.Contains(searchTerm)).ToList();

            var resultList = new List<SearchResult>();
            foreach (var bank in banksResults)
            {
                resultList.Add(new SearchResult { RecordID = bank.Id, Title = bank.Name, Type = "Bank" });
            }

            return View(resultList);
        }


    }
}
