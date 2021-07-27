using System.Reflection;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Converters;
using OpeningHours.API.Bootstrap;
using OpeningHours.API.Services;

namespace OpeningHours.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Env { get;  }
        private const string DefaultCorePolicy = "DefaultCors";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IOpeningHoursFormatter, OpeningHoursFormatter>();

            services
                .AddControllers(option =>
                {
                    option.Filters.Add(new ApiExceptionFilterAttribute());
                })
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>())
                .ConfigureApiBehaviorOptions(option => { option.SuppressModelStateInvalidFilter = true; })
                .AddNewtonsoftJson(options => options.SerializerSettings.Converters.Add(new StringEnumConverter()));

            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddSwaggerService(Configuration);

            services.AddCors(options =>
            {
                options.AddPolicy(DefaultCorePolicy, builder =>
                {
                    builder.AllowAnyMethod();
                    builder.AllowAnyHeader();
                    builder.AllowCredentials();
                    builder.SetIsOriginAllowed(origin =>
                    {
                        if (string.IsNullOrWhiteSpace(origin)) return false;
                        // Only allow this policy for test from local host
                        if (origin.ToLower().StartsWith("http://localhost") && Env.IsDevelopment()) return true;
                        // Detects production domain, for test only
                        if (origin.ToLower().Contains("openinghours")) return true;
                        return false;
                    });
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwaggerService(Configuration, env);
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseCors(DefaultCorePolicy);
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
