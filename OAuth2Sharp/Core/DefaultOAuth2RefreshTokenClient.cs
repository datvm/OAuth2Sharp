using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OAuth2Sharp.Core
{
    public class DefaultOAuth2RefreshTokenClient<TToken, TRefreshToken> : DefaultOAuth2Client<TToken>, IOAuth2RefreshableTokenClient<TToken, TRefreshToken>
    {
        public DefaultOAuth2RefreshTokenClient(Func<OAuth2ClientConfig> clientConfig, Func<OAuth2ClientBuilder> clientBuilder) : base(clientConfig, clientBuilder) { }

        public DefaultOAuth2RefreshTokenClient(OAuth2ClientConfig clientConfig, OAuth2ClientBuilder clientBuilder) : base(clientConfig, clientBuilder) { }

        public virtual async Task<TRefreshToken> RefreshAccessTokenAsync<TOption>(string refreshToken, TOption options) where TOption : DefaultOAuth2ClientOperationOptions, new()
        {
            options = options ?? new TOption();

            var request = this.CreateRefreshTokenRequest(refreshToken, options);
            return await this.ExecuteRequestAsync<TRefreshToken>(request);
        }

        protected virtual RestRequest CreateRefreshTokenRequest<TOption>(string refreshToken, TOption options) where TOption : DefaultOAuth2ClientOperationOptions, new()
        {
            var request = new RestRequest(this.TokenEndpoint, Method.POST);

            request
                .AddQueryParameterWithOverride(options.CustomValues)
                .AddBodyFormUrlEncoded("client_id", this.ClientId)
                .AddBodyFormUrlEncoded("client_secret", this.ClientSecret)
                .AddBodyFormUrlEncoded("refresh_token", refreshToken)
                .AddBodyFormUrlEncoded("grant_type", "refresh_token")
                .AddPostFormUrlEncodedHeader()
                .AddRemainingParameters();

            return request;
        }

        public virtual Task<TRefreshToken> RefreshAccessTokenAsync(string refreshToken)
        {
            return this.RefreshAccessTokenAsync(refreshToken, new DefaultOAuth2ClientOperationOptions());
        }

        public virtual Task<TRefreshToken> RefreshAccessTokenAsync<TOption>(string refreshToken, Action<TOption> options) where TOption : DefaultOAuth2ClientOperationOptions, new()
        {
            return this.RefreshAccessTokenAsync(refreshToken, options.ExecuteOptions());
        }

    }
}
