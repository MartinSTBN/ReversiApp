using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiApp.Data
{
    public class DBInitializer
    {
        public static async Task Initialize(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            if (!roleManager.RoleExistsAsync("Normal").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Normal";
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync("Moderator").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Moderator";
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }


            if (!roleManager.RoleExistsAsync("Administrator").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Administrator";
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }

            if (await userManager.FindByNameAsync("Martin.k.Steenbergen@gmail.com") == null)
            {
                var user = new IdentityUser
                {
                    UserName = "Martin.k.Steenbergen@gmail.com",
                    Email = "Martin.k.Steenbergen@gmail.com",
                    EmailConfirmed = true 
                };
                var result = await userManager.CreateAsync(user, "Heshmt12#");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Administrator");
                }

            }
        }
    }
}
