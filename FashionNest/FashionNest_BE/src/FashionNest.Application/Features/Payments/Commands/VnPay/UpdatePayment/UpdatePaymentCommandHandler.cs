using FashionNest.Application.Common.Messaging;
using FashionNest.Application.Features.Payments.Responses;
using FashionNest.Domain.Common;
using FashionNest.Domain.Common.Repositories;
using FashionNest.Domain.Constants;
using FashionNest.Domain.Enums;

namespace FashionNest.Application.Features.Payments.Commands.VnPay.UpdatePayment
{
    public class UpdatePaymentCommandHandler : ICommandHandler<UpdatePaymentCommand, UpdatePaymentResult>
    {
        private readonly IPaymentRepository paymentRepository;
        private readonly IOrderRepository orderRepository;

        public UpdatePaymentCommandHandler(
            IPaymentRepository paymentRepository, 
            IOrderRepository orderRepository)
        {
            this.paymentRepository = paymentRepository;
            this.orderRepository = orderRepository;
        }

        public async Task<Result<UpdatePaymentResult>> Handle(UpdatePaymentCommand request, CancellationToken cancellationToken)
        {
            var payment = await paymentRepository.GetByIdAsync(PaymentId.Of(request.Id));
            if (payment == null)
                return Result.Failure<UpdatePaymentResult>(PaymentError.NotFound);

            if (payment.Status != PaymentStatus.Pending)
                return Result.Failure<UpdatePaymentResult>(PaymentError.PaymentNotPending);

            try
            {
                payment.Status = request.Status;

                await paymentRepository.UpdateAsync(payment);
                await paymentRepository.SaveAsync();

                if (request.Status == PaymentStatus.Completed)
                {
                    var order = await orderRepository.GetByIdAsync(OrderId.Of(request.OrderId)); 
                    if (order != null)
                    {
                        order.PaymentId = payment.Id;
                        await orderRepository.UpdateAsync(order);
                        await orderRepository.SaveAsync();
                    }
                }

                var response = new UpdatePaymentResult(new PaymentResponse(
                    Id: payment.Id.Value,
                    UserId: payment.UserId.Value,
                    Method: payment.Method!.Value,
                    Status: payment.Status.Value,
                    Amount: payment.Amount,
                    Description: payment.Description,
                    PaymentDate: payment.PaymentDate.ToString("o")));

                return Result.Success(response);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
