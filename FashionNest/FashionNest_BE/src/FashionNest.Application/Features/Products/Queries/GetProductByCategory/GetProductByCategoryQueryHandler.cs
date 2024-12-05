using FashionNest.Application.Features.Products.Responses;
using FashionNest.Application.Features.Categories.Responses;
using FashionNest.Application.Common.Messaging;
using FashionNest.Domain.Common.Repositories;
using FashionNest.Domain.Common;
using FashionNest.Domain.Constants;

namespace FashionNest.Application.Features.Products.Queries.GetProductByCategory
{
    public class GetProductByCategoryQueryHandler : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryResult>
    {
        private readonly IProductRepository productRepository;
        private readonly ICategoryRepository categoryRepository;

        public GetProductByCategoryQueryHandler(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            this.productRepository = productRepository;
            this.categoryRepository = categoryRepository;
        }
        public async Task<Result<GetProductByCategoryResult>> Handle(GetProductByCategoryQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var category = await categoryRepository.GetByNameAsync(request.CategoryName);
                if (category == null)
                    return Result.Failure<GetProductByCategoryResult>(CategoryError.NotFound); 

                var products = await productRepository.GetByCategoryIdAsync(category.Id);

                var response = products.Select(product => new ProductResponse(
                   Id: product.Id.Value,
                   Name: product.Name,
                   Description: product.Description,
                   Price: product.Price,
                   Stock: product.Stock,
                   Image: product.Image,
                   IsRental: product.IsRental,
                   Category: new CategoryResponse(
                       Id: category.Id.Value,
                       Name: category.Name,
                       Description: category.Description))
                   ).ToList();

                return Result.Success(new GetProductByCategoryResult(response));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
