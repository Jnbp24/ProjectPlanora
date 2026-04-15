using System.Data;
using Microsoft.AspNetCore.Identity;
using Planora.DataAccess;
using Planora.DataAccess.Context;
using Planora.DataAccess.Models.Auth;

namespace Planora.Api;

public static class DbInitializer
{
    public static async Task SeedAdminUser(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<AuthUser>>();
        var context = serviceProvider.GetRequiredService<DatabaseContext>();
        
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();

        //Ensure the Admin Role exists
        if (!await roleManager.RoleExistsAsync("Tovholder"))
        {
            await roleManager.CreateAsync(new IdentityRole("Tovholder"));
        }

        //Check if the Admin already exists to avoid duplicates
        var adminEmail = "admin@planora.com";
        var existingAdmin = await userManager.FindByEmailAsync(adminEmail);

        if (existingAdmin == null)
        {
            var user = new UserDB
            {
                Id = Guid.NewGuid(),
                Email = adminEmail,
                FirstName = "System",
                LastName = "Admin",
                Tovholder = true,
                Deleted = false
            };
            
            context.Users.Add(user);
            await context.SaveChangesAsync();

            //Create the AuthUser
            var adminUser = new AuthUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,
                UserDBId = user.Id,
                UserDb = user
            };

            var password = configuration["PasswordManager:adminPassword"];
            
            if (password is null)
                throw new NoNullAllowedException("Loaded password for seedUser is null");
            
            var createResult = await userManager.CreateAsync(adminUser, password);

            if (createResult.Succeeded)
            {
                //Assign the Role
                await userManager.AddToRoleAsync(adminUser, "Tovholder");
            }
        }
    }
}