namespace FashionNest.Application.Features.Users.Responses
{
    public record UserResponse(
        Guid Id,
        string Name,
        string Email,
        string PhoneNumber,
        string Address,
        bool IsActive,
        UserRoleResponse Role);

    public record UserRoleResponse(
        Guid Id,
        string Name);

    public record UserGetAllResponse(
        Guid Id,
        string Name,
        string Email,
        string PhoneNumber,
        string Address,
        bool IsActive);
}
