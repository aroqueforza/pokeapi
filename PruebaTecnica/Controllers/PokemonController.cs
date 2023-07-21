using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace PruebaTecnica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : Controller
    {
        [HttpGet]
        public IActionResult GetPokemons()
        {
            string BseUrl = "https://pokeapi.co/api/v2/";
            HttpClient HtpCli = new HttpClient();
            HtpCli.BaseAddress = new Uri(BseUrl);
            List<string> tps = new List<string> {
                "Fire", "Electric"
            };

            List<PokemonInfoModel> ap = new List<PokemonInfoModel>();
            foreach (var t in tps)
            {
                try
                {
                    string aUrl = $"{BseUrl}type/{t.ToLower()}/";
                    HttpResponseMessage rsp = HtpCli.GetAsync(aUrl).Result; rsp.EnsureSuccessStatusCode();
                    string aRsp = rsp.Content.ReadAsStringAsync().Result;
                    var ptRes = JsonConvert.DeserializeObject<PokemonTypeResultModel>(aRsp);
                    foreach (var p in ptRes.Pokemon)
                    {
                        string pUrl = p.Pokemon.Url;
                        HttpResponseMessage prsp = HtpCli.GetAsync(pUrl).Result;
                        prsp.EnsureSuccessStatusCode();
                        string prspRsp = prsp.Content.ReadAsStringAsync().Result;
                        var pInfo = JsonConvert.DeserializeObject<PokemonInfoModel>(prspRsp);
                        ap.Add(pInfo);
                    }
                }
                catch (Exception)
                {               
                    Console.WriteLine($"¡Err al btr ls Pkmn dl tpo '{t}'!");
                }
            }
            return Ok(JsonConvert.SerializeObject(ap));
        }
    }

    public class PokemonTypeResultModel { 
        public List<PokemonTypePokemonModel> Pokemon { get; set; } 
    }
    public class PokemonTypePokemonModel
    {
        public PokemonTypePokemonItemModel Pokemon { get; set; }
    }
    public class PokemonTypePokemonItemModel
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }
    public class PokemonInfoModel
    {
        public string Name;
        public int Height;
        public int Weight;

        //Completar estos datos.
        public int Attack;
        public int HP;
        public int Defense;
        public int Speed;
    }
}
