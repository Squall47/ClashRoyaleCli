using Microsoft.Rest;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace ClashRoyaleApi
{
    public class BearerCredentials : ServiceClientCredentials
    {
        private string _token;

        public BearerCredentials(string token)
        {
            _token = token;
        }
        public override void InitializeServiceClient<T>(ServiceClient<T> client)
        {
            base.InitializeServiceClient(client);
        }
        public override Task ProcessHttpRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            return base.ProcessHttpRequestAsync(request, cancellationToken);
        }
    }
}
