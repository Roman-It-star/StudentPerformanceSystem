using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace StudentPerformanceSystem.Models
{
    public static class DbInitializer
    {
        public static async Task Initialize(AppDbContext context,
                                         UserManager<IdentityUser> userManager,
                                         RoleManager<IdentityRole> roleManager)
        {
            // Создаем роли, если их нет
            string[] roleNames = { "Admin", "Teacher", "Student" };

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Создаем администратора
            var adminEmail = "admin@example.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var poweruser = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail
                };

                string adminPassword = "Admin123!";
                var createPowerUser = await userManager.CreateAsync(poweruser, adminPassword);

                if (createPowerUser.Succeeded)
                {
                    await userManager.AddToRoleAsync(poweruser, "Admin");
                }
            }
        }
    }
}