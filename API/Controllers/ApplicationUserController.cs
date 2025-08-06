using API.Base;
using BL.Abstracts;
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
      private readonly IApplicationUserService _applicationUserService;

        public ApplicationUserController(IApplicationUserService applicationUserService)
        {
            _applicationUserService = applicationUserService;  
        }

        [HttpPost(Router.ApplicationUserRouting.Register)]
        public async Task<ActionResult<RegisterDTO>> Register(RegisterDTO registerDTO)
        {
            var result = await _applicationUserService.RegisterAsync(registerDTO);
            return NewResult(result);

        }

        // After user Register 
        [HttpGet(Router.ApplicationUserRouting.ConfirmEmail)]
        // Remember With Angular  Set http verb To HttpPost
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            return NewResult(await _applicationUserService.ConfirmUserEmail(userId, code));
        }


        [HttpPost(Router.ApplicationUserRouting.SendResetPassword)]
        public async Task<IActionResult> SendResetPassword(string email)
        {
            return NewResult(await _applicationUserService.SendResetUserPasswordCode(email));
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

        [HttpGet(Router.ApplicationUserRouting.GetAddressForUser)]
        public async Task<IActionResult> GetAddressForUser()
        {
           return NewResult( await _applicationUserService.GetUserAddressAsync(UserId));
        }

    
    }
}

