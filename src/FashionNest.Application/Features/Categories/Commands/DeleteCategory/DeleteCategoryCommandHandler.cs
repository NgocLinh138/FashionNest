using FashionNest.Application.Common.Messaging;
using FashionNest.Domain.Common;
using FashionNest.Domain.Common.Repositories;
using FashionNest.Domain.Constants;

namespace FashionNest.Application.Features.Categories.Commands.DeleteCategory
{
    public class DeleteCategoryCommandHandler : ICommandHandler<DeleteCategoryCommand, Guid>
    {
        private readonly ICategoryRepository categoryRepository;

        public DeleteCategoryCommandHandler(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public async Task<Result<Guid>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await categoryRepository.GetByIdAsync(CategoryId.Of(request.Id));
            if (category == null)
                return Result.Failure<Guid>(CategoryError.NotFound);

            try
            {
                await categoryRepository.DeleteAsync(category);
                await categoryRepository.SaveAsync();

                return Result.Success(category.Id.Value);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}