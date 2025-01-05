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
        public DbSet<Conversation> Conversation { get; set; } = default!;
        public DbSet<Message> Message { get; set; } = default!;


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<JobApplication>()
                    .HasOne(j => j.User)
                    .WithMany()
                    .HasForeignKey(j => j.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

            // conversation deleted when job application is deleted
            builder.Entity<Conversation>()
                    .HasOne(c => c.Candidate)
                    .WithMany()
                    .HasForeignKey(c => c.CandidateId)
                    .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Conversation>()
                    .HasOne(c => c.HiringManager)
                    .WithMany()
                    .HasForeignKey(c => c.HiringManagerId)
                    .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Message>()
                .HasOne(m => m.User)
                .WithMany()
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.NoAction);

        }
       
    }
}