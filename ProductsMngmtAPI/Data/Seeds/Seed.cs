using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using ProductsMngmtAPI.Models;

namespace ProductsMngmtAPI.Data.Seeds
{
    public class Seed
    {
        public static void SeedUsers(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            if (!userManager.Users.Any())
            {
                var userData = System.IO.File.ReadAllText("Data/Seeds/seedUsersData.json");
                var users = JsonConvert.DeserializeObject<List<User>>(userData);

                var roles = new List<Role>
                {
                    new Role{Name="Employee"},
                    new Role{Name="TeamLeader"},
                    new Role{Name="Manager"},
                    new Role{Name="Admin"},
                };

                foreach (var role in roles)
                {
                    roleManager.CreateAsync(role).Wait();
                }

                var adminUser = new User
                {
                    UserName = "Admin",
                    FirstName = "Admin",
                    LastName = "Admin",
                };

                var result = userManager.CreateAsync(adminUser, "Pa$$w0rd").Result;

                if (result.Succeeded)
                {
                    var admin = userManager.FindByNameAsync("Admin").Result;
                    userManager.AddToRolesAsync(admin, new[] { "Admin", "Manager", "TeamLeader", "Employee" }).Wait();
                }

                foreach (var user in users)
                {
                    userManager.CreateAsync(user, "Pa$$w0rd").Wait();
                    userManager.AddToRoleAsync(user, "Employee").Wait();
                }

                var teamLeader = userManager.FindByNameAsync("Bazancik").Result;
                if (teamLeader != null)
                    userManager.AddToRoleAsync(teamLeader, "TeamLeader").Wait();

                var manager = userManager.FindByNameAsync("MarianKox").Result;
                if (manager != null)
                    userManager.AddToRoleAsync(manager, "Manager").Wait();

            }
        }

    }
}