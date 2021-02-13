using JobsityChatroom.Areas.Identity.Data;
using JobsityChatroom.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(JobsityChatroom.Areas.Identity.IdentityHostingStartup))]
namespace JobsityChatroom.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<JobsityChatroomContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("JobsityChatroomContextConnection")));

                services.AddDefaultIdentity<JobsityChatroomUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<JobsityChatroomContext>();
            });
        }
    }
}