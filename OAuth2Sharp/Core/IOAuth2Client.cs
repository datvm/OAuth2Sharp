using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OAuth2Sharp.Core
{

    public interface IOAuth2Client
    {

        string ClientId { get; }
        string ClientSecret { get; }
        string RedirectUri { get; }

        string AuthorizationEndpoint { get; }
        
        Task<Uri> CreateAuthorizationUriAsync();
        Task<Uri> CreateAuthorizationUriAsync<TOption>(TOption options)
            where TOption : DefaultOAuth2ClientCreateAuthorizationUriOption, new();
        Task<Uri> CreateAuthorizationUriAsync<TOption>(Action<TOption> options)
            where TOption : DefaultOAuth2ClientCreateAuthorizationUriOption, new();

    }

    public interface IOAuth2Client<TToken> : IOAuth2Client
    {
        string TokenEndpoint { get; }

        Task<TToken> RequestAccessTokenAsync(string code);
        Task<TToken> RequestAccessTokenAsync<TOption>(string code, TOption options)
            where TOption : DefaultOAuth2ClientOperationOptions, new();
        Task<TToken> RequestAccessTokenAsync<TOption>(string code, Action<TOption> options)
            where TOption : DefaultOAuth2ClientOperationOptions, new();
    }

    public interface IOAuth2RefreshableTokenClient<TToken, TRefreshToken> : IOAuth2Client<TToken>
    {

        Task<TRefreshToken> RefreshAccessTokenAsync(string refreshToken);
        Task<TRefreshToken> RefreshAccessTokenAsync<TOption>(string refreshToken, TOption options)
            where TOption : DefaultOAuth2ClientOperationOptions, new();
        Task<TRefreshToken> RefreshAccessTokenAsync<TOption>(string refreshToken, Action<TOption> options)
            where TOption : DefaultOAuth2ClientOperationOptions, new();

    }

    public interface IOAuth2RevokableTokenClient<TToken> : IOAuth2Client<TToken>
    {
        string TokenRevocationEndpoint { get; }

        Task<TToken> RevokeAccessTokenAsync(string token);
        Task<TToken> RevokeAccessTokenAsync<TOption>(string token, TOption options)
            where TOption : DefaultOAuth2ClientOperationOptions, new();
        Task<TToken> RevokeAccessTokenAsync<TOption>(string token, Action<TOption> options)
            where TOption : DefaultOAuth2ClientOperationOptions, new();

    }

    public interface IOAuth2UserInfoClient<TToken, TUserInfo> : IOAuth2Client<TToken>
    {

        string UserInfoEndpoint { get; set; }

        Task<TUserInfo> GetUserInfoAsync(string token);
        Task<TUserInfo> GetUserInfoAsync<TOption>(string token, TOption options)
            where TOption : DefaultOAuth2ClientOperationOptions, new();
        Task<TUserInfo> GetUserInfoAsync<TOption>(string token, Action<TOption> options)
            where TOption : DefaultOAuth2ClientOperationOptions, new();

    }

}
