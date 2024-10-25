using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using AngularClient.ViewModel;
using Clients;
using FileServer.Clients;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace AngularClient
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ClientAppSettings>(Configuration.GetSection("ClientAppSettings"));
            services.AddSingleton(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<ClientAppSettings>>().Value;
                var client = sp.GetRequiredService<IHttpClientFactory>().CreateClient();

                return new ResourceServerClient(settings.apiServer, client);
            });

            services.AddSingleton(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<ClientAppSettings>>().Value;
                var client = sp.GetRequiredService<IHttpClientFactory>().CreateClient();

                return new ResourceFileServerClient(settings.apiFileServer, client);
            });
            
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder
                            .AllowCredentials()
                            .WithOrigins(
                                "null",
                                "https://localhost:8080",
                                "http://localhost:14100",
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

            services.AddControllersWithViews();
            services.AddHttpClient();
            
            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                })
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,options =>
                {
                    options.Cookie.Name = "OIDCDemoSite";
                    options.Cookie.HttpOnly = true;
                    //options.
                    options.Events = new CookieAuthenticationEvents
                    {
                        OnValidatePrincipal = cc =>
                        {
                            cc.HttpContext.Items.Add("AccessToken", cc.Properties.Items.FirstOrDefault(c => c.Key == ".Token.access_token").Value);
                            return Task.CompletedTask;
                        },
                    };
                })
                .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
                {
                    options.Authority = "https://idp.ca.testkontur.ru:8444/realms/Kontur";
                    options.ClientId = "TestShmakov";
                    options.ClientSecret = Environment.GetEnvironmentVariable("FEMIDA_SECRET_CLIENT_ID");

                    // code flow
                    options.ResponseType = OpenIdConnectResponseType.Code;
                    options.UsePkce = false;
                    
                    options.Scope.Clear();
                    options.Scope.Add("openid");
                    options.Scope.Add("profile");
                    options.Scope.Add("email");

                    options.SaveTokens = true;
                    
                    // the duration of the cookie will be the same as the id token lifetime
                    options.UseTokenLifetime = true;
                });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseCors("AllowAllOrigins");

            var angularRoutes = new[] {
                "/home",
                "/forbidden",
                "/authorized",
                "/authorize",
                "/unauthorized",
                "/dataeventrecords",
                "/dataeventrecords/list",
                "/dataeventrecords/create",
                "/dataeventrecords/edit",
                "/logoff",
                "/securefiles",
            };

            app.Use(async (context, next) =>
            {
                if (context.Request.Path.HasValue && null != angularRoutes.FirstOrDefault(
                    (ar) => context.Request.Path.Value.StartsWith(ar, StringComparison.OrdinalIgnoreCase)))
                {
                    context.Request.Path = new PathString("/");
                }

                await next();
            });

            app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
