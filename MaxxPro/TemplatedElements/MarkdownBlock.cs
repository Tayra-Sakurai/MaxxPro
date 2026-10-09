// SPDX-FileCopyrightText: 2026 Tayra Sakurai
// SPDX-License-Identifier: GPL-3.0-or-later

using Markdig;
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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MaxxPro.TemplatedElements
{
    [TemplatePart(Name = "View", Type = typeof(WebView2))]
    [ContentProperty(Name = nameof(Text))]
    public sealed partial class MarkdownBlock : Control
    {
        private WebView2? webView2;

        public MarkdownBlock()
        {
            DefaultStyleKey = typeof(MarkdownBlock);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            webView2 = (WebView2)GetTemplateChild("View");
            DisplayData();
        }

        private void DisplayData()
        {
            if (webView2 == null ||
                string.IsNullOrWhiteSpace(Text))
                return;

            string html = Markdown.ToHtml(Text);
            webView2.NavigateToString(html);
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
