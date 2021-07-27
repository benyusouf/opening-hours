using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace OpeningHours.API.Bootstrap
{
    public static class SwaggerConfig
    {
        public static IServiceCollection AddSwaggerService(this IServiceCollection services, IConfiguration configuration)
        {
            SwaggerOptions options = new SwaggerOptions
            {
                Title = configuration["Swagger:Title"],
                Version = configuration["Swagger:Version"]
            };

            services.AddSwaggerGenNewtonsoftSupport();
            services.AddSwaggerGen(c =>
            {
                c.AddServer(new OpenApiServer
                {
                    Url = "",
                    Description = "Stagging",
                });
                c.AddServer(new OpenApiServer
                {
                    Url = "https://localhost:5004",
                    Description = "Local host",
                });
                c.SwaggerDoc("v2", new OpenApiInfo { Title = $"{options.Title} API", Version = $"{options.Version}" });
                c.IncludeXmlComments(Path.ChangeExtension(Assembly.GetEntryAssembly().Location, "xml"));
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
                c.CustomSchemaIds(x => x.FullName);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });

            });
            return services;
        }


        public static IApplicationBuilder UseSwaggerService(this IApplicationBuilder app,
            IConfiguration configuration, IWebHostEnvironment environment)
        {
            if (!environment.IsProduction())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    string swaggerJsonBasePath = string.IsNullOrWhiteSpace(c.RoutePrefix) ? "." : "..";
                    c.SwaggerEndpoint($"{swaggerJsonBasePath}/swagger/v2/swagger.json",
                        $"{configuration["Swagger:Title"]} API {configuration["Swagger:Version"]}");
                });
            }
            return app;
        }
    }
}
