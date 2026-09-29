// SPDX-FileCopyrightText: 2026 Tayra Sakurai
// SPDX-License-Identifier: GPL-3.0-or-later

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Caiman.Models
{
    public class MediumCategory : Category
    {
        public int ParentId { get; set; }
        public LargeCategory? Parent { get; set; }
        public ObservableCollection<SmallCategory> Children { get; } = [];
    }
}
