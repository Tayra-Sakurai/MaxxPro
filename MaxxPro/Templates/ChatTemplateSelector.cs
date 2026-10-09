// SPDX-FileCopyrightText: 2026 Tayra Sakurai
// SPDX-License-Identifier: GPL-3.0-or-later

using Microsoft.Extensions.AI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaxxPro.Templates
{
    public class ChatTemplateSelector : DataTemplateSelector
    {
        public DataTemplate UserTemplate { get; set; }
        public DataTemplate ModelResponseTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            ArgumentNullException.ThrowIfNull(item);

            if (item is not ChatMessage chatMessage)
                throw new ArgumentException("The value must be a ChatMessage.", nameof(item));

            if (chatMessage.Role == ChatRole.User)
                return UserTemplate;

            if (chatMessage.Role == ChatRole.Assistant)
                return ModelResponseTemplate;

            throw new NotImplementedException();
        }
    }
}
