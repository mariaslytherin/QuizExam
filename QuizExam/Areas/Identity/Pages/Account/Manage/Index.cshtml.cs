// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuizExam.Core.Constants;
using QuizExam.Infrastructure.Data.Identity;

namespace QuizExam.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            [Required(ErrorMessage = "Полето {0} е задължително.")]
            [StringLength(50, ErrorMessage = "Полето {0} трябва да бъде максимум {1} знака.")]
            [Display(Name = "Име")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "Полето {0} е задължително.")]
            [StringLength(50, ErrorMessage = "Полето {0} трябва да бъде максимум {1} знака.")]
            [Display(Name = "Фамилия")]
            public string LastName { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userInfo = await _userManager.GetUserAsync(User);

            Input = new InputModel
            {
                FirstName = userInfo.FirstName,
                LastName = userInfo.LastName,
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            bool isUserUpdated = false;
            if (user.FirstName != Input.FirstName || user.LastName != Input.LastName)
            {
                user.FirstName = Input.FirstName;
                user.LastName = Input.LastName;
                isUserUpdated = true;
            }

            if (isUserUpdated)
            {
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    TempData[ErrorMessageConstants.ErrorMessage] = "Възникна грешка при опит за редактиране на данните.";
                    return RedirectToPage();
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            TempData[SuccessMessageConstants.SuccessMessage] = "Вашите данни бяха редактирани успешно.";
            return RedirectToPage();
        }
    }
}
