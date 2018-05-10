using OAuth2Sharp.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace OAuth2Sharp
{
    public static class ServicesClientConfigs
    {

        public static readonly OAuth2ClientConfig Facebook = new OAuth2ClientConfig()
        {
            AuthorizationEndpoint = "https://www.facebook.com/v2.12/dialog/oauth",
            TokenEndpoint = "https://graph.facebook.com/v2.12/oauth/access_token",
            UserInfoEndpoint = "https://graph.facebook.com/me",
        };

    }
}
