using AutoMapper;
using System.Linq;
using RMS.Application.DTOs;
using RMS.Application.Interfaces;
using RMS.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RMS.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        // [NEW] 1. Add the CurrentUserService variable!
        private readonly ICurrentUserService _currentUserService;

        // [NEW] 2. Inject the CurrentUserService into the constructor!
        public OrderService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;   
        }

        public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
        {
            var orders = await _unitOfWork.Repository<Order>().GetAllAsync();
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        public async Task<OrderDto> CreateOrderAsync(CreateOrderDto createDto)
        {
            var order = _mapper.Map<Order>(createDto);
            decimal calculatedTotal = 0;

            // 1. We manually loop through what the frontend requested
            foreach (var itemRequest in createDto.Items)
            {
                // 2. Fetch the REAL MenuItem from the database to get the true price!
                var menuItem = await _unitOfWork.Repository<MenuItem>().GetByIdAsync(itemRequest.MenuItemId);
                if (menuItem == null) throw new System.Exception($"Menu Item {itemRequest.MenuItemId} does not exist!");

                // --- INVENTORY DEDUCTION ENGINE ---
                // 1. Look up the recipe for this specific Menu Item
                var recipes = await _unitOfWork.Repository<Recipe>().GetAllAsync();
                var itemRecipes = recipes.Where(r => r.MenuItemId == menuItem.Id).ToList();

                // 2. Loop through every ingredient required to make this item
                foreach (var recipe in itemRecipes)
                {
                    // 3. Find the raw ingredient in the stock room
                    var inventoryItem = await _unitOfWork.Repository<InventoryItem>().GetByIdAsync(recipe.InventoryItemId);
                    if (inventoryItem != null)
                    {
                        // 4. Calculate how much we used (e.g. 1 patty * 2 burgers ordered = 2 patties used)
                        decimal amountUsed = recipe.QuantityUsed * itemRequest.Quantity;

                        // [NEW] The ChatGPT Guard!
                        if (inventoryItem.CurrentStock < amountUsed)
                        {
                            throw new System.Exception($"CRITICAL: Not enough stock! We only have {inventoryItem.CurrentStock} of '{inventoryItem.Id}' left.");
                        }

                        // 5. Deduct it from the stock room!
                        inventoryItem.CurrentStock -= amountUsed;

                        // 6. Tell Entity Framework to update the stock room
                        _unitOfWork.Repository<InventoryItem>().Update(inventoryItem);
                        // 7. Create a Transaction Receipt!
                        var transaction = new InventoryTransaction
                        {
                            // [NEW] 3. Stamp the restaurant's ID onto the receipt!
                            TenantId = System.Guid.Parse(_currentUserService.TenantId),
                            BranchId = order.BranchId,
                            InventoryItemId = inventoryItem.Id,
                            TransactionType = "Sale",
                            QuantityChanged = -amountUsed,
                            Notes = $"Automatically deducted from Kitchen"
                        };


                        await _unitOfWork.Repository<InventoryTransaction>().AddAsync(transaction);

                    }
                }
                // ----------------------------------

                // 3. Calculate the true cost for this line item
                decimal lineTotal = menuItem.BasePrice * itemRequest.Quantity;
                calculatedTotal += lineTotal;

                // 4. Create the actual OrderItem record
                var orderItem = new OrderItem
                {
                    MenuItemId = menuItem.Id,
                    Quantity = itemRequest.Quantity,
                    UnitPrice = menuItem.BasePrice,
                    KdsStatus = "Pending" // Automatically send to Kitchen Display System!
                };

                // Add it to the Order's list of items
                order.OrderItems.Add(orderItem);
            }

            // 5. Hardcode the GrandTotal based on our backend math, NOT the frontend!
            order.GrandTotal = calculatedTotal;

            // 6. Save the Order (Entity Framework will automatically save the OrderItems too!)
            await _unitOfWork.Repository<Order>().AddAsync(order);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<OrderDto>(order);
        }

    }
}
