using ClientLogic.Manager;
using ClientLogic;
using ExternalCServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace Practica3.Controllers
{
    [ApiController]
    [Route("/client-controller")]
    public class ClientController : ControllerBase
    {
        
        private InternalClientManager _internalClientManager;

        public ClientController(InternalClientManager internalClientMAnager)
        {
            _internalClientManager = internalClientMAnager;
            try
            {
                _internalClientManager.SetClientsFromDB(JsonConvert.DeserializeObject<List<InternalClient>>(System.IO.File.ReadAllText(@"..\Practica3\clients.txt")));
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No DB created yet");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        [HttpGet]
        [Route("/internal-clients")]
        public IActionResult Get()
        {
            return Ok(_internalClientManager.getClients());
        }
        [HttpPost]
        [Route("/internal-clients")]
        public IActionResult Post([FromHeader]string Nombre,[FromHeader]string PrimerApellido, [FromHeader]string SegundoApellido,[FromHeader] int CI, [FromHeader]string Direccion,[FromHeader]string Telefono, [FromHeader]int Ranking )
        {
            InternalClient created = _internalClientManager.createClients(Nombre, PrimerApellido, SegundoApellido, CI, Direccion, Telefono, Ranking);
            string json = JsonConvert.SerializeObject(_internalClientManager.getClients());
            System.IO.File.WriteAllText(@"..\Practica3\clients.txt", json);
            return Ok(created);
        }
        [HttpPut]
        [Route("/internal-clients")]
        public IActionResult Put([FromHeader]string Codigo,[FromHeader]string Direccion, [FromHeader]string Telefono)
        {
            InternalClient edited = _internalClientManager.updateClients(Direccion, Telefono, Codigo);
            string json = JsonConvert.SerializeObject(_internalClientManager.getClients());
            System.IO.File.WriteAllText(@"..\Practica3\clients.txt", json);
            return Ok(edited);
        }
        [HttpDelete]
        [Route("/internal-clients")]
        public IActionResult Delete([FromHeader]string Codigo)
        {
            InternalClient deleted = _internalClientManager.removeClients(Codigo);
            string json = JsonConvert.SerializeObject(_internalClientManager.getClients());
            System.IO.File.WriteAllText(@"..\Practica3\clients.txt", json);
            return Ok(deleted);
        }

        [HttpGet]
        [Route("/external-clients")]
        public IActionResult GetExternals([FromHeader]int clients)  //clients es el numero de clientes externos que se quiere pedir 
        {
            return Ok(_internalClientManager.GetExternalClients(clients));
        }
    }
}
