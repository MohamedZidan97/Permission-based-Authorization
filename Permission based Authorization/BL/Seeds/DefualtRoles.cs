using Microsoft.AspNetCore.Identity;
using Permission_based_Authorization.BL.Constants;

namespace Permission_based_Authorization.BL.Seeds
{
    public static class DefualtRoles
    {
        public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.SuperAdmin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.User.ToString()));
        }
    }
}
