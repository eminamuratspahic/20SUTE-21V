namespace EFCore.Domain
{
    public interface IPaymentGateway
    {
        bool CapturePayment(decimal totalAmount);
        bool RefundPayment(decimal totalAmount);
    }
}