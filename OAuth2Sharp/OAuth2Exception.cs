using RestSharp;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace OAuth2Sharp
{
    public class OAuth2Exception : Exception
    {

        public IRestResponse Response { get; private set; }

        public OAuth2Exception()
        {
        }

        public OAuth2Exception(string message) : base(message)
        {
        }

        public OAuth2Exception(string message, IRestResponse response) : this(message)
        {
            this.Response = response;
        }

        public OAuth2Exception(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected OAuth2Exception(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
