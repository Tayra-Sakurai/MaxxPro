// SPDX-FileCopyrightText: 2026 Tayra Sakurai
// SPDX-License-Identifier: GPL-3.0-or-later

using Cougar.Contexts;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cougar.Fakes
{
    public class FakeCougarContextFactory : IDesignTimeDbContextFactory<CougarContext>
    {
        public CougarContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<CougarContext> optionsBuilder = new();

            SqliteConnection connection = new("Filename=:memory:");
            connection.Open();

            optionsBuilder.UseSqlite(connection);

            return new(optionsBuilder.Options);
        }
    }
}
