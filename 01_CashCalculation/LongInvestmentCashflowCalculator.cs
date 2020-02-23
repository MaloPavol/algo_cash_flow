using System;
using System.Collections.Generic;
using System.Text;
using Serilog;
using FileHelpers;
using AlgoCashFlow.LocalFileSystem;
using AlgoCashFlow.ErrorHandling;
using System.IO;

namespace AlgoCashFlow.CashCalculation
{
    public class LongInvestmentCashflowCalculator {
        public DataMapLongInvestmentCashflow[] GetLongInvestmentData(string path)
        {

                if (!File.Exists(path))
                {
                    throw (new LocalFileNotFoundException("Long investment data file not found."));
                }

            var engine = new FileHelperEngine<DataMapLongInvestmentCashflow>();

            var result = engine.ReadFile(path);

            Log.Information("GetLongInvestmentData(path) retrieved information from " + path);

            return result;
        }

        public void SaveLongInvestmentData(DataMapLongInvestmentCashflow[] investmentData, string path)
        {
            var engine = new FileHelperEngine<DataMapLongInvestmentCashflow>();
            engine.HeaderText = engine.GetFileHeader();
            engine.WriteFile(path, investmentData);
        }

        public DataMapAutoCashCalculation[] CalculateLongInvestmentCashflow(DataMapAutoCashCalculation[] cashTable, string investmentDataFilePath)
        {
            DataMapLongInvestmentCashflow[] investmentData = GetLongInvestmentData(investmentDataFilePath);

            for (int i = 0; i < investmentData.Length; i++)
            {
                if (investmentData[i].Eingerechnet == false)
                {
                    int daysPassed = Convert.ToInt32((investmentData[i].Investitionsdatum - cashTable[0].Datum).TotalDays);
                    cashTable[daysPassed].Anlagenbuchhaltung += investmentData[i].Investitionsbetrag;
                    cashTable[daysPassed].Kommentar += investmentData[i].Kommentar;
                    investmentData[i].Eingerechnet = true;
                }
            }
            SaveLongInvestmentData(investmentData, investmentDataFilePath);
            Log.Information("Long investments data processed, status saved to " + investmentDataFilePath);
            return cashTable;
        }
    }
}
