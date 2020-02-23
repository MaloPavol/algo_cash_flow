using System;
using System.Collections.Generic;
using System.Text;
using FileHelpers;

namespace AlgoCashFlow.LocalFileSystem
{
    [IgnoreFirst]
    [DelimitedRecord(",")]
    public class DataMapPayrollCashflow
    {
        public double Nettogehaelter;

        public double Lohnabgaben;

        [FieldTrim(TrimMode.Both)]
        [FieldQuoted('"', QuoteMode.OptionalForRead, MultilineMode.AllowForRead)]
        [FieldConverter(ConverterKind.Date, "dd.MM.yyyy")]
        public DateTime Faellig;

        public bool Eingerechnet;
    }
}
