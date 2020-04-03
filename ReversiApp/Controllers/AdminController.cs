using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using ReversiApp.Data;
using ReversiApp.Models;

namespace ReversiApp.Controllers
{
    [Authorize(Roles = "Administrator, Moderator")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: Admin
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var vm = new List<AppUser>();
           

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var role = roles.FirstOrDefault();

                vm.Add(
                   new AppUser()
                   {
                       Id = user.Id,
                       Email = user.Email,
                       Password = user.PasswordHash,
                       Role = role
                   });
            }

            return View(vm);
        }

        // GET: Admin/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Admin/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Email,Password,Role")] AppUser appUser)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = appUser.Email, Email = appUser.Email, EmailConfirmed = true };
                var result = await _userManager.CreateAsync(user, appUser.Password);
                await _userManager.AddToRoleAsync(user, appUser.Role);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Admin");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(appUser);
        }

        // GET: Admin/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            AppUser appUser = new AppUser();
            appUser.Email = user.Email;
            appUser.Password = user.PasswordHash;
            appUser.TwoFactorEnabled = user.TwoFactorEnabled;
            var role = await _userManager.GetRolesAsync(user);
            foreach (var item in role)
            {
                appUser.Role = item;
            }

            return View(appUser);
        }

        // POST: Admin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id,Email,Password,Role,TwoFactorEnabled")] AppUser appUser)
        {
            if (ModelState.IsValid)
            {
                var result = new IdentityResult();
                var user = await _userManager.FindByIdAsync(appUser.Id);
                user.Email = appUser.Email;
                result = await _userManager.UpdateAsync(user);

                IdentityResult reset = new IdentityResult();
                if (appUser.Password != null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    reset = await _userManager.ResetPasswordAsync(user, token, appUser.Password);
                }

                var role = await _userManager.GetRolesAsync(user);
                string currentRole = "";
                foreach (var item in role)
                {
                    currentRole = item;
                }
                var removeRole = await _userManager.RemoveFromRoleAsync(user, currentRole);
                var setRole = await _userManager.AddToRoleAsync(user, appUser.Role);

                if(appUser.TwoFactorEnabled == false)
                {
                    await _userManager.SetTwoFactorEnabledAsync(user, appUser.TwoFactorEnabled);
                }
                

                foreach (var error in removeRole.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                foreach (var error in setRole.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                foreach (var error in reset.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return RedirectToAction("Index");
            }
            
            return View(appUser);
        }

        // GET: Admin/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(string id)
        {

            if (id == null || !UserExists(id))
            {
                return NotFound();
            }

            var user = await _userManager.Users
                .FirstOrDefaultAsync(m => m.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            AppUser appUser = new AppUser();
            appUser.Email = user.Email;
            var role = await _userManager.GetRolesAsync(user);
            foreach (var item in role)
            {
                appUser.Role = item;
            }


            return View(appUser);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed([Bind("Id,Email,Password")] AppUser appUser)
        {
            var user = await _userManager.FindByIdAsync(appUser.Id);
            await _userManager.DeleteAsync(user);
            
            //await _userManager.RemoveFromRoleAsync(user, appUser.Role);
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(string id)
        {
            return _userManager.Users.Any(e => e.Id == id);
        }
    }
}
