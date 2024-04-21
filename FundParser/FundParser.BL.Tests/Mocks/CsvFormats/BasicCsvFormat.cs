using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundParser.BL.Tests.Mocks.CsvFormats
{
    public class BasicCsvFormat
    {
        public DateTime DateTime { get; set; }

        public string String { get; set; } 

        public decimal Decimal { get; set; }

        public int Int { get; set; }
    }
}
