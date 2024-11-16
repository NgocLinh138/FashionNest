using FashionNest.Application.Features.Auth.Queries.Login;
using FluentValidation;

namespace FashionNest.Application.Features.Auth.Validators
{
    public class LoginUserQueryValidator : AbstractValidator<LoginUserQuery>
    {
        public LoginUserQueryValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email format is not valid.");
        }
    }
}
