// SPDX-FileCopyrightText: 2026 Tayra Sakurai
// SPDX-License-Identifier: GPL-3.0-or-later

using Caiman.Models;
using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Caiman.Messages
{
    public class MediumCategoryInvokedMessage : ValueChangedMessage<MediumCategory>
    {
        public MediumCategoryInvokedMessage(MediumCategory value) : base(value) { }
    }
}
