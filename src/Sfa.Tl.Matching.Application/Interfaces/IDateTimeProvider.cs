﻿using System;
using System.Collections.Generic;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IDateTimeProvider
    {
        string UtcNowString(string format);
        DateTime UtcNow();
        DateTime MinValue();
        DateTime AddWorkingDays(DateTime date, int days, IList<DateTime> holidays);
    }
}