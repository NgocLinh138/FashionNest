using AutoMapper;
using FashionNest.Application.Common.Messaging;
using FashionNest.Application.Features.Products.Responses;
using FashionNest.Domain.Common;
using FashionNest.Domain.Common.Repositories;
using Microsoft.Extensions.Logging;

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

                var products = await productRepository.GetAsync(
                    orderByAsc: true,
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
    }
}
