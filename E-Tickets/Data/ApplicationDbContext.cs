using E_Tickets.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Tickets.Data
{
    public class ApplicationDbContext : DbContext
    {
        public  DbSet<Category> Categories { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<Movie> Movies { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", true, true).Build();
            var connect = builder.GetConnectionString("MyConnection");
            optionsBuilder.UseSqlServer(connect);
        }
    }
}
