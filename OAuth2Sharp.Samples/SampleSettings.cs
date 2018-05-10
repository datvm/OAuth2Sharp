using OAuth2Sharp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth2Sharp.Samples
{
    internal class SampleSettings
    {

        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string RedirectUri { get; set; }
        public string Service { get; set; }

        public OAuth2ClientBuilder ToOAuth2ClientBuilder()
        {
            return new OAuth2ClientBuilder()
            {
                ClientId = this.ClientId,
                ClientSecret = this.ClientSecret,
                RedirectUri = this.RedirectUri,
            };
        }

    }
}
