﻿
using BL.GenericResponse;
using Domains.Identity;
using Shared.DTOs.User;

namespace BL.Abstracts
{
    public interface IApplicationUserService
    {
        public Task<ApplicationUser> CreateUser(ApplicationUser user, string password);
        public Task SendConfirmUserEmailToken(ApplicationUser user);
        //Task ConfirmUserEmail(ApplicationUser user, string code);
        Task<Response<string>> ConfirmUserEmail(string userId, string code);
        Task<ApplicationUser?> FindByIdAsync(string userId);
        public Task<string> SoftDeleteUserAsync(ApplicationUser user, Guid updatedBy);
        public Task<string> DeleteUserAsync(ApplicationUser user);
        Task<ApplicationUser?> FindByEmailAsync(string email);
        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
        Task<string> UpdateAsync(ApplicationUser user);
        public Task<string> ChangePasswordAsync(ApplicationUser user, string oldPassword, string confirmPassword);

        Task<string> SendResetUserPasswordCode(string email);
        Task<string> ConfirmResetPasswordCodeAsyn(string Code, string Email);
        Task<string> ResetPassword(string ResetCode, string newPassword);
        IQueryable<ApplicationUser> GetAllUsersQueryable();
        Task<ApplicationUser?> FindByNameAsync(string userName);
        Task<bool> IsEmailConfirmedAsync(ApplicationUser user);
        //
        Task<IList<ApplicationUser>> GetUsersInRoleAsync(string role);
        public Task<Response<RegisterDTO>> RegisterAsync(RegisterDTO user);

    }
}
