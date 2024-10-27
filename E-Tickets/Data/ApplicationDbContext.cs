using E_Tickets.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using E_Tickets.ViewModel;

namespace E_Tickets.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public  DbSet<Category> Categories { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<E_Tickets.ViewModel.ApplicationUserVM> ApplicationUserVM { get; set; } = default!;
        public DbSet<E_Tickets.ViewModel.LoginVM> LoginVM { get; set; } = default!;
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    base.OnConfiguring(optionsBuilder);
        //    var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", true, true).Build();
        //    var connect = builder.GetConnectionString("MyConnection");
        //    optionsBuilder.UseSqlServer(connect);
        //}
    }
}
