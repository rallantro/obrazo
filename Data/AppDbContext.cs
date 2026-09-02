using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using obrazo.Models.Entities;

namespace obrazo.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Media> Medias { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserMedia> UserMedias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserMedia>()
                .HasOne(um => um.User)
                .WithMany()
                .HasForeignKey(um => um.UserId)
                .OnDelete(DeleteBehavior.Cascade);

           
            modelBuilder.Entity<UserMedia>()
                .HasOne(um => um.Media)
                .WithMany() 
                .HasForeignKey(um => um.MediaId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }

}