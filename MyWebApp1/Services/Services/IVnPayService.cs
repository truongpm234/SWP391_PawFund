using MyWebApp1.Models;

namespace MyWebApp1.Services.Services
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(HttpContent content, VnPaymentRequestModel model);
        VnPaymentRequestModel PaymentExecute(IQueryCollection collections);
    }
}
