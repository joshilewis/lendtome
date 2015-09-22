namespace Lending.Core.ConnectionRequest
{
    public class ConnectionRequest
    {
        public long FromUserId { get; set; }
        public long ToUserId { get; set; }

        public ConnectionRequest()
        {
            
        }

        public ConnectionRequest(long fromUserId, long toUserId)
        {
            FromUserId = fromUserId;
            ToUserId = toUserId;
        }
    }
}