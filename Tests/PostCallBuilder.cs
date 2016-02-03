using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Lending.Cqrs.Command;
using Lending.Execution.Auth;

namespace Tests
{
    public class PostCallBuilder : CallBuilder
    {
        public AuthenticatedCommand Command { get; }
        public HttpResponseMessage Response { get; private set; }
        public string Url { get; private set; }

        public PostCallBuilder(HttpClient client, Tokeniser tokeniser, AuthenticatedCommand command)
            : base(client, tokeniser)
        {
            Command = command;
        }

        public void IsPOSTedTo(string url)
        {
            try
            {
                
                Client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(Tokeniser.CreateToken("username", Command.UserId));
                Url = $"https://localhost/api/{url}";
                Response = Client.PostAsJsonAsync(Url, Command).Result;
            }
            catch (Exception exception)
            {
                Exception = exception;
            }
        }
    }
}