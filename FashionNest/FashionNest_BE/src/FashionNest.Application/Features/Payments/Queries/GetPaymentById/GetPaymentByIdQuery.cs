using FashionNest.Application.Common.Messaging;
using FashionNest.Application.Features.Payments.Responses;

namespace FashionNest.Application.Features.Payments.Queries.GetPaymentById
{
    public record GetPaymentByIdQuery(Guid Id) : IQuery<GetPaymentByIdResult>;

    public record GetPaymentByIdResult(PaymentResponse Payment);
}
