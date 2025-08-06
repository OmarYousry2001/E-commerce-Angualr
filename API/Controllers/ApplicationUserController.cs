using API.Base;
using BL.Abstracts;
using BL.DTO.User;
using Domains.AppMetaData;
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
            var result = await _applicationUserService.ConfirmUserEmail(userId , code);
            return NewResult(result);
        }


        [HttpPost(Router.ApplicationUserRouting.SendResetPassword)]
        public async Task<IActionResult> SendResetPassword(string email)
        {
            var result = await _applicationUserService.SendResetUserPasswordCode(email);
            return NewResult(result);
        }

        [HttpPost(Router.ApplicationUserRouting.ResetPassword)]
        public async Task<IActionResult> ResetPassword(RestPasswordDTO restPasswordDTO)
        {
            var result = await _applicationUserService.ResetPassword(restPasswordDTO);

            return NewResult(result);
        }


    }
}

