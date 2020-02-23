using System;
using System.Collections.Generic;
using System.Text;
using FileHelpers;
using AlgoCashFlow.LocalFileSystem;
using AlgoCashFlow.ErrorHandling;
using Serilog;
using System.IO;

namespace AlgoCashFlow.CashCalculation
{
    public class LoanCashflowCalculator
    {

         public DataMapLoanCashflow[] GetLoanData(string path)
         {
            var engine = new FileHelperEngine<DataMapLoanCashflow>();


                if (!File.Exists(path))
                {
                    throw (new LocalFileNotFoundException("Loan data file not found."));
                }
  

            var result = engine.ReadFile(path);

            Log.Information("GetLoanData(path) retrieved information from " + path);

            return result;
         }

        public void SaveLoanData(DataMapLoanCashflow[] loanData, string path)
        {
            var engine = new FileHelperEngine<DataMapLoanCashflow>();
            engine.HeaderText = engine.GetFileHeader();
            engine.WriteFile(path, loanData);
        }


        public DataMapAutoCashCalculation[] CalculateLoanCashFlow(DataMapAutoCashCalculation[] cashTable, string loanDataFilePath)
        {
            
            DataMapLoanCashflow[] loanData = GetLoanData(loanDataFilePath);

            for (int i = 0; i < loanData.Length; i++)
            {
                if (loanData[i].Eingerechnet == false)
                {
                    int daysPassed = Convert.ToInt32((loanData[i].Tilgungsdatum - cashTable[0].Datum).TotalDays);
                    cashTable[daysPassed].Treasury += loanData[i].Tilgungsbetrag;
                    cashTable[daysPassed].Kommentar += loanData[i].Kommentar;
                    loanData[i].Eingerechnet = true;
                }
            }
            SaveLoanData(loanData, loanDataFilePath);
            Log.Information("Loan data processed, status saved to " + loanDataFilePath);
            return cashTable;
        }
    }
}
