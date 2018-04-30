using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Threading.Tasks;

namespace OAuth2Sharp.Test
{
    internal static class Utils
    {

        public const string MockOAuthAccessCode = "123";
        public const string MockReturnedAccessToken = "456";
        public const string MockReturnedRefreshToken = "789";

        public static NameValueCollection ParseQueryString(this Uri uri)
        {
            return System.Web.HttpUtility.ParseQueryString(uri.Query);
        }

        public static async Task<IRestResponse> FakeAccessTokenRequest(IRestClient client, IRestRequest request)
        {
            return await Task.Run(() =>
            {
                if (request.Parameters.Find(q => q.Name == "code")
                        ?.Value.ToString() 
                        == Utils.MockOAuthAccessCode)
                {
                    return new RestResponse()
                    {
                        StatusCode = System.Net.HttpStatusCode.OK,
                        ResponseStatus = ResponseStatus.Completed,
                        Content = JsonConvert.SerializeObject(new
                        {
                            access_token = Utils.MockReturnedAccessToken,
                            refresh_token = Utils.MockReturnedRefreshToken,
                            expires_in = 3600,
                        }),
                    };
                }
                else
                {
                    return new RestResponse()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        ResponseStatus = ResponseStatus.Completed,
                        Content = JsonConvert.SerializeObject(new
                        {
                            Error = "Invalid Code",
                        }),
                    };
                }
            });
        }
    }


}
