using Newtonsoft.Json;
using OAuth2Sharp.Samples.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OAuth2Sharp.Samples
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public static readonly string[] Services = {
            "Facebook",
        };

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.LoadServicesList();

            // Load the settings, convenient when testing with same values
            this.LoadSettings();
        }

        private void LoadServicesList()
        {
            this.lstServices.Items.Clear();

            foreach (var item in Services)
            {
                this.lstServices.Items.Add(item);
            }
        }

        private void LoadSettings()
        {
            var settingsRaw = Settings.Default.SampleSettings;

            if (string.IsNullOrEmpty(settingsRaw))
            {
                settingsRaw = "{}";
            }

            var settings = JsonConvert.DeserializeObject<SampleSettings>(settingsRaw);
            this.txtClientId.Text = settings.ClientId;
            this.txtClientSecret.Text = settings.ClientSecret;
            this.txtRedirectUrl.Text = settings.RedirectUri;

            if (settings.Service != null)
            {
                this.lstServices.SelectedItem = settings.Service;
            }
        }

        private void SaveSettings(SampleSettings settings)
        {
            Settings.Default.SampleSettings = JsonConvert.SerializeObject(settings);
            Settings.Default.Save();
        }

        private async void btnStartFlow_Click(object sender, RoutedEventArgs e)
        {
            var settings = this.GetSettingsFromUi();
            SaveSettings(settings);

            this.txtResult.Text = "";

            switch (settings.Service)
            {
                case "Facebook":
                    await this.StartFacebookFlow(settings);
                    break;
                default:
                    MessageBox.Show("Unknown service.", this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
            }
        }

        private async Task StartFacebookFlow(SampleSettings settings)
        {
            var client = new FacebookOAuth2Client(settings.ToOAuth2ClientBuilder());

            // Get OAuth2 request URI
            var options = new FacebookOAuth2ClientCreateAuthorizationUriOption();
            options.Scope.Add("email");
            var requestUri = await client.CreateAuthorizationUriAsync(options);

            var f = new OAuth2Window(requestUri.AbsoluteUri, settings.RedirectUri);
            if (f.ShowDialog() != true)
            {
                return;
            }

            var returnedUri = f.ReturnedUri;
            this.PrintOutput("Redirect Uri: " + returnedUri.AbsoluteUri);

            var parameters = returnedUri.ParseQueryString();

            var state = parameters["state"];
            if (state != options.State)
            {
                this.PrintOutput("WARNING: State is not returned or does not match");
            }

            var error = parameters["error"];
            if (!string.IsNullOrEmpty(error))
            {
                this.PrintOutput("Error returned: " + error);
                return;
            }

            var code = parameters["code"];
            if (string.IsNullOrEmpty(code))
            {
                this.PrintOutput("No code in the returned uri.");
                return;
            }

            var accessToken = await client.RequestAccessTokenAsync(code);
            this.PrintOutput("Access Token: " + Environment.NewLine + JsonConvert.SerializeObject(accessToken));

            await client.RequestAppAccessTokenAsync();
            this.PrintOutput("App Access Token: " + client.AppAccessToken.AccessToken);

            var userInfo = await client.GetUserInfoAsync(accessToken.AccessToken);
            this.PrintOutput("User Info: " + Environment.NewLine + JsonConvert.SerializeObject(userInfo));

            this.PrintOutput("Done!");
        }

        private void PrintOutput(string message)
        {
            this.Dispatcher.Invoke(() =>
            {
                this.txtResult.AppendText(message + Environment.NewLine);
            });
        }

        private SampleSettings GetSettingsFromUi()
        {
            return new SampleSettings()
            {
                ClientId = this.txtClientId.Text,
                ClientSecret = this.txtClientSecret.Text,
                RedirectUri = this.txtRedirectUrl.Text,
                Service = this.lstServices.SelectedItem as string,
            };
        }

    }
}
