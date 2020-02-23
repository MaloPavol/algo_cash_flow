using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AlgoCashFlow.CashCalculation;

namespace UnitTesting.CashCalculation_Testing
{
    [TestClass]
    public class AvailableCashCalculator_Testing
    {
        [TestMethod]
        [Timeout(2000)]
        public void GetBankAccountData_Testing()
        {
            //Arrange
            DateTime expectedDatum = Convert.ToDateTime("07.01.2020");
            double expectedKontostand = 1750000;
            
            string path = "../../../00_TestCashData/Kontostand.csv";
            AvailableCashCalculator AvailableCashCalculator = new AvailableCashCalculator();

            //Act
            var result = AvailableCashCalculator.GetBankAccountData(path);

            //Assert
            DateTime actualDatum = result[0].Datum;
            double actualKontostand = result[0].Kontostand;

            Assert.AreEqual(expectedDatum, actualDatum, "Datum is wrong.");
            Assert.AreEqual(expectedKontostand, actualKontostand, 0.00, "Kontostand is wrong.");
        }

        [TestMethod]
        [Timeout(2000)]
        public void GetBankBalanceAsOf_Testing()
        {
            //Arrange
            double expectedBalance = 1750000;
            DateTime datum = Convert.ToDateTime("07.01.2020");
            string path = "../../../00_TestCashData/Kontostand.csv";
            AvailableCashCalculator AvailableCashCalculator = new AvailableCashCalculator();
            
            //Act
            double actualBalance = AvailableCashCalculator.GetBankBalanceAsOf(path, datum);

            //Assert
            Assert.AreEqual(expectedBalance, actualBalance, 0.00, "GetBankBalance is wrong or wrong data.");
        }
    }
    


}
