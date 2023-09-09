using PruebaTecnica.Models;

namespace PruebaTecnica.Services.Interfaces
{
    public interface IPokemonHttpClientService
    {
        string url { get; set; }

        Task<List<PokemonInfoModel>> getPokemonsByTypeAsync(List<string> types, int topByType);
        Task<List<PokemonInfoModel?>> getPokemonsByTypeParallelAsync(List<string> types, int topByType);
    }
}