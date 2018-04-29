using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OAuth2Sharp
{

    public abstract class BaseOAuth2Client<TToken> : IOAuth2Client<TToken>
        where TToken : OAuth2AccessToken
    {
        protected RestClient RestClient { get; set; }

        public string ClientId { get; protected set; }
        public string ClientSecret { get; protected set; }
        public Uri RedirectUri { get; protected set; }

        public Uri DiscoveryDocument { get; protected set; }
        public Uri AuthorizationEndpoint { get; protected set; }
        public Uri TokenEndpoint { get; protected set; }
        public Uri RevocationEndpoint { get; protected set; }

        public BaseOAuth2Client(OAuth2ClientConfig config, OAuth2ClientBuilder builder)
        {
            this.DiscoveryDocument = config.DiscoveryDocument;
            this.AuthorizationEndpoint = config.AuthorizationEndpoint;
            this.TokenEndpoint = config.TokenEndpoint;
            this.RevocationEndpoint = config.RevocationEndpoint;

            this.ClientId = builder.ClientId;
            this.ClientSecret = builder.ClientSecret;
            this.RedirectUri = builder.RedirectUri;

            this.RestClient = new RestClient(this.AuthorizationEndpoint);
        }

        public async Task<Uri> CreateRequestUriAsync()
        {
            return await this.CreateRequestUriAsync(null);
        }

        public Task<Uri> CreateRequestUriAsync(OAuth2UriRequestOptions options)
        {
            options = options ?? new OAuth2UriRequestOptions();
            options.State = options.State ?? Guid.NewGuid().ToString();

            return Task.Run(() =>
            {
                var request = this.CreateRequestUriRestRequest(options);
                return this.RestClient.BuildUri(request);
            });
        }

        protected virtual RestRequest CreateRequestUriRestRequest(OAuth2UriRequestOptions options)
        {
            var request = new RestRequest(this.AuthorizationEndpoint.AbsoluteUri);
            request
                .AddQueryParameter("client_id", this.ClientId)
                .AddQueryParameter(
                    "redirect_uri",
                    (options.OverrideRedirectUri ?? this.RedirectUri).AbsoluteUri)
                .AddQueryParameter("state", options.State)
                .AddQueryParameter("response_type", options.OverrideResponseType ?? "code")
                .AddQueryParameter("scope", string.Join(" ", options.Scope))
                .AddQueryParameters(options.CustomParameters);

            return request;
        }

        public Task<TToken> RequestAccessTokenAsync(string code)
        {
            return this.RequestAccessTokenAsync(code, null);
        }

        public async Task<TToken> RequestAccessTokenAsync(string code, OAuth2AccessTokenRequestOptions options)
        {
            options = options ?? new OAuth2AccessTokenRequestOptions();

            var request = this.CreateRequestAccessTokenRestRequest(code, options);
            return await this.ExecuteRequestAsync<TToken>(request);
        }

        protected virtual RestRequest CreateRequestAccessTokenRestRequest(string code, OAuth2AccessTokenRequestOptions options)
        {
            var request = new RestRequest(this.TokenEndpoint.AbsoluteUri, Method.POST);

            request
                .AddQueryParameter("client_id", this.ClientId)
                .AddQueryParameter("client_secret", this.ClientSecret)
                .AddQueryParameter("grant_type", options.OverrideGrantType ?? "authorization_code")
                .AddQueryParameter("code", code)
                .AddQueryParameter("redirect_uri", (options.OverrideRedirectUri ?? this.RedirectUri).AbsoluteUri)
                .AddQueryParameters(options.CustomParameters);

            return request;
        }

        public Task<TToken> RefreshTokenAsync(string refreshToken)
        {
            return this.RefreshTokenAsync(refreshToken, null);
        }

        public async Task<TToken> RefreshTokenAsync(string refreshToken, OAuth2RefreshAccessTokenRequestOptions options)
        {
            options = options ?? new OAuth2RefreshAccessTokenRequestOptions();

            var request = this.CreateRefreshTokenRequest(refreshToken, options);
            return await this.ExecuteRequestAsync<TToken>(request);
        }

        protected virtual RestRequest CreateRefreshTokenRequest(string refreshToken, OAuth2RefreshAccessTokenRequestOptions options)
        {
            var request = new RestRequest(this.TokenEndpoint.AbsoluteUri, Method.POST);

            request
                .AddQueryParameter("client_id", this.ClientId)
                .AddQueryParameter("client_secret", this.ClientSecret)
                .AddQueryParameter("refresh_token", refreshToken)
                .AddQueryParameter("grant_type", options.OverrideGrantType ?? "refresh_token");

            return request;
        }

        public async Task RevokeTokenAsync(string token)
        {
            var request = this.CreateRevokeTokenRequest(token);
            await this.ExecuteRequestAsync(request);
        }

        protected virtual RestRequest CreateRevokeTokenRequest(string token)
        {
            var request = new RestRequest(this.RevocationEndpoint.AbsoluteUri, Method.POST);

            request.AddQueryParameter("token", token);

            return request;
        }

        protected async Task ExecuteRequestAsync(IRestRequest request)
        {
            var response = await this.RestClient.ExecuteTaskAsync(request);
            response.EnsureSuccessful();
        }

        protected async Task<T> ExecuteRequestAsync<T>(IRestRequest request)
        {
            var response = await this.RestClient.ExecuteTaskAsync(request);

            response.EnsureSuccessful();

            return JsonConvert.DeserializeObject<T>(response.Content);
        }

    }

    public abstract class BaseOAuth2Client<TToken, TUserInfo> : BaseOAuth2Client<TToken>, IOAuth2Client<TToken, TUserInfo>
        where TToken : OAuth2AccessToken
    {
        public Uri UserInfoEndpoint { get; protected set; }

        public BaseOAuth2Client(OAuth2ClientConfig config, OAuth2ClientBuilder builder) : base(config, builder)
        {
            this.UserInfoEndpoint = config.UserInfoEndpoint;
        }

        public async Task<TUserInfo> GetUserInfoAsync(string accessToken)
        {
            var request = this.CreateUserInfoRequest(accessToken);
            return await this.ExecuteRequestAsync<TUserInfo>(request);
        }

        protected virtual RestRequest CreateUserInfoRequest(string accessToken)
        {
            var request = new RestRequest(this.UserInfoEndpoint.AbsoluteUri);

            request.AddHeader("Authorization", "Bearer " + accessToken);

            return request;
        }

    }

}
