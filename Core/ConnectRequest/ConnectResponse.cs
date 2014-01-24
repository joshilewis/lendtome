namespace Core.ConnectRequest
{
    public class ConnectResponse
    {
        public const string AlreadyConnected = "The Users are already connected.";

        public bool Success { get; set; }
        public string FailureDescription { get; set; }

        public ConnectResponse()
        {
            Success = true;
            FailureDescription = null;
        }

        public ConnectResponse(string failureDescription)
        {
            FailureDescription = failureDescription;
        }
    }
}