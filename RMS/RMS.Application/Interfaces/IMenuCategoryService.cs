using RMS.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RMS.Application.Interfaces
{
    public interface IMenuCategoryService
    {
        Task<IEnumerable<MenuCategoryDto>> GetAllCategoriesAsync();
        Task<MenuCategoryDto> CreateCategoryAsync(CreateMenuCategoryDto createDto);
    }
}
