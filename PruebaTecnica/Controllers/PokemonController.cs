using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PruebaTecnica.Models;
using PruebaTecnica.Services;

namespace PruebaTecnica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : Controller
    {
        private readonly PokemonService _pokemonService;

        //public PokemonService(PokemonService pokemonService) { 
        //    _pokemonService = pokemonService;
        //}

        [HttpGet]
        public IActionResult GetPokemons()
        {
            try {
                var pokemonParams = new List<PokemonParams>();
                pokemonParams[0].valor1 = "Fire";
                pokemonParams[1].valor1 = "Electric";
                var response = _pokemonService.GetPokemons(pokemonParams);
                return Ok(response);
            }
            catch (Exception ex) { 
                return BadRequest(ex.Message);
            }
        }
    }
}
