using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Lending.Cqrs.Query;
using Lending.Execution.Auth;
using NUnit.Framework;
using ServiceStack.ServiceModel.Serialization;

namespace Tests
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

        public void Returns<TPayload>(TPayload expected)
        {
            ReturnsResult<Result<TPayload>>(new Result<TPayload>(expected));
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
            TestEqualityHelpers.CompareValueEquality(actualResult, expected);
        }

    }
}
