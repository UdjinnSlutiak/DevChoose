using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevChoose.Domain;
using DevChoose.Domain.Models;
using DevChoose.Domain.Repositories;
using DevChoose.Services;
using DevChoose.Services.Abstractions;
using DevChoose.Services.Implementations;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DevChoose.Web
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
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => //CookieAuthenticationOptions
                {
                    options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/authorization/login");
                });

            services.AddControllersWithViews();

            services.AddSession();

            services.AddDbContext<ProjectContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DevChooseDatabase")));

            services.AddTransient<IRepository<User>, Repository<User>>();
            services.AddTransient<IRepository<Message>, Repository<Message>>();
            services.AddTransient<IRepository<Dialog>, Repository<Dialog>>();

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IDialogService, DialogService>();
            services.AddTransient<IMessageService, MessageService>();
            services.AddTransient<IAuthorizationService, AuthorizationService>();
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

            app.UseSession();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=authorization}/{action=main}/{id?}");
            });
        }
    }
}
