using FashionNest.Application.Features.Roles.Commands.UpdateRole;
using FluentValidation;

namespace FashionNest.Application.Features.Roles.Validators
{
    public class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
    {
        public UpdateRoleCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Role name is required.");
        }
    }
}
