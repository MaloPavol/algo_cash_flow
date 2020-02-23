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
    public class SalesCashflowCalculator
    {
        public DataMapSalesCashflow[] GetSalesData(string path)
        {

                if (!File.Exists(path))
                {
                    throw (new LocalFileNotFoundException("Sales data file not found."));
                }

            var engine = new FileHelperEngine<DataMapSalesCashflow>();

            var result = engine.ReadFile(path);

            Log.Information("GetSalesData(path) retrieved information from " + path);

            return result;
        }

        public void SaveSalesData(DataMapSalesCashflow[] salesData, string path)
        {
            var engine = new FileHelperEngine<DataMapSalesCashflow>();
            engine.HeaderText = engine.GetFileHeader();
            engine.WriteFile(path, salesData);
        }


        public DataMapAutoCashCalculation[] CalculateSalesCashflow(DataMapAutoCashCalculation[] cashTable, string salesDataFilePath)
        {

            DataMapSalesCashflow[] salesData = GetSalesData(salesDataFilePath);

            WeekendSkipper weekendSkipper = new WeekendSkipper();

            SalesTaxScheduler salesTaxScheduler = new SalesTaxScheduler();

            for (int i = 0; i < salesData.Length; i++)
            {
                if (salesData[i].Eingerechnet == false)
                {
                    int daysTillDelivery = Convert.ToInt32((salesData[i].Lieferdatum - cashTable[0].Datum).TotalDays);


                    //customer's prepayment 9 days before delivery
                    int daysTillPrepayment = daysTillDelivery - 9;
                    //wire only on work days
                    DateTime prepaymentDate = cashTable[daysTillPrepayment].Datum;
                    daysTillPrepayment += weekendSkipper.SkipWeekend(prepaymentDate);
                    prepaymentDate = cashTable[daysTillPrepayment].Datum;
                    //prepayment, incl value added tax
                    cashTable[daysTillPrepayment].Sales += (salesData[i].Nettopreis * 0.2) * 1.2;
                    //VAT clearing
                    cashTable[daysTillPrepayment + salesTaxScheduler.CalcDaysTillVATclearing(prepaymentDate)].Sales -= (salesData[i].Nettopreis * 0.2) * 0.2;


                    //vendor's material purchase 5 days before delivery
                    int daysTillMaterialPurchase = daysTillDelivery - 5;
                    //money transfer works only on work days
                    DateTime materialPurchaseDate = cashTable[daysTillMaterialPurchase].Datum;
                    daysTillMaterialPurchase += weekendSkipper.SkipWeekend(materialPurchaseDate);
                    materialPurchaseDate = cashTable[daysTillMaterialPurchase].Datum;
                    //material purchase, incl. value added tax
                    cashTable[daysTillMaterialPurchase].Sales -= (salesData[i].Nettopreis * 0.17) * 1.2;
                    //VAT clearing
                    cashTable[daysTillMaterialPurchase + salesTaxScheduler.CalcDaysTillVATclearing(materialPurchaseDate)].Sales += (salesData[i].Nettopreis * 0.17) * 0.2;

                    //remaining 80% paid by customer on delivery day, incl. value added tax
                    //do weekend skipping needed, because delivery can be entered with weekdays only
                    cashTable[daysTillDelivery].Sales += (salesData[i].Nettopreis * 0.8) * 1.2;
                    //VAT clearing
                    cashTable[daysTillDelivery + salesTaxScheduler.CalcDaysTillVATclearing(salesData[i].Lieferdatum)].Sales -= (salesData[i].Nettopreis * 0.8) * 0.2;

                    salesData[i].Eingerechnet = true;
                }
            }
            SaveSalesData(salesData, salesDataFilePath);
            Log.Information("Sales data processed, status saved to " + salesDataFilePath);
            return cashTable;
        }
    }
}
