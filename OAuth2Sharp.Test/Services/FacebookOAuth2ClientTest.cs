using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using OAuth2Sharp.Core;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OAuth2Sharp.Test.Services
{

    [TestClass]
    public class FacebookOAuth2ClientTest
    {

        private FacebookOAuth2Client client;

        private TestSettings settings;
        private TestSettings.OAuthConfigs.OAuthClientConfig facebookSettings;
        private OAuth2ClientConfig facebookClientConfig;
        private OAuth2ClientBuilder builder;



        [TestInitialize]
        public void Initialize()
        {
            this.settings = TestSettings.Instance;
            this.facebookSettings = settings.OAuth.Facebook;
            this.facebookClientConfig = ServicesClientConfigs.Facebook;

            this.builder = settings.OAuth.Facebook.ToBuilder();
            this.client = new FacebookOAuth2Client(this.builder);
        }

        [TestMethod]
        public async Task CreateRequestUriAsyncTest1()
        {
            var result = await this.client.CreateAuthorizationUriAsync();

            Assert.IsNotNull(result);

            var authUri = new Uri(this.facebookClientConfig.AuthorizationEndpoint);
            Assert.AreEqual(authUri.Host, result.Host);

            var parameters = result.ParseQueryString();
            Assert.AreEqual(this.facebookSettings.ClientId, parameters["client_id"]);
            Assert.AreEqual(this.facebookSettings.RedirectUri, parameters["redirect_uri"]);
            Assert.IsNotNull(parameters["state"]);
        }

        [TestMethod]
        public async Task CreateRequestUriAsyncTest2()
        {
            const string overrideRedirectUri = "https://www.example.com/";
            const string state = "123";

            var result = await this.client.CreateAuthorizationUriAsync(config =>
            {
                config.CustomValues.Add("testParam", "123");
                config.CustomValues.Add("redirect_uri", overrideRedirectUri);
                config.State = state;
            });

            Assert.IsNotNull(result);

            var authUri = new Uri(this.facebookClientConfig.AuthorizationEndpoint);
            Assert.AreEqual(authUri.Host, result.Host);

            var parameters = result.ParseQueryString();
            Assert.AreEqual(this.facebookSettings.ClientId, parameters["client_id"]);
            Assert.AreEqual(overrideRedirectUri, parameters["redirect_uri"]);
            Assert.AreEqual(state, parameters["state"]);
            Assert.AreEqual("123", parameters["testParam"]);
        }

        [TestMethod]
        public async Task RequestAccessTokenAsyncTest()
        {
            using (ShimsContext.Create())
            {
                RestSharp.Fakes.ShimRestClient.AllInstances.ExecuteTaskAsyncIRestRequest = 
                    Utils.FakeAccessTokenRequest;

                var result = await this.client.RequestAccessTokenAsync(Utils.MockOAuthAccessCode);

                Assert.IsNotNull(result);
                Assert.AreEqual(Utils.MockReturnedAccessToken, result.AccessToken);
            }
        }

    }

}
