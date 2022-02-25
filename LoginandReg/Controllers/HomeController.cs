using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using LoginandReg.Models;

namespace LoginandReg.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private MyContext _context;

        public HomeController(ILogger<HomeController> logger, MyContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("register")]
        public IActionResult Register(User newUser)
        {
            if (ModelState.IsValid)
            {
                // Validate the given email in the database. 
                if (_context.Users.Any(o => o.Email == newUser.Email))
                {
                    ModelState.AddModelError("Email", "Email is already taken!");
                    return View("register");
                }
                //Hash the password only after verifying everything else
                PasswordHasher<User> Hashbrown = new PasswordHasher<User>();
                newUser.Password = Hashbrown.HashPassword(newUser, newUser.Password);
                // Now add to the database
                _context.Add(newUser);
                // Also save all the changes
                _context.SaveChanges();
                return RedirectToAction("dashboard");
            }
            else
            {
                return View("register");
            }
        }
        [HttpPost("login")]
        public IActionResult Login(User logUser)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("dashboard");
            }
            else
            {
                return View("login");
            }
        }

        [HttpGet("success")]
        public IActionResult Success()
        {

            return View();
        }
        public IActionResult Privacy()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

