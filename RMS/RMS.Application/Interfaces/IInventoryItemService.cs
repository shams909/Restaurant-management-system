using RMS.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RMS.Application.Interfaces
{
    public interface IInventoryItemService
    {
        Task<IEnumerable<InventoryItemDto>> GetAllInventoryItemsAsync();
        Task<InventoryItemDto> CreateInventoryItemAsync(CreateInventoryItemDto createDto);
    }
}
