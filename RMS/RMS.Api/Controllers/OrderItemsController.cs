using Microsoft.AspNetCore.Mvc;
using RMS.Application.Interfaces;
using RMS.Application.DTOs;
using System.Threading.Tasks;

namespace RMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemsController : ControllerBase
    {
        private readonly IOrderItemService _itemService;

        public OrderItemsController(IOrderItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrderItems()
        {
            var items = await _itemService.GetAllOrderItemsAsync();
            return Ok(items);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrderItem([FromBody] CreateOrderItemDto createDto)
        {
            var newItem = await _itemService.CreateOrderItemAsync(createDto);
            return Ok(newItem);
        }
    }
}
