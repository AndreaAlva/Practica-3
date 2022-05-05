using ClientLogic.Manager;
using ExternalCServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Practica3.Controllers
{
    [ApiController]
    [Route("/client-controller")]
    public class ClientController : ControllerBase
    {
        private ExternalClientManager _externalClientManager;
        private InternalClientManager _internalClientManager;

        public ClientController(ExternalClientManager externalClientManager, InternalClientManager internalClientMAnager,ClientGenerator clientGenerator)
        {
            _externalClientManager = externalClientManager;
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
        public IActionResult Post([FromHeader]string Nombre,[FromHeader]string PrimerApellido, [FromHeader]string SegundoApellido,[FromHeader] int CI, [FromHeader]string Direccion,[FromHeader]int Telefono, [FromHeader]int Ramking )
        {
            return Ok(_internalClientManager.createClients(Nombre,PrimerApellido,SegundoApellido,CI,Direccion,Telefono,Ramking));
        }
        [HttpPut]
        [Route("/internal-clients")]
        public IActionResult Put([FromHeader]string Codigo,[FromHeader]string Direccion, [FromHeader]int Telefono)
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
            return Ok(_externalClientManager.GetStudents(clients));
        }
    }
}
