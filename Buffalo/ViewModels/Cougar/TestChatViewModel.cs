// SPDX-FileCopyrightText: 2026 Tayra Sakurai
// SPDX-License-Identifier: GPL-3.0-or-later

using Cougar.Enums;
using Cougar.ViewModels;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OllamaSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.ViewModels.Cougar
{
    [TestClass]
    public class TestChatViewModel
    {
        private AIAgent? agent;
        private ChatViewModel? chatViewModel;

        [TestInitialize]
        public void SetupTest()
        {
            IChatClient chatClient = new OllamaApiClient(
                new Uri("http://localhost:11434/api"),
                "gemma4:e2b");

            agent = chatClient
                .AsAIAgent(
                    new ChatClientAgentOptions
                    {
                        Name = "TestAgent",
                        ChatOptions = new() { Instructions = "You are a helpful agent." },
                        ChatHistoryProvider = new InMemoryChatHistoryProvider(),
                    });

            chatViewModel = new(agent);
        }

        [TestMethod]
        public async Task TestInitializeChat()
        {
            if (chatViewModel == null)
                Assert.Fail("Initiation failed");

            await chatViewModel.StartSessionCommand.ExecuteAsync(null);
            Assert.AreEqual(ChatClientState.Healthy, chatViewModel.State);

            chatViewModel.Prompt = "What can you do?";
            await chatViewModel.SendTextMessageCommand.ExecuteAsync(null);
            Debug.WriteLine("");
            Debug.WriteLine(chatViewModel.Messages);

            Assert.IsNotEmpty(chatViewModel.Messages);
        }
    }
}
