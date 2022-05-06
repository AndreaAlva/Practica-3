using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExternalCServices
{
    public class ClientGenerator
    {
        private IConfiguration configuration;
        public ClientGenerator(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task<ExternalClient> GetClient()
        {
            string clientsURL = configuration.GetSection("clientsURL").Value;
            HttpClient client = new HttpClient();
            HttpResponseMessage reponse = await client.GetAsync(clientsURL);

            ExternalClient clients;
            if (reponse.IsSuccessStatusCode)
            {
                string responseData = await reponse.Content.ReadAsStringAsync();
                clients = JsonConvert.DeserializeObject<ExternalClient>(responseData);
            }
            else
            {
                clients = new ExternalClient();
            }

            return clients;
        }
    }
}
