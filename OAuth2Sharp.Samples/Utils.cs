using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth2Sharp.Samples
{
    internal static class Utils
    {

        public static NameValueCollection ParseQueryString(this Uri uri)
        {
            return System.Web.HttpUtility.ParseQueryString(uri.Query);
        }

    }
}
