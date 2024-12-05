using FashionNest.Application.Common.Messaging;
using FashionNest.Application.Features.Payments.Responses;

namespace FashionNest.Application.Features.Payments.Commands.VnPay.CreatePaymentVnPay
{
    public record CreatePaymentVnPayCommand(
        Guid UserId,
        Guid OrderId,
        decimal Amount) : ICommand<CreatePaymentVnPayResult>;

    public record CreatePaymentVnPayResult(CreatePaymentVnPayResponse Payment);
}
