using ExternalCServices.Exceptions;
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
            try
            {
                string clientsURL = configuration.GetSection("clientsURL").Value;
                ExternalClient clients;
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage reponse = await client.GetAsync(clientsURL);
                    
                    if (reponse.IsSuccessStatusCode)
                    {
                        string responseData = await reponse.Content.ReadAsStringAsync();
                        clients = JsonConvert.DeserializeObject<ExternalClient>(responseData);
                    }
                    else
                    {
                        throw new ExternalClientServiceNotFoundException("Service not found");
                    }
                }
                return clients;
            }
            catch(Exception)
            {
                throw new ExternalClientServiceException("Backing service external error");
            }
           
        }
    }
}
