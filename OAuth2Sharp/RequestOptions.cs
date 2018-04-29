using System;
using System.Collections.Generic;
using System.Text;

namespace OAuth2Sharp
{

    public class OAuth2UriRequestOptions
    {
        public Uri OverrideRedirectUri { get; set; }
        public string OverrideResponseType { get; set; }
        public string State { get; set; }
        public List<string> Scope { get; set; } = new List<string>();
        public Dictionary<string, string> CustomParameters { get; set; } = new Dictionary<string, string>();
    }

    public class OAuth2AccessTokenRequestOptions
    {
        public Uri OverrideRedirectUri { get; set; }
        public string OverrideGrantType { get; set; }
        public Dictionary<string, string> CustomParameters { get; set; } = new Dictionary<string, string>();
    }

    public class OAuth2RefreshAccessTokenRequestOptions
    {
        public string OverrideGrantType { get; set; }
        public Dictionary<string, string> CustomParameters { get; set; } = new Dictionary<string, string>();
    }

}
