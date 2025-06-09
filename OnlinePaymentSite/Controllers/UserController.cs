using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlinePaymentSite.Services.DTOs.User;
using OnlinePaymentSite.Services.Interfaces;
using OnlinePaymentSite.Web.ViewModels.User;

namespace OnlinePaymentSite.Web.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> Profile()
        {
            var userId = HttpContext.Session.GetInt32("UserId").Value;
            var user = await _userService.GetByIdAsync(userId);
            return View(new UserProfileViewModel
            {
                UserId = user.UserId,
                Username = user.Username,
                FullName = user.FullName
            });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateFullName(UserProfileViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Profile", model);

            var result = await _userService.UpdateFullNameAsync(new UpdateFullNameRequest
            {
                UserId = model.UserId,
                NewFullName = model.FullName
            });

            if (result.Success)
            {
                HttpContext.Session.SetString("FullName", model.FullName);
                TempData["Success"] = "Full name updated successfully.";
            }
            else
            {
                ModelState.AddModelError("", result.ErrorMessage ?? "Failed to update full name.");
            }

            return View("Profile", model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Profile", new UserProfileViewModel { UserId = model.UserId });

            var result = await _userService.UpdatePasswordAsync(new UpdatePasswordRequest
            {
                UserId = model.UserId,
                NewPassword = model.NewPassword
            });

            if (result.Success)
            {
                TempData["Success"] = "Password updated successfully.";
            }
            else
            {
                ModelState.AddModelError("", result.ErrorMessage ?? "Failed to update password.");
            }

            var user = await _userService.GetByIdAsync(model.UserId);
            return View("Profile", new UserProfileViewModel
            {
                UserId = user.UserId,
                Username = user.Username,
                FullName = user.FullName
            });
        }
    }
}
