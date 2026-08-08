using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Application.Interfaces;
using System.Threading.Tasks;

namespace RMS.Api.Controllers
{
    [Authorize] // Only logged in kitchen staff can use this!
    [Route("api/[controller]")]
    [ApiController]
    public class KitchenController : ControllerBase
    {
        private readonly IKitchenService _kitchenService;

        public KitchenController(IKitchenService kitchenService)
        {
            _kitchenService = kitchenService;
        }

        // GET: api/Kitchen/Tickets
        [HttpGet("Tickets")]
        public async Task<IActionResult> GetPendingTickets()
        {
            var tickets = await _kitchenService.GetPendingTicketsAsync();
            return Ok(tickets);
        }

        // PATCH: api/Kitchen/Tickets/5/Status
        [HttpPatch("Tickets/{id}/Status")]
        public async Task<IActionResult> UpdateTicketStatus(int id, [FromBody] string newStatus)
        {
            var success = await _kitchenService.UpdateTicketStatusAsync(id, newStatus);
            if (!success) return NotFound("Ticket not found.");

            return Ok("Ticket status updated to " + newStatus);
        }
    }
}
