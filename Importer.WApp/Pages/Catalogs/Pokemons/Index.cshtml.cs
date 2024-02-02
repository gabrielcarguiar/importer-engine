using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Importer.WApp.Data;
using Importer.Domain.Enums;
using Importer.Domain.Entities;

namespace Importer.WApp.Pages.Catalogs.Pokemons
{
    public class IndexModel : PageModel
    {
        private readonly PokemonDbContext _context;

        public IndexModel(PokemonDbContext context)
        {
            _context = context;
        }

        public IList<Pokemon> Pokemons { get; private set; }
        public IList<PokemonDTO> MyPokemons { get; private set; }
        public IList<PokemonDTO> MatchPokemons { get; private set; }

        [BindProperty]
        public string SearchCatalog { get; set; }

        public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
        {
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) is var userId && string.IsNullOrEmpty(userId))
                return Redirect("/index");

            if (_context.MyPokemons != null)
            {
                var myPokemon = await _context.MyPokemons
                .Include(m => m.Pokemon)
                .Where(x => x.UserId == Guid.Parse(userId))
                .ToListAsync(cancellationToken);

                var distinctPokemons = myPokemon
                    .GroupBy(p => p.PokemonId)
                    .Select(g => new PokemonDTO
                    {
                        Id = g.Key,
                        Name = g.First(x => x.PokemonId == g.Key).Pokemon.Name,
                        Type = g.First(x => x.PokemonId == g.Key).Pokemon.Type,
                        Quantity = g.Count(),
                        Pokemon = g.First(x => x.PokemonId == g.Key).Pokemon
                    })
                    .ToList();

                MyPokemons = distinctPokemons;
                HttpContext.Session.Set("MyPokemons", MyPokemons);
            }

            if (_context.Pokemons != null)
            {
                var pokemonsQuery = _context.Pokemons;
                Pokemons = await pokemonsQuery.Distinct().ToListAsync(cancellationToken);
                HttpContext.Session.Set("Pokemons", Pokemons);
            }

            if (_context.MatchPokemon != null)
            {
                var matchPokemonsQuery = _context.MatchPokemon.Where(x => x.UserId == Guid.Parse(userId));
                var matchPokemonsGroup = await matchPokemonsQuery
                        .GroupBy(p => p.PokemonId)
                        .Select(g => new PokemonDTO
                        {
                            Id = g.Key,
                            Name = "Não Identificado",
                            Type = EPokemonType.None,
                            Quantity = g.Count(),
                        })
                        .ToListAsync(cancellationToken);

                MatchPokemons = matchPokemonsGroup;
                HttpContext.Session.Set("MatchPokemons", MatchPokemons);
            }


            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string searchBy)
        {
            Pokemons = HttpContext.Session.Get<IList<Pokemon>>("Pokemons");
            MyPokemons = HttpContext.Session.Get<IList<PokemonDTO>>("MyPokemons");
            MatchPokemons = HttpContext.Session.Get<IList<PokemonDTO>>("MatchPokemons");

            if (searchBy.Equals(nameof(Pokemons)))
            {
                if (!string.IsNullOrEmpty(SearchCatalog))
                    Pokemons = Pokemons.Where(x => x.Id.ToString().Contains(SearchCatalog) || x.Name.Contains(SearchCatalog) || x.Type.ToString().Contains(SearchCatalog)).ToList();
            }

            if (searchBy.Equals(nameof(MyPokemons)))
            {
                if (!string.IsNullOrEmpty(SearchCatalog))
                    MyPokemons = MyPokemons.Where(x => x.Id.ToString().Contains(SearchCatalog) || x.Name.Contains(SearchCatalog) || x.Type.ToString().Contains(SearchCatalog)).ToList();
            }

            return Page();
        }

        public class PokemonDTO
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public EPokemonType Type { get; set; }
            public int Quantity { get; set; }
            public Pokemon Pokemon { get; set; }
        }
    }

    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
