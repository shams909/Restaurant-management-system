using AutoMapper;
using RMS.Application.DTOs;
using RMS.Application.Interfaces;
using RMS.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RMS.Application.Services
{
    public class MenuItemService : IMenuItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MenuItemService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MenuItemDto>> GetAllItemsAsync()
        {
            var items = await _unitOfWork.Repository<MenuItem>().GetAllAsync();
            return _mapper.Map<IEnumerable<MenuItemDto>>(items);
        }

        public async Task<MenuItemDto> CreateItemAsync(CreateMenuItemDto createDto)
        {
            var item = _mapper.Map<MenuItem>(createDto);

            await _unitOfWork.Repository<MenuItem>().AddAsync(item);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<MenuItemDto>(item);
        }
    }
}
