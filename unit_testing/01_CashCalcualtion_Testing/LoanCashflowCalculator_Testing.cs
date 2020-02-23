using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AlgoCashFlow.CashCalculation;
using Serilog;

namespace UnitTesting.CashCalculation_Testing
{
    [TestClass]
    public class LoanCashflowCalculator_Testing
    {
        [TestMethod]
        [Timeout(2000)]
        public void GetLoanData_Testing()
        {
            //Arrange
            DateTime expectedTilgungsdatum = Convert.ToDateTime("10.01.2020");
            double expectedTilgungsBetrag = -750000;
            string expectedKommentar = "CHF Kredit aus 2014";
            bool expectedEingerechnet = false;

            string path = "../../../00_TestCashData/Treasury.csv";
            LoanCashflowCalculator LoanCashflowCalculator = new LoanCashflowCalculator();

            //Act
            var result = LoanCashflowCalculator.GetLoanData(path);

            //Assert
            DateTime actualTilgungsdatum = result[0].Tilgungsdatum;
            double actualTilgungsBetrag = result[0].Tilgungsbetrag;
            string actualKommentar = result[0].Kommentar;
            bool actualEingerechnet = result[0].Eingerechnet;

            Assert.AreEqual(expectedTilgungsdatum, actualTilgungsdatum, "Tilgungsdatum is wrong.");
            Assert.AreEqual(expectedTilgungsBetrag, actualTilgungsBetrag, 0.00, "Tilgungsbetrag is wrong.");
            Assert.AreEqual(expectedKommentar, actualKommentar, "Kommentar is wrong.");
            Assert.AreEqual(expectedEingerechnet, actualEingerechnet, "Eingerechnet status is wrong.");
        }
    }
}
