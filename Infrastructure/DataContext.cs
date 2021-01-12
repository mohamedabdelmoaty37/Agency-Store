using Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure
{
    public class DataContext : IdentityDbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Contect>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<PortfolioItem>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<TeamMember>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<Services>().Property(x => x.Id).HasDefaultValueSql("NEWID()");



            //modelBuilder.Entity<Owner>().HasData(
            //    new Owner()
            //    {
            //        Id = Guid.NewGuid(),
            //        Avatar = "avatar.jpg",
            //        FullName = "Mohamed Abd-ElMoaty ",
            //        Profil = "Microsoft MVP / .NET Consultant"
            //    }
            //    );
        }

        public DbSet<Contect> Contect { get; set; }
        public DbSet<TeamMember> TeamMember { get; set; }
        public DbSet<Services> Services { get; set; }
        public DbSet<PortfolioItem> PortfolioItems { get; set; }
    }
}
