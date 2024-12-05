using FashionNest.Infrastructure.VnPay.DependencyInjection.Options.Config;
using FashionNest.Infrastructure.VnPay.DependencyInjection.Options.Request;
using FashionNest.Infrastructure.VnPay.Repository.Interface;
using Microsoft.Extensions.Options;

namespace FashionNest.Infrastructure.VnPay.Repository
{
    public class VnPayRepository : IVnPayRepository
    {
        private readonly VnPayConfig _config;
        private readonly IOptions<VnPayConfig> configOptions;

        public VnPayRepository(IOptions<VnPayConfig> configOptions)
        {
            _config = configOptions?.Value ?? throw new ArgumentNullException(nameof(configOptions));
        }

        public string CreatePaymentUrl(decimal amount, string orderInfo, string txnRef, string ipAddress)
        {
            var createDate = DateTime.UtcNow;

            var vnpayRequest = new VnPayRequest(
                _config.Version,
                _config.TmnCode,
                createDate,
                ipAddress,
                amount,
                "VND",
                "other",
                orderInfo,
                _config.ReturnUrl,
                txnRef
            );

            var paymentUrl = vnpayRequest.GetLink(_config.PaymentUrl, _config.HashSecret);
            return paymentUrl;
        }
    }
}
