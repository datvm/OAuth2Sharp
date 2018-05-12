using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace OAuth2Sharp.Extensions
{

    internal class OverrideQueryParameterRestRequest
    {

        private Dictionary<string, string> overrideValues;
        private HashSet<string> addedValues = new HashSet<string>();

        public IRestRequest RestRequest { get; private set; }

        public OverrideQueryParameterRestRequest(IRestRequest request, Dictionary<string, string> overrideValues)
        {
            this.RestRequest = request;
            this.overrideValues = overrideValues;
        }

        public OverrideQueryParameterRestRequest AddQueryParameter(string name, string value)
        {
            if (this.overrideValues.TryGetValue(name, out var overrideValue))
            {
                this.RestRequest.AddQueryParameter(name, overrideValue);
            }
            else
            {
                this.RestRequest.AddQueryParameter(name, value);
            }
            addedValues.Add(name);

            return this;
        }

        public OverrideQueryParameterRestRequest AddBodyFormUrlEncoded(string name, string value)
        {
            if (this.overrideValues.TryGetValue(name, out var overrideValue))
            {
                this.RestRequest.AddParameter(name, overrideValue, ParameterType.GetOrPost);
            }
            else
            {
                this.RestRequest.AddParameter(name, value, ParameterType.GetOrPost);
            }
            addedValues.Add(name);

            return this;
        }

        public OverrideQueryParameterRestRequest AddHeader(string name, string value)
        {
            if (this.overrideValues.TryGetValue(name, out var overrideValue))
            {
                this.RestRequest.AddHeader(name, overrideValue);
            }
            else
            {
                this.RestRequest.AddHeader(name, value);
            }
            addedValues.Add(name);

            return this;
        }

        public OverrideQueryParameterRestRequest AddPostFormUrlEncodedHeader()
        {
            this.RestRequest.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                
            return this;
        }

        public OverrideQueryParameterRestRequest AddUrlSegment(string name, string value)
        {
            if (this.overrideValues.TryGetValue(name, out var overrideValue))
            {
                this.RestRequest.AddUrlSegment(name, overrideValue);
            }
            else
            {
                this.RestRequest.AddUrlSegment(name, value);
            }
            addedValues.Add(name);

            return this;
        }

        public OverrideQueryParameterRestRequest AddRemainingParameters()
        {
            foreach (var parameter in this.overrideValues)
            {
                if (!addedValues.Contains(parameter.Key))
                {
                    this.RestRequest.AddQueryParameter(parameter.Key, parameter.Value);
                }
            }

            return this;
        }

    }

}
