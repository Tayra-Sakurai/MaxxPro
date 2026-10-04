// SPDX-FileCopyrightText: 2026 Tayra Sakurai
// SPDX-License-Identifier: GPL-3.0-or-later

using System;
using System.Collections.Generic;
using System.Text;

namespace Caiman.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTimeOffset Life { get; set; } = DateTimeOffset.Now.Date;
        public int CategoryId { get; set; }
        public SmallCategory? Category { get; set; }
        public int PlaceId { get; set; }
        public Place? Place { get; set; }
    }
}
