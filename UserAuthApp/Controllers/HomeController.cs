using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using UserAuthApp.Models;
using Microsoft.AspNetCore.Identity;
using UserAuthApp.Areas.Identity.Data;
using UserAuthApp.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;

namespace UserAuthApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
       // private readonly ILogger<HomeController> _logger;
        private UserAuthAppContext _application;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> _sm;

        /*       public HomeController(ILogger<HomeController> logger)
               {
                   _logger = logger;
               }*/
        public HomeController(UserAuthAppContext application, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> sm)
        {
            _application = application;
            this.userManager = userManager;
            _sm = sm;
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            var currentUser = userManager.GetUserId(User);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return Redirect("~/Identity/Account/Login");
            }
            else
            {
                var result = await userManager.DeleteAsync(user);
                await userManager.UpdateSecurityStampAsync(user);
                if (result.Succeeded && user.Id == currentUser)
                {
                    await _sm.SignOutAsync();
                    return Redirect("~/Identity/Account/Login");
                } else
                {
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                }


                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View("Index");
            }
        }

/*     //   [HttpPost]
        public async Task<ActionResult> Delete123(string[] tags)
        {
            var user = await userManager.FindByIdAsync(tags[0]);
            await userManager.DeleteAsync(user);
            *//* foreach (string tag in tags)
             {

                 // Customer obj = db.Customers.Find(customerID);
                 ApplicationUser user = await userManager.FindByIdAsync(Id);
                 await userManager.DeleteAsync(user);
                 //  db.Customers.Remove(obj);
             }
             return RedirectToAction("Index");*//*
            return RedirectToAction("Index");
        }*/

        [HttpPost]
        public async Task<IActionResult> BlockUser(string id)
        {

                var user = await userManager.FindByIdAsync(id);
                var currentUser = userManager.GetUserId(User);
                if (user == null)
                {
                    ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                    return Redirect("~/Identity/Account/Login");
                }
                else
                {
                    user.Status = "blocked";
                    user.LockoutEnd = DateTime.UtcNow.AddMinutes(999999);
                    await userManager.UpdateAsync(user);
                    await userManager.UpdateSecurityStampAsync(user);
                    if (user.Id == currentUser)
                    {
                        await _sm.SignOutAsync();
                        return Redirect("~/Identity/Account/Login");
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                }
        }

        [HttpPost]
        public async Task<IActionResult> UnblockUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            var currentUser = userManager.GetUserId(User);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return Redirect("~/Identity/Account/Login");
            }
            else
            {
                user.Status = "active";
                user.LockoutEnd = DateTime.UtcNow.AddMinutes(0);
                await userManager.UpdateAsync(user);
                await userManager.UpdateSecurityStampAsync(user);
                    return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public IActionResult Index()
        {
                return View(_application.Users.ToList());            
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
