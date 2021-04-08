using System;
using System.Diagnostics;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Domain.Tests
{
    public class BookingContextSqliteFixture : IDisposable
    {
        public BookingContext Context { get; set; }
        private SqliteConnection connection;

        public BookingContextSqliteFixture()
        {
            connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            // Instruct EF to use a sqlite in-memory (FAKE!!!) database instance.
            var builder = new DbContextOptionsBuilder<BookingContext>()
                .UseSqlite(connection);

            Context = new BookingContext(builder.Options);

            var result = Context.Database.EnsureCreated();

            Debug.Assert(result);
        }

        public void Dispose()
        {
            connection.Close();
            Context.Dispose();
        }
    }
}