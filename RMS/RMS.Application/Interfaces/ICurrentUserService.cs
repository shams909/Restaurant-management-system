namespace RMS.Application.Interfaces
{
    public interface ICurrentUserService
    {
        string TenantId { get; }
        int BranchId { get; }
        int RoleId { get; }
    }
}
