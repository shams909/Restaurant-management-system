using AutoMapper;
using RMS.Application.DTOs;
using RMS.Domain.Entities;

namespace RMS.Application.Mappings
{
    // Inheriting from 'Profile' tells AutoMapper that this file contains the translation rules
    public class MappingProfile : Profile
    {
        public MappingProfile()


        {

            // Inventory Item Rules
            CreateMap<InventoryItem, InventoryItemDto>();
            CreateMap<CreateInventoryItemDto, InventoryItem>();


            // Payment Rules
            CreateMap<Payment, PaymentDto>();
            CreateMap<CreatePaymentDto, Payment>();


            // Order Item Rules
            CreateMap<OrderItem, OrderItemDto>();
            CreateMap<CreateOrderItemDto, OrderItem>();

            // Order Rules
            CreateMap<Order, OrderDto>();
            CreateMap<CreateOrderDto, Order>();


            // Table Rules
            CreateMap<Table, TableDto>();
            CreateMap<CreateTableDto, Table>();


            // Menu Item Rules
            CreateMap<MenuItem, MenuItemDto>();
            CreateMap<CreateMenuItemDto, MenuItem>();


            // Menu Category Rules
            CreateMap<MenuCategory, MenuCategoryDto>();
            CreateMap<CreateMenuCategoryDto, MenuCategory>();



            // User Rules
            CreateMap<User, UserDto>();
            CreateMap<CreateUserDto, User>();



            // Branch Rules
            CreateMap<Branch, BranchDto>();
            CreateMap<CreateBranchDto, Branch>();



            // Rule 1: We are allowed to convert a raw Tenant into a safe TenantDto (for GET requests)
            CreateMap<Tenant, TenantDto>();

            // Rule 2: We are allowed to convert a CreateTenantDto into a raw Tenant (for POST requests)
            CreateMap<CreateTenantDto, Tenant>();
        }
    }
}
