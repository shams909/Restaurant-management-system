using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Application.Interfaces;
using RMS.Application.DTOs;
using System.Threading.Tasks;

namespace RMS.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TablesController : ControllerBase
    {
        private readonly ITableService _tableService;

        public TablesController(ITableService tableService)
        {
            _tableService = tableService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTables()
        {
            var tables = await _tableService.GetAllTablesAsync();
            return Ok(tables);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTable([FromBody] CreateTableDto createDto)
        {
            var newTable = await _tableService.CreateTableAsync(createDto);
            return Ok(newTable);
        }
    }
}

