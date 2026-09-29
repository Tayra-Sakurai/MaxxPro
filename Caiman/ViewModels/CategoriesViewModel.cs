// SPDX-FileCopyrightText: 2026 Tayra Sakurai
// SPDX-License-Identifier: GPL-3.0-or-later

using Caiman.Contexts;
using Caiman.Messages;
using Caiman.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caiman.ViewModels
{
    public partial class CategoriesViewModel : ObservableRecipient, IRecipient<LargeCategoryUpdatedMessage>, IRecipient<LargeCategoryDeletedMessage>, IRecipient<MediumCategoryUpdatedMessage>, IRecipient<MediumCategoryDeletedMessage>, IRecipient<SmallCategoryUpdatedMessage>, IRecipient<SmallCategoryDeletedMessage>
    {
        private readonly IDbContextFactory<CaimanContext> factory;

        public CategoriesViewModel(IDbContextFactory<CaimanContext> factory)
            : base()
        {
            this.factory = factory;
            LargeCategories = [];
        }

        [ObservableProperty]
        public partial ObservableCollection<LargeCategory> LargeCategories { get; set; }

        [RelayCommand(AllowConcurrentExecutions = false)]
        public async Task LoadAsync()
        {
            using CaimanContext caimanContext = await factory.CreateDbContextAsync();

            LargeCategories.Clear();

            foreach (
                LargeCategory category in
                await caimanContext
                .LargeCategories
                .Include(l => l.Children)
                .Include(l => l.Children)
                .OrderBy(l => l.Name)
                .ToListAsync())
                LargeCategories.Add(category);
        }

        [RelayCommand(AllowConcurrentExecutions = false)]
        private static async Task AddAsync(Category? category)
        {
            if (category is LargeCategory largeCategory)
            {
                MediumCategory mediumCategory = new()
                {
                    ParentId = category.Id,
                };
                WeakReferenceMessenger.Default.Send(new MediumCategoryAddedMessage(mediumCategory));
                return;
            }

            if (category is MediumCategory smallCategory)
            {
                SmallCategory smallCategory1 = new()
                {
                    ParentId = smallCategory.Id,
                };

                WeakReferenceMessenger.Default.Send(new SmallCategoryAddedMessage(smallCategory1));
                return;
            }

            LargeCategory largeCategory1 = new();
            WeakReferenceMessenger.Default.Send(new LargeCategoryAddedMessage(largeCategory1));
        }

        [RelayCommand(CanExecute = nameof(CanInvoke))]
        private static void Invoke(Category? category)
        {
            if (category is null)
                return;

            if (category is LargeCategory largeCategory)
            {
                WeakReferenceMessenger.Default.Send(new LargeCategoryInvokedMessage(largeCategory));
                return;
            }

            if (category is MediumCategory mediumCategory)
            {
                WeakReferenceMessenger.Default.Send(new MediumCategoryInvokedMessage(mediumCategory));
                return;
            }

            if (category is SmallCategory smallCategory)
            {
                WeakReferenceMessenger.Default.Send(new SmallCategoryInvokedMessage(smallCategory));
                return;
            }
        }

        private static bool CanInvoke(Category? category)
        {
            return category is not null;
        }

        public async void Receive(LargeCategoryUpdatedMessage message)
        {
            await LoadAsync();
        }

        public async void Receive(MediumCategoryUpdatedMessage message)
        {
            await LoadAsync();
        }

        public async void Receive(SmallCategoryUpdatedMessage message)
        {
            await LoadAsync();
        }

        public async void Receive(LargeCategoryDeletedMessage message)
        {
            await LoadAsync();
        }

        public async void Receive(MediumCategoryDeletedMessage message)
        {
            await LoadAsync();
        }

        public async void Receive(SmallCategoryDeletedMessage message)
        {
            await LoadAsync();
        }

        protected override void OnActivated()
        {
            Messenger.RegisterAll(this);
        }

        protected override void OnDeactivated()
        {
            Messenger.UnregisterAll(this);
        }
    }
}
