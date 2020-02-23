using System;
using System.Collections.Generic;
using System.Text;
using Serilog;

namespace AlgoCashFlow.Logs
{
    public class Logger
    {
        public void SetUp()
        {
            Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .WriteTo.File("consoleapp.log")
                    .CreateLogger();
        }
    }
}
