// SPDX-FileCopyrightText: 2026 Tayra Sakurai
// SPDX-License-Identifier: GPL-3.0-or-later

using Caiman.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaxxPro.Templates
{
    public class TreeViewTemplateSelector : DataTemplateSelector
    {
        public DataTemplate LargeCategoryTemplate { get; set; }
        public DataTemplate MediumCategoryTemplate { get; set; }
        public DataTemplate SmallCategoryTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            if (item is LargeCategory)
                return LargeCategoryTemplate;

            if (item is MediumCategory)
                return MediumCategoryTemplate;

            if (item is SmallCategory)
                return SmallCategoryTemplate;

            throw new NotImplementedException();
        }
    }
}
