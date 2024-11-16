using FashionNest.Application.Common.Messaging;
using FashionNest.Application.Features.Payments.Responses;
using FashionNest.Domain.Common;
using FashionNest.Domain.Common.Repositories;
using FashionNest.Domain.Constants;
using FashionNest.Domain.Enums;

namespace FashionNest.Application.Features.Payments.Queries.GetPaymentByUserId
{
    public class GetPaymentByUserIdQueryHandler : IQueryHandler<GetPaymentByUserIdQuery, GetPaymentByUserIdResult>
    {
        private readonly IPaymentRepository paymentRepository;

        public GetPaymentByUserIdQueryHandler(IPaymentRepository paymentRepository)
        {
            this.paymentRepository = paymentRepository;
        }

        public async Task<Result<GetPaymentByUserIdResult>> Handle(GetPaymentByUserIdQuery request, CancellationToken cancellationToken)
        {
            var payments = await paymentRepository.GetAsync(x => x.UserId == new UserId(request.UserId));
            if (payments == null || !payments.Any())
                return Result.Failure<GetPaymentByUserIdResult>(PaymentError.NotFound);

            var paymentList = payments.ToList();

            var response = paymentList.Select(payment => new PaymentResponse(
                payment.Id.Value,
                payment.UserId.Value,
                payment.Method ?? PaymentMethod.None, 
                payment.Status ?? PaymentStatus.None,
                payment.Amount,
                payment.Description ?? string.Empty,
                payment.PaymentDate.ToString("o"))).ToList();

            return Result.Success(new GetPaymentByUserIdResult(response));
        }
    }
}
