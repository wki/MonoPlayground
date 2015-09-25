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
        private ActorSystem actorSystem;

        public ActorController(ActorSystem actorSystem)
        {
            this.actorSystem = actorSystem;
        }

        [Route("ask")]
        [HttpGet]
        public async Task<string> Ask()
        {
            var askable = actorSystem.ActorSelection("/user/Askable");
            return await askable.Ask<string>(new AskAQuestion("huhu?"));
        }

        [Route("dummy")]
        [HttpGet]
        public string Dummy()
        {
            return "nothing to see here";
        }
    }
}
