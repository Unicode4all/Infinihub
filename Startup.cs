//   Copyright 2017 Solaris 13 Foundation
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Infinihub.Data;
using Infinihub.Models;
using Infinihub.so.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Threading;
using System.Threading.Tasks;
using System;
using OpenIddict.Core;
using OpenIddict.Models;
using Infinihub.Services;
using Infinihub.Configuration;
using Microsoft.AspNetCore.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Infinihub.so
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
                .EnableUserinfoEndpoint("/api/userinfo")
                .DisableHttpsRequirement()
     //           .UseJsonWebTokens()
                .AllowPasswordFlow()
                .AllowAuthorizationCodeFlow()
                .AllowRefreshTokenFlow()
                .EnableRequestCaching()
                .SetAccessTokenLifetime(new TimeSpan(1, 2, 0, 30, 0))
                .AddEphemeralSigningKey();

            services.AddOptions();

            services.Configure<AppearanceOptions>(Configuration.GetSection("Appearance"));

            services.AddMvc()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();
            services.AddApiVersioning(o =>
            {
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(1,0);
            });

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            services.Configure<AuthMessageSenderOptions>(Configuration);
            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, AppClaimsPrincipalFactory>();
            services.AddScoped<IBanRepository, BanRepository>();



            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireStaff", policy => policy.RequireRole("Trial Admin", "Game Admin", "Game Master", "Developer", "Host"));

                options.AddPolicy("RequireAdminScope", policy => policy.RequireClaim("scope", "admin"));
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

            app.UseWhen(context => context.Request.Path.StartsWithSegments("/api"), branch =>
            {
                branch.UseOAuthValidation();
                branch.UseExceptionHandler(errorApp =>
                {
                    errorApp.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        context.Response.ContentType = "application/json";

                        var error = context.Features.Get<IExceptionHandlerFeature>();
                        if (error != null)
                        {
                            var ex = error.Error;

                            await context.Response.WriteAsync(new ErrorDTO()
                            {
                                Code = context.Response.StatusCode,
                                Message = ex.Message
                            }.ToString(), Encoding.UTF8);
                        }
                    });
                });

                branch.UseStatusCodePages(async context =>
                {
                    context.HttpContext.Response.ContentType = "application/json";
                    switch(context.HttpContext.Response.StatusCode)
                    {
                        case 404:
                            await context.HttpContext.Response.WriteAsync(new ErrorDTO(context.HttpContext.Response.StatusCode, "The specified resource could not be found").ToString(), Encoding.UTF8);
                            break;
                        case 401:
                            await context.HttpContext.Response.WriteAsync(new ErrorDTO(context.HttpContext.Response.StatusCode, "You are not authorized to access this resource").ToString(), Encoding.UTF8);
                            break;
                        case 400:
                            await context.HttpContext.Response.WriteAsync(new ErrorDTO(context.HttpContext.Response.StatusCode, "Bad request").ToString(), Encoding.UTF8);
                            break;
                        default:
                            await context.HttpContext.Response.WriteAsync(new ErrorDTO(context.HttpContext.Response.StatusCode, "Unexpected error").ToString(), Encoding.UTF8);
                            break;
                    }
                    
                });
            });

            app.UseWhen(context => !context.Request.Path.StartsWithSegments("/api"), branch =>
            {
                branch.UseStatusCodePagesWithReExecute("/error/{0}");
            });

                app.UseStaticFiles();

            app.UseIdentity();


            


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

                // Default OAuth 2.0 applications
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
