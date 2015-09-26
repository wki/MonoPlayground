using Akka.Actor;
using System;
using System.Web.Http;
// using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApiAkkaDemo
{
    [RoutePrefix("api/actor")]
    public class ActorController : ApiController
    {
        private IAskableService askableService;

        public ActorController(IAskableService askableService)
        {
            this.askableService = askableService;
        }

        [Route("ask")]
        [HttpGet]
        public Task<string> Ask()
        {
            return askableService.AskAsync("huhu");
        }

        [Route("sync")]
        [HttpGet]
        public string Sync()
        {
            return askableService.Ask("hihi");
        }

        [Route("dummy")]
        [HttpGet]
        public string Dummy()
        {
            return "nothing to see here";
        }
    }
}
