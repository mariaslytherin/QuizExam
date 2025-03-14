﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuizExam.Core.Constants;
using QuizExam.Core.Contracts;
using QuizExam.Core.Models;
using QuizExam.Infrastructure.Data.Identity;

namespace QuizExam.Areas.Admin.Controllers
{
    [Authorize(Roles = UserRolesConstants.SuperAdmin)]
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserService userService;

        public UserController(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            IUserService userService)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetAllUsers(int p = 1, int s = 5)
        {
            var users = await this.userService.GetAllUsers(p, s);

            foreach (var user in users.Users)
            {
                var currentUser = await this.userService.GetUserById(user.Id);
                var roles = (await this.userManager.GetRolesAsync(currentUser)).Select(role =>
                    role switch
                    {
                        "SuperAdmin" => "Главен администратор",
                        "Administrator" => "Администратор",
                        _ => "Ученик"
                    }).ToList();

                user.Roles = roles;
            }

            return View("UsersList", users);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = await this.userService.GetUserForEdit(id);

            return View("Edit", user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserEditVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (await this.userService.EditUserData(model))
            {
                TempData[SuccessMessageConstants.SuccessMessage] = SuccessMessageConstants.SuccessfulEditMessage;
            }
            else
            {
                throw new Exception("An error appeard!");
            }

            return RedirectToAction(nameof(GetAllUsers));
        }

        public async Task<IActionResult> GetRoles(string id)
        {
            ViewBag.UserId = id;
            var user = await this.userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }

            ViewBag.UserFullName = $"{user.FirstName} {user.LastName}";
            var model = new List<UserRolesVM>();

            foreach (var role in this.roleManager.Roles)
            {
                string displayName = role.Name;
                switch (role.Name)
                {
                    case UserRolesConstants.SuperAdmin: displayName = "Главен администратор"; break;
                    case UserRolesConstants.Administrator: displayName = "Администратор"; break;
                    case UserRolesConstants.Student: displayName = "Ученик"; break;
                    default:
                        break;
                }
                var userRolesViewModel = new UserRolesVM
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    DisplayName = displayName,
                };
                if (await this.userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.Selected = true;
                }
                else
                {
                    userRolesViewModel.Selected = false;
                }
                model.Add(userRolesViewModel);
            }

            return View("Roles", model);
        }

        [HttpPost]
        public async Task<IActionResult> SetRoles(List<UserRolesVM> model, string id)
        {
            var user = await this.userManager.FindByIdAsync(id);

            if (user == null)
            {
                TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.ErrorAppeardMessage;
                return RedirectToAction("GetAllUsers", "User");
            }

            if (!model.Any(u => u.Selected))
            {
                TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.ErrorMustSelectRoleMessage;
                return RedirectToAction("GetRoles", "User", new { id = id });
            }

            var roles = await this.userManager.GetRolesAsync(user);
            var result = await this.userManager.RemoveFromRolesAsync(user, roles);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles");
                return View(model);
            }

            result = await this.userManager.AddToRolesAsync(user, model.Where(x => x.Selected).Select(y => y.RoleName));

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected roles to user");
                return View(model);
            }
            else if (result.Succeeded)
            {
                TempData[SuccessMessageConstants.SuccessMessage] = SuccessMessageConstants.SuccessfulAddedRoleMessage;
            }

            return RedirectToAction(nameof(GetAllUsers));
        }

        public async Task<IActionResult> CreateRole()
        {
            await roleManager.CreateAsync(new IdentityRole()
            {
                Name = "Administrator"
            });

            return Ok();
        }
    }
}
