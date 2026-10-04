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
    public partial class ItemsViewModel : ObservableRecipient, IRecipient<ItemDeletedMessage>, IRecipient<ItemUpdatedMessage>
    {
        private readonly IDbContextFactory<CaimanContext> factory;

        public ItemsViewModel(IDbContextFactory<CaimanContext> factory)
            : base()
        {
            this.factory = factory;
            Items = [];
        }

        [ObservableProperty]
        public partial ObservableCollection<Item> Items { get; set; }

        [RelayCommand(AllowConcurrentExecutions = false)]
        public async Task LoadAsync()
        {
            using CaimanContext context = await factory.CreateDbContextAsync();

            Items.Clear();

            foreach (
                Item item in
                (await context
                .Items
                .Include(i => i.Category)
                .ThenInclude(s => s!.Parent)
                .ThenInclude(s => s!.Parent)
                .Include(i => i.Place)
                .AsNoTracking()
                .AsSplitQuery()
                .ToListAsync())
                .OrderBy(i => i.Life)
                .ThenBy(i => i.Name)
                .ToList())
                Items.Add(item);
        }

        [RelayCommand]
        private static void Add()
        {
            WeakReferenceMessenger.Default.Send(new ItemAddedMessage(new()));
        }

        [RelayCommand(CanExecute = nameof(CanInvoke))]
        private static void Invoke(Item? item)
        {
            if (item == null)
                return;

            WeakReferenceMessenger.Default.Send(new ItemInvokedMessage(item));
        }

        private static bool CanInvoke(Item? item)
        {
            return item is not null;
        }

        public async void Receive(ItemDeletedMessage message)
        {
            await LoadAsync();
        }

        public async void Receive(ItemUpdatedMessage message)
        {
            await LoadAsync();
        }

        protected override void OnActivated()
        {
            Messenger.Register<ItemDeletedMessage>(this);
            Messenger.Register<ItemUpdatedMessage>(this);
        }

        protected override void OnDeactivated()
        {
            Messenger.UnregisterAll(this);
        }
    }
}
