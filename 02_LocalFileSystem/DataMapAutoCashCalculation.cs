using System;
using System.Collections.Generic;
using System.Text;
using FileHelpers;

namespace AlgoCashFlow.LocalFileSystem
{
    [IgnoreFirst]
    [DelimitedRecord(",")]
    public class DataMapAutoCashCalculation
    {

        [FieldTrim(TrimMode.Both)]
        [FieldQuoted('"', QuoteMode.OptionalForRead, MultilineMode.AllowForRead)]
        [FieldConverter(ConverterKind.Date, "dd.MM.yyyy")]
        public DateTime Datum;

        public double Kontostand;

        public double Treasury;

        public double Anlagenbuchhaltung;

        public double Sales;

        public double Payroll;

        public double Sonstige;

        public double Summe;

        public string Kommentar;
    }
}
