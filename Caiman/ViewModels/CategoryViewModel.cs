// SPDX-FileCopyrightText: 2026 Tayra Sakurai
// SPDX-License-Identifier: GPL-3.0-or-later

using Caiman.Contexts;
using Caiman.Messages;
using Caiman.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caiman.ViewModels
{
    [ObservableRecipient]
    public partial class CategoryViewModel : ObservableValidator, IRecipient<LargeCategoryAddedMessage>, IRecipient<LargeCategoryInvokedMessage>, IRecipient<MediumCategoryAddedMessage>, IRecipient<MediumCategoryInvokedMessage>, IRecipient<SmallCategoryAddedMessage>, IRecipient<SmallCategoryInvokedMessage>
    {
        private readonly IDbContextFactory<CaimanContext> factory;

        private Category category;

        public CategoryViewModel(IDbContextFactory<CaimanContext> factory)
        {
            Messenger = WeakReferenceMessenger.Default;
            this.factory = factory;
            category = new LargeCategory();
            LargeCategories = [];
            MediumCategories = [];
            ErrorsChanged += CategoryViewModel_ErrorsChanged;
        }

        private void CategoryViewModel_ErrorsChanged(object? sender, System.ComponentModel.DataErrorsChangedEventArgs e)
        {
            UpdateCommand.NotifyCanExecuteChanged();
        }

        [RelayCommand(AllowConcurrentExecutions = false)]
        public async Task LoadAsync()
        {
            using CaimanContext context = await factory.CreateDbContextAsync();

            LargeCategories.Clear();

            foreach (
                LargeCategory largeCategory in
                context
                .LargeCategories
                .Include(l => l.Children)
                .ThenInclude(m => m.Children))
                LargeCategories.Add(largeCategory);

            MediumCategories.Clear();

            if (LargeCategory is LargeCategory)
                foreach (
                    MediumCategory mediumCategory in
                    LargeCategory.Children)
                    MediumCategories.Add(mediumCategory);
        }

        public async Task InitializeForExistingValueAsync(Category category)
        {
            await LoadAsync();
            using CaimanContext context = await factory.CreateDbContextAsync();

            if (category is LargeCategory largeCategory)
            {
                EntityEntry<LargeCategory> entityEntry = context.Attach(largeCategory);
                this.category = largeCategory;
            }

            if (category is MediumCategory mediumCategory)
            {
                EntityEntry<MediumCategory> entityEntry = context.Attach(mediumCategory);
                await entityEntry
                    .Reference(e => e.Parent)
                    .LoadAsync();

                this.category = mediumCategory;
            }

            if (category is SmallCategory smallCategory)
            {
                EntityEntry<SmallCategory> entityEntry = context.Attach(smallCategory);
                await entityEntry
                    .Reference(e => e.Parent)
                    .LoadAsync();

                if (smallCategory.Parent is not null)
                    await context
                        .Entry(smallCategory.Parent)
                        .Reference(e => e.Parent)
                        .LoadAsync();

                this.category = smallCategory;
            }

            OnPropertyChanged(nameof(LargeCategory));
            OnPropertyChanged(nameof(MediumCategory));
            OnPropertyChanged(nameof(Name));
            ValidateAllProperties();
        }

        [ObservableProperty]
        public partial ObservableCollection<LargeCategory> LargeCategories { get; set; }

        [ObservableProperty]
        public partial ObservableCollection<MediumCategory> MediumCategories { get; set; }

        public LargeCategory? LargeCategory
        {
            get
            {
                if (category is LargeCategory)
                    return null;
                if (category is MediumCategory mediumCategory)
                    return LargeCategories.FirstOrDefault(c => c.Id == mediumCategory.ParentId);
                if (category is SmallCategory smallCategory)
                    return LargeCategories.FirstOrDefault(c => c.Id == smallCategory.Parent!.ParentId);
                return null;
            }
            set
            {
                if (value is not null)
                {
                    if (category is MediumCategory mediumCategory)
                    {
                        if (mediumCategory.ParentId == value.Id)
                            return;

                        mediumCategory.Parent = value;
                        mediumCategory.ParentId = value.Id;
                    }

                    if (category is SmallCategory smallCategory)
                    {
                        if (smallCategory.Parent?.ParentId == value.Id)
                            return;

                        smallCategory.Parent = value.Children.FirstOrDefault();
                        smallCategory.ParentId = value.Children.FirstOrDefault()?.Id ?? smallCategory.ParentId;

                        MediumCategories.Clear();
                        foreach (MediumCategory child in value.Children)
                            MediumCategories.Add(child);
                    }
                }

                OnPropertyChanged();
            }
        }

        public MediumCategory? MediumCategory
        {
            get
            {
                if (category is SmallCategory smallCategory)
                    return MediumCategories.FirstOrDefault(m => m.Id == smallCategory.ParentId);

                return null;
            }
            set
            {
                if (value is not null)
                    if (category is SmallCategory smallCategory)
                    {
                        if (smallCategory.ParentId == value.Id)
                            return;

                        smallCategory.ParentId = value.Id;
                        smallCategory.Parent = value;
                    }

                OnPropertyChanged();
            }
        }

        [Required]
        public string Name
        {
            get => category.Name;
            set => SetProperty(category.Name, value, category, (m, v) => m.Name = v, true);
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanUpdate))]
        private async Task UpdateAsync()
        {
            if (HasErrors)
                return;

            using CaimanContext caimanContext = await factory.CreateDbContextAsync();

            if (category is SmallCategory smallCategory)
            {
                smallCategory.Parent = null;
                caimanContext.Update(smallCategory);
            }
            else if (category is MediumCategory mediumCategory)
            {
                mediumCategory.Parent = null;
                caimanContext.Update(mediumCategory);
            }
            else if (category is LargeCategory largeCategory)
                caimanContext.Update(largeCategory);
            else
                return;

            await caimanContext.SaveChangesAsync();

            if (category is LargeCategory largeCategory1)
                WeakReferenceMessenger.Default.Send(new LargeCategoryUpdatedMessage(largeCategory1));
            else if (category is MediumCategory mediumCategory1)
                WeakReferenceMessenger.Default.Send(new MediumCategoryUpdatedMessage(mediumCategory1));
            else if (category is SmallCategory smallCategory1)
                WeakReferenceMessenger.Default.Send(new SmallCategoryUpdatedMessage(smallCategory1));
        }

        private bool CanUpdate()
        {
            return !HasErrors;
        }

        [RelayCommand(AllowConcurrentExecutions = false)]
        private async Task RemoveAsync()
        {
            using CaimanContext context = await factory.CreateDbContextAsync();

            if (category is LargeCategory largeCategory)
            {
                context.Remove(largeCategory);
                await context.SaveChangesAsync();
                WeakReferenceMessenger.Default.Send(new LargeCategoryDeletedMessage(largeCategory));
            }
            else if (category is MediumCategory mediumCategory)
            {
                context.Remove(mediumCategory);
                await context.SaveChangesAsync();
                WeakReferenceMessenger.Default.Send(new MediumCategoryDeletedMessage(mediumCategory));
            }
            else if (category is SmallCategory smallCategory)
            {
                context.Remove(smallCategory);
                await context.SaveChangesAsync();
                WeakReferenceMessenger.Default.Send(new SmallCategoryDeletedMessage(smallCategory));
            }
            else
                return;
        }

        public async void Receive(LargeCategoryAddedMessage message)
        {
            await InitializeForExistingValueAsync(message.Value);
        }

        public async void Receive(LargeCategoryInvokedMessage message)
        {
            await InitializeForExistingValueAsync(message.Value);
        }

        public async void Receive(MediumCategoryAddedMessage message)
        {
            await InitializeForExistingValueAsync(message.Value);
        }

        public async void Receive(MediumCategoryInvokedMessage message)
        {
            await InitializeForExistingValueAsync(message.Value);
        }

        public async void Receive(SmallCategoryAddedMessage message)
        {
            await InitializeForExistingValueAsync(message.Value);
        }

        public async void Receive(SmallCategoryInvokedMessage message)
        {
            await InitializeForExistingValueAsync(message.Value);
        }

        protected virtual partial void OnActivated()
        {
            Messenger.RegisterAll(this);
        }

        protected virtual partial void OnDeactivated()
        {
            Messenger.UnregisterAll(this);
        }
    }
}
