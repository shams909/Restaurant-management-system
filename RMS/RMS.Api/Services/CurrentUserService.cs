using Microsoft.AspNetCore.Http;
using RMS.Application.Interfaces;
using System.Security.Claims;

namespace RMS.Api.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string TenantId => _httpContextAccessor.HttpContext?.User?.FindFirstValue("tenantId") ?? string.Empty;

        public int BranchId
        {
            get
            {
                var branchIdString = _httpContextAccessor.HttpContext?.User?.FindFirstValue("branchId");
                return int.TryParse(branchIdString, out var branchId) ? branchId : 0;
            }
        }

        public int RoleId
        {
            get
            {
                var roleIdString = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Role);
                return int.TryParse(roleIdString, out var roleId) ? roleId : 0;
            }
        }
    }
}
