using FashionNest.Application.Common.Messaging;

namespace FashionNest.Application.Features.Orders.Commands.DeleteOrder
{
    public record DeleteOrderCommand(Guid Id) : ICommand<Guid>;

}
