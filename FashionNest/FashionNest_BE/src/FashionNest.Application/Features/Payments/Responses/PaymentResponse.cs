using FashionNest.Domain.Enums;

namespace FashionNest.Application.Features.Payments.Responses
{
    public record PaymentResponse(
        Guid Id,
        Guid UserId,
        PaymentMethod Method,
        PaymentStatus Status,
        decimal Amount,
        string? Description,
        string PaymentDate);

    public record CreatePaymentVnPayResponse(
        Guid Id,
        Guid UserId,
        PaymentMethod Method,
        string PaymentUrl);
}
