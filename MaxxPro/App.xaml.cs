// SPDX-FileCopyrightText: 2026 Tayra Sakurai
// SPDX-License-Identifier: GPL-3.0-or-later

using Caiman.Contexts;
using Caiman.Models;
using Caiman.ViewModels;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.VectorData.SqliteVec;
using Cougar.Tools;
using Cougar.ViewModels;
using MaxxPro.ViewModels;
using Microsoft.Agents.AI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.VectorData;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using Microsoft.Windows.Storage;
using OllamaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MaxxPro
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        private Window? _window;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();
            Ioc.Default.ConfigureServices(GetService());
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected async override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            IDbContextFactory<CaimanContext> dbContextFactory = Ioc.Default.GetRequiredService<IDbContextFactory<CaimanContext>>();
            using (CaimanContext caimanContext = await dbContextFactory.CreateDbContextAsync())
            {
                await caimanContext.Database.MigrateAsync();
            }

            _window = new MainWindow();
            _window.Activate();
        }

        private static ServiceProvider GetService()
        {
            ServiceCollection services = new ServiceCollection();

            services.AddDbContextFactory<CaimanContext>(
                optionsBuilder => optionsBuilder.UseSqlite($"Data Source={System.IO.Path.Combine(ApplicationData.GetDefault().LocalFolder.Path, "Caiman.db")}"));

            services.AddSingleton<IModelTool<LargeCategory>, CaimanModelTool<LargeCategory>>();
            services.AddSingleton<IModelTool<MediumCategory>, CaimanModelTool<MediumCategory>>();
            services.AddSingleton<IModelTool<SmallCategory>, CaimanModelTool<SmallCategory>>();
            services.AddSingleton<IModelTool<Place>, CaimanModelTool<Place>>();
            services.AddSingleton<IModelTool<Item>, CaimanModelTool<Item>>();

            services.AddTransient<SettingsViewModel>();

            services.AddEmbeddingGenerator(
                sp =>
                {
                    SettingsViewModel viewModel = sp.GetRequiredService<SettingsViewModel>();
                    return new OllamaApiClient(
                        new Uri($"http://localhost:{(int)viewModel.OllamaPort}"),
                        viewModel.EmbeddingModelName);
                });
            services.AddChatClient(
                sp =>
                {
                    SettingsViewModel viewModel = sp.GetRequiredService<SettingsViewModel>();
                    return new OllamaApiClient(
                        new Uri($"http://localhost:{(int)viewModel.OllamaPort}"),
                        viewModel.ModelName);
                });

            services.AddSqliteVectorStore(
                _ => $"Data Source={System.IO.Path.Join(ApplicationData.GetDefault().LocalFolder.Path, "chathistory.db")}");

            services.AddSingleton(
                sp =>
                {
                    VectorStore vectorStore = sp.GetRequiredService<SqliteVectorStore>();

                    List<AITool> tools = sp.GetRequiredService<IModelTool<LargeCategory>>().GetAITools()
                    .Concat(sp.GetRequiredService<IModelTool<MediumCategory>>().GetAITools())
                    .Concat(sp.GetRequiredService<IModelTool<SmallCategory>>().GetAITools())
                    .Concat(sp.GetRequiredService<IModelTool<Place>>().GetAITools())
                    .Concat(sp.GetRequiredService<IModelTool<Item>>().GetAITools())
                    .ToList();

                    AIAgent agent = sp.GetRequiredService<IChatClient>()
                    .AsAIAgent(
                        new ChatClientAgentOptions
                        {
                            ChatOptions = new()
                            {
                                Instructions = "You are the agent to manage the database.",
                                Tools = tools,
                            },
                            Name = "Cougar",
                            AIContextProviders = [new ChatHistoryMemoryProvider(
                                vectorStore: vectorStore,
                                collectionName: "chathistory",
                                vectorDimensions: 768,
                                session => new ChatHistoryMemoryProvider.State(
                                    storageScope: new(){UserId = Environment.UserName, SessionId = Guid.NewGuid().ToString() },
                                    searchScope: new() {UserId = Environment.UserName }))],
                        });

                    return agent;
                });

            services.AddTransient<ChatViewModel>();

            services.AddTransient<CategoriesViewModel>();
            services.AddTransient<CategoryViewModel>();
            services.AddTransient<PlacesViewModel>();
            services.AddTransient<PlaceViewModel>();
            services.AddTransient<ItemsViewModel>();
            services.AddTransient<ItemViewModel>();

            return services.BuildServiceProvider();
        }
    }
}
