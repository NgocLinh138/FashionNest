using FashionNest.Application.Common.Messaging;
using FashionNest.Application.Features.Categories.Responses;
using FashionNest.Application.Features.Products.Responses;
using FashionNest.Domain.Common;
using FashionNest.Domain.Common.Repositories;
using FashionNest.Domain.Constants;

namespace FashionNest.Application.Features.Products.Queries.GetProductById
{
    public class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
    {
        private readonly IProductRepository productRepository;

        public GetProductByIdQueryHandler(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public async Task<Result<GetProductByIdResult>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await productRepository.GetByIdAsync(ProductId.Of(request.Id));
            if (product == null)
                return Result.Failure<GetProductByIdResult>(ProductError.NotFound);

            var categoryId = product.CategoryId;
            var category = await productRepository.GetCategoryByIdAsync(CategoryId.Of(categoryId.Value));
            if (category == null)
                return Result.Failure<GetProductByIdResult>(CategoryError.NotFound);

            var response = new GetProductByIdResult(new ProductResponse(
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
                     Description: category.Description)));

            return Result.Success(response);
        }
    }
}
