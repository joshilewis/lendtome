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
    public class GetCallBuilder<TResult> : CallBuilder where TResult : Result
    {
        public string Url { get; private set; }
        public TResult Result { get; private set; }
        public Guid UserId { get; private set; }

        public GetCallBuilder(HttpClient client, Tokeniser tokeniser, string url)
            : base(client, tokeniser)
        {
            Url = url;
            UserId = Guid.Empty;
        }

        public GetCallBuilder<TResult> As(Guid userId)
        {
            UserId = userId;
            return this;
        } 
        
        public void Returns(TResult expectedResult) 
        {
            var response = Client.GetAsync($"https://localhost/api/{Url}").Result;
            if (UserId != Guid.Empty)
            {
                Client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(Tokeniser.CreateToken("username", UserId));
            }

            if (!response.IsSuccessStatusCode)
                throw new AssertionException(
                    $"GET call to '{Url}' was not successful, response code is {response.StatusCode}, reason {response.ReasonPhrase}");
            string getResponseString = response.Content.ReadAsStringAsync().Result;
            TResult actualResult = JsonDataContractDeserializer.Instance.DeserializeFromString<TResult>(getResponseString);
            actualResult.ShouldEqual(expectedResult);
        }
    }
}
