// SPDX-FileCopyrightText: 2026 Tayra Sakurai
// SPDX-License-Identifier: GPL-3.0-or-later

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.Windows.ApplicationModel.Resources;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MaxxPro
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BasePage : Page
    {
        public BasePage()
        {
            InitializeComponent();
            MainNavigation.SelectionChanged += MainNavigation_SelectionChanged;
            MainFrame.Navigated += MainFrame_Navigated;
            MainNavigation.BackRequested += MainNavigation_BackRequested;
        }

        private void MainNavigation_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            if (MainFrame.CanGoBack)
                MainFrame.GoBack();
            else
                sender.IsBackEnabled = false;
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            ResourceLoader resourceLoader = new();
            MainNavigation.IsBackEnabled = MainFrame.CanGoBack;

            if (e.SourcePageType == typeof(Views.CategoriesView))
            {
                MainNavigation.Header = resourceLoader.GetString("SourceTypeCategories");
                return;
            }

            if (e.SourcePageType == typeof(Views.PlacesView))
            {
                MainNavigation.Header = resourceLoader.GetString("SourceTypePlaces");
                return;
            }

            if (e.SourcePageType == typeof(Views.ItemsView))
            {
                MainNavigation.Header = resourceLoader.GetString("SourceTypeItems");
                return;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (MainFrame.SourcePageType == null)
                MainFrame.Navigate(typeof(Views.CategoriesView));
        }

        private void MainNavigation_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            NavigationViewItem? selectedItem = args.SelectedItem as NavigationViewItem;

            if (selectedItem == null)
            {
                Debug.WriteLine(args.SelectedItem);
                return;
            }

            string? tag = selectedItem.Tag as string;

            if (tag == null)
            {
                Debug.WriteLine($"{args.SelectedItem}");
                MainFrame.Navigate(typeof(Views.CategoriesView));
                return;
            }

            Type pageType = tag switch
            {
                "Items" => typeof(Views.ItemsView),
                "Places" => typeof(Views.PlacesView),
                "Categories" => typeof(Views.CategoriesView),
                _ => typeof(Views.CategoriesView),
            };

            MainFrame.Navigate(pageType);
        }
    }
}
