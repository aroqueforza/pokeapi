using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PruebaTecnica.Models;
using PruebaTecnica.Services;
using PruebaTecnica.Services.Interfaces;

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
        public async Task<ActionResult<List<PokemonInfoModel>>> GetPokemons([FromBody] List<string> extraTypes, int topByType=0)
        {
            var result=await this._pokemonHttpClientService.getPokemonsByTypeAsync(extraTypes, topByType);
            return Ok(JsonConvert.SerializeObject(result));
        }

        [HttpPost("Parallel")]
        public async Task<ActionResult<List<PokemonInfoModel>>> GetPokemons2([FromBody] List<string> extraTypes,int topByType=0)
        {
            var result = await this._pokemonHttpClientService.getPokemonsByTypeParallelAsync(extraTypes, topByType);
            return Ok(JsonConvert.SerializeObject(result));
        }

    }
}
