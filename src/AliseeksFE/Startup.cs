using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using AliseeksFE.Services.Api;
using AliseeksFE.Services.Feedback;
using AliseeksFE.Services.Search;
using AliseeksFE.Services.User;
using AliseeksFE.Authentication;
using Microsoft.AspNetCore.Http;    
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Cryptography;
using AliseeksFE.Configuration.Options;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Session;
using AliseeksFE.Services.Logging;
using AliseeksFE.Injectables.Search;
using AliseeksFE.Middleware;

namespace AliseeksFE
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddJsonFile("appsecrets.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc(config =>
            {
                
            });

            services.AddOptions();

            services.AddSession();

            services.Configure<JwtOptions>(Configuration.GetSection("JwtOptions"));

            configureDependencyInjection(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            var options = app.ApplicationServices.GetService<IOptions<JwtOptions>>().Value;
            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                AuthenticationScheme = "AliseeksCookie",
                CookieName = "access_token",
                TicketDataFormat = new AliseeksJwtCookieAuthentication(AliseeksJwtAuthentication.TokenValidationParameters(options.SecretKey))
            });

            app.UseStaticFiles();

            app.UseSession(new SessionOptions()
            {
                
            });

            app.UseMiddleware<Logger>();
    
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        void configureDependencyInjection(IServiceCollection services)
        {
            services.AddTransient<IApiService, ApiService>();
            services.AddTransient<ISearchService, SearchService>();
            services.AddTransient<IUserService, UserService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IFeedbackService, FeedbackService>();
            services.AddTransient<ILoggingService, LoggingService>();
            services.AddTransient<AliseeksJwtAuthentication, AliseeksJwtAuthentication>();

            //Injectable Services
            services.AddTransient<SearchCriteriaInject>();
        }
    }
}
