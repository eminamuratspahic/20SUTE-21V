using System;
using Autofac.Extras.FakeItEasy;
using EFCore.Domain;
using EFCore.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.AspNet.Tests
{
    /// <summary>
    /// We use this to override service registration in the dependency injection container. 
    /// </summary>
    /// <typeparam name="TStartup"></typeparam>
    public class CustomWebApplicationFactory<TStartup>
    : WebApplicationFactory<TStartup> where TStartup : class
    {
        public AutoFake AutoFake { get; private set; }

        public CustomWebApplicationFactory()
        {
            this.AutoFake = new AutoFake();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Add ApplicationDbContext using an in-memory database for testing.
                services.AddDbContext<BookingContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });

                services.AddScoped<BookingService>();

                // replace registrations of 
                services.AddScoped<IPaymentGateway>(c => AutoFake.Resolve<IPaymentGateway>());
                services.AddScoped<IPriceCalculator>(c => AutoFake.Resolve<IPriceCalculator>());
            });
        }
    }
}