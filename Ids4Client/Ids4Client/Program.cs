using System;
using System.Net.Http;
using System.Threading.Tasks;

using IdentityModel;
using IdentityModel.Client;

using Newtonsoft.Json.Linq;

namespace Ids4Client
{
    class Program
    {
        async static Task Main(string[] args)
        {


            HttpClient client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5001/");
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }

            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                ClientId = "credentialsClient",
                Address = disco.TokenEndpoint,
                ClientSecret = "511536EF-F270-4058-80CA-1C89C192F69A",
                Scope = "scope1"
            }); ;
            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }
            Console.WriteLine(tokenResponse.Json);

            //发送tokenAsscess 
            // call api 
            var apiClient = new HttpClient();
            
            apiClient.SetBearerToken(tokenResponse.AccessToken);

            var response = await apiClient.GetAsync("http://localhost:6000/IdentityApi");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(JArray.Parse(content));
            }
        } 
    }
}
