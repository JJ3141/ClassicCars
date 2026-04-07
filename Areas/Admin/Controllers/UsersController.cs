using ClassicCars.Areas.Admin.ViewModels;
using ClassicCars.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ClassicCars.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users.ToList();
            var model = new List<UserListItemViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                model.Add(new UserListItemViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Roles = string.Join(", ", roles),
                    SelectedRole = roles.FirstOrDefault(),
                    AllRoles = _roleManager.Roles.Select(r => r.Name).ToList()
                });
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateRole(string userId, string selectedRole)
        {
            if (string.IsNullOrEmpty(userId)) return BadRequest();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user);

            if (!string.IsNullOrEmpty(selectedRole) && !await _roleManager.RoleExistsAsync(selectedRole))
            {
                TempData["RoleMessage"] = $"Role '{selectedRole}' does not exist.";
                return RedirectToAction(nameof(Index));
            }

            var rolesToRemove = userRoles.Where(r => r != selectedRole).ToArray();

            IdentityResult removeResult = IdentityResult.Success;
            IdentityResult addResult = IdentityResult.Success;

            if (rolesToRemove.Any())
            {
                removeResult = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
            }

            if (!string.IsNullOrEmpty(selectedRole) && !userRoles.Contains(selectedRole))
            {
                addResult = await _userManager.AddToRoleAsync(user, selectedRole);
            }

            if (addResult.Succeeded && removeResult.Succeeded)
            {
                TempData["RoleMessage"] = "User roles updated.";
                return RedirectToAction(nameof(Index));
            }

            TempData["RoleMessage"] = string.Join("; ",
                addResult.Errors.Concat(removeResult.Errors).Select(e => e.Description));

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                TempData["RoleMessage"] = "Role name cannot be empty.";
                return RedirectToAction(nameof(Index));
            }

            if (await _roleManager.RoleExistsAsync(roleName))
            {
                TempData["RoleMessage"] = $"Role '{roleName}' already exists.";
                return RedirectToAction(nameof(Index));
            }

            var result = await _roleManager.CreateAsync(new IdentityRole(roleName));

            if (result.Succeeded)
            {
                TempData["RoleMessage"] = $"Role '{roleName}' created.";
                return RedirectToAction(nameof(Index));
            }

            TempData["RoleMessage"] = string.Join("; ", result.Errors.Select(e => e.Description));
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> EditRoles(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user);
            var allRoles = _roleManager.Roles.Select(r => r.Name).ToList();

            var model = new EditUserRolesViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                AllRoles = allRoles,
                SelectedRole = userRoles.FirstOrDefault()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRoles(EditUserRolesViewModel model)
        {
            if (model == null) return BadRequest();

            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null) return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user);
            var targetRole = model.SelectedRole;

            var rolesToRemove = userRoles.Where(r => r != targetRole).ToArray();

            IdentityResult removeResult = IdentityResult.Success;
            IdentityResult addResult = IdentityResult.Success;

            if (rolesToRemove.Any())
            {
                removeResult = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
            }

            if (!string.IsNullOrEmpty(targetRole) && !userRoles.Contains(targetRole))
            {
                addResult = await _userManager.AddToRoleAsync(user, targetRole);
            }

            if (addResult.Succeeded && removeResult.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in addResult.Errors.Concat(removeResult.Errors))
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            model.AllRoles = _roleManager.Roles.Select(r => r.Name).ToList();
            return View(model);
        }
    }
}