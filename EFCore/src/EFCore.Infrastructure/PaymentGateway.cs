using EFCore.Domain;

namespace EFCore.Infrastructure
{
    public class PaymentGateway : IPaymentGateway
    {
        public PaymentGateway()
        {
        }

        /// <summary>
        /// Always return true.
        /// </summary>
        /// <param name="totalAmount"></param>
        /// <returns></returns>
        public bool CapturePayment(decimal totalAmount)
        {
            return true;
        }

        /// <summary>
        /// Always return true;
        /// </summary>
        /// <param name="totalAmount"></param>
        /// <returns></returns>
        public bool RefundPayment(decimal totalAmount)
        {
            return true;
        }
    }
}
