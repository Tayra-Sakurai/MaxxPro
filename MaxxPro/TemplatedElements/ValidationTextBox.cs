// SPDX-FileCopyrightText: 2026 Tayra Sakurai
// SPDX-License-Identifier: GPL-3.0-or-later

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MaxxPro.TemplatedElements
{
    [TemplatePart(Name = "TextBox", Type = typeof(TextBox))]
    [TemplatePart(Name = "TextBlock", Type = typeof(TextBlock))]
    public sealed partial class ValidationTextBox : Control
    {
        private TextBlock? textBlock;
        private TextBox? textBox;

        public ValidationTextBox()
        {
            DefaultStyleKey = typeof(ValidationTextBox);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            VisualStateManager.GoToState(this, "ValidState", false);

            textBlock = (TextBlock)GetTemplateChild("TextBlock");
            textBox = (TextBox)GetTemplateChild("TextBox");

            textBox.TextChanged += TextBox_TextChanged;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Text = ((TextBox)sender).Text;
        }

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static DependencyProperty TextProperty { get; } = DependencyProperty.Register(
            nameof(Text),
            typeof(string),
            typeof(ValidationTextBox),
            new(string.Empty, HandleTextPropertyChanged));

        public string Header
        {
            get => (string)GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        public static DependencyProperty HeaderProperty { get; } = DependencyProperty.Register(
            nameof(Header),
            typeof(string),
            typeof(ValidationTextBox),
            new(string.Empty));

        public string PlaceholderText
        {
            get => (string)GetValue(PlaceholderTextProperty);
            set => SetValue(PlaceholderTextProperty, value);
        }

        public static DependencyProperty PlaceholderTextProperty { get; } = DependencyProperty.Register(
            nameof(PlaceholderText),
            typeof(string),
            typeof(ValidationTextBox),
            new(string.Empty));

        public bool AcceptsReturn
        {
            get => (bool)GetValue(AcceptsReturnProperty);
            set => SetValue(AcceptsReturnProperty, value);
        }

        public static DependencyProperty AcceptsReturnProperty { get; } = DependencyProperty.Register(
            nameof(AcceptsReturn),
            typeof(bool),
            typeof(ValidationTextBox),
            new(false));

        public TextWrapping TextWrapping
        {
            get => (TextWrapping)GetValue(TextWrappingProperty);
            set => SetValue(TextWrappingProperty, value);
        }

        public static DependencyProperty TextWrappingProperty { get; } = DependencyProperty.Register(
            nameof(TextWrapping),
            typeof(TextWrapping),
            typeof(ValidationTextBox),
            new(TextWrapping.NoWrap));

        public string Property
        {
            get => (string)GetValue(PropertyProperty);
            set => SetValue(PropertyProperty, value);
        }

        public static DependencyProperty PropertyProperty { get; } = DependencyProperty.Register(
            nameof(Property),
            typeof(string),
            typeof(ValidationTextBox),
            new(string.Empty, HandlePropertyPropertyChanged));

        public INotifyDataErrorInfo ViewModel
        {
            get => (INotifyDataErrorInfo)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        public static DependencyProperty ViewModelProperty { get; } = DependencyProperty.Register(
            nameof(ViewModel),
            typeof(INotifyDataErrorInfo),
            typeof(ValidationTextBox),
            new(default(INotifyDataErrorInfo), HandleViewModelPropertyChanged));

        private static void HandleTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ValidationTextBox)d).HandleErrors();
        }

        private static void HandlePropertyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ValidationTextBox)d).HandleErrors();
        }

        private static void HandleViewModelPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is INotifyDataErrorInfo viewModel)
                viewModel.ErrorsChanged -= ((ValidationTextBox)d).ValidationTextBox_ErrorsChanged;

            ((INotifyDataErrorInfo)e.NewValue).ErrorsChanged += ((ValidationTextBox)d).ValidationTextBox_ErrorsChanged;
        }

        private void ValidationTextBox_ErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
        {
            HandleErrors();
        }

        private void HandleErrors()
        {
            if (ViewModel is not INotifyDataErrorInfo viewModel ||
                textBlock is not TextBlock textBlock1 ||
                textBox is not TextBox ||
                Property is not string property)
                return;

            ValidationResult? validationResult = viewModel.GetErrors(property).OfType<ValidationResult>().FirstOrDefault();

            if (validationResult != null)
            {
                textBlock1.Text = validationResult.ErrorMessage ?? string.Empty;
                VisualStateManager.GoToState(this, "InvalidState", true);
            }
            else
            {
                textBlock1.Text = string.Empty;
                VisualStateManager.GoToState(this, "ValidState", true);
            }
        }
    }
}
