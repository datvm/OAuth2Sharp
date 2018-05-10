using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace OAuth2Sharp.Core
{

    public class OAuth2ClientBuilder
    {

        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string RedirectUri { get; set; }

        internal void CopyToClient(object client)
        {
            Utils.CopyProperties(this, client);
        }

    }

}
