namespace FashionNest.Infrastructure.VnPay.Repository.Interface
{
    public interface IVnPayRepository
    {
        public string CreatePaymentUrl(decimal amount, string orderInfo, string txnRef, string ipAddress);
    }
}
