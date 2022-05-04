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
            return Ok();
        }
        [HttpPost]
        [Route("/internal-clients")]
        public IActionResult Post()
        {
            return Ok();
        }
        [HttpPut]
        [Route("/internal-clients")]
        public IActionResult Put()
        {
            return Ok();
        }
        [HttpDelete]
        [Route("/internal-clients")]
        public IActionResult Delete()
        {
            return Ok();
        }

        [HttpGet]
        [Route("/external-clients")]
        public IActionResult GetExternals([FromHeader]int clients)
        {
            return Ok();
        }
    }
}
