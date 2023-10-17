namespace PruebaTecnica.Models
{
    public class Pokemon
    {
        public string Name;
        public int Height;
        public int Weight;
        public int Attack;
        public int HP;
        public int Defense;
        public int Speed;
    }

    public class PokemonTypeResultModel
    {
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

    public class PokemonParams { 
        public string valor1 { get; set; }
        public string valor2 { get; set; }
    }
}
