

using Domains.Identity;

namespace BL.Contracts.GeneralService.CMS
{
    public interface ICurrentUserService
    {
        public Task<ApplicationUser> GetUserAsync();
        public Guid GetUserId();
        public  Task<bool> CheckIfRuleExist(string roleName);
    }
}
