using System.Linq;

namespace EFCore.Domain
{
    // Managed dependency
    public class BookingRepositoryEFCore
    {
        // DbContext is a 'Unit of Work'
        readonly BookingContext context;

        public BookingRepositoryEFCore(BookingContext context)
        {
            this.context = context;
        }

        public void AddBooking(Booking bookingToAdd)
        {
            this.context.Bookings.Add(bookingToAdd);

            // must call SaveChanges to persist the booking.
            this.context.SaveChanges();
        }

        public Booking GetById(string id)
        {
            return this.context.Bookings.FirstOrDefault(s => s.Id == id);
        }
    }
}
