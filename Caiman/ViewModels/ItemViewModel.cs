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
    public partial class ItemViewModel : ObservableValidator, IRecipient<ItemAddedMessage>, IRecipient<ItemInvokedMessage>
    {
        private readonly IDbContextFactory<CaimanContext> factory;
        private Item item;

        public ItemViewModel(IDbContextFactory<CaimanContext> factory)
        {
            Messenger = WeakReferenceMessenger.Default;
            this.factory = factory;
            item = new();
            LargeCategories = [];
            MediumCategories = [];
            SmallCategories = [];
            Places = [];
            ErrorsChanged += ItemViewModel_ErrorsChanged;
        }

        private void ItemViewModel_ErrorsChanged(object? sender, System.ComponentModel.DataErrorsChangedEventArgs e)
        {
            UpdateCommand.NotifyCanExecuteChanged();
        }

        [ObservableProperty]
        public partial ObservableCollection<LargeCategory> LargeCategories { get; set; }

        public LargeCategory? LargeCategory
        {
            get => LargeCategories.FirstOrDefault(l => l.Id == MediumCategory?.ParentId);
            set
            {
                if (MediumCategory?.ParentId != value?.Id)
                {
                    if (value is not null)
                        Load(value);

                    OnPropertyChanged();
                    OnPropertyChanged(nameof(MediumCategory));
                    OnPropertyChanged(nameof(SmallCategory));
                }
            }
        }

        [ObservableProperty]
        public partial ObservableCollection<MediumCategory> MediumCategories { get; set; }

        public MediumCategory? MediumCategory
        {
            get => MediumCategories.FirstOrDefault(l => l.Id == SmallCategory?.ParentId);
            set
            {
                if (SmallCategory?.ParentId != value?.Id)
                {
                    if (value is not null)
                        Load(value);

                    OnPropertyChanged();
                    OnPropertyChanged(nameof(SmallCategory));
                }
            }
        }

        [ObservableProperty]
        public partial ObservableCollection<SmallCategory> SmallCategories { get; set; }

        [Required]
        public SmallCategory? SmallCategory
        {
            get => SmallCategories.FirstOrDefault(l => l.Id == item.CategoryId);
            set
            {
                ValidateProperty(value);

                if (item.CategoryId != value?.Id)
                {
                    if (value is null)
                        return;

                    item.CategoryId = value.Id;
                    OnPropertyChanged();
                }
            }
        }

        [ObservableProperty]
        public partial ObservableCollection<Place> Places { get; set; }

        [Required]
        public Place? Place
        {
            get => Places.FirstOrDefault(p => p.Id == item.PlaceId);
            set
            {
                ValidateProperty(value);

                if (item.PlaceId != value?.Id)
                {
                    if (value is null)
                        return;

                    item.PlaceId = value.Id;
                    OnPropertyChanged();
                }
            }
        }

        [RelayCommand(AllowConcurrentExecutions = false)]
        public async Task LoadAsync()
        {
            using CaimanContext context = await factory.CreateDbContextAsync();

            LargeCategories.Clear();
            MediumCategories.Clear();
            SmallCategories.Clear();
            Places.Clear();

            foreach (
                Place place in
                await context
                .Places
                .OrderBy(p => p.Id)
                .ToListAsync())
                Places.Add(place);

            foreach (
                LargeCategory large in
                await context.LargeCategories
                .OrderBy(l => l.Id)
                .ToListAsync())
                LargeCategories.Add(large);

            EntityEntry<Item> entry = context.Attach(item);

            await entry
                .Reference(i => i.Category)
                .LoadAsync();

            LargeCategory? largeCategory = null;

            if (item.Category is not null)
            {
                await context.Entry(item.Category)
                    .Reference(c => c.Parent)
                    .LoadAsync();

                largeCategory = await context.LargeCategories.SingleAsync(l => l.Id == item.Category.Parent!.ParentId);

                MediumCategory? mediumCategory = item.Category.Parent;

                if (mediumCategory is not null)
                {
                    await context.Entry(largeCategory)
                        .Collection(l => l.Children)
                        .LoadAsync();

                    foreach (
                        MediumCategory mediumCategory1 in
                        largeCategory.Children)
                        MediumCategories.Add(mediumCategory1);

                    await context.Entry(mediumCategory)
                        .Collection(m => m.Children)
                        .LoadAsync();

                    foreach (
                        SmallCategory smallCategory1 in
                        mediumCategory.Children)
                        SmallCategories.Add(smallCategory1);

                    OnPropertyChanged(nameof(LargeCategory));
                    OnPropertyChanged(nameof(MediumCategory));
                    OnPropertyChanged(nameof(SmallCategory));
                }
            }
        }

        private void Load(LargeCategory largeCategory)
        {
            using (CaimanContext context = factory.CreateDbContext())
            {
                context.Attach(largeCategory)
                    .Collection(l => l.Children)
                    .Load();

                MediumCategories.Clear();
                foreach (
                    MediumCategory mediumCategory in
                    largeCategory.Children)
                    MediumCategories.Add(mediumCategory);
            }

            MediumCategory = largeCategory.Children.FirstOrDefault();
        }

        private void Load(MediumCategory mediumCategory)
        {
            using (CaimanContext context = factory.CreateDbContext())
            {
                EntityEntry<MediumCategory> entry = context.Attach(mediumCategory);
                entry.Collection(m => m.Children).Load();

                SmallCategories.Clear();

                foreach (SmallCategory smallCategory in mediumCategory.Children)
                    SmallCategories.Add(smallCategory);
            }

            SmallCategory = mediumCategory.Children.FirstOrDefault();
        }

        [Required]
        public string Name
        {
            get => item.Name;
            set => SetProperty(item.Name, value, item, (m, v) => m.Name = v, true);
        }

        public string Description
        {
            get => item.Description;
            set => SetProperty(item.Description, value, item, (m, v) => m.Description = v, true);
        }

        [Required]
        public DateTimeOffset? Date
        {
            get => item.Life.Date;
            set
            {
                ValidateProperty(value);

                if (value is not DateTimeOffset dateTimeOffset)
                    return;

                if (dateTimeOffset != item.Life.Date)
                {
                    TimeSpan timeOfDay = item.Life.TimeOfDay;

                    item.Life = dateTimeOffset.Date;
                    item.Life += timeOfDay;
                    OnPropertyChanged();
                }
            }
        }

        [Required]
        public TimeSpan? Time
        {
            get => item.Life.TimeOfDay;
            set
            {
                ValidateProperty(value);

                if (value is not TimeSpan timeOfDay)
                    return;

                if (timeOfDay != item.Life.TimeOfDay)
                {
                    item.Life = item.Life.Date;
                    item.Life += timeOfDay;
                    OnPropertyChanged();
                }
            }
        }

        private async Task InitializeForExistingValueAsync(Item item)
        {
            this.item = item;
            await LoadAsync();
            ValidateAllProperties();
        }

        public async void Receive(ItemAddedMessage message)
        {
            await InitializeForExistingValueAsync(message.Value);
        }

        public async void Receive(ItemInvokedMessage message)
        {
            await InitializeForExistingValueAsync(message.Value);
        }

        protected virtual void OnActivated()
        {
            Messenger.RegisterAll(this);
        }

        protected virtual void OnDeactivated()
        {
            Messenger.UnregisterAll(this);
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanUpdate))]
        private async Task UpdateAsync()
        {
            if (HasErrors)
                return;

            item.Category = null;
            item.Place = null;

            using (CaimanContext context = await factory.CreateDbContextAsync())
            {
                context.Update(item);
                await context.SaveChangesAsync();
            }

            WeakReferenceMessenger.Default.Send(new ItemUpdatedMessage(item));
        }

        private bool CanUpdate()
        {
            return !HasErrors;
        }

        [RelayCommand(AllowConcurrentExecutions = false)]
        private async Task RemoveAsync()
        {
            item.Place = null;
            item.Category = null;

            using (CaimanContext context = await factory.CreateDbContextAsync())
            {
                context.Remove(item);
                await context.SaveChangesAsync();
            }

            WeakReferenceMessenger.Default.Send(new ItemDeletedMessage(item));
        }
    }
}
