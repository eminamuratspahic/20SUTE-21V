using System;
using Xunit;
using Lab01.Domain;
using static Lab01.Domain.Playlist;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using FluentAssertions;
namespace Lab01.Domain.Tests
{
    public class PlaylistTests
    {
        private List<Song> songsSaved = new List<Song>();

        public PlaylistTests() {
            var sut = new Playlist();
        }

        [Fact]
        public void playlist_should_be_active()
        {
            // arrange
            var sut = new Playlist();

            // act
            sut.IsActive = true;

            // assert
            sut.IsActive.Should().BeTrue();
        }

        [Fact]
        public void title_must_not_be_null()
        {
            // arrange
            var sut = new Playlist();

            // act
            sut.Title = "";

            // assert
           sut.Title.Should().NotBeNull();
        }

        [Fact]
        public void title_must_not_be_empty()
        {
            // arrange
            var sut = new Playlist();

            // act
            sut.Title = "My title";

            // assert
            sut.Title.Should().NotBeEmpty();
        }

        [Fact]
        public void can_add_songs_to_playlist()
        {
            // arrange
            var sut = new Playlist();

            // act
            sut.AddSong(new Song());

            // assert
            sut.Songs.Should().HaveCount(1);
        }

        [Fact]
        public void new_playlist_has_empty_song_list()
        {
            // arrange
            var sut = new Playlist();

            sut.Songs.Should().BeEmpty();
        }

        [Fact]
        public void song_added_to_playlist_is_same_instance()
        {
            // arrange
            var sut = new Playlist();

            var song = new Song() { Title = "My song 123" };

            // act
            sut.AddSong(song);

            sut.Songs[0].Should().BeSameAs(song);
        }

        [Fact(Skip = "demo")]
        public void songs_by_abba_are_not_allowed()
        {
            // arrange
            var sut = new Playlist();

            var song = new Song() { Artist = "ABBA", Title = "My song 123" };

            sut.Invoking(s => s.AddSong(song)).Should().Throw<InvalidOperationException>();
            Assert.Throws<InvalidOperationException>(() => sut.AddSong(song));
        }

        [Fact]
        public void playlist_can_be_cleared()
        {
            // arrange
            var sut = new Playlist();

            sut.AddSong(new Song());

            var song1 = new Song() { Title  = "ABC"};
           // var song2 = song1;

            var song2 = new Song() { Title  = "ABC"};

            sut.Clear();

            sut.Songs.Should().BeEmpty();
        }

        [Fact]
        public void playlist_sorted_by_artist_then_title()
        {
            // arrange
            var sut = new Playlist();

            sut.AddSong(new Song() { Title = "My song title", Artist = "C" });
            sut.AddSong(new Song() { Title = "A song beginning with A", Artist = "B" });

            sut.Songs[0].Artist.Should().Be("B");
        }

        [Theory]
        [InlineData(5,2,7)]
        [InlineData(10, 10, 20)]
        public void add_two_numbers_inline_data(int x, int y, int expectedResult)
        {
            // arrange
            var calculator = new SimpleCalculator();

            // act
            var result = calculator.Add(x, y);

            // assert
            Assert.Equal(expectedResult, result);
        }


        public static IEnumerable<object[]> GetNumbers()
        {
            yield return new object[] { 5, 2, 7 };
            yield return new object[] { 10, 10, 20 };
        }

        [Theory]
        [MemberData(nameof(GetNumbers))]
        public void add_two_numbers_member_data(int x, int y, int expectedResult)
        {
            // arrange
            var calculator = new SimpleCalculator();

            // act
            var result = calculator.Add(x, y);

            // assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [ClassData(typeof(NumberGenerator))]
        public void add_two_numbers_class_data(int x, int y, int expectedResult)
        {
            // arrange
            var calculator = new SimpleCalculator();

            // act
            var result = calculator.Add(x, y);

            // assert
            Assert.Equal(expectedResult, result);
        }

        public void Builder_test() {
            var carBuilder = CarBuilder.Create()
                        .AddEngine()
                        .AddWheel().Build();
        }
    }

    public class NumberGenerator : IEnumerable<object[]>
    {
        public NumberGenerator()
        {
            _data.Add(new object[] { 5, 2, 7 });
            _data.Add(new object[] { 10, 10, 20 });
        }

        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();
        private readonly List<object[]> _data = new List<object[]>();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
