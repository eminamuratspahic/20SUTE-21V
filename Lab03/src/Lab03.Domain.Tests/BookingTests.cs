using System;
using Xunit;
using FluentAssertions;
using Lab03.Domain;
using FakeItEasy;

namespace Lab03.Domain.Tests
{
    public class BookingTests
    {
        [Fact]
        public void Booking_confirm_should_take_a_payment()
        {
            // arrange
            var paymentGateway = A.Fake<IPaymentGateway>();
            var priceCalculator = A.Fake<IPriceCalculator>();
            var booking = new Booking(paymentGateway, priceCalculator);

            // act
            booking.Confirm1();

            // assert
            A.CallTo(() => paymentGateway.CapturePayment(A<decimal>.Ignored, A<decimal>.Ignored)).MustHaveHappened();
        }

        [Fact]
        public void Booking_confirm_should_take_two_payments_with_amount_50()
        {
            // arrange
            var paymentGateway = A.Fake<IPaymentGateway>();
            var priceCalculator = A.Fake<IPriceCalculator>();
            var booking = new Booking(paymentGateway, priceCalculator);

            // act
            booking.Confirm2();

            // assert
            A.CallTo(() => paymentGateway.CapturePayment(50, A<decimal>.Ignored)).MustHaveHappenedTwiceExactly();
        }

        [Fact]
        public void Booking_confirm_should_take_one_payment_with_amount_100_and_any_vat()
        {
            // arrange
            var paymentGateway = A.Fake<IPaymentGateway>();
            var priceCalculator = A.Fake<IPriceCalculator>();
            var booking = new Booking(paymentGateway, priceCalculator);

            // act
            booking.Confirm3();

            // assert
            A.CallTo(() => paymentGateway.CapturePayment(100, A<decimal>.Ignored)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void Booking_should_take_one_payment_with_amount_50_and_another_with_amount_65()
        {
            // arrange
            var paymentGateway = A.Fake<IPaymentGateway>();
            var priceCalculator = A.Fake<IPriceCalculator>();
            var booking = new Booking(paymentGateway, priceCalculator);

            // act
            booking.Confirm4();

            // assert
            A.CallTo(() => paymentGateway.CapturePayment(50, 3)).MustHaveHappenedOnceExactly();
            A.CallTo(() => paymentGateway.CapturePayment(65, 4)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void Booking_confirm_should_use_the_amount_suggested_by_the_price_calculator()
        {
            // arrange
            var paymentGateway = A.Fake<IPaymentGateway>(); // mock
            var priceCalculator = A.Fake<IPriceCalculator>(); // stub
            var booking = new Booking(paymentGateway, priceCalculator);
            var amount = 50m;
            var vatAmount = 150m;

            A.CallTo(() => priceCalculator.CalculatePrice()).Returns(new Price { Amount = amount, VatAmount = vatAmount });

            // act
            booking.Confirm5();

            // assert
            A.CallTo(() => paymentGateway.CapturePayment(amount, A<decimal>.Ignored)).MustHaveHappenedOnceExactly();
        }


        [Fact]
        public void Booking_confirm_should_take_two_different_payments_suggested_by_the_price_calculator()
        {
            // arrange
            using var fake = new AutoFake();
            var booking = fake.Resolve<Booking>();
            var priceCalculator = fake.Resolve<IPriceCalculator>();
            var paymentGateway = fake.Resolve<IPaymentGateway>();

            var price1 = new Price { Amount = 100, VatAmount = 5m };
            var price2 = new Price { Amount = 50, VatAmount = 2.5m };

            A.CallTo(() => priceCalculator.CalculatePrice())
                .ReturnsNextFromSequence(price1, price2);

            // act
            booking.Confirm6();

            // assert
            A.CallTo(() => paymentGateway.CapturePayment(price2.Amount, price2.VatAmount)).MustHaveHappenedOnceExactly();
            A.CallTo(() => paymentGateway.CapturePayment(price1.Amount, price1.VatAmount)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void Booking_confirm_should_take_two_different_payments_suggested_by_the_price_calculator_in_the_correct_order()
        {
            // arrange
            using var fake = new AutoFake();
            var booking = fake.Resolve<Booking>();
            var priceCalculator = fake.Resolve<IPriceCalculator>();
            var paymentGateway = fake.Resolve<IPaymentGateway>();

            var price1 = new Price { Amount = 100, VatAmount = 5m };
            var price2 = new Price { Amount = 50, VatAmount = 2.5m };

            A.CallTo(() => priceCalculator.CalculatePrice())
                .ReturnsNextFromSequence(price2, price1);

            // act
            booking.Confirm6();

            // assert
            A.CallTo(() => paymentGateway.CapturePayment(price1.Amount, price1.VatAmount)).MustHaveHappenedOnceExactly()
            .Then(
            A.CallTo(() => paymentGateway.CapturePayment(price2.Amount, price2.VatAmount)).MustHaveHappenedOnceExactly());
        }
    }

}