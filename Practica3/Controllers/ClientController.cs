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

        public ClientController(InternalClientManager internalClientMAnager,ClientGenerator clientGenerator)
        {
            _internalClientManager = internalClientMAnager;
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
            System.IO.File.WriteAllText(@"..\Practica3\JSONDB\clients.txt", json);
            return Ok(created);
        }
        [HttpPut]
        [Route("/internal-clients")]
        public IActionResult Put([FromHeader]string Codigo,[FromHeader]string Direccion, [FromHeader]string Telefono)
        {
            return Ok(_internalClientManager.updateClients(Direccion,Telefono,Codigo));
        }
        [HttpDelete]
        [Route("/internal-clients")]
        public IActionResult Delete([FromHeader]string Codigo)
        {
            return Ok(_internalClientManager.removeClients(Codigo));
        }

        [HttpGet]
        [Route("/external-clients")]
        public IActionResult GetExternals([FromHeader]int clients)  //clients es el numero de clientes externos que se quiere pedir 
        {
            return Ok(_internalClientManager.GetExternalStudents(clients));
        }
    }
}
