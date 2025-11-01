using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Selfcare_meets_Beautify.Model;

namespace Selfcare_meets_Beautify.Models
{
    public class SelfcareDb : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public DbSet<Brand> Brands { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }
        public SelfcareDb(DbContextOptions<SelfcareDb> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder build)
        {
            base.OnModelCreating(build);

            //build.Entity<Brand>().HasData(
            //    new Brand() { Id = 1, Name = "Ordinary" },
            //    new Brand() { Id = 2, Name = "AXIS-Y" },
            //    new Brand() { Id = 3, Name = "RoHTo" });

            build.Entity<Category>().HasData(
        new Category() { Id = 1, Type = "Sunscreen" },
        new Category() { Id = 2, Type = "Moisturizer" },
        new Category() { Id = 3, Type = "Cleanser" },
        new Category() { Id = 4, Type = "Serum" },
        new Category() { Id = 5, Type = "Toner" });
        }
    }
}
