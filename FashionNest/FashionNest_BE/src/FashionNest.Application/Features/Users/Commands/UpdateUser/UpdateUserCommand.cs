using FashionNest.Application.Common.Messaging;
using FashionNest.Application.Features.Users.Responses;

namespace FashionNest.Application.Features.Users.Commands.UpdateUser
{
    public record UpdateUserCommand(
         Guid UserId,
         string? Name,          
         string? Password,     
         string? ConfirmPassword, 
         string? PhoneNumber,   
         string? Address        
     ) : ICommand<UpdateUserResult>;


    public record UpdateUserResult(UserResponse User);
}
