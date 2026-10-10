// SPDX-FileCopyrightText: 2026 Tayra Sakurai
// SPDX-License-Identifier: GPL-3.0-or-later

using DocSharp.Markdown;
using DocSharp.Primitives;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Markup;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MaxxPro.TemplatedElements
{
    [TemplatePart(Name = "View", Type = typeof(RichEditBox))]
    [ContentProperty(Name = nameof(Text))]
    public sealed partial class MarkdownBlock : Control
    {
        private RichEditBox? view2;

        public MarkdownBlock()
        {
            DefaultStyleKey = typeof(MarkdownBlock);
        }

        protected override async void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            view2 = (RichEditBox)GetTemplateChild("View");
            DisplayData();
        }

        private void DisplayData()
        {
            if (view2 is null ||
                string.IsNullOrWhiteSpace(Text))
                return;

            MarkdownSource markdownSource = MarkdownSource.FromMarkdownString(Text);
            MarkdownConverter converter = new()
            {
                PageSize = PageSize.A5_Landscape,
                PageMargins = PageMargins.Narrow,
            };

            string rtfString = converter.ToRtfString(markdownSource);
            view2.IsReadOnly = false;
            view2.Document.SetText(Microsoft.UI.Text.TextSetOptions.FormatRtf, rtfString);
            view2.IsReadOnly = true;
        }

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static DependencyProperty TextProperty { get; } = DependencyProperty.Register(
            nameof(Text),
            typeof(string),
            typeof(MarkdownBlock),
            new(string.Empty, OnTextPropertyChanged));

        private static void OnTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MarkdownBlock)d).DisplayData();
        }
    }
}
