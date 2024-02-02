using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Importer.Domain.Entities;
using Importer.WApp.Data;
using Importer.WApp.Services;

namespace Importer.WApp.Pages.Imports.Films
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public IFormFile File { get; set; }

        public PokemonDbContext DbContext { get; set; }
        public IMyFilmService MyFilmService { get; set; }

        public IndexModel(IMyFilmService myFilmService, PokemonDbContext context)
        {
            DbContext = context;
            MyFilmService = myFilmService;
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

                var importedFile = await MyFilmService.ImportFileAsync(File);

                import = new Import(importedFile.FileName, importedFile.FileContent, Guid.Parse(userId));
                DbContext.Imports.Add(import);

                var importedFilms = await MyFilmService.ImportMyFilmsFileAsync(import.FileContent);
                var films = importedFilms.Select(x => new Film(x.Name, 1, Guid.Parse(userId)));
                DbContext.Films.AddRange(films);

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

    public class MyFilmModel
    {
        public string Name { get; set; }
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
