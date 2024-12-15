using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace StudyProj.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RolesController : Controller
    {
        RoleManager<Role> _roleManager;
        UserManager<User> _userManager;
        public RolesController(RoleManager<Role> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> UserList()
        {
            return new JsonResult(await _userManager.Users.ToListAsync());
        }
        [HttpPut]
        public async Task<IActionResult> Put(string userId, List<string> roles)
        {
            bool success = true;
            // получаем пользователя
            User user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                // получем список ролей пользователя
                var userRoles = await _userManager.GetRolesAsync(user);
                // получаем все роли
                var allRoles = _roleManager.Roles.ToList();
                // получаем список ролей, которые были добавлены
                var addedRoles = roles.Except(userRoles);
                // получаем роли, которые были удалены
                var removedRoles = userRoles.Except(roles);

                await _userManager.AddToRolesAsync(user, addedRoles);

                await _userManager.RemoveFromRolesAsync(user, removedRoles);

                return success ? new JsonResult($"Update successful for user with ID: {user.Id}") : new JsonResult("Update was not successful");
            }
            return success ? new JsonResult($"Update successful for user with ID: {user.Id}") : new JsonResult("Update was not successful");


        }
    }
}
