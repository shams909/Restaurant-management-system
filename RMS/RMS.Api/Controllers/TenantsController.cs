using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Application.Interfaces;
using RMS.Application.DTOs;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace RMS.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TenantsController : ControllerBase
    {
        private readonly ITenantService _tenantService; // The Chef!

        // We deleted UnitOfWork and AutoMapper. The Waiter only talks to the Chef now!
        public TenantsController(ITenantService tenantService)
        {
            _tenantService = tenantService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTenants()
        {
            // We just ask the Chef for the safe data and return it
            var safeTenants = await _tenantService.GetAllTenantsAsync();
            return Ok(safeTenants);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTenant([FromBody] CreateTenantDto createDto)
        {
            // We just hand the Chef the customer's request, and he hands us back the finished meal!
            var safeResponse = await _tenantService.CreateTenantAsync(createDto);
            return Ok(safeResponse);
        }
    }
}

