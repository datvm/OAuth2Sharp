using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace OAuth2Sharp
{

    internal static class Utils
    {

        public static void EnsureSuccessful(this IRestResponse restResponse)
        {
            if (restResponse.ResponseStatus != ResponseStatus.Completed)
            {
                throw new OAuth2Exception(
                    $"Request was not completed. Response Status: {restResponse.ResponseStatus}",
                    restResponse);
            }

            if (!restResponse.IsSuccessful)
            {
                throw new OAuth2Exception(
                    $"Status code is not successful. Status Code: {(int) restResponse.StatusCode} ({restResponse.StatusDescription})",
                    restResponse);
            }
        }

        public static IRestRequest AddQueryParameters(this IRestRequest request, IEnumerable<KeyValuePair<string, string>> parameters)
        {
            foreach (var param in parameters)
            {
                request.AddQueryParameter(param.Key, param.Value);
            }

            return request;
        }

    }

}
