using System;
using System.Web.Http;
using WebApiAkkaDemo.AppService;
using System.Threading.Tasks;

namespace WebApiAkkaDemo.Controllers
{
    [RoutePrefix("api/measurement")]
    public class MeasurementController : ApiController
    {
        private IMeasurementService measurementService;

        public MeasurementController(IMeasurementService measurementService)
        {
            this.measurementService = measurementService;
        }

        // curl -4 -vXGET http://localhost:9000/api/measurement/13 

        [Route("{id}")]
        [HttpGet]
        public Task<int> Read(int id)
        {
            Console.WriteLine("Read Sensor {0}", id);

            return measurementService.ReadMeasurementAsync(id);
        }

        // curl -4 -vXPOST HContent-Type:application/json -d 42 http://localhost:9000/api/measurement/13 

        [Route("{id}")]
        [HttpPost]
        public IHttpActionResult Provide(int id, [FromBody] int result)
        {
            Console.WriteLine("Provide {0} for Sensor {1}", result, id);

            measurementService.ProvideMeasurementAsync(id, result);

            return Ok(); // FIXME: Created() would be better
        }
    }
}
