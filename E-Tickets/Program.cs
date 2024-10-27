using E_Tickets.Repository.IRepository;
using E_Tickets.Repository;
using E_Tickets.Data;
using Microsoft.EntityFrameworkCore;
using E_Tickets.Models;
using Microsoft.AspNetCore.Identity;

namespace E_Tickets
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Register ApplicationDbContext with DbContextOptions
            builder.Services.AddDbContext<ApplicationDbContext>
                (
                    options => options.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection"))
                );

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(
                option => { option.Password.RequiredLength = 8; }
                )
               .AddEntityFrameworkStores<ApplicationDbContext>();

            // Register your generic repository
            builder.Services.AddScoped(typeof(IDbRepository<>), typeof(DbRepository<>));

            

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

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
