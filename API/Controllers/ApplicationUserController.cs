using API.Base;
using BL.Abstracts;
using BL.Contracts.GeneralService.CMS;
using BL.DTO.Entities;
using BL.DTO.User;
using Domains.AppMetaData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.User;

namespace API.Controllers
{
    [ApiController]
    public class ApplicationUserController : AppControllerBase
    {

        #region Fields
        private readonly IApplicationUserService _applicationUserService;
        private readonly ICurrentUserService _currentUserService;
        #endregion

        #region Constructor
        public ApplicationUserController(IApplicationUserService applicationUserService,
           ICurrentUserService currentUserService)
        {
            _applicationUserService = applicationUserService;
            _currentUserService = currentUserService;
        }
        #endregion

        #region Apis
        [HttpPost(Router.ApplicationUserRouting.Register)]
        public async Task<ActionResult<RegisterDTO>> Register(RegisterDTO registerDTO)
        {
            var result = await _applicationUserService.RegisterAsync(registerDTO);
            return NewResult(result);

        }

        // After user Register 
        [HttpPost(Router.ApplicationUserRouting.ConfirmEmail)]
        // Remember With Angular  Set http verb To HttpPost
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailDTO confirmEmailDTO)
        {
            return NewResult(await _applicationUserService.ConfirmUserEmail(confirmEmailDTO.UserId, confirmEmailDTO.Code));
        }

        [HttpGet(Router.ApplicationUserRouting.SendResetPassword)]
        public async Task<IActionResult> SendResetPassword(string email)
        {
            return NewResult(await _applicationUserService.SendResetUserPasswordCodeForAngular(email));
        }

        [HttpPost(Router.ApplicationUserRouting.ResetPassword)]
        public async Task<IActionResult> ResetPassword(RestPasswordDTO restPasswordDTO)
        {
            return NewResult(await _applicationUserService.ResetPassword(restPasswordDTO));
        }

        [Authorize]
        [HttpPut(Router.ApplicationUserRouting.UpdateAddress)]
        public async Task<IActionResult> UpdateAddress(ShipAddressDTO addressDTO)
        {
            return NewResult(await _applicationUserService.UpdateAddressAsync(UserId, addressDTO));
        }
        [Authorize]
        [HttpGet(Router.ApplicationUserRouting.GetAddressForUser)]
        public async Task<IActionResult> GetAddressForUser()
        {
            return NewResult(await _applicationUserService.GetUserAddressAsync(UserId));
        }

        [HttpGet(Router.ApplicationUserRouting.IsAuthenticated)]
        public IActionResult IsAuthenticated()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return Ok();
            }

            return Unauthorized();
        }

        [HttpGet(Router.ApplicationUserRouting.GetUserName)]
        public IActionResult GetUserName()
        {
            return NewResult(_currentUserService.GetUserName());

        } 
        #endregion

    }
}

