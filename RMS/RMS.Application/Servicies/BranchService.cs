using AutoMapper;
using RMS.Application.DTOs;
using RMS.Application.Interfaces;
using RMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RMS.Application.Services
{
    public class BranchService : IBranchService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BranchService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BranchDto>> GetAllBranchesAsync()
        {
            var branches = await _unitOfWork.Repository<Branch>().GetAllAsync();
            return _mapper.Map<IEnumerable<BranchDto>>(branches);
        }

        public async Task<BranchDto> CreateBranchAsync(CreateBranchDto createDto)
        {
            var branch = _mapper.Map<Branch>(createDto);

            //branch.Id = Guid.NewGuid();
            //branch.CreatedAt = DateTime.UtcNow;

            await _unitOfWork.Repository<Branch>().AddAsync(branch);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<BranchDto>(branch);
        }
    }
}
    