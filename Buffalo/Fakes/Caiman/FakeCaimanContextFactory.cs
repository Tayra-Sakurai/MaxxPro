// SPDX-FileCopyrightText: 2026 Tayra Sakurai
// SPDX-License-Identifier: GPL-3.0-or-later

using Caiman.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Windows.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Fakes.Caiman
{
    public class FakeCaimanContextFactory : IDbContextFactory<CaimanContext>
    {
        public CaimanContext CreateDbContext()
        {
            string dbPath = System.IO.Path.Join(ApplicationData.GetDefault().LocalFolder.Path, "Db.db");
            DbContextOptionsBuilder<CaimanContext> optionsBuilder = new();

            optionsBuilder.UseSqlite($"Data Source={dbPath}");

            return new(optionsBuilder.Options);
        }
    }
}
