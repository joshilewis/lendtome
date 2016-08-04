using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lending.Execution.Auth
{
    public class Code
    {
        public string clientId { get; set; }
        public string code { get; set; }
        public string redirectUri { get; set; }
    }
}
