using Microsoft.EntityFrameworkCore;
using Support110Media.Data.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Support110Media.Data.Context
{
    public class MasterContext : DbContext
    {
        public MasterContext(DbContextOptions<MasterContext> option) : base(option)
        {
        }
        public DbSet<CostumerModel> CostumerModel { get; set; }

        public DbSet<FileModel> FileModel { get; set; }

        public DbSet<UserModel> UserModel { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CostumerModel>()
                .HasMany(m => m.FileModel)
                .WithOne(c => c.CostumerModel)
                .HasForeignKey(k => k.CostumerId);       
        }
    }
}
