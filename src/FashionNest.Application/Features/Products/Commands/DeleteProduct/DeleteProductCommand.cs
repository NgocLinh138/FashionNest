using FashionNest.Application.Common.Messaging;

namespace FashionNest.Application.Features.Products.Commands.DeleteProduct
{
    public record DeleteProductCommand(Guid Id) : ICommand<Guid>;
}
