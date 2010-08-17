using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommandLine;

namespace PrintNCI
{
    public sealed class Options
    {
        [Option("s", "settings", Required = false, HelpText = "Settings")]
        public bool Settings;

        [Option("st", "status", Required = false, HelpText = "Status")]
        public bool Status;

        [Option("b", "bill", Required = false, HelpText = "Billing")]
        public bool Billing;

        [Option("t", "test", Required = false, HelpText = "Test")]
        public bool Test;


    }
}
