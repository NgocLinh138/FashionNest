using FashionNest.Application.Common.Messaging;
using FashionNest.Application.Features.Payments.Responses;
using FashionNest.Domain.Common;
using FashionNest.Domain.Common.Repositories;
using FashionNest.Domain.Enums;

namespace FashionNest.Application.Features.Payments.Queries.GetPayments
{
    public class GetPaymentsQueryHandler : IQueryHandler<GetPaymentsQuery, GetPaymentsResult>
    {
        private readonly IPaymentRepository paymentRepository;

        public GetPaymentsQueryHandler(IPaymentRepository paymentRepository)
        {
            this.paymentRepository = paymentRepository;
        }

        public async Task<Result<GetPaymentsResult>> Handle(GetPaymentsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var payments = await paymentRepository.GetAsync(
                    orderByAsc: true,
                    pageIndex: request.PageIndex,
                    pageSize: request.PageSize);

                var response = payments.Select(payment => new PaymentResponse(
                    payment.Id.Value,
                    payment.UserId.Value,
                    payment.Method ?? PaymentMethod.None,
                    payment.Status ?? PaymentStatus.None,
                    payment.Amount,
                    payment.Description ?? string.Empty,
                    payment.PaymentDate.ToString("o"))).ToList();

                return Result.Success(new GetPaymentsResult(new PaginatedResult<PaymentResponse>(response, response.Count, request.PageIndex, request.PageSize)));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
