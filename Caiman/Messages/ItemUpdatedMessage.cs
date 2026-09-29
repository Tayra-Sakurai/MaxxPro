// SPDX-FileCopyrightText: 2026 Tayra Sakurai
// SPDX-License-Identifier: GPL-3.0-or-later

using Caiman.Models;
using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Caiman.Messages
{
    public class ItemUpdatedMessage : ValueChangedMessage<Item>
    {
        public ItemUpdatedMessage(Item value) : base(value) { }
    }
}
