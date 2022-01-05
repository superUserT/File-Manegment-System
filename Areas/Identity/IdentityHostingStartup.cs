using System;
using FreshGoldPractice2.Areas.Identity.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(FreshGoldPractice2.Areas.Identity.IdentityHostingStartup))]
namespace FreshGoldPractice2.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<FreshGoldPractice2IdentityDbContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("FreshGoldPractice2IdentityDbContextConnection")));

               // services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                 //   .AddEntityFrameworkStores<FreshGoldPractice2IdentityDbContext>();
            });
        }
    }
}