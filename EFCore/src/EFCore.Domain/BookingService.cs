using System;
using System.Linq;

namespace EFCore.Domain
{
    public class BookingService
    {
        private BookingRepositoryEFCore repository;
        private readonly IPaymentGateway paymentGateway;
        private readonly IPriceCalculator priceCalculator;

        public BookingService(BookingRepositoryEFCore repository, IPaymentGateway paymentGateway, IPriceCalculator priceCalculator)
        {
            this.repository = repository;
            this.paymentGateway = paymentGateway;
            this.priceCalculator = priceCalculator;
        }

        // create a new booking and persist it to our fake database.
        public void CreateBooking(CreateBookingRequest request)
        {
            var totalAmount = priceCalculator.GetPriceForBookingWith(1, 2);

            var capturePaymentWasSuccessful = paymentGateway.CapturePayment(totalAmount);

            if (capturePaymentWasSuccessful)
            {
                repository.AddBooking(new Booking(request.Id, request.Date));
            }
        }

        public Booking GetBooking(string id)
        {
            return new Booking(id, DateTime.UtcNow);
        }

        // Input parameters to method CreateBooking
        // data transfer object (dto) that collects
        // all the required input to the booking method
        public class CreateBookingRequest
        {
            public string Id { get; }
            public string RequestedBy { get; }
            public DateTime Date { get; }
            public CreateBookingRequest(string id, string requestedBy, DateTime date)
            {
                this.Id = id;
                this.RequestedBy = requestedBy;
                this.Date = date;
            }
        }
    }
}
