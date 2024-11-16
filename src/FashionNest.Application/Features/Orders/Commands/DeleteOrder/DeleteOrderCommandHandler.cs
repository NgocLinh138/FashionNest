using FashionNest.Application.Common.Messaging;
using FashionNest.Domain.Common;
using FashionNest.Domain.Common.Repositories;
using FashionNest.Domain.Constants;

namespace FashionNest.Application.Features.Orders.Commands.DeleteOrder
{
    public class DeleteOrderCommandHandler : ICommandHandler<DeleteOrderCommand, Guid>
    {
        private readonly IOrderRepository orderRepository;

        public DeleteOrderCommandHandler(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        public async Task<Result<Guid>> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await orderRepository.GetByIdAsync(OrderId.Of(request.Id));
            if (order == null)
                return Result.Failure<Guid>(OrderError.NotFound);

            try
            {
                order.SoftDelete();
                await orderRepository.SaveAsync();

                return Result.Success(order.Id.Value);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
