using KnifeCompany.DataAccess.Data;
using KnifeCompany.DataAccess.Repository;
using KnifeCompany.DataAccess.Repository.IRepository;
using KnifeCompany.Models;
using KnifeCompany.Utility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KnifeCompany
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();

            //services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //    .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddIdentity<IdentityUser, IdentityRole>().AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            //services.AddDefaultIdentity<IdentityUser>()
            //    .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
            });

            //services.AddSingleton<IEmailSender, EmailSender>();
            services.AddTransient<IEmailSender, EmailSender>();
            services.Configure<AppSettings>(options =>
            {
                //options.SmtpHost = "smtp.gmail.com";
                //options.SmtpPort = 587;
                //options.SmtpUser = "alexanderwanengkirtyo@gmail.com";
                //options.SmtpPass = "Paladins1";
                //options.Sender_Email = "noreply@mytest.com";
                //options.Sender_Name = "abw";

                options.SmtpHost = Configuration["ExternalProviders:MailKit:SMTP:Address"];
                options.SmtpPort = Convert.ToInt32(Configuration["ExternalProviders:MailKit:SMTP:Port"]);
                options.SmtpUser = Configuration["ExternalProviders:MailKit:SMTP:Account"];
                options.SmtpPass = Configuration["ExternalProviders:MailKit:SMTP:Password"];
                options.Sender_Email = Configuration["ExternalProviders:MailKit:SMTP:SenderEmail"];
                options.Sender_Name = Configuration["ExternalProviders:MailKit:SMTP:SenderName"];
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
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
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{area=customer}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
