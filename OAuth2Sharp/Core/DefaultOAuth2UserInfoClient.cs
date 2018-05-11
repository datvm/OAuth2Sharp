using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OAuth2Sharp.Core
{
    public class DefaultOAuth2UserInfoClient<TToken, TRefreshToken, TUserInfo> : DefaultOAuth2RefreshTokenClient<TToken, TRefreshToken>, IOAuth2UserInfoClient<TToken, TUserInfo>
        where TUserInfo : IOAuth2UserInfo, new()
    {

        public string UserInfoEndpoint { get; set; }

        public DefaultOAuth2UserInfoClient(Func<OAuth2ClientConfig> clientConfig, Func<OAuth2ClientBuilder> clientBuilder) : base(clientConfig, clientBuilder) { }

        public DefaultOAuth2UserInfoClient(OAuth2ClientConfig clientConfig, OAuth2ClientBuilder clientBuilder) : base(clientConfig, clientBuilder) { }

        public virtual async Task<TUserInfo> GetUserInfoAsync<TOption>(string token, TOption options) where TOption : DefaultOAuth2ClientOperationOptions, new()
        {
            var request = this.CreateUserInfoRequest(token, options);
            return await this.ExecuteRequestAsync<TUserInfo>(request);
        }

        protected virtual RestRequest CreateUserInfoRequest<TOption>(string accessToken, TOption options) where TOption : DefaultOAuth2ClientOperationOptions, new()
        {
            var request = new RestRequest(this.UserInfoEndpoint);

            request
                .AddQueryParameterWithOverride(options.CustomValues)
                .AddHeader("Authorization", "Bearer " + accessToken)
                .AddRemainingParameters();

            return request;
        }

        public virtual Task<TUserInfo> GetUserInfoAsync(string token)
        {
            return this.GetUserInfoAsync(token, new DefaultOAuth2ClientOperationOptions());
        }

        public virtual Task<TUserInfo> GetUserInfoAsync<TOption>(string token, Action<TOption> options) where TOption : DefaultOAuth2ClientOperationOptions, new()
        {
            return this.GetUserInfoAsync(token, options.ExecuteOptions());
        }

    }
}
