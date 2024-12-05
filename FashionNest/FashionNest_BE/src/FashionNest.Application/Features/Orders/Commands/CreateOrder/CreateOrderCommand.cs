using FashionNest.Application.Common.Messaging;
using FashionNest.Application.Features.Orders.Responses;
using FashionNest.Domain.Enums;

namespace FashionNest.Application.Features.Orders.Commands.CreateOrder
{
    public record CreateOrderCommand(
        Guid UserId,
        Guid? CouponId,
        List<OrderItemCommand> OrderItems,
        PaymentMethod PaymentMethod) : ICommand<CreateOrderResult>;

    public record OrderItemCommand(
        Guid ProductId, 
        int Quantity, 
        decimal Price
        );

    public record CreateOrderResult(CreateOrderResponse Order);
}
