using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sheryl_Project.Models;

namespace Sheryl_Project.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Booking> Bookings { get; set; } // Renamed to follow convention

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Additional configurations for Booking model
            builder.Entity<Booking>(entity =>
            {
                entity.Property(e => e.BookingStatus)
                      .HasDefaultValue("Pending"); // Example: Default status
            });
        }
    }
}
