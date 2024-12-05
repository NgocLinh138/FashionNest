using FashionNest.Application.Common.Messaging;
using FashionNest.Application.Features.Coupons.Responses;

namespace FashionNest.Application.Features.Coupons.Commands.DeleteCoupon
{
    public record DeleteCouponCommand(Guid Id) : ICommand<Guid>;
}
