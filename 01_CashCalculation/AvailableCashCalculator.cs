using System;
using System.Collections.Generic;
using System.Text;
using AlgoCashFlow.LocalFileSystem;
using AlgoCashFlow.CashCalculation;
using AlgoCashFlow.ErrorHandling;
using AlgoCashFlow.Logs;
using FileHelpers;
using Serilog;
using System.IO;

namespace AlgoCashFlow.CashCalculation
{
    public class AvailableCashCalculator
    {

        public double CalculateAvailableCash()
        {
            //preliminary value
            double availableCash = 0;

            GlobalLocalFileNames globalLocalFileNames = new GlobalLocalFileNames();
            LocalFileReader localFileReader = new LocalFileReader();

            try
            {
                //(0) Search for local financial data with latest modifications  
                string bankDataFilePath = localFileReader.GetLatestFilePath(GlobalLocalFileNames.finDataDirectoryPath, GlobalLocalFileNames.bankAccountDataFileName);
                string loanDataFilePath = localFileReader.GetLatestFilePath(GlobalLocalFileNames.finDataDirectoryPath, GlobalLocalFileNames.treasuryDataFileName);
                string investmentDataFilePath = localFileReader.GetLatestFilePath(GlobalLocalFileNames.finDataDirectoryPath, GlobalLocalFileNames.investmentDataFileName);
                string salesDataFilePath = localFileReader.GetLatestFilePath(GlobalLocalFileNames.finDataDirectoryPath, GlobalLocalFileNames.salesDataFileName);
                string payrollDataFilePath = localFileReader.GetLatestFilePath(GlobalLocalFileNames.finDataDirectoryPath, GlobalLocalFileNames.payrollDataFileName);
                string autoCashCalculationFilePath = localFileReader.GetLatestFilePath(GlobalLocalFileNames.finReportsDirectoryPath, GlobalLocalFileNames.autoCashCalculationFileName);

                //(1) Start Available Cash Calculation
                AvailableCashCalculator availableCashCalculator = new AvailableCashCalculator();
                DataMapAutoCashCalculation[] autoCashCalculationTable = availableCashCalculator.GetAutoCashCalculationData(autoCashCalculationFilePath);

                //(1.a) GetBankAccountBalance
                var currentBankBalance = availableCashCalculator.GetCurrentBankBalance(bankDataFilePath);
                
                //For demo purposes 23rd Feb 2020 is used as current date
                //DateTime dateToday = DateTime.Now.Date;
                DateTime dateToday = Convert.ToDateTime("23.02.2020");

                //Enter current bank balance into cash calculation table
                int daysPassed = Convert.ToInt32((dateToday - autoCashCalculationTable[0].Datum).TotalDays);
                autoCashCalculationTable[daysPassed].Kontostand = currentBankBalance;

                //(1.b) Get expected loan data and calculate loan cash flow
                LoanCashflowCalculator loanCashflowCalculator = new LoanCashflowCalculator();
                autoCashCalculationTable = loanCashflowCalculator.CalculateLoanCashFlow(autoCashCalculationTable, loanDataFilePath);

                //(1.c) Get long term investment data and calculate cashflow
                LongInvestmentCashflowCalculator longInvestmentCashflowCalculator = new LongInvestmentCashflowCalculator();
                autoCashCalculationTable = longInvestmentCashflowCalculator.CalculateLongInvestmentCashflow(autoCashCalculationTable, investmentDataFilePath);

                //(1.d-e) Get sales data and calculate sales cashflow
                SalesCashflowCalculator salesCashflowCalculator = new SalesCashflowCalculator();
                autoCashCalculationTable = salesCashflowCalculator.CalculateSalesCashflow(autoCashCalculationTable, salesDataFilePath);

                //(1.f-g) Get payroll data and calculate payroll cashflow
                PayrollCashflowCalculator payrollCashflowCalculator = new PayrollCashflowCalculator();
                autoCashCalculationTable = payrollCashflowCalculator.CalculatePayrollCashflow(autoCashCalculationTable, payrollDataFilePath);

                //(1.h) Calculate available cash

                //First entry till today have to be adjusted according to the actuals
                for (int j = 0; j<daysPassed; j++)
                {
                    //Calculate unpredicted cash developments
                    autoCashCalculationTable[j].Sonstige =
                                                    autoCashCalculationTable[j+1].Kontostand -
                                                    autoCashCalculationTable[j].Kontostand -
                                                    autoCashCalculationTable[j].Treasury -
                                                    autoCashCalculationTable[j].Anlagenbuchhaltung -
                                                    autoCashCalculationTable[j].Sales -
                                                    autoCashCalculationTable[j].Payroll;
                }

                for (int g = 0; g < daysPassed; g++)
                {
                    //Create sums
                    autoCashCalculationTable[g].Summe =
                                                    autoCashCalculationTable[g].Kontostand +
                                                    autoCashCalculationTable[g].Treasury +
                                                    autoCashCalculationTable[g].Anlagenbuchhaltung +
                                                    autoCashCalculationTable[g].Sales +
                                                    autoCashCalculationTable[g].Payroll +
                                                    autoCashCalculationTable[g].Sonstige;
                }

                //Create prognosis for cash available today
                autoCashCalculationTable[daysPassed].Summe =
                                                    autoCashCalculationTable[daysPassed].Kontostand +
                                                    autoCashCalculationTable[daysPassed].Treasury +
                                                    autoCashCalculationTable[daysPassed].Anlagenbuchhaltung +
                                                    autoCashCalculationTable[daysPassed].Sales +
                                                    autoCashCalculationTable[daysPassed].Payroll;

                //Cash available for next 24h 
                availableCash = autoCashCalculationTable[daysPassed].Summe;

                //Create prognosis for the future cash available
                for (int k = daysPassed +1; k < autoCashCalculationTable.Length; k++)
                {
                    //Assume: Balance at the begining of the day is the same as at the end of the last

                    autoCashCalculationTable[k].Kontostand = autoCashCalculationTable[k - 1].Summe;
                    //Create sums
                    autoCashCalculationTable[k].Summe =
                                                    autoCashCalculationTable[k].Kontostand +
                                                    autoCashCalculationTable[k].Treasury +
                                                    autoCashCalculationTable[k].Anlagenbuchhaltung +
                                                    autoCashCalculationTable[k].Sales +
                                                    autoCashCalculationTable[k].Payroll; 
                }

                availableCashCalculator.SaveAutoCashCalculation(autoCashCalculationTable, autoCashCalculationFilePath);
            }
            catch (BankBalanceException eBB)
            {
                Console.WriteLine(" Exception: " + eBB);
                Log.Debug("Exception: " + eBB);

            }
            catch (LocalFileNotFoundException eFNF)
            {
                Console.WriteLine(" Exception: " + eFNF);
                Log.Debug("Exception: " + eFNF);

            }
            catch (DataNotFoundException eDNF)
            {
                Console.WriteLine(" Exception: " + eDNF);
                Log.Debug("Exception: " + eDNF);

            } 
                return availableCash;
            
        }


