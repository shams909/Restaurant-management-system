using AutoMapper;
using RMS.Application.DTOs;
using RMS.Application.Interfaces;
using RMS.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RMS.Application.Services
{
    public class MenuCategoryService : IMenuCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MenuCategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MenuCategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _unitOfWork.Repository<MenuCategory>().GetAllAsync();
            return _mapper.Map<IEnumerable<MenuCategoryDto>>(categories);
        }

        public async Task<MenuCategoryDto> CreateCategoryAsync(CreateMenuCategoryDto createDto)
        {
            var category = _mapper.Map<MenuCategory>(createDto);

            await _unitOfWork.Repository<MenuCategory>().AddAsync(category);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<MenuCategoryDto>(category);
        }
    }
}
