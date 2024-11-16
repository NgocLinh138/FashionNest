using FashionNest.Application.Common.Messaging;
using FashionNest.Application.Features.Orders.Responses;
using FashionNest.Domain.Common;
using FashionNest.Domain.Common.Repositories;
using FashionNest.Domain.Constants;

namespace FashionNest.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommandHandler : ICommandHandler<UpdateOrderCommand, UpdateOrderResult>
    {
        private readonly IOrderRepository orderRepository;
        private readonly IPaymentRepository paymentRepository;

        public UpdateOrderCommandHandler(IOrderRepository orderRepository, IPaymentRepository paymentRepository)
        {
            this.orderRepository = orderRepository;
            this.paymentRepository = paymentRepository;
        }

        public async Task<Result<UpdateOrderResult>> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await orderRepository.GetByIdAsync(OrderId.Of(request.OrderId));
            if (order == null)
                return Result.Failure<UpdateOrderResult>(OrderError.NotFound);

            order.Status = request.Status;
            await orderRepository.UpdateAsync(order);
            await orderRepository.SaveAsync();

            var response = new UpdateOrderResult(new OrderResponse(
                order.Id.Value,
                order.UserId.Value,
                order.PaymentId?.Value,
                order.Status,
                order.OrderDate,
                order.CouponId?.Value,
                order.TotalPrice));

            return Result.Success(response);
        }
    }
}
