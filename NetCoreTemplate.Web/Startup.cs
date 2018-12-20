namespace NetCoreTemplate.Web
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    using NetCoreTemplate.DAL;
    using NetCoreTemplate.DAL.Extensions;
    using NetCoreTemplate.Web.Authentication;

    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Localization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Authorization;
    using Microsoft.AspNetCore.Mvc.Controllers;
    using Microsoft.AspNetCore.Mvc.ViewComponents;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using SimpleInjector;
    using SimpleInjector.Integration.AspNetCore.Mvc;
    using SimpleInjector.Lifestyles;

    public class Startup
    {
        private Container container = new Container();

        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }

        public IHostingEnvironment Environment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            IntegrateSimpleInjector(services);
            RegisterDbContext(services);
            EnableHTTPS(services);
            EnableAuthorization(services);
            SetupAuthentication(services);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            InitializeContainer(app);
            UpdateDatabase();

            container.Verify();

            app.UseStatusCodePagesWithRedirects("/error/{0}");
            app.UseHsts();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseCookiePolicy();
            app.UseSession();

            var defaultCulture = new CultureInfo("nl-NL");
            defaultCulture.NumberFormat.NumberDecimalSeparator = ",";
            defaultCulture.NumberFormat.CurrencyDecimalSeparator = ",";
            defaultCulture.DateTimeFormat.ShortDatePattern = "dd-MM-yyyy";

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(defaultCulture),
                SupportedCultures = new List<CultureInfo>
                {
                    defaultCulture
                },
                SupportedUICultures = new List<CultureInfo>
                {
                    defaultCulture
                }
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Dashboard}/{action=Index}/{id?}");
            });
        }

        private void IntegrateSimpleInjector(IServiceCollection services)
        {
            container.Options.AllowOverridingRegistrations = true;
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IControllerActivator>(new SimpleInjectorControllerActivator(container));
            services.AddSingleton<IViewComponentActivator>(new SimpleInjectorViewComponentActivator(container));

            services.EnableSimpleInjectorCrossWiring(container);
            services.UseSimpleInjectorAspNetRequestScoping(container);

            container.RegisterSingleton(() => Configuration);

            SimpleInjectorConfig.Register(container);
        }

        private void RegisterDbContext(IServiceCollection services)
        {
            services.AddDbContext<DatabaseContext>();
        }

        private void EnableHTTPS(IServiceCollection services)
        {
            var enableHTTPS = Configuration.GetValue<bool>("EnableSSL");

            services.Configure<MvcOptions>(options =>
            {
                if (!Environment.IsDevelopment() && enableHTTPS)
                {
                    options.Filters.Add(new RequireHttpsAttribute());
                }
            });
        }

        private void EnableAuthorization(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();

                options.Filters.Add(new AuthorizeFilter(policy));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        private void UpdateDatabase()
        {
            using (var scope = AsyncScopedLifestyle.BeginScope(container))
            {
                using (var serviceScope = scope.GetInstance<IServiceScope>())
                {
                    var databaseService = serviceScope.ServiceProvider.GetService<DatabaseContext>();
                    var allMigrationsApplied = databaseService.AllMigrationApplied();

                    if (!allMigrationsApplied)
                    {
                        databaseService.Database.Migrate();
                    }

                    databaseService.Seed();
                }

                scope.Dispose();
            }
        }

        private void InitializeContainer(IApplicationBuilder app)
        {
            container.RegisterMvcControllers(app);
            container.RegisterMvcViewComponents(app);
            container.AutoCrossWireAspNetComponents(app);
            container.RegisterSingleton(() => app.ApplicationServices.GetRequiredService<IServiceProvider>());
        }

        private void SetupAuthentication(IServiceCollection services)
        {
            services.AddSession();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.LoginPath = "/";
                options.LogoutPath = "/signout/";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(15);
                options.SessionStore = new MemoryCacheTicketStore();
            });
        }
    }
}
