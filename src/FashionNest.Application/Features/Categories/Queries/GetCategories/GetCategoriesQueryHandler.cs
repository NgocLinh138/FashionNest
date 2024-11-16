using FashionNest.Application.Common.Messaging;
using FashionNest.Application.Features.Categories.Responses;
using FashionNest.Domain.Common;
using FashionNest.Domain.Common.Repositories;

namespace FashionNest.Application.Features.Categories.Queries.GetCategories
{
    public class GetCategoriesQueryHandler : IQueryHandler<GetCategoriesQuery, GetCategoriesResult>
    {
        private readonly ICategoryRepository categoryRepository;

        public GetCategoriesQueryHandler(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }
        public async Task<Result<GetCategoriesResult>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var categories = await categoryRepository.GetAsync(
                    orderByAsc: true,
                    pageIndex: request.PageIndex,
                    pageSize: request.PageSize);

                var response = categories.Select(category => new CategoryResponse(
                    category.Id.Value, 
                    category.Name,
                    category.Description)).ToList();

                return Result.Success(new GetCategoriesResult(new PaginatedResult<CategoryResponse>(response, response.Count, request.PageIndex, request.PageSize)));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
