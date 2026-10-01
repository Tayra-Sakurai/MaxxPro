// SPDX-FileCopyrightText: 2026 Tayra Sakurai
// SPDX-License-Identifier: GPL-3.0-or-later

using Cougar.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Fakes
{
    public class FakeCougarContextFactory : IDbContextFactory<CougarContext>
    {
        public CougarContext CreateDbContext()
        {
            DbContextOptionsBuilder<CougarContext> optionsBuilder = new();
            optionsBuilder.UseInMemoryDatabase("CougarDb");

            return new(optionsBuilder.Options);
        }
    }
}
