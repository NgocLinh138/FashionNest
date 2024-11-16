using FashionNest.Application.Common.Messaging;
using FashionNest.Application.Features.Categories.Responses;

namespace FashionNest.Application.Features.Categories.Commands.CreateCategory
{
    public record CreateCategoryCommand(
        string Name,
        string Description) : ICommand<CreateCategoryResult>;

    public record CreateCategoryResult(CategoryResponse CategoryResponse);
}
