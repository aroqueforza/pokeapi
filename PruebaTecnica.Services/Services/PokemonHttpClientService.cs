using Newtonsoft.Json;
using PruebaTecnica.Models;
using PruebaTecnica.Services.Interfaces;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace PruebaTecnica.Services
{
    public class PokemonHttpClientService : IPokemonHttpClientService
    {

        public string url { get; set; } = "https://pokeapi.co/api/v2/";

        private readonly HttpClient _httpclient;

        public PokemonHttpClientService(HttpClient httpclient)
        {
            this._httpclient = httpclient;
            this._httpclient.BaseAddress = new Uri(url);
        }

        public async Task<List<PokemonInfoModel>> getPokemonsByTypeAsync(List<string> types, int topByType)
        {
            List<PokemonInfoModel> ResultPokemonInfoModel = new List<PokemonInfoModel>();

            foreach (string pokemonType in types)
            {
                try
                {
                    PokemonTypeResultModel? pokemonTypeResultModel = await this.getPokemonTypeResultModel(pokemonType);
                    if (pokemonTypeResultModel is null)
                        continue;

                    //We take 1 pokemon from each type just to show the result quicker
                    foreach (var p in pokemonTypeResultModel.Pokemon.Take(topByType > 0 ? topByType : pokemonTypeResultModel.Pokemon.Count()))
                    {
                        PokemonInfoModel? pokemonInfoModel = await this.getPokemonInfoModel(p.Pokemon.Url);
                        if (pokemonInfoModel is null)
                            continue;

                        ResultPokemonInfoModel.Add(pokemonInfoModel);
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine($"¡Err al btr ls Pkmn dl tpo '{pokemonType}'!");
                }
            }
            return await Task.FromResult(ResultPokemonInfoModel);
        }

        public async Task<List<PokemonInfoModel?>> getPokemonsByTypeParallelAsync(List<string> types, int topByType)
        {
            List<PokemonInfoModel?> ResultPokemonInfoModel = new List<PokemonInfoModel?>();

            var taskPokemonTypes = types.Select(async o => await processPokemonType(o, topByType));

            var taskResults = await Task.WhenAll(taskPokemonTypes);

            taskResults.ToList().ForEach(o => ResultPokemonInfoModel.AddRange(o));
            
            return await Task.FromResult(ResultPokemonInfoModel);
        }

        private async Task<PokemonTypeResultModel?> getPokemonTypeResultModel(string pokemonType)
        {
            PokemonTypeResultModel? pokemonTypeResultModel;

            string pokemonTypesEndPointUrl = $"type/{pokemonType.ToLower()}/";

            HttpResponseMessage pokemonTypesResult = this._httpclient.GetAsync(pokemonTypesEndPointUrl).Result; pokemonTypesResult.EnsureSuccessStatusCode();
            pokemonTypesResult.EnsureSuccessStatusCode();

            string stringPokemonTypesResult = pokemonTypesResult.Content.ReadAsStringAsync().Result;
            pokemonTypeResultModel = JsonConvert.DeserializeObject<PokemonTypeResultModel>(stringPokemonTypesResult);

            return await Task.FromResult(pokemonTypeResultModel);
        }

        private async Task<PokemonInfoModel?> getPokemonInfoModel(string pokemonInfoEndPointUrl)
        {
            HttpResponseMessage pokemonInfoResult = this._httpclient.GetAsync(pokemonInfoEndPointUrl).Result;

            pokemonInfoResult.EnsureSuccessStatusCode();

            string stringPokemonInfoResult = pokemonInfoResult.Content.ReadAsStringAsync().Result;

            PokemonInfoModel? pokemonInfoModel = JsonConvert.DeserializeObject<PokemonInfoModel>(stringPokemonInfoResult);

            return await Task.FromResult(pokemonInfoModel);
        }

        private async Task<List<PokemonInfoModel?>> processPokemonType(string pokemonType, int topByType)
        {
            List<PokemonInfoModel?> ResultPokemonInfoModel = new List<PokemonInfoModel?>();
            try
            {
                PokemonTypeResultModel? pokemonTypeResultModel = await this.getPokemonTypeResultModel(pokemonType);
                if (pokemonTypeResultModel is null)
                    return await Task.FromResult(ResultPokemonInfoModel);

                var tasksGetPokemonInfoModel = pokemonTypeResultModel.Pokemon.Take(topByType > 0 ? topByType : pokemonTypeResultModel.Pokemon.Count()).Select(async o => await this.getPokemonInfoModel(o.Pokemon.Url));

                var taskResults = await Task.WhenAll(tasksGetPokemonInfoModel);

                if (taskResults != null)
                    ResultPokemonInfoModel.AddRange(taskResults.ToList());
            }
            catch (Exception)
            {
                Console.WriteLine($"¡Err al btr ls Pkmn dl tpo '{pokemonType}'!");
            }
            return await Task.FromResult(ResultPokemonInfoModel);
        }
    }
}
