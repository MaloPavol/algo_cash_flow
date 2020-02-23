using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AlgoCashFlow.CashCalculation;
using Serilog;

namespace UnitTesting.CashCalculation_Testing
{
    [TestClass]
    public class LongInvestmentCashflowCalculator_Testing
    {
        [TestMethod]
        [Timeout(2000)]
        public void GetLongInvestmentData_Testing()
        {
            //Arrange
            DateTime expectedInvestitionsdatum = Convert.ToDateTime("08.01.2020");
            double expectedInvestitionsBetrag = -480000;
            string expectedKommentar = "Neue Machine";
            bool expectedEingerechnet = false;

            string path = "../../../00_TestCashData/Anlagenbuchhaltung.csv";
            LongInvestmentCashflowCalculator LongInvestmentCashflowCalculator = new LongInvestmentCashflowCalculator();

            //Act
            var result = LongInvestmentCashflowCalculator.GetLongInvestmentData(path);

            //Assert
            DateTime actualInvestitionsdatum = result[0].Investitionsdatum;
            double actualInvestitionsBetrag = result[0].Investitionsbetrag;
            string actualKommentar = result[0].Kommentar;
            bool actualEingerechnet = result[0].Eingerechnet;

            Assert.AreEqual(expectedInvestitionsdatum, actualInvestitionsdatum, "Investitionsdatum is wrong.");
            Assert.AreEqual(expectedInvestitionsBetrag, actualInvestitionsBetrag, 0.00, "Investitionsbetrag is wrong.");
            Assert.AreEqual(expectedKommentar, actualKommentar, "Kommentar is wrong.");
            Assert.AreEqual(expectedEingerechnet, actualEingerechnet, "Eingerechnet status is wrong.");
        }
    }
}
