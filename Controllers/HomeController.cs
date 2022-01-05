using FreshGoldPractice2.Data;
using FreshGoldPractice2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FreshGoldPractice2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

       
        public HomeController(ILogger<HomeController> logger, UserManager<IdentityUser> userManager, ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
        }

       

        public async Task<IActionResult> Index()
        {

            IdentityRole freshGoldEmployeeRole = new IdentityRole
            {
                Name = "FreshGold Employee"
            };
            await _roleManager.CreateAsync(freshGoldEmployeeRole);

            IdentityRole shippingEmployeeRole = new IdentityRole
            {
                Name = "Shipping Employee"
            };
            await _roleManager.CreateAsync(shippingEmployeeRole);


            var freshGoldUser = new IdentityUser { Email = "test.user@freshgoldsa.co.za", UserName = "test.user@freshgoldsa.co.za", EmailConfirmed = true };
            await _userManager.CreateAsync(freshGoldUser, "@StrongPassword2");
            var shippingUser = new IdentityUser { Email = "test.user@shipping.co.za", UserName = "test.user@shipping.co.za", EmailConfirmed = true };
            await _userManager.CreateAsync(shippingUser, "@StrongPassword2");
           

            await _userManager.AddToRoleAsync(freshGoldUser, "FreshGold Employee");
            await _userManager.AddToRoleAsync(shippingUser, "Shipping Employee");
          

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
