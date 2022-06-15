﻿using Microsoft.AspNetCore.Mvc;
using SocialMedia.Core.Enums;
using SocialMedia.Core.Interfaces;

namespace SocialMedia.Rest.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly IUserService _service;

        public AccountController(IUserService service)
        {
            _service = service;
        }

        #region ConfirmEmailAsync
        /// <summary>
        /// Used for confirming user email.
        /// </summary>
        /// <param name="userId">Represents the id of the user.</param>
        /// <param name="token">Represents the email confirmation token.</param>
        /// <returns>
        /// An <see cref="Microsoft.AspNetCore.Mvc.IActionResult"/>,
        /// containing details about the operation.
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var result = await _service.ConfirmEmailAsync(userId, token);

            if (result.Succeeded)
            {
                return View();
            }

            return result.Fault.ErrorType switch
            {
                ErrorType.NotFound => View("Error", "User not found."),
                _ => View("Error", result.Fault.ErrorMessage)
            };
        }
        #endregion
    }
}
