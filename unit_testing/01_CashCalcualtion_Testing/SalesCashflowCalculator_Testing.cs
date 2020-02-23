using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AlgoCashFlow.CashCalculation;
using AlgoCashFlow.LocalFileSystem;
using Serilog;

namespace UnitTesting.CashCalculation_Testing
{
    [TestClass]
    public class SalesCashflowCalculator_Testing
    {
        [TestMethod]
        [Timeout(2000)]
        public void GetLoanData_Testing()
        {
            //Arrange
            DateTime expectedLieferdatum = Convert.ToDateTime("20.01.2020");
            double expectedNettopreis = 1000000;
            string expectedKommentar = "Bestellung 1050678";
            bool expectedEingerechnet = false;

            string path = "../../../00_TestCashData/Sales.csv";
            SalesCashflowCalculator SalesCashflowCalculator = new SalesCashflowCalculator();

            //Act
            var result = SalesCashflowCalculator.GetSalesData(path);

            //Assert
            DateTime actualLieferdatum = result[0].Lieferdatum;
            double actualNettopreis = result[0].Nettopreis;
            string actualKommentar = result[0].Kommentar;
            bool actualEingerechnet = result[0].Eingerechnet;

            Assert.AreEqual(expectedLieferdatum, actualLieferdatum, "Lieferdatum is wrong.");
            Assert.AreEqual(expectedNettopreis, actualNettopreis, 0.00, "Nettopreis is wrong.");
            Assert.AreEqual(expectedKommentar, actualKommentar, "Kommentar is wrong.");
            Assert.AreEqual(expectedEingerechnet, actualEingerechnet, "Eingerechnet status is wrong.");
        }
    }
}
