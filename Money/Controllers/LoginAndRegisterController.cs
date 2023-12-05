using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Money_Transformer.Models;
using Money_Transformer.Data;
using Microsoft.Extensions.Hosting;

namespace Money_Transformer.Controllers
{
    public class LoginAndRegisterController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment IwebHostEnvironment;


        public LoginAndRegisterController(AppDbContext context, IWebHostEnvironment iwebHostEnvironment)
        {
            _context = context;
            IwebHostEnvironment = iwebHostEnvironment;
        }


        public IActionResult Register()
        {
            
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }

        //Register method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Id,Fullname,Username,Password,Email,RoleId,Imagepath,ImageFile")]User user)
        {
            if (ModelState.IsValid)
            {
                //Add Customer Details
                string wwwRootPath = IwebHostEnvironment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + "_" + user.ImageFile.FileName;
                string path = Path.Combine(wwwRootPath + "/images/", fileName);
                using (var filestream = new FileStream(path, FileMode.Create))
                {
                    await user.ImageFile.CopyToAsync(filestream);

                }
                user.ImagePath = fileName;


                user.RoleId = 1;

                _context.Add(user);
                await _context.SaveChangesAsync();
               
                return RedirectToAction("Login","LoginAndRegister");
            }
            return View(user);
        }

        //Login method
        [HttpPost]
        public IActionResult Login([Bind("Username ,Password ")] User user)
        {
            var auth = _context.Users.Where(x => x.Username == user.Username && x.Password == user.Password).SingleOrDefault();
            if (auth != null)
            {

                // 1 > user
                // 2 > admin
                switch (auth.RoleId)
                {
                    case 1:
                        HttpContext.Session.SetInt32("CustomerId", (int)auth.Id);
                        return RedirectToAction("User", "Home");

                    case 2:
                        HttpContext.Session.SetString("AdminName", auth.Fullname);
                        return RedirectToAction("Index", "Admin");
                }
            }
            return View();
        }


}
}
