using System.Net.Http;
using System.Net.Http.Headers;
using Joshilewis.Cqrs.Command;
using Joshilewis.Infrastructure.Auth;
using NUnit.Framework;

namespace Joshilewis.Testing.CallBuilders
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
