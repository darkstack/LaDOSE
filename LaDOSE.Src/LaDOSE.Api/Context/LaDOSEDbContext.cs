
using System;
using LaDOSE.Entity;
using Microsoft.EntityFrameworkCore;
namespace LaDOSE.Api.Context
{
    public class LaDOSEDbContext : DbContext
    {
        public DbSet<Game> Game { get; set; }

        public LaDOSEDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}