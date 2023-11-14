using GameCenter.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GameCenter.Data
{
    public class SeedData
    {
        private readonly ApplicationDbContext _context;

        public SeedData(ApplicationDbContext context)
        {
            _context = context;
        }

        public static async Task CreateRole(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roleNames = { UserRoles.Admin, UserRoles.Moderator, UserRoles.User };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        public static async Task Initialize(
            IServiceProvider serviceProvider,
            UserManager<GameCenterUser> userManager,
            string password
            )
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                var adminUid = await Seed(serviceProvider, "admin@demo.com", password, userManager, "Admin");
                await EnsureRole(serviceProvider, userManager, adminUid, "Admin");
            }
        }

        private static async Task<string> Seed(
            IServiceProvider serviceProvider,
            string userName,
            string initPw,
            UserManager<GameCenterUser> userManager,
            string role
            )
        {
            var user = await userManager.FindByEmailAsync(userName);
            if (user == null)
            {
                user = new GameCenterUser
                {
                    UserName = role,
                    Email = userName,
                    FirstName = role,
                    LastName = role,
                };

                var result = await userManager.CreateAsync(user, initPw);
            }

            if (user == null)
                throw new Exception("Nie utworzono użytkownika, problem z hasłem");

            return user.Id;
        }

        private static async Task<IdentityResult> EnsureRole(
            IServiceProvider serviceProvider,
            UserManager<GameCenterUser> userManager,
            string uid,
            string role
            )
        {
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            IdentityResult result;

            if (await roleManager.RoleExistsAsync(role) == false)
            {
                result = await roleManager.CreateAsync(new IdentityRole(role));
            }

            var user = await userManager.FindByIdAsync(uid);
            if (user == null)
                throw new Exception("Użytkownik nie istnieje");

            result = await userManager.AddToRoleAsync(user, role);

            return result;
        }
    }
}
