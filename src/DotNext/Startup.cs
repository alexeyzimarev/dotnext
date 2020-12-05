using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Serilog;

namespace DotNext {
    public class Startup {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services) {
            services.AddControllers();

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
    }
}
