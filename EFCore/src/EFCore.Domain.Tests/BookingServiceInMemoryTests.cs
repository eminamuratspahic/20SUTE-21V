using System;
using FakeItEasy;
using Xunit;
using FluentAssertions;


namespace EFCore.Domain.Tests
{
    public class BookingServiceInMemoryTests : IClassFixture<BookingContextInMemoryFixture>
    {
        BookingContext context;

        public BookingServiceInMemoryTests(BookingContextInMemoryFixture fixture)
        {
            this.context = fixture.Context;
        }

        [Fact]
        public void Create_booking_efcore()
        {
            // arrange
            var id = Guid.NewGuid().ToString();
            var repository = new BookingRepositoryEFCore(context);
            var paymentGateway = A.Fake<IPaymentGateway>();
            var priceCalculator = A.Fake<IPriceCalculator>();

            var sut = new BookingService(repository, paymentGateway, priceCalculator);

            // stub, set expectation of success
            A.CallTo(() => paymentGateway.CapturePayment(A<decimal>.Ignored)).Returns(true);

            // act
            sut.CreateBooking(new BookingService.CreateBookingRequest(
                id: id,
                requestedBy: "a.user",
                date: DateTime.Now));

            var newBooking = repository.GetById(id);

            // assert
            newBooking.Should().NotBeNull();
        }
    }
}
