using Newtonsoft.Json;
using OAuth2Sharp.Core;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OAuth2Sharp
{

    public class FacebookOAuth2Client : DefaultOAuth2UserInfoOptionClient<DefaultOAuth2AccessToken, DefaultOAuth2AccessToken, FacebookUserInfo,
        FacebookOAuth2ClientCreateAuthorizationUriOption, FacebookOAuth2ClientOperationOptions>
    {

        public DefaultOAuth2AccessToken AppAccessToken { get; private set; }

        public FacebookOAuth2Client(OAuth2ClientBuilder builder) : base(ServicesClientConfigs.Facebook, builder) { }

        public FacebookOAuth2Client(Func<OAuth2ClientBuilder> builder) : base(() => ServicesClientConfigs.Facebook, builder) { }

        public FacebookOAuth2Client(OAuth2ClientConfig config, OAuth2ClientBuilder builder) : base(config, builder) { }

        public FacebookOAuth2Client(Func<OAuth2ClientConfig> config, Func<OAuth2ClientBuilder> builder) : base(config, builder) { }

        public async Task RequestAppAccessTokenAsync()
        {
            var request = this.CreateRequestAppAccessTokenRequest();
            this.AppAccessToken = await this.ExecuteRequestAsync<DefaultOAuth2AccessToken>(request);
        }

        protected virtual RestRequest CreateRequestAppAccessTokenRequest()
        {
            var request = new RestRequest(this.TokenEndpoint);

            request
                .AddQueryParameter("client_id", this.ClientId)
                .AddQueryParameter("client_secret", this.ClientSecret)
                .AddQueryParameter("grant_type", "client_credentials");

            return request;
        }

        protected override RestRequest CreateUserInfoRequest<TOption>(string accessToken, TOption options)
        {
            if (string.IsNullOrEmpty(this.AppAccessToken?.AccessToken))
            {
                throw new FacebookOAuth2Exception("AppAccessToken is not set yet. Please call RequestAppAccessTokenAsync method before calling this method.");
            }

            var request = new RestRequest(this.UserInfoEndpoint);

            request
                .AddQueryParameterWithOverride(options.CustomValues)
                .AddQueryParameter("access_token", accessToken)
                .AddQueryParameter("fields", "id,name,email")
                .AddRemainingParameters();

            return request;
        }

    }

    // TODO: Fill FacebookGraphUserInfo with User object from https://developers.facebook.com/docs/graph-api/reference/user
    public class FacebookUserInfo
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }
    }

    public class FacebookOAuth2Exception : OAuth2Exception
    {
        public FacebookOAuth2Exception()
        {
        }

        public FacebookOAuth2Exception(string message) : base(message)
        {
        }

        public FacebookOAuth2Exception(string message, IRestResponse response) : base(message, response)
        {
        }

        public FacebookOAuth2Exception(string message, Exception innerException) : base(message, innerException)
        {
        }

    }

    public class FacebookOAuth2ClientOperationOptions : DefaultOAuth2ClientOperationOptions { }

    public class FacebookOAuth2ClientCreateAuthorizationUriOption : DefaultOAuth2ClientCreateAuthorizationUriOption { }

}
