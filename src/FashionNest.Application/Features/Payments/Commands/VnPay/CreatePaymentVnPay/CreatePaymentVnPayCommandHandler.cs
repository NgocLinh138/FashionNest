using FashionNest.Application.Common.Exceptions;
using FashionNest.Application.Common.Messaging;
using FashionNest.Application.Features.Payments.Responses;
using FashionNest.Domain.Common;
using FashionNest.Domain.Common.Repositories;
using FashionNest.Domain.Entities;
using FashionNest.Domain.Enums;
using FashionNest.Infrastructure.VnPay.Repository.Interface;

namespace FashionNest.Application.Features.Payments.Commands.VnPay.CreatePaymentVnPay
{
    public class CreatePaymentVnPayCommandHandler : ICommandHandler<CreatePaymentVnPayCommand, CreatePaymentVnPayResult>
    {
        private readonly IPaymentRepository paymentRepository;
        private readonly IVnPayRepository vnPayRepository;
        private readonly IOrderRepository orderRepository;

        public CreatePaymentVnPayCommandHandler(
            IPaymentRepository paymentRepository, 
            IVnPayRepository vnPayRepository,
            IOrderRepository orderRepository)
        {
            this.paymentRepository = paymentRepository;
            this.vnPayRepository = vnPayRepository;
            this.orderRepository = orderRepository;
        }

        public async Task<Result<CreatePaymentVnPayResult>> Handle(CreatePaymentVnPayCommand request, CancellationToken cancellationToken)
        {
            var payment = new Payment(
                UserId.Of(request.UserId),
                PaymentMethod.VNPay,
                request.Amount,
                "Payment for order",
                DateTime.UtcNow,
                PaymentStatus.Pending
            );

            await paymentRepository.InsertAsync(payment);
            await paymentRepository.SaveAsync();

            var paymentUrl = vnPayRepository.CreatePaymentUrl(
                request.Amount,
                payment.Description,
                payment.Id.Value.ToString(),
                "127.0.0.1");

            if (paymentUrl == null)
                return Result.Failure<CreatePaymentVnPayResult>(Error.PaymentUrlGenerationFailed);

            var paymentResponse = new CreatePaymentVnPayResponse(
               payment.Id.Value,
               payment.UserId.Value,
               payment.Method.Value,
               paymentUrl);

            return Result.Success(new CreatePaymentVnPayResult(paymentResponse));
        }
    }
}
