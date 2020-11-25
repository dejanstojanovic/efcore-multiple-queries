using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiQuery.Sample.Api.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultiQuery.Sample.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("[controller]")]
    [Route("v{version:apiVersion}/[controller]")]
    public class PlanetsController : ControllerBase
    {
        readonly PlanetsContext _planetsContext;

        public PlanetsController(PlanetsContext planetsContext)
        {
            _planetsContext = planetsContext;
        }

        [HttpGet]
        public async Task<IEnumerable<Planet>> Get([FromQuery] String[] terms)
        {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();

            IEnumerable<Planet> result = new List<Planet>();

            foreach(var term in terms)
            {
                IEnumerable<Planet> planets = await _planetsContext.Planets.FromSqlInterpolated($"sp__GetPlanetsByName {term}").ToArrayAsync();
                result = result.Concat(planets);
            }            

            watch.Stop();

            Console.WriteLine($"Execution time:{watch.ElapsedMilliseconds} milliseconds");

            return result;
        }
    }
}
