using AlphaForums.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AlphaForums.Data;

public class DataSeeder
{
    private readonly ApplicationDbContext _context;

    public DataSeeder(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task SeedSuperUser()
    {
        var roleStore = new RoleStore<IdentityRole>(_context);
        var userStore = new UserStore<ApplicationUser>(_context);

        var hasher = new PasswordHasher<ApplicationUser>();
        var user = new ApplicationUser
        {
            UserName = "ForumAdmin",
            NormalizedUserName = "forumadmin",
            Email = "admin@example.com",
            NormalizedEmail = "admin@example.com",
            EmailConfirmed = true,
            LockoutEnabled =false,
            SecurityStamp = Guid.NewGuid().ToString(),
            
        };
        var hashedPassword = hasher.HashPassword(user, "admin");
        user.PasswordHash = hashedPassword;
        

        var hasAdminRole = _context.Roles.Any(r => r.Name == "Admin");

        if (!hasAdminRole) 
        {
            await roleStore.CreateAsync(new IdentityRole
            {
                Name = "Amin", 
                NormalizedName = "admin"
            });
        }

        var hasSuperUser = _context.Users.Any(u => u.UserName == user.UserName);

        if (!hasSuperUser)
        {
            await userStore.CreateAsync(user);
            await userStore.AddToRoleAsync(user, "Admin");
        }

        await _context.SaveChangesAsync();
    }
}