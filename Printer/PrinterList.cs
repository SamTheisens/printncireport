using System.Collections.Generic;
using System.Drawing.Printing;

namespace Printer
{
    public class PrinterList : SortedDictionary<int, string>
    {
        public PrinterList()
        {
            int i = 0;
            foreach (var printer in PrinterSettings.InstalledPrinters)
            {
                Add(i, printer.ToString());
                i++;
            }
        }
    }
}
