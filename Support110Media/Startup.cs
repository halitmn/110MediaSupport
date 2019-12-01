using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Support110Media.Data.Context;
using Support110Media.Utils.Log;
using System;
using System.IO;

namespace Support110Media
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddSession();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.LoginPath = "/Login/LogIn";
                options.LogoutPath = "/Login/LogOut";
            });
            services.AddDbContext<MasterContext>(op => op.UseMySQL(string.Format
                                ("server={0};port={1};database={2};user={3};password={4};",
                                         Environment.GetEnvironmentVariable("MYSQL_HOST"),
                                         Environment.GetEnvironmentVariable("MYSQL_PORT"),
                                         Environment.GetEnvironmentVariable("MYSQL_DB"),
                                         Environment.GetEnvironmentVariable("MYSQL_USER"),
                                         Environment.GetEnvironmentVariable("MYSQL_PASS"))));

            //services.AddDbContext<MasterContext>(options =>
            //                      options.UseMySQL(Configuration.GetConnectionString("DefaultConnection")));
            //services.AddSingleton<IFileProvider>(
            //new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "AudioFileUploaded"),
            //                Microsoft.Extensions.FileProviders.Physical.ExclusionFilters.None));


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, MasterContext masterContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            Logger.Configure();
            Logger.Info("Application Started.");
            masterContext.Database.Migrate();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSession();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Login}/{action=LoginIndex}/{id?}");
            });
        }
    }
}
