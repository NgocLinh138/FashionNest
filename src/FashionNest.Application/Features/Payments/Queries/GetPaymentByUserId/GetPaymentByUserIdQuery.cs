using FashionNest.Application.Common.Messaging;
using FashionNest.Application.Features.Payments.Responses;

namespace FashionNest.Application.Features.Payments.Queries.GetPaymentByUserId
{
    public record GetPaymentByUserIdQuery(Guid UserId) : IQuery<GetPaymentByUserIdResult>;

    public record GetPaymentByUserIdResult(IEnumerable<PaymentResponse> Payments);
}
