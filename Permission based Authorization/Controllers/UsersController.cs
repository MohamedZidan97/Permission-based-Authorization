using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Permission_based_Authorization.Models.Identities.Users;

namespace Permission_based_Authorization.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UsersController (UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task<IActionResult> GetUsers()
        {
            var users = await userManager.Users.Select(user => new UsersVM
            { Id = user.Id, Email = user.Email, Roles = userManager.GetRolesAsync(user).Result }).ToListAsync();

            return View(users);
        }

        public async Task<IActionResult> ManagerRoles(string Id)
        {
            var user  = await userManager.FindByIdAsync(Id);
            var roles = await roleManager.Roles.ToListAsync();

            var userRole = new UserRolesVM()
            {
                UserId = user.Id,
                Email = user.Email,
                Roles = roles.Select(e => new RoleVM
                {
                    RoleName = e.Name,
                    IsSelected =  userManager.IsInRoleAsync(user, e.Name).Result
                }).ToList()
            };

            return View(userRole);
        }

        [HttpPost]
        public async Task<IActionResult> ManagerRoles(UserRolesVM userRolesVM)
        {
            var user = await userManager.FindByIdAsync(userRolesVM.UserId);
            var roles = await userManager.GetRolesAsync(user);

            await userManager.RemoveFromRolesAsync(user, roles);

            for(int i = 0; i < userRolesVM.Roles.Count(); i++)
            {
                if (userRolesVM.Roles[i].IsSelected)
                {
                    await userManager.AddToRoleAsync(user, userRolesVM.Roles[i].RoleName);
                }
            }

            return RedirectToAction("GetUsers");
        }
    }
}
