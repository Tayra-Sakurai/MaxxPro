// SPDX-FileCopyrightText: 2026 Tayra Sakurai
// SPDX-License-Identifier: GPL-3.0-or-later

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Cougar.Enums;
using Microsoft.Agents.AI;
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
        private readonly AIAgent agent;

        private AgentSession? session;

        public ChatViewModel(AIAgent agent)
        {
            this.agent = agent;
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
        [NotifyCanExecuteChangedFor(nameof(SendTextMessageCommand))]
        public partial string Prompt { get; set; }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(StartSessionCommand))]
        public partial ChatClientState State { get; set; }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanStartSession))]
        private async Task StartSessionAsync()
        {
            session = await agent.CreateSessionAsync();
            State = ChatClientState.Healthy;
            Messages.Clear();

            SendTextMessageCommand.NotifyCanExecuteChanged();
        }

        private bool CanStartSession()
        {
            return State == ChatClientState.NotBegun;
        }

        /// <summary>
        /// Sends the message to the agent and receives the response.
        /// </summary>
        /// <param name="message">The chat message to be sent.</param>
        /// <returns>The task to control the asynchronous process.</returns>
        private async Task ReceiveMessage(ChatMessage message)
        {
            if (session == null)
                return;

            State = ChatClientState.Loading;
            AgentResponse response = await agent.RunAsync(message, session);
            if (response == null) return;

            foreach (
                ToolApprovalRequestContent requestContent in
                response.Messages
                .SelectMany(m => m.Contents)
                .OfType<ToolApprovalRequestContent>())
                Requests.Add(requestContent);

            foreach (
                ChatMessage chatMessage in
                response.Messages
                .Where(e => e.Contents.All(c => c is TextContent)))
                Messages.Add(chatMessage);
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanSendTextMessage))]
        private async Task SendTextMessageAsync()
        {
            if (string.IsNullOrWhiteSpace(Prompt) ||
                session == null)
                return;

            ChatMessage chatMessage = new(ChatRole.User, Prompt);
            Messages.Add(chatMessage);

            Prompt = string.Empty;

            await ReceiveMessage(chatMessage);
        }

        private bool CanSendTextMessage()
        {
            return !string.IsNullOrWhiteSpace(Prompt) && session != null;
        }
    }
}
