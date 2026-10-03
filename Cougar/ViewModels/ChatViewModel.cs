// SPDX-FileCopyrightText: 2026 Tayra Sakurai
// SPDX-License-Identifier: GPL-3.0-or-later

using Caiman.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Cougar.Enums;
using Cougar.Tools;
using Microsoft.Extensions.AI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cougar.ViewModels
{
    public partial class ChatViewModel : ObservableObject
    {
        private readonly IChatClient client;

        public ChatViewModel(IChatClient client)
        {
            this.client = client;
            Messages = [];
            Prompt = string.Empty;
            Requests = [];
            State = ChatClientState.NotBegun;
        }

        [ObservableProperty]
        public partial ObservableCollection<ChatMessage> Messages { get; set; }

        [ObservableProperty]
        public partial ObservableCollection<ToolApprovalRequestContent> Requests { get; set; }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SendMessageCommand))]
        public partial string Prompt { get; set; }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SendMessageCommand))]
        public partial ChatClientState State { get; set; }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanSendMessage))]
        private async Task SendMessageAsync()
        {
            ChatMessage message = new(ChatRole.User, Prompt);
            Messages.Add(message);
            State = ChatClientState.Loading;
            ChatResponse response = await client.GetResponseAsync(message);
            Requests.Clear();
            if (response.FinishReason == ChatFinishReason.ToolCalls)
            {
                State = ChatClientState.WaitingForApproval;
                foreach (ChatMessage chatMessage in response.Messages)
                    foreach (
                        ToolApprovalRequestContent toolApprovalRequestContent in
                        chatMessage.Contents
                        .OfType<ToolApprovalRequestContent>())
                        Requests.Add(toolApprovalRequestContent);
            }
            else if (response.FinishReason == ChatFinishReason.ContentFilter ||
                response.FinishReason == ChatFinishReason.Length)
                State = ChatClientState.Error;
            else if (response.FinishReason == ChatFinishReason.Stop)
            {
                State = ChatClientState.Healthy;
                Messages.Add(response.Messages[0]);
            }
        }

        private bool CanSendMessage()
        {
            return !string.IsNullOrWhiteSpace(Prompt) &&
                (State == ChatClientState.Healthy ||
                State == ChatClientState.NotBegun);
        }

        
    }
}
