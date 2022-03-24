using E_Commerce.Areas.Admin.Repository;
using E_Commerce.Data;
using E_Commerce.Helpers;
using E_Commerce.Repository;
using E_Commerce.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce
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
            services.AddDbContext<ECommerceContext>(options=>options.UseSqlServer(Configuration.GetConnectionString("DefaultConnectionString")));            
            services.AddControllersWithViews();

            //reporitories registration here

            services.AddScoped<IAccountRepository, AccountRepository>();



            services.ConfigureApplicationCookie(configure => {
                configure.LoginPath = Configuration["Application:LoginPath"]; 
            });
                //Configuration["Application:LoginPath"];

            services.Configure<IdentityOptions>(options =>
            {

                /*                we can configure here password and identity framwork configuration like tokan
                 *                
                 *                options.Password.RequireDigit = true;
                                options.Password.RequireDigit = true;*/
                options.SignIn.RequireConfirmedEmail = true;

            });
            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ApplicationUserCalimsPrincipalFactory>();


            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ECommerceContext>().AddDefaultTokenProviders();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IProductRepository, ProductRepository>();

            services.AddScoped<ICategoryRepository, CategoryRepository>();

            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<ICartOrderRepository, CartOrderRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
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
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllerRoute(
                      name: "admin",
                      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                    );
                });

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
