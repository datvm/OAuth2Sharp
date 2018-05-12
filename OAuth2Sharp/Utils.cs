using OAuth2Sharp.Core;
using OAuth2Sharp.Extensions;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Reflection;
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
                    $"Status code is not successful. Status Code: {(int)restResponse.StatusCode} ({restResponse.StatusDescription})",
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

        public static Dictionary<string, PropertyInfo> GetTypeProperties(Type type)
        {
            var result = new Dictionary<string, PropertyInfo>();

            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                result.Add(property.Name, property);
            }

            return result;
        }

        public static TOption ExecuteOptions<TOption>(this Action<TOption> options) where TOption : new()
        {
            var result = new TOption();
            options(result);

            return result;
        }

        public static OverrideQueryParameterRestRequest AddQueryParameterWithOverride(this IRestRequest request, Dictionary<string, string> overrideValues)
        {
            return new OverrideQueryParameterRestRequest(request, overrideValues);
        }

        public static void CopyProperties<TTarget>(object source, TTarget target)
        {
            var sourceProperties = GetTypeProperties(source.GetType());
            CopyProperties(sourceProperties, source, target);
        }

        public static void CopyProperties(Dictionary<string, PropertyInfo> sourceProperties, object source, object target)
        {
            var clientProperties = target.GetType().GetProperties();

            foreach (var clientProperty in clientProperties)
            {
                if (sourceProperties.TryGetValue(clientProperty.Name, out var sourceProperty))
                {
                    var value = sourceProperty.GetValue(source);
                    clientProperty.SetValue(target, value);
                }
            }
        }

        public static void AddIfNotExist<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            if (!dict.ContainsKey(key))
            {
                dict.Add(key, value);
            }
        }

    }

}
