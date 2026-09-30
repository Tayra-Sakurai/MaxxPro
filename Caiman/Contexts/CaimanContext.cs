// SPDX-FileCopyrightText: 2026 Tayra Sakurai
// SPDX-License-Identifier: GPL-3.0-or-later

using Caiman.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Caiman.Contexts
{
    public class CaimanContext : DbContext
    {
        public DbSet<LargeCategory> LargeCategories { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<MediumCategory> MediumCategories { get; set; }
        public DbSet<SmallCategory> SmallCategories { get; set; }
        public DbSet<Place> Places { get; set; }

        public CaimanContext(DbContextOptions<CaimanContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>()
                .Property(i => i.Life)
                .HasDefaultValueSql("datetime('now') || 'Z'");
            modelBuilder.Entity<LargeCategory>()
                .HasBaseType<Category>();
            modelBuilder.Entity<MediumCategory>()
                .HasBaseType<Category>();
            modelBuilder.Entity<SmallCategory>()
                .HasBaseType<Category>();
            modelBuilder.Entity<Place>();
        }
    }
}
