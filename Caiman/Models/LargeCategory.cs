// SPDX-FileCopyrightText: 2026 Tayra Sakurai
// SPDX-License-Identifier: GPL-3.0-or-later

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Caiman.Models
{
    public class LargeCategory : Category
    {
        public ObservableCollection<MediumCategory> Children { get; } = [];
    }
}
