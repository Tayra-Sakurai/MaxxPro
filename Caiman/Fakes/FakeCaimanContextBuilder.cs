// SPDX-FileCopyrightText: 2026 Tayra Sakurai
// SPDX-License-Identifier: GPL-3.0-or-later

using Caiman.Contexts;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace Caiman.Fakes
{
    public class FakeCaimanContextBuilder : IDesignTimeDbContextFactory<CaimanContext>
    {
        public CaimanContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<CaimanContext> builder = new();

            SqliteConnection _connection = new("Filename=:memory:");
            _connection.Open();

            builder.UseSqlite(_connection);

            return new(builder.Options);
        }
    }
}
