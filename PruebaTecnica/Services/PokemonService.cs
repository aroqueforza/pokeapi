using Newtonsoft.Json;
using PruebaTecnica.Models;
using System.Security.Cryptography;

namespace PruebaTecnica.Services
{
    public class PokemonService
    {
        private readonly HttpClient _httpClient;

        public PokemonService(HttpClient httpClient){
            this._httpClient = httpClient;
        }

        public List<Pokemon> GetPokemons(List<PokemonParams> pokemonParams) { 
            var lstPokemon = new List<Pokemon>();
            string baseUrl = "https://pokeapi.co/api/v2/";
            /*Temporal, borrar. Cambiar por params*/
            List<string> typePokemons = new List<string> {
                "Fire", "Electric"
            };
            try
            {
                foreach (var pokemon in pokemonParams) {
                    string urlType = $"{baseUrl}type/{pokemon}/";
                    HttpResponseMessage response = _httpClient.GetAsync(urlType).Result;
                    response.EnsureSuccessStatusCode();/*Agregar if*/
                    string responseContent = response.Content.ReadAsStringAsync().Result;
                    var jsonPokemon = JsonConvert.DeserializeObject<PokemonTypeResultModel>(responseContent);
                    foreach (var item in jsonPokemon.Pokemon){
                        string itemUri = item.Pokemon.Url;
                        HttpResponseMessage responseMessage = _httpClient.GetAsync(itemUri).Result;
                        responseMessage.EnsureSuccessStatusCode();
                        string respopnseContent = responseMessage.Content.ReadAsStringAsync().Result;
                        var pokemonValue = JsonConvert.DeserializeObject<Pokemon>(respopnseContent);
                        lstPokemon.Add(pokemonValue);
                    }
                }

                return lstPokemon;
            }
            catch (Exception ex)
            {
                return new List<Pokemon>();
            }
        }
    }
}
