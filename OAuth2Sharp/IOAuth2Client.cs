using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OAuth2Sharp
{

    public interface IOAuth2Client<TToken>
        where TToken : OAuth2AccessToken
    {
        string ClientId { get; }
        string ClientSecret { get; }
        Uri RedirectUri { get; }

        Uri DiscoveryDocument { get; }
        Uri AuthorizationEndpoint { get; }
        Uri TokenEndpoint { get; }
        Uri RevocationEndpoint { get; }

        Task<Uri> CreateRequestUriAsync();
        Task<Uri> CreateRequestUriAsync(OAuth2UriRequestOptions options);

        Task<TToken> RequestAccessTokenAsync(string code);
        Task<TToken> RequestAccessTokenAsync(string code, OAuth2AccessTokenRequestOptions options);

        Task<TToken> RefreshTokenAsync(string refreshToken);
        Task<TToken> RefreshTokenAsync(string refreshToken, OAuth2RefreshAccessTokenRequestOptions options);

        Task RevokeTokenAsync(string token);
    }

    public interface IOAuth2Client<TToken, TUserInfo> : IOAuth2Client<TToken>
        where TToken : OAuth2AccessToken
    {

        Uri UserInfoEndpoint { get; }

        Task<TUserInfo> GetUserInfoAsync(string accessToken);
        
    }

}
