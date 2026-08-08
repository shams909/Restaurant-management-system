using System.Collections.Generic;
using System.Threading.Tasks;
using RMS.Application.DTOs;

namespace RMS.Application.Interfaces
{
    public interface IKitchenService
    {
        // 1. Fetch tickets for the chefs
        Task<IEnumerable<OrderItemDto>> GetPendingTicketsAsync();

        // 2. Chef taps the iPad to update the status (Pending -> Cooking -> Ready)
        Task<bool> UpdateTicketStatusAsync(int orderItemId, string newStatus);
    }
}
