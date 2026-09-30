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
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace Caiman.ViewModels
{
    [ObservableRecipient]
    public partial class PlaceViewModel : ObservableValidator, IRecipient<PlaceAddedMessage>, IRecipient<PlaceInvokedMessage>
    {
        private readonly IDbContextFactory<CaimanContext> factory;
        private Place place;

        public PlaceViewModel(IDbContextFactory<CaimanContext> factory)
        {
            Messenger = WeakReferenceMessenger.Default;
            this.factory = factory;
            place = new();
            ErrorsChanged += PlaceViewModel_ErrorsChanged;
        }

        private void PlaceViewModel_ErrorsChanged(object? sender, System.ComponentModel.DataErrorsChangedEventArgs e)
        {
            UpdateCommand.NotifyCanExecuteChanged();
        }

        protected virtual partial void OnActivated()
        {
            Messenger.RegisterAll(this);
        }

        protected virtual partial void OnDeactivated()
        {
            Messenger.UnregisterAll(this);
        }

        [Required]
        public string Name
        {
            get => place.Name;
            set => SetProperty(place.Name, value, place, (m, v) => m.Name = v, true);
        }

        public async Task InitializeForExistingValueAsync(Place place)
        {
            using CaimanContext context = await factory.CreateDbContextAsync();

            EntityEntry<Place> entry = context.Attach(place);
            await entry
                .Collection(e => e.Items)
                .LoadAsync();

            this.place = place;

            OnPropertyChanged(nameof(Name));
            UpdateCommand.NotifyCanExecuteChanged();
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanUpdate))]
        private async Task UpdateAsync()
        {
            if (HasErrors)
                return;

            using (CaimanContext context = await factory.CreateDbContextAsync())
            {
                context.Update(place);
                await context.SaveChangesAsync();
            }

            WeakReferenceMessenger.Default.Send(new PlaceUpdatedMessage(place));
        }

        private bool CanUpdate()
        {
            return !HasErrors;
        }

        [RelayCommand(AllowConcurrentExecutions = false)]
        private async Task RemoveAsync()
        {
            using (CaimanContext context = await factory.CreateDbContextAsync())
            {
                context.Remove(place);
                await context.SaveChangesAsync();
            }

            WeakReferenceMessenger.Default.Send(new PlaceDeletedMessage(place));
        }

        public async void Receive(PlaceAddedMessage message)
        {
            await InitializeForExistingValueAsync(message.Value);
        }

        public async void Receive(PlaceInvokedMessage message)
        {
            await InitializeForExistingValueAsync(message.Value);
        }
    }
}
