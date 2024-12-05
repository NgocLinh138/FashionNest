using FashionNest.Application.Features.Roles.Commands.CreateRole;
using FluentValidation;

namespace FashionNest.Application.Features.Roles.Validators
{
    public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
    {
        public CreateRoleCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Role name is required.");
        }
    } 
}
