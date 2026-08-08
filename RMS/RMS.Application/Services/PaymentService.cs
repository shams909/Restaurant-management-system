using AutoMapper;
using RMS.Application.DTOs;
using RMS.Application.Interfaces;
using RMS.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RMS.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PaymentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PaymentDto>> GetAllPaymentsAsync()
        {
            var payments = await _unitOfWork.Repository<Payment>().GetAllAsync();
            return _mapper.Map<IEnumerable<PaymentDto>>(payments);
        }

        public async Task<PaymentDto> CreatePaymentAsync(CreatePaymentDto createDto)
        {
            // 1. Find the Order the customer is trying to pay for
            var order = await _unitOfWork.Repository<Order>().GetByIdAsync(createDto.OrderId);
            if (order == null) throw new System.Exception("Order not found.");

            // 2. Security Check: Are they trying to pay for an order that is already paid?
            if (order.Status == "Paid") throw new System.Exception("This order has already been paid in full!");

            // 3. Security Check: Did they hand us enough money?
            if (createDto.Amount < order.GrandTotal)
            {
                throw new System.Exception($"Insufficient funds. The total is ${order.GrandTotal}, but you only provided ${createDto.Amount}.");
            }

            // 4. Create the Payment Receipt
            var payment = _mapper.Map<Payment>(createDto);
            
            // Generate a random Payment Number if the frontend didn't send one
            if (string.IsNullOrEmpty(payment.PaymentNo))
            {
                payment.PaymentNo = "PAY-" + System.Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
            }

            // 5. Officially close the Order ticket!
            order.Status = "Paid";

            // 6. Save the Payment and the updated Order status to the database at the exact same time
            await _unitOfWork.Repository<Payment>().AddAsync(payment);
            _unitOfWork.Repository<Order>().Update(order);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<PaymentDto>(payment);
        }
    }
}
