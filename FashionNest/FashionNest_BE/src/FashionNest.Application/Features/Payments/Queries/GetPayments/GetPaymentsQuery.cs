using FashionNest.Application.Common.Messaging;
using FashionNest.Application.Features.Payments.Responses;

namespace FashionNest.Application.Features.Payments.Queries.GetPayments
{
    public record GetPaymentsQuery(
        int PageIndex,
        int PageSize)
        : IQuery<GetPaymentsResult>;

    public record GetPaymentsResult(PaginatedResult<PaymentResponse> Payments);
}
