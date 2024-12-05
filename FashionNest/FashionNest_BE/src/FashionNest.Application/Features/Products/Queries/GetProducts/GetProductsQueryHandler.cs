using AutoMapper;
using FashionNest.Application.Common.Messaging;
using FashionNest.Application.Features.Products.Responses;
using FashionNest.Domain.Common;
using FashionNest.Domain.Common.Repositories;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace FashionNest.Application.Features.Products.Queries.GetProducts
{
    public class GetProductsQueryHandler : IQueryHandler<GetProductsQuery, GetProductsResult>
    {
        private readonly IProductRepository productRepository;
        private readonly ILogger<GetProductsQueryHandler> logger;
        private readonly IMapper mapper;

        public GetProductsQueryHandler(IProductRepository productRepository, ILogger<GetProductsQueryHandler> logger, IMapper mapper)
        {
            this.productRepository = productRepository;
            this.logger = logger;
            this.mapper = mapper;
        }

        public async Task<Result<GetProductsResult>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Get all products {Product}", request);

                Expression<Func<Product, bool>> filter = product => true;

                if (!string.IsNullOrWhiteSpace(request.Filter))
                {
                    var filterExpression = BuildFilterExpression(request.Filter);
                    filter = CombineFilters(filter, filterExpression);
                }

                Expression<Func<Product, object>> sortBy = request.SortBy switch
                {
                    "Name" => product => product.Name,
                    "Price" => product => product.Price,
                    "Category" => product => product.Category.Name,
                    _ => product => product.Name // Default sort by Name
                };

                var products = await productRepository.GetAsync(
                    filter: filter,
                    orderBy: sortBy,
                    orderByAsc: request.SortOrderAscending,
                    includeProperties: "Category",
                    pageIndex: request.PageIndex,
                    pageSize: request.PageSize);

                var response = mapper.Map<List<ProductResponse>>(products);

                return Result.Success(new GetProductsResult(new PaginatedResult<ProductResponse>(response, response.Count(), request.PageIndex, request.PageSize)));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private Expression<Func<Product, bool>> BuildFilterExpression(string filter)
        {
            return product => product.Name.Contains(filter) ||
                              product.Description.Contains(filter) ||
                              product.Category.Name.Contains(filter);
        }

        private Expression<Func<Product, bool>> CombineFilters(Expression<Func<Product, bool>> first, Expression<Func<Product, bool>> second)
        {
            var parameter = Expression.Parameter(typeof(Product));

            var combinedBody = Expression.AndAlso(
                Expression.Invoke(first, parameter),
                Expression.Invoke(second, parameter)
            );

            return Expression.Lambda<Func<Product, bool>>(combinedBody, parameter);
        }
    }
}
