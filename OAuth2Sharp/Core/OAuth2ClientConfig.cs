using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace OAuth2Sharp.Core
{

    /// <summary>
    /// Configuration of generally unchanged value of clients (endpoints).
    /// All of the values are optional and usually are set by Service implementation, not by user.
    /// </summary>
    public class OAuth2ClientConfig
    {

        private static Dictionary<string, PropertyInfo> ClientConfigProperties
            = Utils.GetTypeProperties(typeof(OAuth2ClientConfig));

        public string AuthorizationEndpoint { get; set; }
        public string TokenEndpoint { get; set; }
        public string TokenRevocationEndpoint { get; set; }
        public string UserInfoEndpoint { get; set; }
        public string DiscoveryEndpoint { get; set; }

        /// <summary>
        /// Copy all properties of the Config into the Client (only for those exist)
        /// </summary>
        internal void CopyToClient(object client)
        {
            Utils.CopyProperties(ClientConfigProperties, this, client);
        }

    }

}
