using ClientLogic.Manager;
using ClientLogic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        }

        [HttpGet]
        [Route("/internal-clients")]
        public IActionResult Get()
        {
            return Ok(_internalClientManager.getClients());
        }
        [HttpPost]
        [Route("/internal-clients")]
        public IActionResult Post([FromBody]InternalClient client)
        {
            InternalClient created = _internalClientManager.createClients(client.Nombre, client.ApellidoPaterno, client.ApellidoMaterno, client.CI, client.Direccion, client.Telefono, client.Ranking);
            return Ok(created);
        }
        [HttpPut]
        [Route("/internal-clients")]
        public IActionResult Put([FromBody] InternalClient client)
        {
            return Ok(_internalClientManager.updateClients(client.Direccion, client.Telefono, client.CodigoCliente));
        }
        [HttpDelete]
        [Route("/internal-clients")]
        public IActionResult Delete([FromHeader]string Codigo)
        {   
            return Ok(_internalClientManager.removeClients(Codigo));
        }
       /* public IActionResult Delete([FromBody] InternalClient client)
        {
            return Ok(_internalClientManager.removeClients(client.CodigoCliente));
        }*/

        [HttpGet]
        [Route("/external-clients")]
        public IActionResult GetExternals([FromHeader]int clients) 
        {
            return Ok(_internalClientManager.GetExternalClients(clients));
        }
    }
}
