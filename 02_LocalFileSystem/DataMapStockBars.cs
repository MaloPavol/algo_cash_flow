using System;
using System.Collections.Generic;
using System.Text;
using FileHelpers;

namespace AlgoCashFlow.LocalFileSystem
{
    [IgnoreFirst]
    [DelimitedRecord(",")]
    public class DataMapStockBars
    {
        public int t;
        public double o;
        public double h;
        public double l;
        public double c;
        public double v;
    }
}
