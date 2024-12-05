using FashionNest.Application.Common.Messaging;
using FashionNest.Application.Features.Categories.Responses;

namespace FashionNest.Application.Features.Categories.Commands.UpdateCategory
{
    public record UpdateCategoryCommand(
        Guid Id,
        string Name,
        string Description) : ICommand<UpdateCategoryResult>;

    public record UpdateCategoryResult(CategoryResponse CategoryResponse);
}
