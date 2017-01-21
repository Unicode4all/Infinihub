using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Infinity.so.Data;
using Infinity.so.Models;
using Infinity.so.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Threading;
using System.Threading.Tasks;
using System;
using OpenIddict.Core;
using OpenIddict.Models;

namespace Infinity.so
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
                options.UseOpenIddict();
            });

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddOpenIddict()
                .AddEntityFrameworkCoreStores<ApplicationDbContext>()
                .AddMvcBinders()
                .EnableAuthorizationEndpoint("/connect/authorize")
                .EnableLogoutEndpoint("/connect/logout")
                .EnableTokenEndpoint("/connect/token")
     //           .EnableUserinfoEndpoint("/api/userinfo")
                .UseJsonWebTokens()
                .AllowPasswordFlow()
                .AllowAuthorizationCodeFlow()
                .EnableRequestCaching()
                .AddEphemeralSigningKey();

            services.AddOptions();

            services.Configure<AppearanceOptions>(Configuration.GetSection("Appearance"));

            services.AddMvc()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            services.Configure<AuthMessageSenderOptions>(Configuration);



            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireStaff", policy => policy.RequireRole("Trial Admin", "Game Admin", "Game Master", "Developer", "Host"));
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }


            app.UseStaticFiles();

            app.UseIdentity();

            app.UseStatusCodePagesWithReExecute("/error/{0}");


            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
                        .CreateScope())
            {
                serviceScope.ServiceProvider.GetService<ApplicationDbContext>()
                     .Database.Migrate();

                var userManager = app.ApplicationServices.GetService<UserManager<ApplicationUser>>();
                var roleManager = app.ApplicationServices.GetService<RoleManager<IdentityRole>>();

                serviceScope.ServiceProvider.GetService<ApplicationDbContext>().EnsureSeedData(userManager, roleManager);
            }

            //app.UseStatusCodePages(context => context.HttpContext.Response.SendAsync("Handler, status code: " + context.HttpContext.Response.StatusCode, "text/plain"));
            // app.UseStatusCodePages("text/plain", "Response, status code: {0}");
            // app.UseStatusCodePagesWithRedirects("~/errors/{0}"); // PathBase relative
            // app.UseStatusCodePagesWithRedirects("/base/errors/{0}"); // Absolute
            // app.UseStatusCodePages(builder => builder.UseWelcomePage());
            // app.UseStatusCodePagesWithReExecute("/errors/{0}");

            // Add external authentication middleware below. To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715

            app.UseOAuthValidation();
            app.UseOpenIddict();



            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });



            InitializeAsync(app, CancellationToken.None).GetAwaiter().GetResult();
        }

        private async Task InitializeAsync(IApplicationBuilder app, CancellationToken cancellationToken)
        {
            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                await context.Database.EnsureCreatedAsync();

                var manager = scope.ServiceProvider.GetRequiredService<OpenIddictApplicationManager<OpenIddictApplication>>();

                // Postman - REST client for development
                if (await manager.FindByClientIdAsync("postman", cancellationToken) == null)
                {
                    var application = new OpenIddictApplication
                    {
                        ClientId = "postman",
                        DisplayName = "Postman REST Client",
                        RedirectUri = "https://www.getpostman.com/oauth2/callback"
                    };

                    await manager.CreateAsync(application, "306564A5-E7FE-49CB-A10D-61AF6E8F3654", cancellationToken);
                }
            }
        }
    }
}
