using Newtonsoft.Json;
using OAuth2Sharp.Core;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;

namespace OAuth2Sharp
{
    public class MicrosoftOAuth2Client : DefaultOAuth2UserInfoOptionClient<
        MicrosoftOAuth2OpenIdToken, MicrosoftOAuth2OpenIdToken, MicrosoftUserInfo,
        MicrosoftOAuth2ClientCreateAuthorizationUriOption, MicrosoftOAuth2ClientOperationOptions>
    {
        public MicrosoftOAuth2Client(Func<OAuth2ClientConfig> clientConfig, Func<OAuth2ClientBuilder> clientBuilder) : base(clientConfig, clientBuilder)
        {
        }

        public MicrosoftOAuth2Client(OAuth2ClientConfig clientConfig, OAuth2ClientBuilder clientBuilder) : base(clientConfig, clientBuilder)
        {
        }

        public MicrosoftOAuth2Client(Func<OAuth2ClientBuilder> clientBuilder) : base(() => ServicesClientConfigs.Microsoft, clientBuilder)
        {
        }

        public MicrosoftOAuth2Client(OAuth2ClientBuilder clientBuilder) : base(ServicesClientConfigs.Microsoft, clientBuilder)
        {
        }

        public override Task<Uri> CreateAuthorizationUriAsync(MicrosoftOAuth2ClientCreateAuthorizationUriOption options)
        {
            options.Nonce = options.Nonce ?? Guid.NewGuid().ToString();
            options.State = options.State ?? Guid.NewGuid().ToString();

            var responseType = "";
            switch (options.ResponseType)
            {
                case MicrosoftOperationResponseType.IdToken:
                    responseType = "id_token";
                    break;
                case MicrosoftOperationResponseType.Code:
                    responseType = "code";
                    break;
                case MicrosoftOperationResponseType.IdTokenAndCode:
                    responseType = "id_token code";
                    break;
                default:
                    break;
            }
            options.CustomValues.AddIfNotExist("response_type", responseType);

            options.CustomValues.AddIfNotExist("nonce", options.Nonce);
            options.CustomValues.AddIfNotExist("state", options.State);

            return base.CreateAuthorizationUriAsync(options);
        }

        protected override RestRequest CreateUserInfoRequest<TOption>(string accessToken, TOption options)
        {
            var request = new RestRequest(this.UserInfoEndpoint, Method.GET);

            request
                .AddQueryParameterWithOverride(options.CustomValues)
                .AddHeader("Authorization", "Bearer " + accessToken)
                .AddRemainingParameters();

            return request;
        }

    }

    // TODO: Fill the user info from this documentation https://developer.microsoft.com/en-us/graph/docs/api-reference/v1.0/resources/user
    /// <summary>
    /// The User Info of a Microsoft Account
    /// </summary>
    public class MicrosoftUserInfo : IOAuth2UserInfo
    {

        public string Name
        {
            get
            {
                return
                    this.PreferredName ??
                    this.DisplayName ??
                    (this.Surname + " " + this.GivenName).Trim();
            }
        }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("aboutMe")]
        public string AboutMe { get; set; }

        [JsonProperty("accountEnabled")]
        public bool AccountEnabled { get; set; }

        [JsonProperty("birthday")]
        public string Birthday { get; set; }

        [JsonProperty("businessPhones")]
        public string[] BusinessPhones { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("companyName")]
        public string CompanyName { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("department")]
        public string Department { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("givenName")]
        public string GivenName { get; set; }

        [JsonProperty("hireDate")]
        public string HireDate { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("interests")]
        public string[] Interests { get; set; }

        [JsonProperty("jobTitle")]
        public string JobTitle { get; set; }

        [JsonProperty("mail")]
        public string Mail { get; set; }

        [JsonProperty("mailNickname")]
        public string MailNickname { get; set; }

        [JsonProperty("mobilePhone")]
        public string MobilePhone { get; set; }

        [JsonProperty("mySite")]
        public string MySite { get; set; }

        [JsonProperty("officeLocation")]
        public string OfficeLocation { get; set; }

        [JsonProperty("onPremisesImmutableId")]
        public string OnPremisesImmutableId { get; set; }

        [JsonProperty("onPremisesLastSyncDateTime")]
        public string OnPremisesLastSyncDateTime { get; set; }

        [JsonProperty("onPremisesSecurityIdentifier")]
        public string OnPremisesSecurityIdentifier { get; set; }

        [JsonProperty("onPremisesSyncEnabled")]
        public bool OnPremisesSyncEnabled { get; set; }

        [JsonProperty("passwordPolicies")]
        public string PasswordPolicies { get; set; }

        [JsonProperty("pastProjects")]
        public string[] PastProjects { get; set; }

        [JsonProperty("postalCode")]
        public string PostalCode { get; set; }

        [JsonProperty("preferredLanguage")]
        public string PreferredLanguage { get; set; }

        [JsonProperty("preferredName")]
        public string PreferredName { get; set; }

        [JsonProperty("proxyAddresses")]
        public string[] ProxyAddresses { get; set; }

        [JsonProperty("responsibilities")]
        public string[] Responsibilities { get; set; }

        [JsonProperty("schools")]
        public string[] Schools { get; set; }

        [JsonProperty("skills")]
        public string[] Skills { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("streetAddress")]
        public string StreetAddress { get; set; }

        [JsonProperty("surname")]
        public string Surname { get; set; }

        [JsonProperty("usageLocation")]
        public string UsageLocation { get; set; }

        [JsonProperty("userPrincipalName")]
        public string UserPrincipalName { get; set; }

        [JsonProperty("userType")]
        public string UserType { get; set; }
    }

    public class MicrosoftOAuth2Exception : OAuth2Exception
    {
        public MicrosoftOAuth2Exception()
        {
        }

        public MicrosoftOAuth2Exception(string message) : base(message)
        {
        }

        public MicrosoftOAuth2Exception(string message, IRestResponse response) : base(message, response)
        {
        }

        public MicrosoftOAuth2Exception(string message, Exception innerException) : base(message, innerException)
        {
        }

    }

    public class MicrosoftOAuth2ClientOperationOptions : DefaultOAuth2ClientOperationOptions
    {

    }

    public class MicrosoftOAuth2ClientCreateAuthorizationUriOption : DefaultOAuth2ClientCreateAuthorizationUriOption
    {
        public MicrosoftOperationResponseType ResponseType { get; set; }
        public string Nonce { get; set; }
    }

    public class MicrosoftOAuth2OpenIdToken : DefaultOAuth2AccessToken
    {

        [JsonProperty("id_token")]
        public string IdToken { get; set; }

        public JwtSecurityToken JwtToken
        {
            get
            {
                if (this.IdToken == null)
                {
                    return null;
                }

                return new JwtSecurityToken(this.IdToken);
            }
        }

    }

    public enum MicrosoftOperationResponseType
    {
        IdToken,
        Code,
        IdTokenAndCode,
    }

}
