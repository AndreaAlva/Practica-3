using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using ExternalCServices;
namespace ClientLogic.Manager
{
	public class ExternalClientManager
	{
		
        private List<ExternalClient> clients;
        private IConfiguration _configuration;
        private ClientGenerator _service;
        public ExternalClientManager(IConfiguration configuration, ClientGenerator service)
        {
            _configuration = configuration;
            _service = service;
            clients = new List<ExternalClient>();
        }
        public List<ExternalClient> GetStudents(int clientes)
        {
            ExternalClient client;
            for(int i =0; i<clientes; i++)
            {
                var externalClient = _service.GetClient();
                client = new ExternalClient()
                {
                    First_name = externalClient.Result.First_name,
                    Last_name = externalClient.Result.Last_name,
                    Id = externalClient.Result.Id,
                    Address = new Address() { City= externalClient.Result.Address.City ,
                                              Street_name = externalClient.Result.Address.Street_name },
                    Phone_number = externalClient.Result.Phone_number
                };
                clients.Add(client);
            }
            return clients;
        }
    }
}

