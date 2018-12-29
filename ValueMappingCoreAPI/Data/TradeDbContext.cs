using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ValueMappingCoreAPI.Models;

namespace ValueMappingCoreAPI.Data
{

    public partial class TradeDbContext : IdentityDbContext<ApplicationUser>
    {
        public TradeDbContext(DbContextOptions<TradeDbContext> options) : base(options)
        {
        }

        public virtual DbSet<ValueMaps> ValueMaps { get; set; }

        public virtual DbSet<OperateSystem> OperateSystem { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(@"Server=DELL;Database=Trade;UID=sa;PWD=");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OperateSystem>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.SystemName)
                    .HasColumnName("SystemName")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.TransdateTime).HasColumnType("datetime");

                entity.Property(e => e.Uid)
                            .HasColumnName("UID")
                            .HasMaxLength(50)
                            .IsUnicode(false);
            });


            modelBuilder.Entity<ValueMaps>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.SystemId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TransdateTime).HasColumnType("datetime");

                entity.Property(e => e.Uid)
                    .HasColumnName("UID")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.ValuationFunction)
                    .IsRequired()
                    .HasColumnName("Valuation_Function")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });
        }
    }
}
