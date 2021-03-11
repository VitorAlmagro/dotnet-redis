using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ApiRedis.Controllers
{
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IDistributedCache _cache;

        public HomeController(IDistributedCache cache)
        {
            _cache = cache;
        }

        [HttpGet]
        [Route("redis")]
        public async Task<IActionResult> Get()
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            string valorJSON = _cache.GetString("KeyExample");

            if (valorJSON == null)
            {
                // Simulando busca informação do banco
                await Task.Delay(5000);
                valorJSON = "example";

                DistributedCacheEntryOptions opcoesCache =
                          new DistributedCacheEntryOptions();
                opcoesCache.SetAbsoluteExpiration(
                    TimeSpan.FromSeconds(30));

                _cache.SetString("KeyExample", valorJSON, opcoesCache);
            }

            stopwatch.Stop();

            return Ok(new 
            {
                tempo = stopwatch.ElapsedMilliseconds,
                valor = valorJSON
            });
        }
    }
}
