using Microsoft.EntityFrameworkCore;
using Importer.Domain.Entities;

namespace Importer.WApp.Data
{
    public class PokemonDbContext : DbContext
    {
        public PokemonDbContext(DbContextOptions<PokemonDbContext> options)
            : base(options)
        {
        }

        public DbSet<Import> Imports { get; set; }
        public DbSet<Pokemon> Pokemons { get; set; }
        public DbSet<MatchPokemon> MatchPokemon { get; set; }
        public DbSet<MyPokemon> MyPokemons { get; set; }
        public DbSet<Film> Films { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Film>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Film>()
                .Property(x => x.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<MyPokemon>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<MyPokemon>()
                .Property(x => x.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<MatchPokemon>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<MatchPokemon>()
                .Property(x => x.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<MyPokemon>()
                .HasOne(x => x.Pokemon)
                .WithMany()
                .HasForeignKey(pm => pm.PokemonId);

            modelBuilder.Entity<Pokemon>().HasData(new List<Pokemon>
            {
                new Pokemon(01, "Bulbasaur", Domain.Enums.EPokemonType.Grass, 318, 45, 49, 49, 65, 65, 45),
                new Pokemon(02, "Ivysaur", Domain.Enums.EPokemonType.Poison, 405, 47, 49, 49, 65, 65, 45),
                new Pokemon(03, "Venusaur", Domain.Enums.EPokemonType.Fire, 525, 58, 49, 49, 65, 65, 45),
                new Pokemon(04, "Charmander", Domain.Enums.EPokemonType.Normal, 625, 33, 49, 49, 65, 65, 45),
                new Pokemon(05, "Charmeleon", Domain.Enums.EPokemonType.Water, 309, 87, 49, 49, 65, 65, 45),
                new Pokemon(06, "Charizard", Domain.Enums.EPokemonType.Ground, 578, 26, 49, 49, 65, 65, 45),
                new Pokemon(07, "Squirtle", Domain.Enums.EPokemonType.Dark, 523, 45, 49, 49, 65, 65, 45),
                new Pokemon(08, "Wartortle", Domain.Enums.EPokemonType.Bug, 643, 45, 95, 49, 65, 65, 45),
                new Pokemon(09, "Blastoise", Domain.Enums.EPokemonType.Eletric, 641, 15, 49, 49, 65, 65, 45),
                new Pokemon(10, "Caterpie", Domain.Enums.EPokemonType.Fairy, 312, 45, 49, 49, 65, 65, 45),
                new Pokemon(11, "Metapod", Domain.Enums.EPokemonType.Flying, 541, 45, 96, 49, 65, 65, 45),
                new Pokemon(12, "Butterfree", Domain.Enums.EPokemonType.Ice, 453, 45, 49, 49, 65, 65, 45),
                new Pokemon(13, "Weedle", Domain.Enums.EPokemonType.Psychic, 245, 45, 48, 49, 65, 65, 45),
                new Pokemon(14, "Kakuna", Domain.Enums.EPokemonType.Steel, 513, 45, 49, 49, 65, 65, 45),
                new Pokemon(15, "Beedrill", Domain.Enums.EPokemonType.Grass, 316, 45, 49, 49, 65, 65, 45),
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}