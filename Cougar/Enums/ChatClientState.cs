// SPDX-FileCopyrightText: 2026 Tayra Sakurai
// SPDX-License-Identifier: GPL-3.0-or-later

using System;
using System.Collections.Generic;
using System.Text;

namespace Cougar.Enums
{
    public enum ChatClientState
    {
        NotBegun,
        Healthy,
        Error,
        Loading,
        WaitingForApproval
    }
}
