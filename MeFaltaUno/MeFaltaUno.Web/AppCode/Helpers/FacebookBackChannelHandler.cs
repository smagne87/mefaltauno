using MeFaltaUno.Web.AppCode.Base;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Web;

namespace MeFaltaUno.Web.AppCode.Helpers
{
    public class FacebookBackChannelHandler : HttpClientHandler
    {
        protected override async System.Threading.Tasks.Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            var result = await base.SendAsync(request, cancellationToken);
            if (!request.RequestUri.AbsolutePath.Contains("access_token"))
                return result;

            // For the access token we need to now deal with the fact that the response is now in JSON format, not form values. Owin looks for form values.
            var content = await result.Content.ReadAsStringAsync();
            var facebookOauthResponse = JsonConvert.DeserializeObject<FacebookOauthResponse>(content);

            var outgoingQueryString = HttpUtility.ParseQueryString(string.Empty);
            outgoingQueryString.Add(nameof(facebookOauthResponse.access_token), facebookOauthResponse.access_token);
            outgoingQueryString.Add(nameof(facebookOauthResponse.expires_in), facebookOauthResponse.expires_in + string.Empty);
            outgoingQueryString.Add(nameof(facebookOauthResponse.token_type), facebookOauthResponse.token_type);
            var postdata = outgoingQueryString.ToString();

            var modifiedResult = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(postdata)
            };

            return modifiedResult;
        }
    }
}