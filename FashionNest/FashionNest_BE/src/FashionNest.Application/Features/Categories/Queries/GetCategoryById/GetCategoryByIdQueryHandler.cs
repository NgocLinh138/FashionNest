using FashionNest.Application.Common.Messaging;
using FashionNest.Application.Features.Categories.Responses;
using FashionNest.Domain.Common;
using FashionNest.Domain.Common.Repositories;
using FashionNest.Domain.Constants;

namespace FashionNest.Application.Features.Categories.Queries.GetCategoryById
{
    public class GetCategoryByIdQueryHandler : IQueryHandler<GetCategoryByIdQuery, GetCategoryByIdResult>
    {
        private readonly ICategoryRepository categoryRepository;

        public GetCategoryByIdQueryHandler(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }
        public async Task<Result<GetCategoryByIdResult>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var category = await categoryRepository.GetByIdAsync(CategoryId.Of(request.Id));

            if (category == null)
                return Result.Failure<GetCategoryByIdResult>(CategoryError.NotFound);

            var response = new CategoryResponse(
               category.Id.Value,
               category.Name,
               category.Description);

            return Result.Success(new GetCategoryByIdResult(response));
        }
    }
}
