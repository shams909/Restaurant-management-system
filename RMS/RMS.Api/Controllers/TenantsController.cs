using Microsoft.AspNetCore.Mvc;
using RMS.Application.Interfaces;
using RMS.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace RMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenantsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        // The API automatically grabs the UnitOfWork engine we just registered!
        public TenantsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/Tenants
        [HttpGet]
        public async Task<IActionResult> GetAllTenants()
        {
            // Magic! We ask the UnitOfWork for the Tenant repository, and get all of them!
            var tenants = await _unitOfWork.Repository<Tenant>().GetAllAsync();
            return Ok(tenants);
        }

        // POST: api/Tenants
        [HttpPost]
        public async Task<IActionResult> CreateTenant([FromBody] Tenant tenant)
        {
            // We use a GUID for high security as requested by the professor
            tenant.Id = Guid.NewGuid();
            tenant.CreatedAt = DateTime.UtcNow;

            await _unitOfWork.Repository<Tenant>().AddAsync(tenant);
            await _unitOfWork.SaveAsync(); // Safely commit to SQL Server

            return Ok(tenant);
        }
    }
}
