using FashionNest.Application.Common.Messaging;
using FashionNest.Application.Features.Categories.Responses;
using FashionNest.Application.Features.Products.Responses;
using FashionNest.Domain.Common;
using FashionNest.Domain.Common.Repositories;
using FashionNest.Domain.Constants;

namespace FashionNest.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IProductRepository productRepository;

        public CreateProductCommandHandler(ICategoryRepository categoryRepository, IProductRepository productRepository)
        {
            this.categoryRepository = categoryRepository;
            this.productRepository = productRepository;
        }

        public async Task<Result<CreateProductResult>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var category = await categoryRepository.GetByIdAsync(CategoryId.Of(request.CategoryId));
            if (category == null)
                return Result.Failure<CreateProductResult>(CategoryError.NotFound);

            try
            {
                var product = new Product(
                    name: request.Name,
                    description: request.Description,
                    price: request.Price,
                    stock: request.Stock,
                    image: request.Image,
                    isRental: request.IsRental,
                    categoryId: CategoryId.Of(request.CategoryId));

                await productRepository.InsertAsync(product);
                await productRepository.SaveAsync();

                var productResponse = new ProductResponse(
                      Id: product.Id.Value,
                      Name: product.Name,
                      Description: product.Description,
                      Price: product.Price,
                      Stock: product.Stock,
                      Image: product.Image,
                      IsRental: product.IsRental,
                      Category: new CategoryResponse(
                          category.Id.Value,
                          category.Name,
                          category.Description));

                return Result.Success(new CreateProductResult(productResponse));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
