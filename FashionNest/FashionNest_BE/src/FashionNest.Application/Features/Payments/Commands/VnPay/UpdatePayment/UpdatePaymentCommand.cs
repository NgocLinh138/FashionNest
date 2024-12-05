using FashionNest.Application.Common.Messaging;
using FashionNest.Application.Features.Payments.Responses;
using FashionNest.Domain.Enums;

namespace FashionNest.Application.Features.Payments.Commands.VnPay.UpdatePayment
{
    public record UpdatePaymentCommand(
       Guid Id,
       Guid OrderId,
       PaymentStatus Status) : ICommand<UpdatePaymentResult>;

    public record UpdatePaymentResult(PaymentResponse Payment);
}
