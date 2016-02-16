using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Joshilewis.Cqrs.Query;
using Joshilewis.Infrastructure.Auth;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Joshilewis.Testing.CallBuilders
{
    public class GetCallBuilder : CallBuilder
    {
        public string Url { get; private set; }
        public Guid? UserId { get; private set; }

        public GetCallBuilder(HttpClient client, Tokeniser tokeniser, string url)
            : base(client, tokeniser)
        {
            Url = url;
            UserId = null;
        }

        public GetCallBuilder As(Guid userId)
        {
            UserId = userId;
            return this;
        }

        public void Returns<TResult>(params TResult[] expected)
        {
            if (UserId.HasValue)
            {
                Client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(Tokeniser.CreateToken("username", UserId.Value));
            }

            var response = Client.GetAsync($"https://localhost/api/{Url}").Result;
            if (!response.IsSuccessStatusCode)
                throw new AssertionException(
                    $"GET call to '{Url}' was not successful, response code is {response.StatusCode}, reason {response.ReasonPhrase}");
            string getResponseString = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine("Response for GET {0} is {1}", Url, getResponseString);
            TResult[] actualResult = JsonConvert.DeserializeObject<TResult[]>(getResponseString);
            Assert.That(actualResult, Is.EquivalentTo(expected));
        }

    }
}
