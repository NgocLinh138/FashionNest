using FashionNest.Application.Common.Messaging;
using FashionNest.Application.Features.Coupons.Responses;
using FashionNest.Application.Features.Orders.Responses;
using FashionNest.Domain.Common;
using FashionNest.Domain.Common.Repositories;

namespace FashionNest.Application.Features.Orders.Queries.GetOrders
{
    public class GetOrdersQueryHandler : IQueryHandler<GetOrdersQuery, GetOrdersResult>
    {
        private readonly IOrderRepository orderRepository;
        private readonly ICouponRepository couponRepository;
        private readonly IUserRepository userRepository;

        public GetOrdersQueryHandler(IOrderRepository orderRepository, ICouponRepository couponRepository, IUserRepository userRepository)
        {
            this.orderRepository = orderRepository;
            this.couponRepository = couponRepository;
            this.userRepository = userRepository;
        }

        public async Task<Result<GetOrdersResult>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var orders = await orderRepository.GetAsync(
                    orderByAsc: true,
                    pageIndex: request.PageIndex,
                    pageSize: request.PageSize);
                var activeOrders = orders.Where(order => !order.IsDeleted).ToList();

                if (request.UserId != null)
                {
                    activeOrders = activeOrders.Where(order => order.UserId != null && order.UserId.Value == request.UserId.Value).ToList();
                }

                var orderResponses = new List<OrderResponse>();

                foreach (var order in activeOrders)
                {
                    CouponResponse? couponResponse = null;
                    if (order.CouponId != null)
                    {
                        var coupon = await couponRepository.GetByIdAsync(CouponId.Of(order.CouponId.Value));
                        if (coupon != null)
                        {
                            couponResponse = new CouponResponse(
                                id: coupon.Id.Value,
                                code: coupon.Code,
                                discountAmount: coupon.DiscountAmount,
                                expirationDate: coupon.ExpirationDate,
                                isActive: coupon.IsActive,
                                description: coupon.Description
                            );
                        }
                    }

                    var orderResponse = new OrderResponse(
                        id: order.Id.Value,
                        userId: order.UserId.Value,
                        paymentId: order.PaymentId?.Value ?? Guid.Empty,
                        status: order.Status,
                        orderDate: order.OrderDate,
                        couponId: order.CouponId?.Value,
                        totalPrice: order.TotalPrice,
                        coupon: couponResponse 
                    );

                    orderResponses.Add(orderResponse);
                }

                var result = new PaginatedResult<OrderResponse>(
                    orderResponses,
                    orderResponses.Count,
                    request.PageIndex,
                    request.PageSize);

                return Result.Success(new GetOrdersResult(result));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
