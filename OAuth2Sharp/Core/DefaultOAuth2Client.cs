using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace OAuth2Sharp.Core
{

    public class DefaultOAuth2Client<TToken> : IOAuth2Client<TToken>
    {

        public string TokenEndpoint { get; protected set; }

        public string ClientId { get; protected set; }

        public string ClientSecret { get; protected set; }

        public string RedirectUri { get; protected set; }

        public string AuthorizationEndpoint { get; protected set; }

        protected OAuth2ClientConfig ClientConfig { get; set; }
        protected OAuth2ClientBuilder ClientBuilder { get; set; }

        protected RestClient RestClient { get; set; }

        public DefaultOAuth2Client(Func<OAuth2ClientConfig> clientConfig, Func<OAuth2ClientBuilder> clientBuilder)
        {
            this.ClientConfig = clientConfig();
            this.ClientBuilder = clientBuilder();

            this.ClientConfig.CopyToClient(this);
            this.ClientBuilder.CopyToClient(this);

            this.Initialize();

            this.RestClient = new RestClient(
                new Uri(this.TokenEndpoint).GetLeftPart(UriPartial.Authority));
        }

        protected virtual void Initialize() { }

        public DefaultOAuth2Client(OAuth2ClientConfig clientConfig, OAuth2ClientBuilder clientBuilder)
            : this(() => clientConfig, () => clientBuilder) { }

        public virtual Task<Uri> CreateAuthorizationUriAsync<TOption>(TOption options) where TOption : DefaultOAuth2ClientCreateAuthorizationUriOption, new()
        {
            options = options ?? new TOption();
            options.State = options.State ?? Guid.NewGuid().ToString();

            return Task.Run(() =>
            {
                var request = this.CreateAuthorizationUriRequest(options);
                return this.RestClient.BuildUri(request);
            });
        }

        public virtual async Task<TToken> RequestAccessTokenAsync<TOption>(string code, TOption options) where TOption : DefaultOAuth2ClientOperationOptions, new()
        {
            options = options ?? new TOption();

            var request = this.CreateRequestAccessTokenRequest(code, options);
            return await this.ExecuteRequestAsync<TToken>(request);
        }

        #region Helper Methods

        protected async Task ExecuteRequestAsync(IRestRequest request)
        {
            var response = await this.RestClient.ExecuteTaskAsync(request);
            response.EnsureSuccessful();
        }

        protected async Task<T> ExecuteRequestAsync<T>(IRestRequest request)
        {
            var response = await this.RestClient.ExecuteTaskAsync(request);

            Trace.WriteLine(response.Content);
            response.EnsureSuccessful();

            return JsonConvert.DeserializeObject<T>(response.Content);
        }

        #endregion

        #region Request Generation Methods

        protected virtual RestRequest CreateAuthorizationUriRequest<TOption>(TOption options) where TOption : DefaultOAuth2ClientCreateAuthorizationUriOption, new()
        {
            var request = new RestRequest(this.AuthorizationEndpoint);

            request
                .AddQueryParameterWithOverride(options.CustomValues)
                .AddQueryParameter("client_id", this.ClientId)
                .AddQueryParameter("redirect_uri", this.RedirectUri)
                .AddQueryParameter("state", options.State)
                .AddQueryParameter("response_type", "code")
                .AddQueryParameter("scope", string.Join(" ", options.Scope))
                .AddRemainingParameters();

            return request;
        }

        protected virtual RestRequest CreateRequestAccessTokenRequest<TOption>(string code, TOption options) where TOption : DefaultOAuth2ClientOperationOptions, new()
        {
            var request = new RestRequest(this.TokenEndpoint, Method.POST);

            request
                .AddQueryParameterWithOverride(options.CustomValues)
                .AddBodyFormUrlEncoded("client_id", this.ClientId)
                .AddBodyFormUrlEncoded("client_secret", this.ClientSecret)
                .AddBodyFormUrlEncoded("grant_type", "authorization_code")
                .AddBodyFormUrlEncoded("code", code)
                .AddBodyFormUrlEncoded("redirect_uri", this.RedirectUri)
                .AddPostFormUrlEncodedHeader()
                .AddRemainingParameters();

            return request;
        }

        #endregion

        #region Optional Overloads

        public virtual Task<Uri> CreateAuthorizationUriAsync()
        {
            return this.CreateAuthorizationUriAsync(new DefaultOAuth2ClientCreateAuthorizationUriOption());
        }

        public virtual Task<Uri> CreateAuthorizationUriAsync<TOption>(Action<TOption> options) where TOption : DefaultOAuth2ClientCreateAuthorizationUriOption, new()
        {
            return this.CreateAuthorizationUriAsync(options.ExecuteOptions());
        }

        public virtual Task<TToken> RequestAccessTokenAsync(string code)
        {
            return this.RequestAccessTokenAsync(code, new DefaultOAuth2ClientOperationOptions());
        }

        public virtual Task<TToken> RequestAccessTokenAsync<TOption>(string code, Action<TOption> options) where TOption : DefaultOAuth2ClientOperationOptions, new()
        {
            return this.RequestAccessTokenAsync(code, options.ExecuteOptions());
        }

        #endregion

    }

}
