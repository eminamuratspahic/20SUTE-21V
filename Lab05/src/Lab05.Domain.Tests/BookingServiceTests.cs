using System;
using FluentAssertions;
using Xunit;
using AutoFixture;
using AutoFixture.Xunit2;
using AutoFixture.AutoFakeItEasy;
using FakeItEasy;
using System.ComponentModel.DataAnnotations;

namespace Lab05.Domain.Tests
{
    public class AutofixtureTests 
    {
        [Fact]
        public void Create_a_string_fixture()
        {
            // arrange
            var fixture = new Fixture();
            string name = fixture.Create<string>();
            var sut = new TestClass();

            // act
            sut.TestRandomName(name);

        }

        [Fact]
        public void Create_a_quantity_fixture()
        {
            // arrange
            var fixture = new Fixture();
            int quantity = fixture.Create<int>();
            var sut = new TestClass();

            // act
            sut.TestRandomQuantity(quantity);
        }

        [Fact]
        public void Create_a_quantity_fixture_Range()
        {
            // arrange
            var fixture = new Fixture();
            int quantity = fixture.Create<int>();
            var sut = new TestClass();

            // act
            sut.TestRandomQuantityRange(quantity);
        }

        [Fact]
        public void Create_a_date_fixture()
        {
            // arrange
            var fixture = new Fixture();
            DateTime date = fixture.Create<DateTime>();
            var sut = new TestClass();

            // act
            sut.TestRandomDate(date);
        }

        [Fact]
        public void Create_a_date_fixture_in_2018()
        {
            // arrange
            var fixture = new Fixture();
            DateTime date = fixture.Create<DateTime>();
            var sut = new TestClass();

            // act
            sut.TestRandomDateIn2018(date);
        }

    }
}