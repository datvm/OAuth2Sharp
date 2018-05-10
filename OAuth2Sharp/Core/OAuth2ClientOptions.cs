using System;
using System.Collections.Generic;
using System.Text;

namespace OAuth2Sharp.Core
{

    public class DefaultOAuth2ClientOperationOptions
    {
        public Dictionary<string, string> CustomValues { get; set; } = new Dictionary<string, string>();
    }

    public class DefaultOAuth2ClientCreateAuthorizationUriOption : DefaultOAuth2ClientOperationOptions
    {
        public string State { get; set; }
        public List<string> Scope { get; set; } = new List<string>();
    }

}
