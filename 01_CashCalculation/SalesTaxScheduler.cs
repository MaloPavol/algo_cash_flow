using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoCashFlow.CashCalculation
{
    public class SalesTaxScheduler
    {
        public int CalcDaysTillVATclearing(DateTime date)
        {
            int daysTillVAT;

            DateTime calcVATclearingDate = date;

            WeekendSkipper weekendSkipper = new WeekendSkipper();

            calcVATclearingDate = new DateTime(calcVATclearingDate.Year, calcVATclearingDate.Month +2, 15);

            

            daysTillVAT = Convert.ToInt32((calcVATclearingDate - date).TotalDays) + weekendSkipper.SkipWeekend(calcVATclearingDate);

            Console.WriteLine("date  " + date);
            Console.WriteLine("calcVATclearingDate " + calcVATclearingDate);
            Console.WriteLine("daysTillVAT  " + daysTillVAT);


            return daysTillVAT;
        }
    }
}
