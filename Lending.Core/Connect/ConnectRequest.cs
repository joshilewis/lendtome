using System;

namespace Lending.Core.Connect
{
    public class ConnectRequest
    {
        public Guid FromUserId { get; set; }
        public Guid ToUserId { get; set; }
    }
}