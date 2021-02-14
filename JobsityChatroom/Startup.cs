using JobsityChatroom.Entities;
using JobsityChatroom.Data;
using JobsityChatroom.Properties;
using JobsityChatroom.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;

namespace JobsityChatroom
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
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(60);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            // DBContext
            services.AddDbContext<JobsityChatroomContext>(options =>
                    options.UseSqlServer(
                        Configuration.GetConnectionString("JobsityChatroomContextConnection")));
            // .Net Identity
            services.AddDefaultIdentity<JobsityChatroomUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<JobsityChatroomContext>();
            services.PostConfigure<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme,
                    opt =>
                    {
                        opt.LoginPath = "/Account/Login";
                    });

            // RabbitQM Setup
            var rabbitMQSettings = new RabbitMQSettings();
            Configuration.GetSection("RabbitMQ").Bind(rabbitMQSettings);
            services.AddSingleton<RabbitMQSettings>(rabbitMQSettings);
            services.AddSingleton<ConnectionFactory>(x => new ConnectionFactory() { HostName = rabbitMQSettings.HostName });
            services.AddSingleton<ChatroomHub>();

            // Services injection
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IRabbitMQService, RabbitMQService>();

            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddSignalR();
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
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
                endpoints.MapHub<ChatroomHub>("/chatroomhub");
            });
        }
    }
}
