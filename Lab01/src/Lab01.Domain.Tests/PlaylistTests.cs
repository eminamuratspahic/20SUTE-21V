using System;
using Xunit;
using Lab01.Domain;
using static Lab01.Domain.Playlist;
using System.Collections.Generic;
using System.Collections;
using FluentAssertions;

namespace Lab01.Domain.Tests
{
    public class PlaylistTests
    {
        [Fact]
        public void Playlist_should_be_active()
        {
            // arrange
            var sut = new Playlist();

            // act
            sut.IsActive = true;

            // assert

            sut.IsActive.Should().BeTrue();
        }
    }
}
