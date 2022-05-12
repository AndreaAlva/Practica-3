using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using ExternalCServices;
using Newtonsoft.Json;
using ClientLogic.Exceptions;
using Microsoft.AspNetCore.Http;

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
        }

        public List<InternalClient> getClients()
        {
            try
            {
                return JsonConvert.DeserializeObject<List<InternalClient>>(System.IO.File.ReadAllText(config.GetSection("client_path").Value));
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new ClientDatabaseException("Couldn't read json file");
            }
           
        }
        public InternalClient createClients(string name, string lastname,string seclastname, int CI, string address, string phone, int ranking)
        {
            InternalClient client;
            string codigocliente;
            if (ranking < 1 || ranking > 5)
                throw new ClientInvalidInputException("Ranking out of range. Ranking should be between 1 and 5 ");

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
            WriteJson(clients);
            return client;

        }
        public InternalClient updateClients(string address, string phone, string codigo)
        {
            InternalClient client= clients.Find(c => c.CodigoCliente == codigo);
            if(client != null)
            {
                client.Direccion = address;
                client.Telefono = phone;
                WriteJson(clients);
            }
            else
            {
                throw new ClientNotFoundException("Client does not exists");
            }
            return client;
        }
        public InternalClient removeClients(string codigo)
        {
            InternalClient client = clients.Find(p => p.CodigoCliente == codigo);
            if (client != null)
            {
                clients.Remove(client);
                WriteJson(clients);
            }
            else
            {
                throw new ClientNotFoundException("Client does not exists");
               
            }
            return client;
        }

        public List<InternalClient> GetExternalClients(int clientes)
        {
            List<InternalClient> externalClients = new List<InternalClient>();
            InternalClient client;
            for (int i = 0; i < clientes; i++)
            {
                var externalClient = _service.GetClient();
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

        private void WriteJson(List<InternalClient> clients)
        {
            try
            {
                string json = JsonConvert.SerializeObject(clients);
                System.IO.File.WriteAllText(config.GetSection("client_path").Value, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new ClientDatabaseException("Couldn't write jsonfile");
            }
        }
    }

}
