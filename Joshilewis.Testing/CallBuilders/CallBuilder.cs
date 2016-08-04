using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using Joshilewis.Infrastructure.Auth;

namespace Joshilewis.Testing.CallBuilders
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
        protected string Serialize<T>(MediaTypeFormatter formatter, T value)
        {
            // Create a dummy HTTP Content.
            Stream stream = new MemoryStream();
            var content = new StreamContent(stream);
            /// Serialize the object.
            formatter.WriteToStreamAsync(typeof(T), value, stream, content, null).Wait();
            // Read the serialized string.
            stream.Position = 0;
            return content.ReadAsStringAsync().Result;
        }

    }
}