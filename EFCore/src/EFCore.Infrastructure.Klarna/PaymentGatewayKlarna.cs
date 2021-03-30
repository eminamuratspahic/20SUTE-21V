using System;
using EFCore.Domain;

namespace EFCore.Infrastructure.Klarna
{
    public class PaymentGatewayKlarna : IPaymentGateway
    {
        public bool CapturePayment(decimal totalAmount)
        {
            // talk to klarna
            return false;
        }

        public bool RefundPayment(decimal totalAmount)
        {
            // talk to klarna
            return false;
        }
    }
}
