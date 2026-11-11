using AutoMapper;
using Lesson2.Models;
using Lesson2.Models.DTO;

namespace Lesson2.Mapping;

public class OrderMappingProfile : Profile
{
    public OrderMappingProfile()
    {
        CreateMap<AddOrderRequest, Order>()
            .ForMember(order => order.OrderNumber, options => options.Ignore())
            .ForMember(order => order.OrderDateTime, options => options.Ignore())
            .ForMember(order => order.IsCompleted, options => options.Ignore());

        CreateMap<UpdateOrderQuantityRequest, Order>()
            .ForMember(order => order.OrderNumber, options => options.Ignore())
            .ForMember(order => order.OrderDate, options => options.Ignore())
            .ForMember(order => order.ProductsCost, options => options.Ignore())
            .ForMember(order => order.OrderDateTime, options => options.Ignore())
            .ForMember(order => order.IsCompleted, options => options.Ignore())
            .ForMember(order => order.Comment, options => options.Ignore());

        CreateMap<UpdateOrderStatusRequest, Order>()
            .ForMember(order => order.OrderNumber, options => options.Ignore())
            .ForMember(order => order.OrderDate, options => options.Ignore())
            .ForMember(order => order.ProductsQuantity, options => options.Ignore())
            .ForMember(order => order.ProductsCost, options => options.Ignore())
            .ForMember(order => order.OrderDateTime, options => options.Ignore())
            .ForMember(order => order.Comment, options => options.Ignore());

        CreateMap<Order, OrderDto>()
            .ForMember(dto => dto.Status, options => options.MapFrom(order => order.IsCompleted ? "виконано" : "не виконано"));
    }
}
