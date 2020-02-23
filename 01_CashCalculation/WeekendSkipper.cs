using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoCashFlow.CashCalculation
{
    public class WeekendSkipper
    {
        public int SkipWeekend(DateTime date)
        {
            int skipper = 0;

            if (date.DayOfWeek == DayOfWeek.Saturday)
            {
                 skipper = 2;

            }
            else if (date.DayOfWeek == DayOfWeek.Sunday)
            {
                skipper = 1;
            }

            return skipper;
        }
    }
}
