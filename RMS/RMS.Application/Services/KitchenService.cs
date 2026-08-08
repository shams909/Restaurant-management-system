using AutoMapper;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using RMS.Application.DTOs;
using RMS.Application.Interfaces;
using RMS.Domain.Entities;

namespace RMS.Application.Services
{
    public class KitchenService : IKitchenService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public KitchenService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<IEnumerable<OrderItemDto>> GetPendingTicketsAsync()
        {
            // 1. Fetch all items that have been ordered
            var allItems = await _unitOfWork.Repository<OrderItem>().GetAllAsync();

            // 2. Only return items that are NOT "Ready" or "Served"
            var pendingItems = allItems.Where(i => i.KdsStatus == "Pending" || i.KdsStatus == "Cooking").ToList();

            return _mapper.Map<IEnumerable<OrderItemDto>>(pendingItems);
        }

        public async Task<bool> UpdateTicketStatusAsync(int orderItemId, string newStatus)
        {
            // 1. Find the specific order item the chef tapped on
            var item = await _unitOfWork.Repository<OrderItem>().GetByIdAsync(orderItemId);
            if (item == null) return false;

            // 2. Change the status (e.g. to "Cooking")
            item.KdsStatus = newStatus;

            // 3. Save it to the database so the waiters' iPads update instantly!
            _unitOfWork.Repository<OrderItem>().Update(item);
            await _unitOfWork.SaveAsync();

            return true;
        }
    }
}
