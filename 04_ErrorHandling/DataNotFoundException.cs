using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoCashFlow.ErrorHandling
{
    public class DataNotFoundException : Exception
    {
        public DataNotFoundException(string message) : base(message) { }
    }
}
