using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AlgoCashFlow.CashCalculation;
using Serilog;

namespace UnitTesting.CashCalculation_Testing
{
    [TestClass]
    public class PayrollCashflowCalculator_Testing
    {
        [TestMethod]
        [Timeout(2000)]
        public void GetPayrollData_Testing()
        {
            //Arrange
            double expectedNettogehaelter = -1437270;
            double expectedLohnabgaben = -1559630;
            DateTime expectedFaellig = Convert.ToDateTime("31.01.2020");
            bool expectedEingerechnet = false;
            

            string path = "../../../00_TestCashData/Lohnverrechnung.csv";
            PayrollCashflowCalculator PayrollCashflowCalculator = new PayrollCashflowCalculator();

            //Act
            var result = PayrollCashflowCalculator.GetPayrollData(path);

            //Assert
            double actualNettogehaelter = result[0].Nettogehaelter;
            double actualLohnabgaben = result[0].Lohnabgaben;
            DateTime actualFaellig = result[0].Faellig;
            bool actualEingerechnet = result[0].Eingerechnet;

            Assert.AreEqual(expectedNettogehaelter, actualNettogehaelter, "Nettogehaelter is wrong.");
            Assert.AreEqual(expectedLohnabgaben, actualLohnabgaben, 0.00, "Lohnabgaben is wrong.");
            Assert.AreEqual(expectedFaellig, actualFaellig, "Faellig is wrong.");
            Assert.AreEqual(expectedEingerechnet, actualEingerechnet, "Eingerechnet status is wrong.");
        }
    }
}
