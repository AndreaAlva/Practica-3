using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using ExternalCServices;
using Newtonsoft.Json;

namespace ClientLogic.Manager
{
	public class InternalClientManager
	{
        private List<InternalClient> clients;
        private ClientGenerator _service;
        private IConfiguration config;

        public InternalClientManager(ClientGenerator service,IConfiguration configuration)
        {
            _service = service;
            clients = new List<InternalClient>();
            config = configuration;
            try
            {
                System.IO.File.ReadAllText(config.GetSection("client_path").Value);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No DB created yet" + e.Message);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        public List<InternalClient> getClients()
        {
            return JsonConvert.DeserializeObject<List<InternalClient>>(System.IO.File.ReadAllText(config.GetSection("client_path").Value));
        }
        public InternalClient createClients(string name, string lastname,string seclastname, int CI, string address, string phone, int ranking)
        {
            InternalClient client;
            string codigocliente;
            if (seclastname == null)
            {
                codigocliente = name[0].ToString().ToUpper() + lastname[0].ToString().ToUpper() + "_-" + CI.ToString();
                seclastname = "No Specified";  
            }
            else
            {
                codigocliente = name[0].ToString().ToUpper() + lastname[0].ToString().ToUpper() + seclastname[0].ToString().ToUpper() + "-" + CI.ToString();
            }
            client = new InternalClient() { Nombre = name, ApellidoPaterno = lastname, ApellidoMaterno = seclastname, CI = CI, Direccion = address, Telefono = phone, Ranking = ranking, CodigoCliente = codigocliente };
            clients.Add(client);

            string json = JsonConvert.SerializeObject(clients);
            System.IO.File.WriteAllText(config.GetSection("client_path").Value, json);
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
            string json = JsonConvert.SerializeObject(clients);
            System.IO.File.WriteAllText(config.GetSection("client_path").Value, json);
            return client;
        }
        public InternalClient removeClients(string codigo)
        {
            InternalClient client = clients.Find(p => p.CodigoCliente == codigo);
            clients.Remove(client);
            string json = JsonConvert.SerializeObject(clients);
            System.IO.File.WriteAllText(config.GetSection("client_path").Value, json);
            return client;
        }

        public List<InternalClient> GetExternalClients(int clientes)
        {
            List<InternalClient> externalClients = new List<InternalClient>();
            InternalClient client;
            for (int i = 0; i < clientes; i++)
            {
                var externalClient = _service.GetClient();
                //map External To Internal
                string codigocliente = externalClient.Result.First_name[0].ToString().ToUpper() + externalClient.Result.Last_name[0].ToString().ToUpper() + "_-" + externalClient.Result.Id.ToString();
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
    }

}