        public void SaveAutoCashCalculation(DataMapAutoCashCalculation[] autoCashCalculationTable, string autoCashCalculationFilePath)
        {
            var engine = new FileHelperEngine<DataMapAutoCashCalculation>();
            engine.HeaderText = engine.GetFileHeader();
            engine.WriteFile(autoCashCalculationFilePath, autoCashCalculationTable);
        }



        public DataMapAutoCashCalculation[] GetAutoCashCalculationData(string path)
        {
            var engine = new FileHelperEngine<DataMapAutoCashCalculation>();

            var result = engine.ReadFile(path);

            Log.Information("GetAutoCashCalculationData(path) retrieved information from " + path);

            return result;
        }



        public DataMapBankAccountBalance[] GetBankAccountData(string path)
        {
            var engine = new FileHelperEngine<DataMapBankAccountBalance>();


                if (!File.Exists(path))
                {
                    throw (new LocalFileNotFoundException("Bank account data file not found."));
                }
           

                var result = engine.ReadFile(path);

                Log.Information("GetBankAccountBalance(path) retrieved information from " + path);

                return result;  
        }

        public double GetBankBalanceAsOf(string filePath, DateTime date)
        {
            DataMapBankAccountBalance[] bankAccountData = GetBankAccountData(filePath);

            double bankBalance = -1;
            bool dateFound = false;

            try
            {
                for (int i = 0; i < bankAccountData.Length; i++)
                {
                    if (bankAccountData[i].Datum.Date == date.Date)
                    {
                        dateFound = true;
                        bankBalance = bankAccountData[i].Kontostand;
                        break;
                    }
                }
                if (dateFound == false)
                {
                    throw (new BankBalanceException("Current bank balance not found."));
                }
            } catch (BankBalanceException e) {
                Console.WriteLine(" Exception: " + e);

                Log.Debug("Exception: " + e);
            }
                return bankBalance;
        }

        public double GetCurrentBankBalance(string filePath)
        {
            DateTime today = DateTime.Now;
            double currentBankBalance = GetBankBalanceAsOf(filePath, today);
            
            return currentBankBalance;
        }

    }
}
