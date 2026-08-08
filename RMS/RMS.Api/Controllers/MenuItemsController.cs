using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Application.DTOs;
using RMS.Application.Interfaces;
using System.Threading.Tasks;

namespace RMS.Api.Controllers
{
    [Authorize] // <--- THIS IS THE BOUNCER!
    [Route("api/[controller]")]
    [ApiController]
    public class MenuItemsController : ControllerBase
    {
        private readonly IMenuItemService _itemService;

        public MenuItemsController(IMenuItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllItems()
        {
            var items = await _itemService.GetAllItemsAsync();
            return Ok(items);
        }

        [HttpPost]
        public async Task<IActionResult> CreateItem([FromBody] CreateMenuItemDto createDto)
        {
            var newItem = await _itemService.CreateItemAsync(createDto);
            return Ok(newItem);
        }
    }
}
