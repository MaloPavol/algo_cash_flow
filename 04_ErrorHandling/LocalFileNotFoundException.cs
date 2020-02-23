using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoCashFlow.ErrorHandling
{
    public class LocalFileNotFoundException : Exception
    {
        public LocalFileNotFoundException(string message) : base(message) { }
    }
}