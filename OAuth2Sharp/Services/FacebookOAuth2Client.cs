using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace OAuth2Sharp.Services
{

    public class FacebookOAuth2Client : BaseOAuth2Client<OAuth2AccessToken, FacebookUserInfo>
    {

        public FacebookOAuth2Client(OAuth2ClientBuilder builder) : base(ServicesClientConfigs.Facebook, builder) { }

        public FacebookOAuth2Client(OAuth2ClientConfig config, OAuth2ClientBuilder builder) : base(config, builder) { }

        protected override RestRequest CreateUserInfoRequest(string accessToken)
        {
            var request = new RestRequest(this.UserInfoEndpoint);

            request
                .AddQueryParameter("input_token", accessToken)
                .AddQueryParameter("access_token", this.ClientId);

            return request;
        }

    }

    public class FacebookUserInfo
    {
        public FacebookUserInfo Data { get; set; }

        public class FacebookUserInfoData
        {
            [JsonProperty("app_id")]
            public long AppId { get; set; }

            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("application")]
            public string Application { get; set; }

            [JsonProperty("expires_at")]
            public long ExpiresAt { get; set; }

            [JsonProperty("is_valid")]
            public bool IsValid { get; set; }

            [JsonProperty("issued_at")]
            public long IssuedAt { get; set; }

            [JsonProperty("metadata")]
            public FacebookUserInfoMetadata Metadata { get; set; }

            [JsonProperty("scopes")]
            public string[] Scopes { get; set; }

            [JsonProperty("user_id")]
            public string UserId { get; set; }

            public class FacebookUserInfoMetadata
            {
                [JsonProperty("sso")]
                public string Sso { get; set; }
            }
        }
    }

}
