using System;
using System.Collections.Generic;
using System.Text;
using FileHelpers;

namespace AlgoCashFlow.LocalFileSystem
{
   
    [DelimitedRecord("|")]
    public class DataMapStockBarsJSON
    {
        public string JSON;
    }
}
