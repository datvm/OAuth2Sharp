using System;
using System.Collections.Generic;
using System.Text;

namespace OAuth2Sharp.Services
{
    public static class ServicesClientConfigs
    {

        public static readonly OAuth2ClientConfig Facebook = new OAuth2ClientConfig()
        {
            DiscoveryDocument = null,
            AuthorizationEndpoint = new Uri("https://www.facebook.com/v2.12/dialog/oauth"),
            TokenEndpoint = new Uri("https://graph.facebook.com/v2.12/oauth/access_token"),
            UserInfoEndpoint = new Uri("https://graph.facebook.com/debug_token"),
        };

    }
}
