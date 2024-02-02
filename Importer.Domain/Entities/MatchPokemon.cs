namespace Importer.Domain.Entities
{
    public class MatchPokemon
    {
        public int Id { get; private set; }

        public int PokemonId { get; set; }

        public Guid UserId { get; private set; }

        public MatchPokemon(int pokemonId, Guid userId)
        {
            PokemonId = pokemonId;
            UserId = userId;
        }
    }
}
