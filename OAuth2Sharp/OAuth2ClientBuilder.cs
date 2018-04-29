using System;
using System.Collections.Generic;
using System.Text;

namespace OAuth2Sharp
{

    public class OAuth2ClientBuilder
    {

        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public Uri RedirectUri { get; set; }

    }

}
