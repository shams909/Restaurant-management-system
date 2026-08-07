using Microsoft.AspNetCore.Mvc;
using RMS.Application.Interfaces;
using RMS.Domain.Entities;
using RMS.Application.DTOs;
using AutoMapper;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace RMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenantsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper; // AutoMapper engine

        // We inject BOTH the Database Engine and the AutoMapper Engine
        public TenantsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/Tenants
        [HttpGet]
        public async Task<IActionResult> GetAllTenants()
        {
            // 1. Get raw, sensitive data from the database
            var tenants = await _unitOfWork.Repository<Tenant>().GetAllAsync();

            // 2. MAGIC: Automatically convert it into a safe list of DTOs!
            var safeTenants = _mapper.Map<IEnumerable<TenantDto>>(tenants);

            // 3. Return the safe data to the internet
            return Ok(safeTenants);
        }

        // POST: api/Tenants
        [HttpPost]
        public async Task<IActionResult> CreateTenant([FromBody] CreateTenantDto createDto)
        {
            // 1. MAGIC: Convert the safe DTO back into a raw database Entity so SQL Server can read it
            var tenant = _mapper.Map<Tenant>(createDto);

            // 2. Add our secure, backend-only fields that the user isn't allowed to touch
            tenant.Id = Guid.NewGuid();
            tenant.CreatedAt = DateTime.UtcNow;

            // 3. Save to SQL Server
            await _unitOfWork.Repository<Tenant>().AddAsync(tenant);
            await _unitOfWork.SaveAsync();

            // 4. Convert it back to a safe DTO before sending the success message back!
            var safeResponse = _mapper.Map<TenantDto>(tenant);
            return Ok(safeResponse);
        }
    }
}
