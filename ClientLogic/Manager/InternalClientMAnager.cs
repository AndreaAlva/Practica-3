using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using ExternalCServices;

namespace ClientLogic.Manager
{
	public class InternalClientManager
	{
        private List<InternalClient> clients;
        private IConfiguration _configuration;
        private ClientGenerator _service;

        public InternalClientManager(IConfiguration configuration, ClientGenerator service)
        {
            _configuration = configuration;
            _service = service;
            clients = new List<InternalClient>();
        }

        public List<InternalClient> getClients()
        {
            return clients;
        }
        public InternalClient createClients(string name, string lastname,string seclastname, int CI, string address, string phone, int ranking)
        {
            InternalClient client;
            if (seclastname == null)
            {
                string codigocliente = name[0].ToString().ToUpper() + lastname[0].ToString().ToUpper() + "-" + CI.ToString();
                client = new InternalClient() { Nombre = name, ApellidoPaterno = lastname, ApellidoMaterno = "No Specified", CI = CI, Direccion = address, Telefono = phone, Ranking = ranking, CodigoCliente = codigocliente };
            }
            else
            {
                string codigocliente = name[0].ToString().ToUpper() + lastname[0].ToString().ToUpper() + seclastname[0].ToString().ToUpper() + "-" + CI.ToString();
                client = new InternalClient() { Nombre = name, ApellidoPaterno = lastname, ApellidoMaterno = seclastname, CI = CI, Direccion = address, Telefono = phone, Ranking = ranking, CodigoCliente = codigocliente };
            }

            clients.Add(client);
            return client;
        }
        public InternalClient updateClients(string address, string phone, string codigo)
        {
            InternalClient client = null;
            clients.ForEach(c =>
            {
                if (c.CodigoCliente == codigo)
                    c.Direccion = address; c.Telefono = phone; client =c;
            });
            return client;
        }
        public InternalClient removeClients(string codigo)
        {
            InternalClient client = clients.Find(p => p.CodigoCliente == codigo);
            clients.Remove(client);
            return client;
        }

        public List<InternalClient> GetExternalStudents(int clientes)
        {
            List<InternalClient> externalClients = new List<InternalClient>();
            InternalClient client;
            for (int i = 0; i < clientes; i++)
            {
                var externalClient = _service.GetClient();
                //map External To Internal
                string codigocliente = externalClient.Result.First_name[0].ToString().ToUpper() + externalClient.Result.Last_name[0].ToString().ToUpper() + "-" + externalClient.Result.Id.ToString();
                client = new InternalClient()
                {
                    Nombre = externalClient.Result.First_name,
                    ApellidoPaterno = externalClient.Result.Last_name,
                    ApellidoMaterno = "No Specified",
                    CI = externalClient.Result.Id,
                    Direccion = externalClient.Result.Address.City + ", " + externalClient.Result.Address.Street_name,
                    Telefono = (externalClient.Result.Phone_number),
                    Ranking = -1,
                    CodigoCliente = codigocliente,
                };
                externalClients.Add(client);
            }
            return externalClients;
        }

        public void SetClientsFromDB(List<InternalClient> retrievedClients)
        {
            clients = retrievedClients;
        }

    }

}
