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
using System.Windows.Shapes;

namespace OAuth2Sharp.Samples
{
    /// <summary>
    /// Interaction logic for OAuth2Window.xaml
    /// </summary>
    public partial class OAuth2Window : Window
    {

        private string StartingUri { get; set; }
        private Uri ExpectingUri { get; set; }

        public Uri ReturnedUri { get; set; }

        private OAuth2Window()
        {
            InitializeComponent();
        }

        public OAuth2Window(string startingUri, string expectingRedirectUri) : this()
        {
            this.StartingUri = startingUri;
            this.ExpectingUri = new Uri(expectingRedirectUri);
        }

        private void webBrowser_Navigating(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            if (Uri.Compare(
                this.ExpectingUri ,e.Uri, 
                UriComponents.SchemeAndServer | UriComponents.Path,
                UriFormat.SafeUnescaped, StringComparison.OrdinalIgnoreCase) == 0)
            {
                this.ReturnedUri = e.Uri;
                e.Cancel = true;
                this.DialogResult = true;
                this.Close();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.webBrowser.Navigate(this.StartingUri);
        }
    }
}
