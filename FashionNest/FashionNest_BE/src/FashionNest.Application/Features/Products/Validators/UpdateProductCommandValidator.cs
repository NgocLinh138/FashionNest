using FashionNest.Application.Features.Products.Commands.UpdateProduct;
using FluentValidation;

namespace FashionNest.Application.Features.Products.Validators
{
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.Name)
                .Empty().WithMessage("Name is required.");

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Price must be greater than or equal to 0.");

            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0).WithMessage("Stock must be greater than or equal to 0.");
        }
    }
}
