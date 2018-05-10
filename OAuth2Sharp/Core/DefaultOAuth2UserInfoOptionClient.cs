using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OAuth2Sharp.Core
{

    public class DefaultOAuth2UserInfoOptionClient<TToken, TRefreshToken, TUserInfo, TCreateAuthorizationUriOptions, TOperationOptions> :
        DefaultOAuth2UserInfoClient<TToken, TRefreshToken, TUserInfo>
        where TCreateAuthorizationUriOptions: DefaultOAuth2ClientCreateAuthorizationUriOption, new()
        where TOperationOptions : DefaultOAuth2ClientOperationOptions, new()
    {
        public DefaultOAuth2UserInfoOptionClient(Func<OAuth2ClientConfig> clientConfig, Func<OAuth2ClientBuilder> clientBuilder) : base(clientConfig, clientBuilder)
        {
        }

        public DefaultOAuth2UserInfoOptionClient(OAuth2ClientConfig clientConfig, OAuth2ClientBuilder clientBuilder) : base(clientConfig, clientBuilder)
        {
        }


        #region CreateAuthorizationUriAsync

        public virtual Task<Uri> CreateAuthorizationUriAsync(TCreateAuthorizationUriOptions options)
        {
            return base.CreateAuthorizationUriAsync(options);
        }

        public override Task<Uri> CreateAuthorizationUriAsync()
        {
            return this.CreateAuthorizationUriAsync(new TCreateAuthorizationUriOptions());
        }

        public Task<Uri> CreateAuthorizationUriAsync(Action<TCreateAuthorizationUriOptions> options)
        {
            return this.CreateAuthorizationUriAsync(options.ExecuteOptions());
        }

        #endregion

        #region RequestAccessTokenAsync

        public virtual Task<TToken> RequestAccessTokenAsync(string code, TOperationOptions options)
        {
            return base.RequestAccessTokenAsync(code, options);
        }

        public override Task<TToken> RequestAccessTokenAsync(string code)
        {
            return this.RequestAccessTokenAsync(code, new TOperationOptions());
        }

        public Task<TToken> RequestAccessTokenAsync(string code, Action<TOperationOptions> options)
        {
            return this.RequestAccessTokenAsync(code, options.ExecuteOptions());
        }

        #endregion

        #region RefreshAccessTokenAsync

        public virtual Task<TRefreshToken> RefreshAccessTokenAsync(string refreshToken, TOperationOptions options)
        {
            return base.RefreshAccessTokenAsync(refreshToken, options);
        }

        public override Task<TRefreshToken> RefreshAccessTokenAsync(string refreshToken)
        {
            return this.RefreshAccessTokenAsync(refreshToken, new TOperationOptions());
        }

        public virtual Task<TRefreshToken> RefreshAccessTokenAsync(string refreshToken, Action<TOperationOptions> options)
        {
            return this.RefreshAccessTokenAsync(refreshToken, options.ExecuteOptions());
        }

        #endregion

        #region GetUserInfoAsync

        public virtual Task<TUserInfo> GetUserInfoAsync(string token, TOperationOptions options)
        {
            return base.GetUserInfoAsync(token, options);
        }

        public override Task<TUserInfo> GetUserInfoAsync(string token)
        {
            return this.GetUserInfoAsync(token, new TOperationOptions());
        }

        public Task<TUserInfo> GetUserInfoAsync(string token, Action<TOperationOptions> options)
        {
            return this.GetUserInfoAsync(token, options.ExecuteOptions());
        }

        #endregion

    }
}
