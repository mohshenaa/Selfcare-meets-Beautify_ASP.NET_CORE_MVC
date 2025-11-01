using Microsoft.EntityFrameworkCore;
using Selfcare_meets_Beautify.Controllers;
using Selfcare_meets_Beautify.Model;
using Selfcare_meets_Beautify.Models;
using Selfcare_meets_Beautify.Services;
using Serilog;

namespace Selfcare_meets_Beautify
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();



            builder.Services.AddDbContext<SelfcareDb>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("beauty")));

            Log.Logger = new LoggerConfiguration()
  .ReadFrom.Configuration(builder.Configuration)
  .WriteTo.Console()
  .CreateLogger();

            builder.Services.AddSerilog();


            builder.Services.AddIdentity<AppUser, AppRole>()
              .AddEntityFrameworkStores<SelfcareDb>();



            builder.Services.AddScoped<ProductController>();

            builder.Services.AddFileUploader();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
