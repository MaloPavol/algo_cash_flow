using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AlgoCashFlow.LocalFileSystem
{
    class LocalFileReaderAndWriter : LocalFileReader
    {
        private bool canWrite = true;

        
        public bool CanWrite()
        {
            return canWrite;
        }

        public void WriteFile(string text, string path)
        {
            string createText = text + Environment.NewLine;
            File.WriteAllText(path, createText);
        }
    }
}
