using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApplication30.Models;

namespace WebApplication30.Controllers
{
    public class AccountController : Controller
    {
        private string _connectionString =
         "Data Source=.\\sqlexpress;Initial Catalog=AuthenticationHW;Integrated Security=true;";



        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Signup(Users user, string password)
        {
            var db = new UserDb(_connectionString);
            db.AddUser(user, password);
            return Redirect("/");
        }


        public IActionResult Login()
        {
            if (TempData["message"] != null)
            {
                ViewBag.Message = TempData["message"];
            }
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var db = new UserDb(_connectionString);
            var user = db.Login(email, password);
            if (user == null)
            {

                TempData["message"] = "Invalid email/password combination";
                return Redirect("/account/login");
            }
            var claims = new List<Claim>
            {
                new Claim("user", email)
            };

            HttpContext.SignInAsync(new ClaimsPrincipal(
               new ClaimsIdentity(claims, "Cookies", "user", "role"))).Wait();

            return Redirect("/home/index");
        }
        public IActionResult MyAccount()
        {
         
            var db = new UserDb(_connectionString);
            var vm = new HomePageViewModel
            {
                IsAuthenticated = User.Identity.IsAuthenticated


            };
            if (User.Identity.IsAuthenticated)
            {
                var email = User.Identity.Name;
                vm.CurrentUser = db.GetByEmail(email);
            }

            vm.Ads = db.GetAds();
            return View(vm);
        }

          
        
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync().Wait();
            return Redirect("/");
        }
    }
}
