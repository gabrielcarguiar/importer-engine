using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Importer.Domain.Entities;
using Importer.WApp.Data;
using Importer.WApp.Services;

namespace Importer.WApp.Pages.Imports.Pokemons
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public IFormFile File { get; set; }

        public PokemonDbContext DbContext { get; set; }
        public IMyPokemonService MyPokemonService { get; set; }

        public IndexModel(IMyPokemonService myPokemonService, PokemonDbContext context)
        {
            DbContext = context;
            MyPokemonService = myPokemonService;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            Import import = new();

            try
            {
                if (User.FindFirstValue(ClaimTypes.NameIdentifier) is var userId && string.IsNullOrEmpty(userId))
                    return Redirect("/login");

                var importedFile = await MyPokemonService.ImportFileAsync(File);

                import = new Import(importedFile.FileName, importedFile.FileContent, Guid.Parse(userId));
                DbContext.Imports.Add(import);

                var pokemonsIds = await DbContext.Pokemons.Select(x => x.Id).ToListAsync(cancellationToken);

                var importedPokemons = await MyPokemonService.ImportMyPokemonFileAsync(import.FileContent);
                var validPokemons = importedPokemons.Where(x => pokemonsIds.Contains(x.Id)).ToList();
                var pokemons = validPokemons.Select(x => new MyPokemon(x.Id, Guid.Parse(userId))).ToList();

                var exceptPokemons = importedPokemons.Except(validPokemons).ToList();
                var invalidPokemons = exceptPokemons.Select(x => new MatchPokemon(x.Id, Guid.Parse(userId))).ToList();

                DbContext.MyPokemons.AddRange(pokemons);
                DbContext.MatchPokemon.AddRange(invalidPokemons);

                import.ProcessImport();
                await DbContext.SaveChangesAsync(cancellationToken);

                TempData["SuccessMessage"] = "Arquivo importado com sucesso.";

                return RedirectToPage("/Imports/Index");
            }
            catch (Exception ex)
            {
                import.ImportError(ex.Message);
                await DbContext.SaveChangesAsync(cancellationToken);
                TempData["ErrorMessage"] = $"Erro ao importar o arquivo: {ex.Message}";
                return Page();
            }
        }
    }

    public class MyPokemonModel
    {
        public int Id { get; private set; }
        public IDictionary<string, string> MoreProperties { get; private set; }

        public MyPokemonModel(int id)
        {
            Id = id;
            MoreProperties = new Dictionary<string, string>();
        }

        public void AddAditionalProp(string key, string value)
        {
            MoreProperties.Add(key, value);
        }
    }

    public class ImportModel
    {
        public string FileName { get; private set; }

        public byte[] FileContent { get; private set; }

        public ImportModel(string fileName, byte[] fileContent)
        {
            FileName = fileName;
            FileContent = fileContent;
        }
    }
}
