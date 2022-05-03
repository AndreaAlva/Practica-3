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
            return clients;
        }
    }
}

