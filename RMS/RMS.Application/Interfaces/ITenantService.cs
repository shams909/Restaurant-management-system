using RMS.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RMS.Application.Interfaces
{
    public interface ITenantService
    {
        // The Waiter can ask the Chef for all Tenants
        Task<IEnumerable<TenantDto>> GetAllTenantsAsync();

        // The Waiter can hand the Chef a CreateTenantDto to save
        Task<TenantDto> CreateTenantAsync(CreateTenantDto createDto);
    }
}
