using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LuckyPaw.Models;
using Microsoft.AspNetCore.Identity;
using LuckyPaw.Data;
using Microsoft.EntityFrameworkCore;

namespace LuckyPaw.Controllers
{
    public class HomeController : Controller
    {
        private readonly LuckyPawContext _context;

        public HomeController(LuckyPawContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ContactUs()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> ManageAccounts()
        {
            // Get all the registered users and all the available role Admin, Manager and User and userRole,
            // the relationship between the user and their role
            List<IdentityUser> userList = _context.IdentityUser.ToList();
            List<IdentityRole> roleList = _context.IdentityRole.ToList();
            List<IdentityUserRole<string>> userRoleList = _context.IdentityUserRole.ToList();

            List<UserModel> userModelList = new List<UserModel>();

            UserModel userModel = new UserModel();

            userModel.userList = userList;
            userModel.roleList = roleList;
            userModel.userRoleList = userRoleList;

            userModelList.Add(userModel);

            return View(userModelList);
        }

        [HttpPost, ActionName("UpdateAccount")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAccount(string userId, string selectRole)
        {

            var user = await _context.IdentityUser.FindAsync(userId);

            if (user == null)
            {
                return NotFound();
            }


            _context.IdentityUser.Remove(user);
            await _context.SaveChangesAsync();

            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
            }

            var role = _context.IdentityRole.FirstOrDefault(m => m.Name == selectRole);

            if (role == null)
            {
                return NotFound();
            }

            IdentityUserRole<string> newUserRole = new IdentityUserRole<string>();

            newUserRole.UserId = userId;
            newUserRole.RoleId = role.Id;

            if (ModelState.IsValid)
            {
                _context.Add(newUserRole);
                await _context.SaveChangesAsync();
            }
            
            return RedirectToAction("ManageAccounts");
        }
    }
}