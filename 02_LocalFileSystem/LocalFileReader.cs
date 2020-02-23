using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using FileHelpers;
using AlgoCashFlow.ErrorHandling;
using AlgoCashFlow.Logs;
using Serilog;

namespace AlgoCashFlow.LocalFileSystem
{
    public class LocalFileReader
    {
        private bool canWrite = false;

        public bool CanWrite()
        {
            return canWrite;
        }

        public string GetLatestFilePath(string directory, string searchForName)
        {
            string[] filePaths = GetFilePaths(directory, searchForName);
            return GetLatestFilePathFromArray(filePaths);
        }


        public string GetLatestFilePathFromArray(string[] filePaths)
        {
            string latestFilePath = filePaths[0];
            DateTime lastModified = GetFileModificationTime(filePaths[0]);
            foreach(string filePath in filePaths)
                if (GetFileModificationTime(filePath)>lastModified)
                {
                    latestFilePath = filePath;
                }
            return latestFilePath;
        }

        public string[] GetFilePaths(string directory, string searchForName)
        {
            searchForName = "*" + searchForName + "*";

            string[] filePaths = Directory.GetFiles(directory, searchForName);

 
                if (filePaths.Length<1)
                {
                    throw (new DataNotFoundException("Data file not found."));
                }

            return filePaths;
        }

        public DateTime GetFileCreationTime(string path)
        {
            return System.IO.File.GetCreationTime(path);
        }

        public DateTime GetFileModificationTime(string path)
        {
            return System.IO.File.GetLastWriteTime(path);
        }

        public string ReadFile(string path)
        {
            string text = File.ReadAllText(path);
            return text;
        }

        
    }
}
