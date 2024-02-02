using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Importer.WApp.Pages.Imports.Films
{
    public class ImportsModel : PageModel
    {
        private readonly Importer.WApp.Data.PokemonDbContext _context;

        public ImportsModel(Importer.WApp.Data.PokemonDbContext context)
        {
            _context = context;
        }

        public IList<ImportDTO> Imports { get;set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) is var userId && string.IsNullOrEmpty(userId))
                return Redirect("/index");

            if (_context.Imports != null)
            {
                var imports = await _context.Imports.Where(x => x.UserId == Guid.Parse(userId)).ToListAsync();
                Imports = imports.Select(x => new ImportDTO
                {
                    Id = x.Id,
                    FileName = x.FileName,
                    ImportedIn = x.ImportedIn,
                    ProcessedAt = x.ProcessedAt ?? DateTime.MinValue,
                    ErrorMessage = x.ErrorMessage,
                }).ToList();
            }

            return Page();
        }
    }

    public class ImportDTO
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public DateTime ImportedIn { get; set; }
        public DateTime ProcessedAt { get; set; }
        public string IsSuccessful => string.IsNullOrEmpty(ErrorMessage) && ProcessedAt != DateTime.MinValue ? "True" : "False";
        public string ErrorMessage { get; set; }
    }
}
