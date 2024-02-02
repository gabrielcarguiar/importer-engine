namespace Importer.Domain.Entities
{
    public class MyPokemon
    {
        public int Id { get; protected set; }
        public Guid UserId { get; private set; }
        public DateTime ImportedIn { get; private set; }

        public int PokemonId { get; private set; }
        public virtual Pokemon Pokemon { get; protected set; }

        public MyPokemon(int pokemonId, Guid userId)
        {
            UserId = userId;
            PokemonId = pokemonId;

            ImportedIn = DateTime.UtcNow;
        }

        protected MyPokemon()
        {
        }
    }
}
