using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OAuth2Sharp.Test
{
    public class TestSettings
    {
        private const string ConfigEnvVarName = "OAuth2ClientTest";

        public static readonly TestSettings Instance = new TestSettings();

        private TestSettings()
        {
            var filePath = Environment.GetEnvironmentVariable(ConfigEnvVarName);
            var fileContent = File.ReadAllText(filePath);

            JsonConvert.PopulateObject(fileContent, this);
        }

        public OAuthConfigs OAuth { get; set; }

        public class OAuthConfigs
        {

            public OAuthClientConfig Google { get; set; }
            public OAuthClientConfig Facebook { get; set; }
            public OAuthClientConfig Microsoft { get; set; }
            public OAuthClientConfig Twitter { get; set; }

            public class OAuthClientConfig
            {
                public string ClientId { get; set; }
                public string ClientSecret { get; set; }
                public string RedirectUri { get; set; }

                public OAuth2ClientBuilder ToBuilder()
                {
                    return new OAuth2ClientBuilder()
                    {
                        ClientId = this.ClientId,
                        ClientSecret = this.ClientSecret,
                        RedirectUri = new Uri(this.RedirectUri),
                    };
                }

            }
        }
    }

}
