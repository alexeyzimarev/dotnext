using System;
using DotNext.Application;
using DotNext.Domain;
using DotNext.Infrastructure;
using DotNext.Infrastructure.MongoDb;
using DotNext.Lib;
using DotNext.Projections;
using EventStore.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Serilog;
using MongoDefaults = DotNext.Infrastructure.MongoDb.MongoDefaults;

namespace DotNext {
    public class Startup {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services) {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            EventTypeMapper.MapEventTypes();
            MongoDefaults.RegisterConventions();

            services.AddControllers();

            services.AddSingleton(
                ctx
                    => ConfigureEventStore(
                        Configuration["EventStore:ConnectionString"],
                        ctx.GetService<ILoggerFactory>()
                    )
            );

            services.AddSingleton(
                ConfigureMongo(
                    Configuration["MongoDb:ConnectionString"],
                    Configuration["MongoDb:Database"]
                )
            );
            services.AddSingleton<IAggregateStore, AggregateStore>();
            services.AddSingleton<BookingCommandService>();
            services.AddSingleton<IAvailabilityCheck, FakeAvailabilityCheck>();
            services.AddSingleton<ICheckpointStore, MongoCheckpointStore>();
            services.AddSingleton<GuestBookingsProjection>();

            services.AddHostedService(
                ctx =>
                    new MongoProjectionService(
                        ctx.GetService<EventStoreClient>(),
                        ctx.GetService<ICheckpointStore>(),
                        "guestBookings",
                        ctx.GetService<GuestBookingsProjection>()
                    )
            );

            services.AddSwaggerGen(
                c
                    => c.SwaggerDoc("v1", new OpenApiInfo {Title = "DotNext", Version = "v0.1"})
            );
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            app.UseSwagger();

            app.UseSwaggerUI(
                c => c.SwaggerEndpoint(
                    "/swagger/v1/swagger.json",
                    "DotNext v0.1"
                )
            );
            app.UseDeveloperExceptionPage();
            app.UseSerilogRequestLogging();
            app.UseRouting();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }

        static EventStoreClient ConfigureEventStore(string connectionString, ILoggerFactory loggerFactory) {
            var settings = EventStoreClientSettings.Create(connectionString);
            settings.ConnectionName = "bookingApp";
            settings.LoggerFactory  = loggerFactory;
            return new EventStoreClient(settings);
        }

        static IMongoDatabase ConfigureMongo(string connectionString, string database) {
            var settings = MongoClientSettings.FromConnectionString(connectionString);
            return new MongoClient(connectionString).GetDatabase(database);
        }
    }
}
