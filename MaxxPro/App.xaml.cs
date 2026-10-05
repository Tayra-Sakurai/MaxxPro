// SPDX-FileCopyrightText: 2026 Tayra Sakurai
// SPDX-License-Identifier: GPL-3.0-or-later

using Caiman.Contexts;
using Caiman.Models;
using Caiman.ViewModels;
using CommunityToolkit.Mvvm.DependencyInjection;
using Cougar.Tools;
using Cougar.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
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

            services.AddChatClient(
                sp =>
                {
                    IChatClient innerClient = new OllamaApiClient(
                        new Uri("http://localhost:11434/api"),
                        "gemma:e2b");

                    return new ChatClientBuilder(innerClient)
                    .UseFunctionInvocation()
                    .UseDistributedCache()
                    .ConfigureOptions(
                        options =>
                        {
                            options.Tools ??= new List<AITool>();

                            foreach (AITool tool in sp.GetRequiredService<IModelTool<LargeCategory>>().GetAITools())
                                options.Tools.Add(tool);
                            foreach (AITool tool1 in sp.GetRequiredService<IModelTool<MediumCategory>>().GetAITools())
                                options.Tools.Add(tool1);
                            foreach (AITool tool2 in sp.GetRequiredService<IModelTool<SmallCategory>>().GetAITools())
                                options.Tools.Add(tool2);
                            foreach (AITool tool3 in sp.GetRequiredService<IModelTool<Place>>().GetAITools())
                                options.Tools.Add(tool3);
                            foreach (AITool tool4 in sp.GetRequiredService<IModelTool<Item>>().GetAITools())
                                options.Tools.Add(tool4);
                        })
                    .Build();
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
