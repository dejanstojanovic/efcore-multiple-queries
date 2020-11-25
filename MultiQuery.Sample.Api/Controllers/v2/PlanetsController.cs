using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MultiQuery.Sample.Api.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultiQuery.Sample.Api.Controllers.v2
{
    [ApiVersion("2.0")]
    [ApiController]
    [Route("[controller]")]
    [Route("v{version:apiVersion}/[controller]")]
    public class PlanetsController : ControllerBase
    {
        readonly IServiceScopeFactory _serviceScopeFactory;

        public PlanetsController(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        [HttpGet]
        public async Task<IEnumerable<Planet>> Get([FromQuery] String[] terms)
        {
            object locker = new object();
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();

            IEnumerable<Planet> result = new List<Planet>();

            var tasks = terms.Select(async term =>
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var planetsContext = scope.ServiceProvider.GetRequiredService<PlanetsContext>();
                    IEnumerable<Planet> planets = await planetsContext.Planets.FromSqlInterpolated($"sp__GetPlanetsByName {term}").ToArrayAsync();

                    lock (locker)
                        result = result.Concat(planets);
                }
            });

            await Task.WhenAll(tasks);

            watch.Stop();

            Console.WriteLine($"Execution time:{watch.ElapsedMilliseconds} milliseconds");

            return result;
        }
    }
}
