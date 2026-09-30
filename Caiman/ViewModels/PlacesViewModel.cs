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
using System.Text;
using System.Threading.Tasks;

namespace Caiman.ViewModels
{
    public partial class PlacesViewModel : ObservableRecipient, IRecipient<PlaceUpdatedMessage>, IRecipient<PlaceDeletedMessage>
    {
        private readonly IDbContextFactory<CaimanContext> factory;

        public PlacesViewModel(IDbContextFactory<CaimanContext> factory)
            : base()
        {
            this.factory = factory;
            Places = [];
        }

        protected override void OnActivated()
        {
            Messenger.Register<PlaceDeletedMessage>(this);
            Messenger.Register<PlaceUpdatedMessage>(this);
        }

        protected override void OnDeactivated()
        {
            Messenger.UnregisterAll(this);
        }

        [ObservableProperty]
        public partial ObservableCollection<Place> Places { get; set; }

        [RelayCommand(AllowConcurrentExecutions = false)]
        public async Task LoadAsync()
        {
            using CaimanContext context = await factory.CreateDbContextAsync();

            Places.Clear();

            await foreach (
                Place place in
                context
                .Places
                .Include(p => p.Items)
                .AsAsyncEnumerable())
                Places.Add(place);
        }

        [RelayCommand(CanExecute = nameof(CanInvoke))]
        private static void Invoke(Place? place)
        {
            if (place is null)
                return;

            WeakReferenceMessenger.Default.Send(new PlaceInvokedMessage(place));
        }

        [RelayCommand(AllowConcurrentExecutions = false)]
        private async Task AddAsync()
        {
            Place place = new();
            WeakReferenceMessenger.Default.Send(new PlaceAddedMessage(place));
        }

        private static bool CanInvoke(Place? place)
        {
            return place is not null;
        }

        public async void Receive(PlaceUpdatedMessage message)
        {
            await LoadAsync();
        }

        public async void Receive(PlaceDeletedMessage message)
        {
            await LoadAsync();
        }
    }
}
