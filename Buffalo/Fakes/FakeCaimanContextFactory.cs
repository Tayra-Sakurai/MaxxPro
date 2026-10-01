// SPDX-FileCopyrightText: 2026 Tayra Sakurai
// SPDX-License-Identifier: GPL-3.0-or-later

using Caiman.Contexts;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Fakes
{
    public class FakeCaimanContextFactory : IDbContextFactory<CaimanContext>
    {
        public CaimanContext CreateDbContext()
        {
            DbContextOptionsBuilder<CaimanContext> optionsBuilder = new();

            SqliteConnection conn = new("Filename=:memory:");
            conn.Open();

            optionsBuilder.UseSqlite(conn);

            return new(optionsBuilder.Options);
        }
    }
}
