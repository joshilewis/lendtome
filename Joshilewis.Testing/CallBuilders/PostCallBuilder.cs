using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using Joshilewis.Cqrs.Command;
using Joshilewis.Infrastructure.Auth;
using Newtonsoft.Json;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Joshilewis.Testing.CallBuilders
{
    public class PostCallBuilder : CallBuilder
    {
        public AuthenticatedCommand Command { get; }
        public HttpResponseMessage Response { get; private set; }
        public string Url { get; private set; }
        private readonly bool failIfUnsuccessful;

        public PostCallBuilder(HttpClient client, Tokeniser tokeniser, AuthenticatedCommand command, bool failIfUnsuccessful)
            : base(client, tokeniser)
        {
            Command = command;
            this.failIfUnsuccessful = failIfUnsuccessful;
        }

        public void IsPOSTedTo(string url)
        {
            Client.DefaultRequestHeaders.Authorization =
                                //new AuthenticationHeaderValue("eyJhbGciOiJSUzI1NiIsImtpZCI6IjAzYTAzYmRkNjBhNWU1NTFkY2RiZjkyMDVkOTc2YTAzMjU1OWYxOWMifQ.eyJpc3MiOiJodHRwczovL3NlY3VyZXRva2VuLmdvb2dsZS5jb20vbGVuZHRvbWUtOTNjNWMiLCJuYW1lIjoiSm9zaHVhIExld2lzIiwicGljdHVyZSI6Imh0dHBzOi8vbGg0Lmdvb2dsZXVzZXJjb250ZW50LmNvbS8tNlU1UWUwekhoeTAvQUFBQUFBQUFBQUkvQUFBQUFBQUFBUXMvU2F3ckRFWFlpRVEvcGhvdG8uanBnIiwiYXVkIjoibGVuZHRvbWUtOTNjNWMiLCJhdXRoX3RpbWUiOjE1MTEwNDAzNTQsInVzZXJfaWQiOiJnQmFVTmpmYmxNVUI5U2xweHo2cnRQRjJ1ZHAxIiwic3ViIjoiZ0JhVU5qZmJsTVVCOVNscHh6NnJ0UEYydWRwMSIsImlhdCI6MTUxMTA0MDM1NCwiZXhwIjoxNTExMDQzOTU0LCJlbWFpbCI6Impvc2hpbGV3aXNAZ21haWwuY29tIiwiZW1haWxfdmVyaWZpZWQiOnRydWUsImZpcmViYXNlIjp7ImlkZW50aXRpZXMiOnsiZ29vZ2xlLmNvbSI6WyIxMTc0NjI0NzY2MDg1NDE2Njc2ODEiXSwiZW1haWwiOlsiam9zaGlsZXdpc0BnbWFpbC5jb20iXX0sInNpZ25faW5fcHJvdmlkZXIiOiJnb29nbGUuY29tIn19.dU2JcJH06B8CKb51F-57d3G11GkAVGSTVltdg4CAR062wMXnN40KtGPX37AQk66HMEvX_jaaFDU8neRyzIU2olKce6YW4hxAQrGBzvqk31CLRyVwGe3RXaBn6twDxeQmZjHc1J_t3rdajK4Aitex_2nt_w3ZjIpJQmQr8Du0Do8l5CYydXQit4AFpKLBGbKzsXmEfoaalxUeZdzjqsZ8-rr1diKgDkd0VyiQJljg4YUG8gsNdUr2ESnMguJnqwe6-tpzlpDWgTBfnNR2Y5lwf8qe97CJpaws4_JfWPkliEc_MwRczpv73Tu7YeiL_PuTVtwNOSZAMticcqNU0MlcLw");
                new AuthenticationHeaderValue(Tokeniser.CreateToken("username", Command.UserId));
            Url = $"https://localhost/api/{url}";
            Response = Client.PostAsJsonAsync(Url, Command).Result;
            Console.WriteLine("POST to {0} with body {1} returned status {2} with reason {3}", url, Serialize(new JsonMediaTypeFormatter(), Command), Response.StatusCode, Response.ReasonPhrase);
            if (failIfUnsuccessful && !Response.IsSuccessStatusCode)
                throw new AssertionException(
                    $"POST call to '{Url}' was not successful, response code is {Response.StatusCode}, reason {Response.ReasonPhrase}");

        }

        public TResult IsPOSTedTo<TResult>(string url)
        {
            Client.DefaultRequestHeaders.Authorization =
                //new AuthenticationHeaderValue("eyJhbGciOiJSUzI1NiIsImtpZCI6IjAzYTAzYmRkNjBhNWU1NTFkY2RiZjkyMDVkOTc2YTAzMjU1OWYxOWMifQ.eyJpc3MiOiJodHRwczovL3NlY3VyZXRva2VuLmdvb2dsZS5jb20vbGVuZHRvbWUtOTNjNWMiLCJuYW1lIjoiSm9zaHVhIExld2lzIiwicGljdHVyZSI6Imh0dHBzOi8vbGg0Lmdvb2dsZXVzZXJjb250ZW50LmNvbS8tNlU1UWUwekhoeTAvQUFBQUFBQUFBQUkvQUFBQUFBQUFBUXMvU2F3ckRFWFlpRVEvcGhvdG8uanBnIiwiYXVkIjoibGVuZHRvbWUtOTNjNWMiLCJhdXRoX3RpbWUiOjE1MTEwNDAzNTQsInVzZXJfaWQiOiJnQmFVTmpmYmxNVUI5U2xweHo2cnRQRjJ1ZHAxIiwic3ViIjoiZ0JhVU5qZmJsTVVCOVNscHh6NnJ0UEYydWRwMSIsImlhdCI6MTUxMTA0MDM1NCwiZXhwIjoxNTExMDQzOTU0LCJlbWFpbCI6Impvc2hpbGV3aXNAZ21haWwuY29tIiwiZW1haWxfdmVyaWZpZWQiOnRydWUsImZpcmViYXNlIjp7ImlkZW50aXRpZXMiOnsiZ29vZ2xlLmNvbSI6WyIxMTc0NjI0NzY2MDg1NDE2Njc2ODEiXSwiZW1haWwiOlsiam9zaGlsZXdpc0BnbWFpbC5jb20iXX0sInNpZ25faW5fcHJvdmlkZXIiOiJnb29nbGUuY29tIn19.dU2JcJH06B8CKb51F-57d3G11GkAVGSTVltdg4CAR062wMXnN40KtGPX37AQk66HMEvX_jaaFDU8neRyzIU2olKce6YW4hxAQrGBzvqk31CLRyVwGe3RXaBn6twDxeQmZjHc1J_t3rdajK4Aitex_2nt_w3ZjIpJQmQr8Du0Do8l5CYydXQit4AFpKLBGbKzsXmEfoaalxUeZdzjqsZ8-rr1diKgDkd0VyiQJljg4YUG8gsNdUr2ESnMguJnqwe6-tpzlpDWgTBfnNR2Y5lwf8qe97CJpaws4_JfWPkliEc_MwRczpv73Tu7YeiL_PuTVtwNOSZAMticcqNU0MlcLw");
                new AuthenticationHeaderValue(Tokeniser.CreateToken("username", Command.UserId));
            Url = $"https://localhost/api/{url}";
            Response = Client.PostAsJsonAsync(Url, Command).Result;
            Console.WriteLine("POST to {0} with body {1} returned status {2} with reason {3}", url, Serialize(new JsonMediaTypeFormatter(), Command), Response.StatusCode, Response.ReasonPhrase);
            if (failIfUnsuccessful && !Response.IsSuccessStatusCode)
                throw new AssertionException(
                    $"POST call to '{Url}' was not successful, response code is {Response.StatusCode}, reason {Response.ReasonPhrase}");
            string getResponseString = Response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<TResult>(getResponseString);
        }

    }
}