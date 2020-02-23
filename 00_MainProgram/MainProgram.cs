using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AlgoCashFlow.CashCalculation;
using AlgoCashFlow.LocalFileSystem;
using AlgoCashFlow.Logs;
using FileHelpers;
using Serilog;
using AlgoCashFlow.ErrorHandling;

namespace AlgoCashFlow
{
    class MainProgram
    {
        static void Main(string[] args)
        {
      

            //(0) Set up logger
            Logger logger = new Logger();
            logger.SetUp();

            //(I) Calculate available cash and generate cash table
            AvailableCashCalculator availableCashCalculator = new AvailableCashCalculator();
            availableCashCalculator.CalculateAvailableCash();

 


        }
    }
}
