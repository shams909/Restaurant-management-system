using AutoMapper;
using RMS.Application.DTOs;
using RMS.Application.Interfaces;
using RMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RMS.Application.Services
{
    public class TenantService : ITenantService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TenantService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TenantDto>> GetAllTenantsAsync()
        {
            var tenants = await _unitOfWork.Repository<Tenant>().GetAllAsync();
            return _mapper.Map<IEnumerable<TenantDto>>(tenants);
        }

        public async Task<TenantDto> CreateTenantAsync(CreateTenantDto createDto)
        {
            var tenant = _mapper.Map<Tenant>(createDto);

            tenant.Id = Guid.NewGuid();
            tenant.CreatedAt = DateTime.UtcNow;

            await _unitOfWork.Repository<Tenant>().AddAsync(tenant);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<TenantDto>(tenant);
        }
    }
}
