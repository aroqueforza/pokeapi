using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PruebaTecnica.Models;
using PruebaTecnica.Services;
using PruebaTecnica.Services.Interfaces;
using PruebaTecnica.Services.Services;
using System.Collections.Generic;
using System.Data;

namespace PruebaTecnica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : ControllerBase
    {
        private IPokemonHttpClientService _pokemonHttpClientService;
        public PokemonController(IPokemonHttpClientService pokemonHttpClientService) {
            this._pokemonHttpClientService=pokemonHttpClientService;
        }

        [HttpPost]
        public async Task<ActionResult<List<PokemonInfoModel>>> GetPokemons([FromBody] List<string> extraTypes, int topByType = 0)
        {
            var result = await this._pokemonHttpClientService.getPokemonsByTypeAsync(extraTypes, topByType);
            return Ok(JsonConvert.SerializeObject(result));
        }

        [HttpPost("Parallel")]
        public async Task<IActionResult> GetPokemons2([FromBody] List<string> extraTypes,[FromQuery] List<string> properties,int topByType=0)
        {
            var result = await this._pokemonHttpClientService.getPokemonsByTypeParallelAsync(extraTypes, topByType);
            DataTable dt = new DataTable();
            if(result!=null)
                dt=ReflectionService<PokemonInfoModel>.GetObject(properties, result);
            return Ok(JsonConvert.SerializeObject(dt));
        }

    }
}
