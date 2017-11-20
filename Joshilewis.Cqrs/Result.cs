using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Joshilewis.Cqrs
{
    public class Result
    {
        public EResultCode Code { get; }
        public object Payload { get; }

        public Result(EResultCode code, object payload)
        {
            Code = code;
            Payload = payload;
        }

        public Result(EResultCode code)
        {
            Code = code;
        }
    }
}
