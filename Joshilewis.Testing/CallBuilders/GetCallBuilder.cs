using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Joshilewis.Cqrs.Query;
using Joshilewis.Infrastructure.Auth;
using NUnit.Framework;
using ServiceStack.ServiceModel.Serialization;

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

        public void Returns<TPayload>(params TPayload[] expected)
        {
            ReturnsResult(new Result<TPayload[]>(expected));
        }

        private void ReturnsResult<TResult>(TResult expected) where TResult : Result
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
            TResult actualResult = JsonDataContractDeserializer.Instance.DeserializeFromString<TResult>(getResponseString);
            TestValueEqualityHelpers.CompareValueEquality(actualResult, expected);
        }

    }
}
