using FashionNest.Application.Common.Messaging;

namespace FashionNest.Application.Features.Categories.Commands.DeleteCategory
{
    public record DeleteCategoryCommand(Guid Id) : ICommand<Guid>;
}
