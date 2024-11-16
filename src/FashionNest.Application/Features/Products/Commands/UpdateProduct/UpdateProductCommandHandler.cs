using FashionNest.Application.Features.Products.Responses;
using FashionNest.Application.Features.Categories.Responses;
using FashionNest.Application.Common.Messaging;
using FashionNest.Domain.Common.Repositories;
using FashionNest.Domain.Common;
using FashionNest.Domain.Constants;

namespace FashionNest.Application.Features.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand, UpdateProductResult>
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IProductRepository productRepository;

        public UpdateProductCommandHandler(ICategoryRepository categoryRepository, IProductRepository productRepository)
        {
            this.categoryRepository = categoryRepository;
            this.productRepository = productRepository;
        }
        public async Task<Result<UpdateProductResult>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await productRepository.GetByIdAsync(ProductId.Of(request.Id));
            if (product == null)
                return Result.Failure<UpdateProductResult>(ProductError.NotFound);

            var category = await categoryRepository.GetByIdAsync(CategoryId.Of(request.CategoryId));
            if (category == null)
                return Result.Failure<UpdateProductResult>(CategoryError.NotFound);

            try
            {
                product.Update(request.Name, request.Description, request.Price, request.Stock, request.Image, request.IsRental, CategoryId.Of(request.CategoryId));
                await productRepository.UpdateAsync(product);
                await productRepository.SaveAsync();

                var response = new ProductResponse(
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
                       Description: category.Description));

                return Result.Success(new UpdateProductResult(response));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
