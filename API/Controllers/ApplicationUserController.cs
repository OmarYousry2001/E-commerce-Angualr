using API.Base;
using BL.Abstracts;
using Domains.AppMetaData;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.User;

namespace API.Controllers
{
    [Route("api/[controller]")]
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
        [HttpGet(Router.ApplicationUserRouting.ConfirmEmail)]
        // Remmber With Angular  Set http verb To HttpPost
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            var result = await _applicationUserService.ConfirmUserEmail(userId , code);
            return NewResult(result);
        }


    }
}

