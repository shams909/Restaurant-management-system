using Microsoft.AspNetCore.Mvc;
using RMS.Application.Interfaces;
using RMS.Application.DTOs;
using System.Threading.Tasks;

namespace RMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuCategoriesController : ControllerBase
    {
        private readonly IMenuCategoryService _categoryService;

        public MenuCategoriesController(IMenuCategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CreateMenuCategoryDto createDto)
        {
            var newCategory = await _categoryService.CreateCategoryAsync(createDto);
            return Ok(newCategory);
        }
    }
}
