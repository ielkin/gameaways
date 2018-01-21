using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using GameMiner.Web.App.Models;
using GameMiner.Web.App.Services;
using Gameaways.BusinessLayer.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using GameMiner.DataLayer.Model;
using GameMiner.Web.App.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using GameMiner.Web.App.Identity.OpenId.Steam;
using GameMiner.BusinessLayer;
using Microsoft.Extensions.Logging;

namespace GameMiner.Web.App
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            HostingEnvironment = environment;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment HostingEnvironment { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<GameawaysDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<User, UserRole>()
                .AddUserStore<GameawaysUserStore>()
                .AddRoleStore<RoleStore>();

            services.Configure<SecurityStampValidatorOptions>(options => { options.ValidationInterval = TimeSpan.FromDays(14); });

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }) 
            .AddSteam();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Gameaways", policy => policy.RequireUserName("Gameaways"));
            });

            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddMvc()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();

            services.Configure<RequestLocalizationOptions>(
                options =>
                {
                    var numberFormat = new NumberFormatInfo { NumberDecimalSeparator = "." };

                    var supportedCultures = new List<CultureInfo> { new CultureInfo("uk-UA") { NumberFormat = numberFormat }, new CultureInfo("ru-RU") { NumberFormat = numberFormat }, new CultureInfo("en-US") { NumberFormat = numberFormat } };

                    options.DefaultRequestCulture = new RequestCulture(new CultureInfo("uk-UA") { NumberFormat = numberFormat });
                    options.SupportedCultures = supportedCultures;
                    options.SupportedUICultures = supportedCultures;
                });

            services.AddMemoryCache();

            services.AddTransient<ICoinhiveApiClient, CoinhiveApiClient>();
            services.AddTransient<IEconomyService, EconomyService>();
            services.AddTransient<IMessagePublisher, ServiceBusMessagePublisher>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Giveaways}/{action=Index}/{id?}");
                routes.MapRoute(
                    name: "ViewGiveaway",
                    template: "Giveaway/{id}",
                    defaults: new { controller = "Giveaways", action = "ViewGiveaway" });
            });
        }
    }
}
