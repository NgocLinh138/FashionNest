using FashionNest.Application.Common.Messaging;
using FashionNest.Application.Features.Categories.Responses;
using FashionNest.Domain.Common;
using FashionNest.Domain.Common.Repositories;
using FashionNest.Domain.Constants;

namespace FashionNest.Application.Features.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryCommandHandler : ICommandHandler<UpdateCategoryCommand, UpdateCategoryResult>
    {
        private readonly ICategoryRepository categoryRepository;

        public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public async Task<Result<UpdateCategoryResult>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await categoryRepository.GetByIdAsync(CategoryId.Of(request.Id));
            if (category == null)
                return Result.Failure<UpdateCategoryResult>(CategoryError.NotFound);

            var existCategory = await categoryRepository.GetAsync(x => x.Name == request.Name);
            if (existCategory.Any())
                return Result.Failure<UpdateCategoryResult>(CategoryError.DuplicateName);

            try
            {
                category.Update(request.Name, request.Description);
                category.UpdateTimestamp();

                await categoryRepository.UpdateAsync(category);
                await categoryRepository.SaveAsync();

                return Result.Success(new UpdateCategoryResult(
                    new CategoryResponse(
                        category.Id.Value,
                        category.Name,
                        category.Description)));
            }
            catch (Exception ex)
            {
                return Result.Failure<UpdateCategoryResult>(new Error("An error occurred while updating the category", ex.Message));
            }
        }
    }
}
