// SPDX-FileCopyrightText: 2026 Tayra Sakurai
// SPDX-License-Identifier: GPL-3.0-or-later

using Caiman.Models;
using Caiman.ViewModels;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MaxxPro.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CategoriesView : Page
    {
        private CategoriesViewModel? categoriesViewModel;
        private CategoryViewModel? categoryViewModel;

        public CategoriesView()
        {
            InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            categoriesViewModel = Ioc.Default.GetRequiredService<CategoriesViewModel>();
            categoryViewModel = Ioc.Default.GetRequiredService<CategoryViewModel>();

            categoryViewModel.IsActive = true;
            categoriesViewModel.IsActive = true;

            if (e.Parameter is Category category)
                await categoryViewModel.InitializeForExistingValueAsync(category);

            await categoriesViewModel.LoadAsync();
            await categoryViewModel.LoadAsync();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            categoriesViewModel?.IsActive = false;
            categoryViewModel?.IsActive = false;
        }
    }
}
