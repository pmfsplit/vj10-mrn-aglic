using System.Collections.Generic;
using System.Threading.Tasks;
using Akka.Actor;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using RS_01.Actors;
using RS_01.DtoMappers;
using RS_01.Models;
using Shared;

namespace RS_01.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentsApiController : Controller
    {
        [HttpGet]
        public async Task<ActionResult<JArray>> Get()
        {
            var props = Props.Create(() => new ConnectionActor(AkkaService.CClientSettings));
            var actor = AkkaService.ActorSys.ActorOf(props);
            var result = await actor.Ask<JArray>(new GetAll());
            return result;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> Get(int id)
        {
            var props = Props.Create(() => new ConnectionActor(AkkaService.CClientSettings));
            var actor = AkkaService.ActorSys.ActorOf(props);

            var result = await actor.Ask<JObject>(new Get(id));
            var student = StudentDto.FromJson(result);
            return Ok(student);
        }

        [HttpPost("save")]
        public ActionResult Save([FromBody] JObject json)
        {
            return Ok();
        }
    }
}