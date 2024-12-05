using FashionNest.Application.Common.Messaging;
using FashionNest.Application.Features.Orders.Responses;
using FashionNest.Domain.Enums;

namespace FashionNest.Application.Features.Orders.Commands.UpdateOrder
{
    public record UpdateOrderCommand(
        Guid OrderId,
        OrderStatus Status) : ICommand<UpdateOrderResult>;

    public record UpdateOrderResult(OrderResponse Order);
}
