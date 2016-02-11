using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Joshilewis.Cqrs.Command;
using Lending.Execution.Auth;
using Nancy;
using NUnit.Framework;

namespace Tests
{
    public class MultiPostCallBuilder : CallBuilder
    {
        public AuthenticatedCommand[] CommandsToPost { get; }
        public string Url { get; private set; }

        public MultiPostCallBuilder(HttpClient client, Tokeniser tokeniser, params AuthenticatedCommand[] commandsToPost)
            : base(client, tokeniser)
        {
            CommandsToPost = commandsToPost;
        }

        public void ArePOSTedTo(string url)
        {
            Url = $"https://localhost/api/{url}";

            foreach (var command in CommandsToPost)
            {
                Client.DefaultRequestHeaders.Authorization =new AuthenticationHeaderValue(Tokeniser.CreateToken("username", command.UserId));
                HttpResponseMessage response = Client.PostAsJsonAsync(Url, command).Result;
                if (!response.IsSuccessStatusCode)
                    throw new AssertionException(
                        $"POST call to '{Url}' was not successful, response code is {response.StatusCode}, reason {response.ReasonPhrase}");
            }
        }
    }
}
