using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Microsoft.IdentityModel.Logging;
using SALGADBLib;
using Syncfusion.Blazor;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace SALGAPortal
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            IdentityModelEventSource.ShowPII = true;
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddDbContextPool<SALGADbContext>(
        options => options.UseSqlServer(Configuration.GetConnectionString("SALGADBConnection")));



            services.AddTransient<IDemographicsRepository, SQLDemographicsRepository>();
            services.AddTransient<IAssessmentRepository, SQLAssessmentRepository>();
            services.AddTransient<IAuditingRepository, SQLAuditingRepository>();
            services.AddTransient<IDashboardPermissionsRepository, SQLDashboardPermissionsRepository>();
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSyncfusionBlazor();
            services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme).AddMicrosoftIdentityWebApp(Configuration.GetSection("AzureAd"));
            //services.Configure<CookieAuthenticationOptions>(AzureADDefaults.CookieScheme, options =>
            //{
            //    options.Cookie.Name = "MyCookieName";
            //});

            //services.Configure<OpenIdConnectOptions>(
            //   OpenIdConnectDefaults.AuthenticationScheme, options =>
            //   {
            //       // The claim in the Jwt token where App roles are available.
            //       options.TokenValidationParameters.RoleClaimType = "roles";
            //       options.TokenValidationParameters.NameClaimType = "name";
            //   });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Only All Users Group", claim =>
                {
                    
                    claim.RequireClaim("groups", "9286bffd-e73e-4743-b1f2-bb74e7709119");
                    //claim.RequireClaim("groups", "442b39db-23c4-4634-8318-90ce1964ee45");
                });

            });

            services.AddHttpContextAccessor();
            services.AddScoped<HttpContextAccessor>();
            services.AddRazorPages().AddMvcOptions(options =>
            {
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build(); options.Filters.Add(new AuthorizeFilter(policy));
            }).AddMicrosoftIdentityUI();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NDc0NDMwQDMxMzkyZTMyMmUzMGFkZEdEYjFLbXVBVmhaQ2lFcCtGTXlVdFNFUkR6ZUZKbmpzNEUvRmd5bTQ9");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();


            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
