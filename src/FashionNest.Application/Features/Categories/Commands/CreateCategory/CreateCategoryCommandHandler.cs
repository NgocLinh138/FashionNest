using FashionNest.Application.Common.Messaging;
using FashionNest.Application.Features.Categories.Responses;
using FashionNest.Domain.Common;
using FashionNest.Domain.Common.Repositories;

namespace FashionNest.Application.Features.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommandHandler : ICommandHandler<CreateCategoryCommand, CreateCategoryResult>
    {
        private readonly ICategoryRepository categoryRepository;

        public CreateCategoryCommandHandler(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public async Task<Result<CreateCategoryResult>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var category = new Category(
                    name: request.Name,
                    description: request.Description);

                await categoryRepository.InsertAsync(category);
                await categoryRepository.SaveAsync();

                return Result.Success(new CreateCategoryResult(
                    new CategoryResponse(
                        category.Id.Value,
                        category.Name,
                        category.Description)));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

       
    }
}
