// SPDX-FileCopyrightText: 2026 Tayra Sakurai
// SPDX-License-Identifier: GPL-3.0-or-later

using Buffalo.Fakes;
using Caiman.Contexts;
using Caiman.Models;
using Caiman.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.ViewModels.Caiman
{
    [TestClass]
    public class TestCategoriesViewModel
    {
        private CategoriesViewModel? categoriesViewModel;
        private CategoryViewModel? categoryViewModel;
        private static IDbContextFactory<CaimanContext>? factory;

        [ClassInitialize]
        public static void ClassSetup(TestContext testContext)
        {
            factory = new FakeCaimanContextFactory();
            CaimanContext context = factory.CreateDbContext();
            context.Database.Migrate();
        }

        [TestInitialize]
        public void Setup()
        {
            if (factory == null)
                factory = new FakeCaimanContextFactory();

            categoriesViewModel = new(factory);
            categoryViewModel = new(factory);
        }

        [TestMethod]
        public async Task Test_AddNewLargeCategory()
        {
            if (categoriesViewModel == null ||
                categoryViewModel == null)
                return;

            await categoriesViewModel.LoadAsync();
            Assert.IsEmpty(categoriesViewModel.LargeCategories);

            await categoriesViewModel.AddCommand.ExecuteAsync(null);
            categoryViewModel.Name = "Test";

            await categoryViewModel.UpdateCommand.ExecuteAsync(null);
            Assert.IsNotEmpty(categoriesViewModel.LargeCategories);
        }
    }
}
