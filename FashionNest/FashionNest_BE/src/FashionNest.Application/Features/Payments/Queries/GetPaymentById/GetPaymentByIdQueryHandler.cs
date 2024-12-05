using AutoMapper;
using FashionNest.Application.Common.Messaging;
using FashionNest.Application.Features.Payments.Responses;
using FashionNest.Domain.Common;
using FashionNest.Domain.Common.Repositories;
using FashionNest.Domain.Constants;

namespace FashionNest.Application.Features.Payments.Queries.GetPaymentById
{
    public class GetPaymentByIdQueryHandler : IQueryHandler<GetPaymentByIdQuery, GetPaymentByIdResult>
    {
        private readonly IPaymentRepository paymentRepository;
        private readonly IMapper mapper;

        public GetPaymentByIdQueryHandler(IPaymentRepository paymentRepository, IMapper mapper)
        {
            this.paymentRepository = paymentRepository;
            this.mapper = mapper;
        }

        public async Task<Result<GetPaymentByIdResult>> Handle(GetPaymentByIdQuery request, CancellationToken cancellationToken)
        {
            var payment = await paymentRepository.GetByIdAsync(PaymentId.Of(request.Id));
            if (payment == null)
                return Result.Failure<GetPaymentByIdResult>(PaymentError.NotFound);

            var response = mapper.Map<PaymentResponse>(payment);

            return Result.Success(new GetPaymentByIdResult(response));
        }
    }
}
