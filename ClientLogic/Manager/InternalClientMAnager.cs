using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using ExternalCServices;
using Newtonsoft.Json;
using ClientLogic.Exceptions;
using ClientLogic.Models;
using Microsoft.AspNetCore.Http;
using Serilog;
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
            config = configuration;
            try
            {
                if (clients == null)
                {
                    Log.Information("Clients deserialized Succesfully");
                    clients = JsonConvert.DeserializeObject<List<InternalClient>>(System.IO.File.ReadAllText(config.GetSection("client_path").Value));
                }
            }
            catch(Exception )
            {
                throw new ClientDatabaseException("Couldn't read and deserialize json file");
            }
        }

        public List<InternalClient> getClients()
        {
            try
            {
                Log.Information("Clients Retrieved Succesfully");
                return JsonConvert.DeserializeObject<List<InternalClient>>(System.IO.File.ReadAllText(config.GetSection("client_path").Value));

            }
            catch
            {            
                throw new ClientDatabaseException("Couldn't read json file");
            }
        }
        public InternalClient createClients(string name, string lastname,string seclastname, int CI, string address, string phone, int ranking)
        {
            InternalClient client;
            string codigocliente;
            validatingParameters(name, lastname, seclastname, CI, address, phone, ranking);
            if (String.IsNullOrEmpty(seclastname))
            {
                codigocliente = name[0].ToString().ToUpper() + lastname[0].ToString().ToUpper() + "_-" + CI.ToString();
                seclastname = "No Specified";
            }
            else
            {
                codigocliente = name[0].ToString().ToUpper() + lastname[0].ToString().ToUpper() + seclastname[0].ToString().ToUpper() + "-" + CI.ToString();
            }
            if (clients.Exists(c => c.CodigoCliente == codigocliente))
                throw new ClientInvalidInputException("This client already exists");

            client = new InternalClient() { Nombre = name, ApellidoPaterno = lastname, ApellidoMaterno = seclastname, CI = CI, Direccion = address, Telefono = phone, Ranking = ranking, CodigoCliente = codigocliente };
            
            clients.Add(client);
            WriteJson(clients);
            Log.Information("Client created and added to List succesfully");
            return client;

        }
        public InternalClient updateClients(string name, string lastName1, string lastName2, int ci, string address, string phone, int ranking, string codigo)
        {
            InternalClient client= clients.Find(c => c.CodigoCliente == codigo);
            if(client != null)
            {
                validatingParameters(name, lastName1, lastName2, ci, address, phone, ranking);
                client.Nombre = name;
                client.ApellidoPaterno = lastName1;
                client.ApellidoMaterno = lastName2;
                client.CI = ci;
                client.Direccion = address;
                client.Telefono = phone;
                client.Ranking = ranking;
                if (String.IsNullOrEmpty(lastName2))
                {
                    client.CodigoCliente = name[0].ToString().ToUpper() + lastName1[0].ToString().ToUpper() + "_-" + ci.ToString();
                    lastName2 = "No Specified";
                }
                else
                {
                    client.CodigoCliente = name[0].ToString().ToUpper() + lastName1[0].ToString().ToUpper() + lastName2[0].ToString().ToUpper() + "-" + ci.ToString();
                }
                WriteJson(clients);
                Log.Information("Client edited succesfully");
            }
            else
            {
                throw new ClientNotFoundException("Client does not exist");
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
                Log.Information("Client deleted succesfully");    
            }
            else
            {
                throw new ClientNotFoundException("Client does not exist");
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
                Log.Information("Retrieved External Clients Successfully");
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
                Log.Information("Updated DBjson");
            }
            catch (Exception ex)
            {
                Log.Information(ex.Message);
                throw new ClientDatabaseException("Couldn't write jsonfile");
                
            }
        }
        private void validatingParameters(string name, string lastname, string seclastname, int CI, string address, string phone, int ranking)
        {
            if (String.IsNullOrEmpty(name))
                throw new ClientInvalidInputException("Name cannot be empty");
            if (String.IsNullOrEmpty(lastname))
                throw new ClientInvalidInputException("Last name cannot be empty");
            if (String.IsNullOrEmpty(address))
                throw new ClientInvalidInputException("Address cannot be empty");
            if (String.IsNullOrEmpty(phone))
                throw new ClientInvalidInputException("Phone number cannot be empty");
            if (CI.Equals(0) || CI.Equals(null))
                throw new ClientInvalidInputException("CI cannot be empty");
            if (ranking < 1 || ranking > 5 )
                throw new ClientInvalidInputException("Incorrect ranking. Ranking should be between 1 and 5 ");

        }
    }

}
