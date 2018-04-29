using System;
using System.Collections.Generic;
using System.Text;

namespace OAuth2Sharp
{

    public class OAuth2ClientConfig
    {

        public Uri DiscoveryDocument { get; set; }
        public Uri AuthorizationEndpoint { get; set; }
        public Uri TokenEndpoint { get; set; }
        public Uri UserInfoEndpoint { get; set; }
        public Uri RevocationEndpoint { get; set; }

    }

}
