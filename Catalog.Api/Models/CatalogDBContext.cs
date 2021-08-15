using System;
using Catalog.Api.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Catalog.Api.Models {
	public partial class CatalogDBContext : DbContext {
		public CatalogDBContext() {
		}

		public CatalogDBContext(DbContextOptions<CatalogDBContext> options)
		    : base(options) {
		}

		public virtual DbSet<Item> Items { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
			if (!optionsBuilder.IsConfigured) {
				optionsBuilder.UseSqlServer("");
			}
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

			modelBuilder.Entity<Item>(entity => {
				entity.Property(e => e.Name)
				    .IsRequired()
				    .HasMaxLength(10)
				    .IsFixedLength(true);

				entity.Property(e => e.ValidityEnd).HasColumnType("datetime");

				entity.Property(e => e.ValidityStart).HasColumnType("datetime");
			});

			OnModelCreatingPartial(modelBuilder);
		}

		partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
	}
}
