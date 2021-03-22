using System;
using Xunit;
using FluentAssertions;
using NodaTime;

namespace Lab02.Domain.Tests
{
    public class BookingTests
    {
        [Fact]
        public void Booking_has_a_start_time()
        {
            // arrange
            var startTime = DateTime.UtcNow;

            // act
            var sut = new Booking(startTime);

            var sut = Booking.Create(startTime: startTime,
                            durationMinutes: 20,
                            bookingParty: new User(age: 50),
                            vatRate: 0.25m,
                            location: new Location(new LocalTime(07, 00)),
                            price: Money.Create("SEK", 50),
                            systemClock: new Clock(startTime));

            // assert
            sut.StartTime.Should().BeSameDateAs(startTime);
        }

        [Fact]
        public void Booking_duration_may_not_exceed_1_hour()
        {
            // arrange
            var startTime = DateTime.UtcNow;

            Action createBooking =
                    () => Booking.Create(startTime: startTime,
                                    durationMinutes: 61,
                                    Money.Create("SEK", 50),
                                    vatRate: 0.25m,
                                    new User(50), new Location(new LocalTime(07, 00)), new Clock(startTime));

            createBooking.Should().Throw<InvalidOperationException>()
                    .WithMessage("Bookings must not exceed 60 minutes.");
        }

        [Fact]
        public void Default_booking_price_including_vat_should_be_50()
        {
            // arrange
            var sut = CreateBooking(standardPrice: 40,  userAge: 20, startTime: DateTime.UtcNow);

            // act
            var price = sut.GetPrice();

            // assert
            price.Should().Be(50m);
        }

        [Fact]
        public void Booking_should_have_a_duration()
        {
            // arrange
            var sut = CreateBooking(standardPrice: 50m,  userAge: 20, startTime: DateTime.UtcNow);

            // act
            var duration = sut.Duration;

            // assert
            duration.Should().Be(TimeSpan.FromSeconds(0));
        }

        [Fact]
        public void User_must_be_able_to_cancel_the_booking()
        {
            // arrange
            var sut = CreateBooking(standardPrice: 50, userAge: 70, startTime: DateTime.UtcNow.AddHours(-2), new Clock(DateTime.UtcNow));

            // act
            sut.Cancel();

            // assert
            sut.IsCancelled.Should().BeTrue();
        }

        [Fact]
        public void Company_discount_is_applied()
        {
            // arrange
            var sut = CreateCompanyBooking(standardPrice: 50m, discount: 0.2m, startTime: DateTime.UtcNow);

            // act
            var price = sut.GetPrice();

            // assert
            price.Should().Be(40m);
        }

        [Fact]
        public void Price_is_free_for_pensioners()
        {
            // arrange
            var sut = CreateBooking(standardPrice: 50, userAge: 70, startTime: DateTime.UtcNow);

            // act
            var price = sut.GetPrice();

            // assert
            price.Should().Be(0);
        }

        [Fact]
        public void Booking_is_half_price_for_children_under_12_years_of_age()
        {
            // arrange
            var booking = CreateBooking(standardPrice: 50, userAge: 11, startTime: DateTime.UtcNow);

            // act
            var price = booking.GetPrice();

            // assert
            price.Should().Be(31.25m);
        }

        [Fact]
        public void Booking_has_a_location()
        {
            // arrange
            var sut = CreateBooking(standardPrice: 50, userAge: 11, startTime: DateTime.UtcNow);

            // act
            var location = sut.Location;

            // assert
            location.Should().NotBeNull();
        }

        [Fact]
        public void User_may_not_cancel_the_booking_when_less_than_one_hour_until_start()
        {
            // arrange
            var sut = CreateBooking(standardPrice: 50, userAge: 70, startTime: DateTime.UtcNow.AddMinutes(30));

            // act, assert
            sut.Invoking(s => s.Cancel()).Should().Throw<InvalidOperationException>().
                        WithMessage("Booking may not be cancelled.");
        }

        [Fact]
        public void Booking_is_added_to_user_recent_bookings()
        {
            // arrange;
            var booking = CreateBooking(standardPrice: 50, userAge: 70, startTime: DateTime.UtcNow.AddMinutes(30));
            var sut = booking.BookingParty;
            
            sut.RecentBookings.Contains(booking);
        }

        [Fact]
        public void Bookings_with_start_time_before_location_opens_should_be_rejected()
        {
            // arrange
            var startTime = DateTime.UtcNow;

            Action createBooking =
                    () => Booking.Create(startTime: startTime,
                                    durationMinutes: 61,
                                    Money.Create("SEK", 50),
                                    vatRate: 0.25m,
                                    new User(50), new Location(openingTime: new LocalTime(7,0)), new Clock(startTime));

            createBooking.Should().Throw<InvalidOperationException>()
                    .WithMessage("Bookings must not exceed 60 minutes.");
        }

        #region Arrange

        private Booking CreateBooking(decimal standardPrice, int userAge, DateTime startTime, Clock systemClock)
        {
            var user = new User(userAge);
            return Booking.Create(startTime, 50, Money.Create("SEK", standardPrice), 0.25m, user, new Location(new LocalTime(07, 00)), systemClock);
        }

        private Booking CreateBooking(decimal standardPrice, int userAge, DateTime startTime)
        {
            var user = new User(userAge);
            return Booking.Create(startTime, 50, Money.Create("SEK", standardPrice), 0.25m, user, new Location(new LocalTime(07, 00)), new Clock(startTime));
        }

        private Booking CreateCompanyBooking(decimal standardPrice, decimal discount, DateTime startTime)
        {
            var user = new Company(discount);
            return Booking.Create(startTime, 50, Money.Create("SEK", standardPrice), 0.25m, user, new Location(new LocalTime(07, 00)), new Clock(startTime));
        }

        #endregion
    }
}