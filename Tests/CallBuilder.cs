using System;
using System.Net.Http;
using Lending.Execution.Auth;

namespace Tests
{
    public abstract class CallBuilder
    {
        public Exception Exception { get; protected set; }
        protected readonly HttpClient Client;
        protected readonly Tokeniser Tokeniser;

        protected CallBuilder(HttpClient client, Tokeniser tokeniser)
        {
            Client = client;
            Tokeniser = tokeniser;
        }
    }
}