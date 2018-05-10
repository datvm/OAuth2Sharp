using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OAuth2Sharp.Core.OpenId
{

    public interface IOpenIdClient<TToken> : IOAuth2Client<TToken>
    {

        string DiscoveryEndpoint { get; }
        OpenIdDiscoveryDocument DiscoveryDocument { get; }

    }

}
