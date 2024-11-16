using FashionNest.Application.Common.Messaging;
using FashionNest.Application.Features.Orders.Responses;

namespace FashionNest.Application.Features.Orders.Commands.CreateOrder
{
    public record CreateOrderCommand(
        Guid UserId,
        Guid? CouponId,
        List<OrderItemCommand> OrderItems
        ) : ICommand<CreateOrderResult>;

    public record OrderItemCommand(
        Guid ProductId, 
        int Quantity, 
        decimal Price
        );

    public record CreateOrderResult(OrderResponse Order);
}
