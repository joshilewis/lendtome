using System;

namespace Core.ConnectRequest
{
    public class ConnectRequest
    {
        public Guid FromUserId { get; set; }
        public Guid ToUserId { get; set; }
    }
}