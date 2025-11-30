using GymManagementDAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Data.DataSeed
{
    public static class IdentityDbContext
    {
        public static bool SeedData(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            try
            {
                var HasUsers = userManager.Users.Any();
                var HasRoles = roleManager.Roles.Any();
                if (HasRoles && HasUsers)
                    return false;
                if (!HasRoles)
                {
                    var roles = new List<IdentityRole>
                    {
                        new () { Name = "SuperAdmin"},
                        new () { Name = "Admin"}
                    };
                    foreach (var role in roles)
                    {
                        if (!roleManager.RoleExistsAsync(role.Name).GetAwaiter().GetResult())
                            roleManager.CreateAsync(role).Wait();
                    }
                }
                if (!HasUsers)
                {
                    var MainAdmin = new ApplicationUser
                    {
                        FirstName = "Seif",
                        LastName = "Ayman",
                        UserName = "seifayman",
                        Email = "seif@gmail.com",
                        PhoneNumber = "01023523891"
                    };
                    userManager.CreateAsync(MainAdmin, "Seif@0911").Wait();
                    userManager.AddToRoleAsync(MainAdmin, "SuperAdmin").Wait();

                    var Admin = new ApplicationUser
                    {
                        FirstName = "ammar",
                        LastName = "eldesuky",
                        UserName = "am5u",
                        Email = "ammar@gmail.com",
                        PhoneNumber = "01067204840"
                    };
                    userManager.CreateAsync(Admin, "Ammar@1604").Wait();
                    userManager.AddToRoleAsync(Admin, "Admin").Wait();
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Seed Failed: {ex}");
                return false;
            }
        }
    }
}
