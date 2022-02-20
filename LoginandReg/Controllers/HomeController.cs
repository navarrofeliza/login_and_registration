using System;
using System.Linq;
using System.Collections.Generic;
using LoginandReg.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace LoginandReg.Controllers
{
    public class HomeController : Controller
    {
        private MyContext _context;
        public HomeController(MyContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View("Index");
        }

        [HttpGet("login")]
        public IActionResult LoginForm()
        {
            return View("Login");
        }

        [HttpPost("user/register")]
        public IActionResult Register(Wrapper Form)
        {
            if (ModelState.IsValid)
            {
                if (_context.Users.Any(u => u.Email == Form.User.Email))
                {
                    ModelState.AddModelError("User.Email", "Email already in use!");
                    return Index();
                }
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                Form.User.Password = Hasher.HashPassword(Form.User, Form.User.Password);
                _context.Add(Form.User);
                _context.SaveChanges();

                var NewUser = _context.Users.FirstOrDefault(u => u.Email == Form.User.Email);
                int UserID = NewUser.UserId;
                HttpContext.Session.SetInt32("CurrentUser", UserID);

                return RedirectToAction("Success");
            }
            else
            {
                return Index();
            }
        }

        [HttpPost("user/login")]
        public IActionResult Login(Wrapper Form)
        {
            if (ModelState.IsValid)
            {
                User ReturningUser = _context.Users.FirstOrDefault(u => u.Email == Form.UserLogin.LoginEmail);
                if (ReturningUser == null)
                {
                    ModelState.AddModelError("UserLogin.LoginEmail", "Invalid Email Address");
                    return LoginForm();
                }
                PasswordHasher<LoginUser> hasher = new PasswordHasher<LoginUser>();
                var result = hasher.VerifyHashedPassword(Form.UserLogin, ReturningUser.Password, Form.UserLogin.LoginPassword);
                if (result == 0)
                {
                    ModelState.AddModelError("UserLogin.LoginPassword", "Invalid Password");
                    return LoginForm();
                }
                HttpContext.Session.SetInt32("CurrentUser", ReturningUser.UserId);
                return RedirectToAction("Success");
            }
            else
            {
                return View("Login");
            }
        }

        [HttpGet("success")]
        public IActionResult Success()
        {
            Wrapper wrap = new Wrapper();
            int? CurrentUser = HttpContext.Session.GetInt32("CurrentUser");
            User active = _context.Users.FirstOrDefault(u => u.UserId == CurrentUser);
            if (active == null)
            {
                return RedirectToAction("Index");
            }
            wrap.User = active;
            return View("Success", wrap);
        }

        [HttpGet("/logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}