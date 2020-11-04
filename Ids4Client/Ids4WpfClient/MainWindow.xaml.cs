using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

namespace Ids4WpfClient
{
    using IdentityModel;
    using IdentityModel.Client;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }
        private DiscoveryDocumentResponse _disco;
        private TokenResponse _tokenResponse;
        private HttpClient http = new HttpClient();
        private async void RequestToken_Button_Click(object sender, RoutedEventArgs e)
        {
            string username = userNameInputText.Text;
            string password = passwordInputText.Text;

            //发现端点文档
            
            http.DefaultRequestHeaders.Add("grant_type", "password");
            var disco = await http.GetDiscoveryDocumentAsync("https://localhost:5001/");
            _disco = disco;
            if (disco.IsError)
            {
                MessageBox.Show(disco.Error);
                return;
            }
            //请求Token
            var tokenResponse = await http.RequestPasswordTokenAsync (new PasswordTokenRequest
            {
                ClientId = "wpf Client",
                Address = disco.TokenEndpoint,
                ClientSecret = "wpfSecret",
                Scope = "scope1 openid profile",
                UserName = username,
                Password = password 
            }); 
            if (tokenResponse.IsError)
            {

                MessageBox.Show(tokenResponse.Error);
                return;
            }
            _tokenResponse = tokenResponse;
            tokenInputText.Text = tokenResponse.Json.ToString();
            //发送token 并获取Api资源
            
        }
        
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            //发送tokenAsscess 
            // call api 
            var apiClient = new HttpClient();

            apiClient.SetBearerToken(_tokenResponse.AccessToken);

            var response = await apiClient.GetAsync("http://localhost:6000/IdentityApi");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                apiInputText.Text = content;
            }
        }

        private async void Button_Click_openId(object sender, RoutedEventArgs e)
        {
            
            //发送tokenAsscess 
            // call api  
            http.SetBearerToken(_tokenResponse.AccessToken);

            var response = await http.GetAsync(_disco.UserInfoEndpoint);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                apiInputText_Copy.Text = content;
            }
        }
    }
}
