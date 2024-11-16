using FashionNest.Application.Features.Payments.Commands.VnPay.CreatePaymentVnPay;
using FluentValidation;

namespace FashionNest.Application.Features.Payments.Validators
{
    public class CreatePaymentVnPayCommandValidator : AbstractValidator<CreatePaymentVnPayCommand>
    {
        public CreatePaymentVnPayCommandValidator()
        {
            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Amount must not be less than 0.");
        }
    }
}
