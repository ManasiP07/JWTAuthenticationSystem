using JWTAuthenticationSystem.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace JWTAuthenticationSystem.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*
 
             RefreshToken
                 │
                 │ belongs to
                 ▼
               User

            One User
               ↓
            can have
               ↓
            Many RefreshTokens

             */
            base.OnModelCreating(modelBuilder);

            // User -> RefreshTokens
            modelBuilder.Entity<RefreshToken>()
                .HasOne(rt => rt.User)
                .WithMany()
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Refresh token lookup should be unique
            modelBuilder.Entity<RefreshToken>()
                .HasIndex(rt => rt.TokenHash)
                .IsUnique();

            // Useful when retrieving tokens for a user
            modelBuilder.Entity<RefreshToken>()
                .HasIndex(rt => rt.UserId);

        }
    }
}