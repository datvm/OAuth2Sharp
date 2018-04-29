using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace OAuth2Sharp.Test
{
    internal static class Utils
    {

        public static NameValueCollection ParseQueryString(this Uri uri)
        {
            return System.Web.HttpUtility.ParseQueryString(uri.Query);
        }

    }
}
