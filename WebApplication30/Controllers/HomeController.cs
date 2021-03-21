using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebApplication30.Models;

namespace WebApplication30.Controllers
{
    public class HomeController : Controller
    {
        private string _connectionString =
         "Data Source=.\\sqlexpress;Initial Catalog=AuthenticationHW;Integrated Security=true;";
       
        public IActionResult Index()
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

         

        public IActionResult NewAd()
        {
            var vm = new HomePageViewModel
            {
                IsAuthenticated = User.Identity.IsAuthenticated


            };
            if (User.Identity.IsAuthenticated)
            {
                return View(vm);
            }
            else
            {
                return Redirect("/account/login");
            }
                
        }
        [HttpPost]
        public IActionResult NewAd(Ads ads)
        {
            var db = new UserDb(_connectionString);
        
            ads.Date = DateTime.Now;
            Users CurrentUser = db.GetByEmail(User.Identity.Name);
            ads.UserId = CurrentUser.Id;
                   

            db.NewAd(ads);
            return Redirect("/");
        }

        [HttpPost]
        public IActionResult DeleteAd(int id)
        {
            UserDb db = new UserDb(_connectionString);
            db.DeleteAd(id);
            return Redirect("/");
        }

    }
}
