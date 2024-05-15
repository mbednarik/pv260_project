using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundParser.BL.Exceptions
{
    public class ApiErrorException : Exception
    {
        public ApiErrorException(string? message) : base(message) { }
    }
}
