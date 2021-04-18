using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Infrastructure;
using Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Web
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("AganceStoreDB"));
            });


            services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
             .AddEntityFrameworkStores<DataContext>().AddDefaultTokenProviders().AddDefaultUI();

            //services.AddIdentity<ApplicationUser, IdentityRole>()
            //    .AddEntityFrameworkStores<DataContext>()
            //       .AddDefaultTokenProviders()
            //                .AddDefaultUI();


          
            services.AddControllersWithViews();

            services.AddAuthentication().AddGoogle(options =>
            {
                options.ClientId = "984067988014-uu7njn7bdvnug1o29ukcuihqrf2qqgba.apps.googleusercontent.com";
                options.ClientSecret = "3CUz3-8-4bV1w0oeKKfMKQWd";
            })
            .AddFacebook(options =>
            {
                options.AppId = "260516845560295";
                options.AppSecret = "b420384623e0a7e5d56bebfcca17ae63";
            });
             


            services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>)) ;
            services.AddRazorPages();
   
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    "defaultRoute",
                    "{controller=Home}/{action=Index}/{id?}"
                    );
                endpoints.MapRazorPages();
            });
        }
    }
}
