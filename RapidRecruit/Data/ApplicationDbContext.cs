using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RapidRecruit.Models;
using System.Reflection.Emit;

namespace RapidRecruit.Data
{
    public class ApplicationDbContext : IdentityDbContext<UserAccount>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserAccount> Accounts { get; set; }
        public DbSet<JobPosting> JobPosting { get; set; } = default!;
        public DbSet<JobApplication> JobApplication { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<JobApplication>()
                    .HasOne(j => j.User)
                    .WithMany()
                    .HasForeignKey(j => j.UserId)
                    .OnDelete(DeleteBehavior.NoAction);
        }
       
    }
}