using FashionNest.Application.Common.Messaging;
using FashionNest.Application.Features.Categories.Responses;
using FashionNest.Domain.Common;
using FashionNest.Domain.Common.Repositories;
using System.Linq.Expressions;

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
                Expression<Func<Category, bool>> filter = category => true;

                if (!string.IsNullOrWhiteSpace(request.Filter))
                {
                    var filterExpression = BuildFilterExpression(request.Filter);
                    filter = CombineFilters(filter, filterExpression);
                }

                Expression<Func<Category, object>> sortBy = request.SortBy switch
                {
                    "Name" => category => category.Name,
                    _ => category => category.Name // Default sort by Name
                };

                var categories = await categoryRepository.GetAsync(
                    filter: filter,
                    orderBy: sortBy,
                    orderByAsc: request.SortOrderAscending, 
                    pageIndex: request.PageIndex,
                    pageSize: request.PageSize);

                var response = categories.Select(category => new CategoryResponse(
                    category.Id.Value, 
                    category.Name,
                    category.Description)).ToList();

                return Result.Success(new GetCategoriesResult(new PaginatedResult<CategoryResponse>(response, response.Count(), request.PageIndex, request.PageSize)));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private Expression<Func<Category, bool>> BuildFilterExpression(string filter)
        {
            return category => category.Name.Contains(filter) ||
                              category.Description.Contains(filter);
        }

        private Expression<Func<Category, bool>> CombineFilters(Expression<Func<Category, bool>> first, Expression<Func<Category, bool>> second)
        {
            var parameter = Expression.Parameter(typeof(Category));

            var combinedBody = Expression.AndAlso(
                Expression.Invoke(first, parameter),
                Expression.Invoke(second, parameter)
            );

            return Expression.Lambda<Func<Category, bool>>(combinedBody, parameter);
        }
    }
}
