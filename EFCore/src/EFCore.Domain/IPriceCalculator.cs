namespace EFCore.Domain
{
    public interface IPriceCalculator
    {
        decimal GetPriceForBookingWith(int userId, int locationId);
    }
}
