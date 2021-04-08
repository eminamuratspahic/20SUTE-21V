using Microsoft.EntityFrameworkCore;

namespace EFCore.Domain
{
    // Static class for extension methods
    public static class BookingConfig
    {
        public static void AddBooking(this ModelBuilder modelBuilder, string schema)
        {
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.ToTable("Bookings", schema);

                entity.HasKey(p => p.Id);
            });
        }
    }
}
