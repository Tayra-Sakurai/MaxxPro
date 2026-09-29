// SPDX-FileCopyrightText: 2026 Tayra Sakurai
// SPDX-License-Identifier: GPL-3.0-or-later

using Cougar.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cougar.Contexts
{
    public class CougarContext : DbContext
    {
        public DbSet<AgentChatSessionDbData> Sessions { get; set; }

        public CougarContext(DbContextOptions<CougarContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AgentChatSessionDbData>()
                .ToTable("Sessions")
                .HasIndex(e => e.SessionId)
                .IsUnique();
        }
    }
}
