using IdentityModel;
using Mango.Services.Identity.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Mango.Services.Identity.Data
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationDbContext db,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Initialize()
        {
            if (_roleManager.FindByNameAsync(Consts.Admin).Result == null)
            {
                _roleManager.CreateAsync(new IdentityRole(Consts.Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(Consts.Customer)).GetAwaiter().GetResult();
            }
            else
                return;

            ApplicationUser adminUser = new ApplicationUser()
            {
                UserName = "admin@mango.com",
                Email = "admin@mango.com",
                EmailConfirmed = true,
                PhoneNumber = "111111111",
                FirstName = "Frodo",
                LastName = "Baggins"
            };

            var res =  _userManager.CreateAsync(adminUser, "Admin123*").GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(adminUser, Consts.Admin).GetAwaiter().GetResult();

            _userManager.AddClaimsAsync(adminUser, new Claim[]
            {
                new Claim(JwtClaimTypes.Name, adminUser.FirstName + " " + adminUser.LastName),
                new Claim(JwtClaimTypes.GivenName, adminUser.FirstName),
                new Claim(JwtClaimTypes.FamilyName, adminUser.LastName),
                new Claim(JwtClaimTypes.Role, Consts.Admin),
            }).GetAwaiter().GetResult();

            ApplicationUser customerUser = new ApplicationUser()
            {
                UserName = "customer1@mango.com",
                Email = "customer1@mango.com",
                EmailConfirmed = true,
                PhoneNumber = "111111111",
                FirstName = "John",
                LastName = "Snow"
            };

            _userManager.CreateAsync(customerUser, "Admin123*").GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(customerUser, Consts.Customer).GetAwaiter().GetResult();

            _userManager.AddClaimsAsync(customerUser, new Claim[]
            {
                new Claim(JwtClaimTypes.Name, customerUser.FirstName + " " + customerUser.LastName),
                new Claim(JwtClaimTypes.GivenName, customerUser.FirstName),
                new Claim(JwtClaimTypes.FamilyName, customerUser.LastName),
                new Claim(JwtClaimTypes.Role, Consts.Customer),
            }).GetAwaiter().GetResult();
        }
    }
}
