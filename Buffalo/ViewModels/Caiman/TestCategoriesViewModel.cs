// SPDX-FileCopyrightText: 2026 Tayra Sakurai
// SPDX-License-Identifier: GPL-3.0-or-later

using Buffalo.Fakes.Caiman;
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

        [TestInitialize]
        public void Setup()
        {
            if (factory == null)
                factory = new FakeCaimanContextFactory();

            using (CaimanContext context = factory.CreateDbContext())
            {
                context.Database.Migrate();
            }
            categoriesViewModel = new(factory);
            categoryViewModel = new(factory);
            categoriesViewModel.IsActive = true;
            categoryViewModel.IsActive = true;
        }

        [TestCleanup]
        public void CleanUp()
        {
            if (factory != null)
            {
                using CaimanContext context = factory.CreateDbContext();
                context.Database.EnsureDeleted();
            }
            categoriesViewModel?.IsActive = false;
            categoryViewModel?.IsActive = false;
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
            await Task.Delay(1000);
            Assert.IsNotEmpty(categoriesViewModel.LargeCategories);
        }

        [TestMethod]
        public async Task Test_CantAddNewLargeCategoryWithInvalidName()
        {
            if (categoriesViewModel == null ||
                categoryViewModel == null)
                Assert.Fail("Setup incomplete.");

            await categoriesViewModel.LoadAsync();
            Assert.IsEmpty(categoriesViewModel.LargeCategories);

            await categoriesViewModel.AddCommand.ExecuteAsync(null);

            await categoryViewModel.UpdateCommand.ExecuteAsync(null);
            Assert.IsEmpty(categoriesViewModel.LargeCategories);
        }
    }
}
