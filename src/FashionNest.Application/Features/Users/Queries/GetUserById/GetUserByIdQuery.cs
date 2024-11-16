using FashionNest.Application.Common.Messaging;
using FashionNest.Application.Features.Users.Responses;

namespace FashionNest.Application.Features.Users.Queries.GetUserById
{
    public record GetUserByIdQuery(Guid Id) : IQuery<GetUserByIdResult>;

    public record GetUserByIdResult(UserResponse User);
}
