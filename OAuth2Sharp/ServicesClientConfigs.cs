﻿using OAuth2Sharp.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace OAuth2Sharp
{
    public static class ServicesClientConfigs
    {

        public static readonly OAuth2ClientConfig Facebook = new OAuth2ClientConfig()
        {
            AuthorizationEndpoint = "https://www.facebook.com/v3.0/dialog/oauth",
            TokenEndpoint = "https://graph.facebook.com/v3.0/oauth/access_token",
            UserInfoEndpoint = "https://graph.facebook.com/me",
        };

        public static readonly OAuth2ClientConfig Microsoft = new OAuth2ClientConfig()
        {
            AuthorizationEndpoint = "https://login.microsoftonline.com/common/oauth2/v2.0/authorize",
            TokenEndpoint = "https://login.microsoftonline.com/common/oauth2/v2.0/token",
            UserInfoEndpoint = "https://graph.microsoft.com/v1.0/me",
        };

    }
}
