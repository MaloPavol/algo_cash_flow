using System;
using System.Collections.Generic;
using System.Text;
using FileHelpers;

namespace AlgoCashFlow.LocalFileSystem
{
    [IgnoreFirst]
    [DelimitedRecord(",")]
    public class DataMapSalesCashflow
    {
        [FieldTrim(TrimMode.Both)]
        [FieldQuoted('"', QuoteMode.OptionalForRead, MultilineMode.AllowForRead)]
        [FieldConverter(ConverterKind.Date, "dd.MM.yyyy")]
        public DateTime Lieferdatum;

        public double Nettopreis;

        public string Kommentar;

        public bool Eingerechnet;
    }
}
