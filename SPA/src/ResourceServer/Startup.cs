using ResourceServer.Model;
using ResourceServer.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using ResourceServer.DataProtection;
using System;
using System.Threading.Tasks;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace ResourceServer;

public class Startup
{
    public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
    {
        Configuration = configuration;
        _webHostEnvironment = webHostEnvironment;
    }

    public IConfiguration Configuration { get; }

    private readonly IWebHostEnvironment _webHostEnvironment;

    public void ConfigureServices(IServiceCollection services)
    {
        var connection = Configuration.GetConnectionString("DefaultConnection");

        X509Certificate2 cert = new X509Certificate2(Path.Combine(_webHostEnvironment.ContentRootPath, "damienbodserver.pfx"), "");

        services.AddDataProtection()
            .SetApplicationName("ResourceServer")
            .ProtectKeysWithCertificate(cert)
            .AddKeyManagementOptions(options =>
                options.XmlRepository = new SqlXmlRepository(
                    new DataProtectionDbContext(
                        new DbContextOptionsBuilder<DataProtectionDbContext>().UseSqlite(connection).Options
                    )
                )
            );

        services.AddDbContext<DataEventRecordContext>(options =>
            options.UseSqlite(connection)
        );

        services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins",
                builder =>
                {
                    builder
                        .AllowCredentials()
                        .WithOrigins(
                            "https://localhost:14100",
                            "https://localhost:44311",
                            "https://localhost:44352",
                            "https://localhost:44372",
                            "https://localhost:44378",
                            "https://localhost:44390")
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
        });

        var guestPolicy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();

        
        services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Authority = "https://idp.ca.testkontur.ru:8444/realms/Kontur";
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ClockSkew = new TimeSpan(0, 0, 3),
                    ValidateAudience = false,
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                };
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("dataEventRecordsAdmin", guestPolicy);
            options.AddPolicy("dataEventRecordsUser", guestPolicy);
            options.AddPolicy("dataEventRecords", guestPolicy);
        });

        services.AddSwaggerGen(c =>
        {
            // add JWT Authentication
            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "JWT Authentication",
                Description = "Enter JWT Bearer token **_only_**",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer", // must be lower case
                BearerFormat = "JWT",
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };
            c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {securityScheme, new string[] { }}
            });

            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Resource server",
                Version = "v1",
                Description = "Recource Server",
                Contact = new OpenApiContact
                {
                    Name = "damienbod",
                    Email = string.Empty,
                    Url = new Uri("https://damienbod.com/"),
                },
            });
        });

        services.AddControllers()
            .AddNewtonsoftJson();

        services.AddScoped<IDataEventRecordRepository, DataEventRecordRepository>();
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Resource Server");
            c.RoutePrefix = string.Empty;
        });

        app.UseExceptionHandler("/Home/Error");
        app.UseCors("AllowAllOrigins");
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}