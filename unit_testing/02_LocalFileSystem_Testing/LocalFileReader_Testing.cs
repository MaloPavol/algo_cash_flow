using Microsoft.VisualStudio.TestTools.UnitTesting;
using AlgoCashFlow.LocalFileSystem;
using Serilog;
using System;

namespace UnitTesting.LocalFileSystem_Testing
{
    [TestClass]
    public class LocalFileReader_Testing
    {
        [TestMethod]
        [Timeout(2000)]
        public void GetFilePaths_Testing()
        {
            //Arrange
            string directory = "../../../00_TestCashData/";
            string searchForName = "Kontostand";
            string expectedFilePath = "../../../00_TestCashData/Kontostand.csv";
            LocalFileReader localFileReader = new LocalFileReader();
            
            //Act
            string actualFilePath = localFileReader.GetFilePaths(directory, searchForName)[0];
            Console.WriteLine(actualFilePath);
            
            //Assert
            Assert.AreEqual(expectedFilePath, actualFilePath, "FilePathSearch is wrong.");
        }
        [TestMethod]
        [Timeout(2000)]
        public void GetLatestFilePathFromArray_Testing()
        {
            //Arrange
            string directory = "../../../00_TestCashData/";
            string searchForName = "Kontostand";
            string expectedFilePath = "../../../00_TestCashData/Kontostand_newerFile.csv";
            LocalFileReader localFileReader = new LocalFileReader();
            
            //Act
            string[] filePaths = localFileReader.GetFilePaths(directory, searchForName);
            Console.WriteLine(filePaths[0]);
            Console.WriteLine(filePaths[1]);
            string actualFilePath = localFileReader.GetLatestFilePathFromArray(filePaths);
            Console.WriteLine(actualFilePath);
            
            //Assert
            Assert.AreEqual(expectedFilePath, actualFilePath, "FilePath last modified search is wrong.");
        }
        [TestMethod]
        [Timeout(2000)]
        public void GetLatestFilePath_Testing()
        {
            //Arrange
            string directory = "../../../00_TestCashData/";
            string searchForName = "Kontostand";
            string expectedFilePath = "../../../00_TestCashData/Kontostand_newerFile.csv";
            LocalFileReader localFileReader = new LocalFileReader();

            //Act
            string actualFilePath = localFileReader.GetLatestFilePath(directory, searchForName);
            Console.WriteLine(actualFilePath);

            //Assert
            Assert.AreEqual(expectedFilePath, actualFilePath, "FilePath is wrong.");
        }

    }
}
