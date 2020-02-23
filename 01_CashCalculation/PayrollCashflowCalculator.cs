using System;
using System.Collections.Generic;
using System.Text;
using FileHelpers;
using AlgoCashFlow.LocalFileSystem;
using Serilog;
using AlgoCashFlow.ErrorHandling;
using System.IO;

namespace AlgoCashFlow.CashCalculation
{
    public class PayrollCashflowCalculator
    {
        public DataMapPayrollCashflow[] GetPayrollData(string path)
        {

                if (!File.Exists(path))
                {
                    throw (new LocalFileNotFoundException("Payroll data file not found."));
                }

            var engine = new FileHelperEngine<DataMapPayrollCashflow>();

            var result = engine.ReadFile(path);

            Log.Information("GetPayrollData(path) retrieved information from " + path);

            return result;
        }
        public void SavePayrollData(DataMapPayrollCashflow[] payrollData, string path)
        {
            var engine = new FileHelperEngine<DataMapPayrollCashflow>();
            engine.HeaderText = engine.GetFileHeader();
            engine.WriteFile(path, payrollData);
        }


        public DataMapAutoCashCalculation[] CalculatePayrollCashflow(DataMapAutoCashCalculation[] cashTable, string payrollDataFilePath)
        {

            DataMapPayrollCashflow[] payrollData = GetPayrollData(payrollDataFilePath);

            WeekendSkipper weekendSkipper = new WeekendSkipper();

            PayrollTaxScheduler payrollTaxScheduler = new PayrollTaxScheduler();

            for (int i = 0; i < payrollData.Length; i++)
            {
                if (payrollData[i].Eingerechnet == false)
                {
                    //Scheduled paymemt to employees on 28th
                    DateTime employeePayDate = payrollData[i].Faellig;
                    employeePayDate = new DateTime(employeePayDate.Year, employeePayDate.Month, 28);
                    int daysTillPayable = Convert.ToInt32((employeePayDate - cashTable[0].Datum).TotalDays);
                    daysTillPayable += weekendSkipper.SkipWeekend(employeePayDate);
                    employeePayDate = cashTable[daysTillPayable].Datum;
                    cashTable[daysTillPayable].Payroll += payrollData[i].Nettogehaelter;

                    //Payroll taxes payable next month
                    cashTable[daysTillPayable + payrollTaxScheduler.CalcDaysTillPayrollTaxClearing(employeePayDate)].Payroll += payrollData[i].Lohnabgaben;

                    payrollData[i].Eingerechnet = true;
                }
            }
            SavePayrollData(payrollData, payrollDataFilePath);
            Log.Information("Sales data processed, status saved to " + payrollDataFilePath);
            return cashTable;
        }
    }
}
