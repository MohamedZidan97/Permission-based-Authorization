using Microsoft.AspNetCore.Identity;
using Permission_based_Authorization.BL.Constants;
using System.Security.Claims;

namespace Permission_based_Authorization.BL.Seeds
{
    public static class DefualtUsers
    {
        public static async Task SeedSuperAdminUserAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var user = new IdentityUser
            {
                Email = "SuperAdmin@gmail.com",
                UserName = "SuperAdmin@gmail.com",
                EmailConfirmed = true
            };

            if (await userManager.FindByEmailAsync(user.Email) == null)
            {
                await userManager.CreateAsync(user, "Zidan@321.");
                await userManager.AddToRolesAsync(user,new List<string>() { Roles.SuperAdmin.ToString(), Roles.Admin.ToString(), Roles.User.ToString() });

            }

            await roleManager.SeedClaimsForSuperUserAsync();

        }
        public static async Task SeedClaimsForSuperUserAsync(this RoleManager<IdentityRole> roleManager)
        {
            var role = await roleManager.FindByNameAsync(Roles.SuperAdmin.ToString());
          

            await roleManager.AddPermissionClaimsAsync(role, "Prodect");
        }
        public static async Task AddPermissionClaimsAsync(this RoleManager<IdentityRole> roleManager,IdentityRole role,string module)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            var permissions = await Permissions.GeneratedPermissionList(module);


            foreach (var permission in permissions)
            {
                if(!allClaims.Any(p=>p.Type== "Permission"&& p.Value== permission))
                {
                    await roleManager.AddClaimAsync(role,new Claim ("Permission",permission));
                }
            }


        }
    }
}
