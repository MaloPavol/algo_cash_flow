using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoCashFlow.CashCalculation
{
    class PayrollTaxScheduler
    {
        public int CalcDaysTillPayrollTaxClearing(DateTime date)
        {
            int daysTillpayrollTax;

            DateTime calcPayrollTaxclearingDate = date;

            WeekendSkipper weekendSkipper = new WeekendSkipper();

            calcPayrollTaxclearingDate = new DateTime(calcPayrollTaxclearingDate.Year, calcPayrollTaxclearingDate.Month + 1, 15);

            daysTillpayrollTax = Convert.ToInt32((calcPayrollTaxclearingDate - date).TotalDays) + weekendSkipper.SkipWeekend(calcPayrollTaxclearingDate);

            return daysTillpayrollTax;
        }
    }
}
