using FashionNest.Application.Features.Users.Queries.GetUsers;
using FluentValidation;

namespace FashionNest.Application.Features.Users.Validators
{
    public class GetUsersQueryValidator : AbstractValidator<GetUsersQuery>
    {
        private int[] allowPageSize = [5, 10, 15, 30];

        public GetUsersQueryValidator()
        {
            RuleFor(x => x.PageIndex).GreaterThanOrEqualTo(1)
                .WithMessage("Page index must be greater than or equal to 1.");

            RuleFor(x => x.PageSize).Must(x => allowPageSize.Contains(x))
                .WithMessage($"Page size must be in [{string.Join(",", allowPageSize)}]");
        }
    }
}
