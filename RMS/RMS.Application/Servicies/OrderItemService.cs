using AutoMapper;
using RMS.Application.DTOs;
using RMS.Application.Interfaces;
using RMS.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RMS.Application.Services
{
    public class OrderItemService : IOrderItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderItemService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderItemDto>> GetAllOrderItemsAsync()
        {
            var items = await _unitOfWork.Repository<OrderItem>().GetAllAsync();
            return _mapper.Map<IEnumerable<OrderItemDto>>(items);
        }

        public async Task<OrderItemDto> CreateOrderItemAsync(CreateOrderItemDto createDto)
        {
            var item = _mapper.Map<OrderItem>(createDto);

            await _unitOfWork.Repository<OrderItem>().AddAsync(item);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<OrderItemDto>(item);
        }
    }
}
