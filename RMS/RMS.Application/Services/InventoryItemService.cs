using AutoMapper;
using RMS.Application.DTOs;
using RMS.Application.Interfaces;
using RMS.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RMS.Application.Services
{
    public class InventoryItemService : IInventoryItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public InventoryItemService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<InventoryItemDto>> GetAllInventoryItemsAsync()
        {
            var items = await _unitOfWork.Repository<InventoryItem>().GetAllAsync();
            return _mapper.Map<IEnumerable<InventoryItemDto>>(items);
        }

        public async Task<InventoryItemDto> CreateInventoryItemAsync(CreateInventoryItemDto createDto)
        {
            var item = _mapper.Map<InventoryItem>(createDto);

            await _unitOfWork.Repository<InventoryItem>().AddAsync(item);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<InventoryItemDto>(item);
        }
    }
}
