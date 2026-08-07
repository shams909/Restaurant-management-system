using RMS.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RMS.Application.Interfaces
{
    public interface IMenuItemService
    {
        Task<IEnumerable<MenuItemDto>> GetAllItemsAsync();
        Task<MenuItemDto> CreateItemAsync(CreateMenuItemDto createDto);
    }
}
