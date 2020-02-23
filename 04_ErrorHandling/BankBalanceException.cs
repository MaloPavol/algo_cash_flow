using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoCashFlow.ErrorHandling
{
    public class BankBalanceException : Exception
    {
        public BankBalanceException(string message) : base(message) { }
    }
}
