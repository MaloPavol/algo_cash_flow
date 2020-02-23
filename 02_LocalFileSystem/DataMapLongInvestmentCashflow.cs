using System;
using FileHelpers;
using System.Collections.Generic;
using System.Text;

namespace AlgoCashFlow.LocalFileSystem
{
    [IgnoreFirst]
    [DelimitedRecord(",")]
    public class DataMapLongInvestmentCashflow
    {

        [FieldTrim(TrimMode.Both)]
        [FieldQuoted('"', QuoteMode.OptionalForRead, MultilineMode.AllowForRead)]
        [FieldConverter(ConverterKind.Date, "dd.MM.yyyy")]
        public DateTime Investitionsdatum;

        public double Investitionsbetrag;

        public string Kommentar;

        public bool Eingerechnet;
    }
}
