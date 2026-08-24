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

            // Configuração do relacionamento User -> UserMedia (1:N)
            modelBuilder.Entity<UserMedia>()
                .HasOne(um => um.User)
                .WithMany() // Se User tiver uma lista de UserMedia, coloque .WithMany(u => u.UserMedias)
                .HasForeignKey(um => um.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuração do relacionamento Media -> UserMedia (1:N)
            modelBuilder.Entity<UserMedia>()
                .HasOne(um => um.Media)
                .WithMany() // Se Media tiver uma lista de UserMedia, coloque .WithMany(m => m.UserMedias)
                .HasForeignKey(um => um.MediaId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }

}