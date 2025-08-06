using API.Base;
using BL.Abstracts;
using Domains.AppMetaData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.User;

namespace API.Controllers
{
    [ApiController]
    public class AuthenticationController : AppControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost(Router.AuthenticationRouting.Login)]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            var result = await _authenticationService.LoginAsync(loginDTO);

            if (!result.Succeeded || result.Data is null)
                return NewResult(result);

            SetTokenCookie(result.Data.AccessToken);

            return NewResult(result);
        }


        [Authorize]
        [HttpPost(Router.AuthenticationRouting.Logout)]
        public async Task<IActionResult> Logout()
        {
            var result = await _authenticationService.SignOutAsync();
            DeleteTokenCookie();
            return NewResult(result);
        }
        private void SetTokenCookie(string token)
        {
            Response.Cookies.Append("token", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                IsEssential = true,
                Domain = "localhost",
                Expires = DateTimeOffset.UtcNow.AddDays(1)
            });
        }
        private void DeleteTokenCookie()
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                IsEssential = true,
                Domain = "localhost",
            };

            Response.Cookies.Delete("token", cookieOptions);
        }




    }
}

