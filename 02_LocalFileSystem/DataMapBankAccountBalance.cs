using System;
using System.Collections.Generic;
using System.Text;
using FileHelpers;

namespace AlgoCashFlow.LocalFileSystem
{
    [IgnoreFirst]
    [DelimitedRecord(",")]
    public class DataMapBankAccountBalance
    {

        [FieldTrim(TrimMode.Both)]
        [FieldQuoted('"', QuoteMode.OptionalForRead, MultilineMode.AllowForRead)]
        [FieldConverter(ConverterKind.Date, "dd.MM.yyyy")]
        public DateTime Datum;

        public double Kontostand;
    }
}
