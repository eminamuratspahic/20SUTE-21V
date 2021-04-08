namespace EFCore.Domain
{
    public interface IRefundGateway
    {
        void RefundPayment(decimal amountToRefund);
    }
}
